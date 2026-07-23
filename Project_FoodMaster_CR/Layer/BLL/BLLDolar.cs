using appFoodMaster_CR.Layer.DTO;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLDolar : IBLLDolar
    {
        public double GetVentaDolar()
        {
            try
            {
                String json = "";
                string URLPadron = ConfigurationManager.AppSettings["DolarVenta"];
                string url = URLPadron;

                WebRequest request = WebRequest.Create(url);
                request.Method = "GET";

                using (WebResponse webResponse = request.GetResponse())
                {
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    json = reader.ReadToEnd();
                }

                DolarDTO oDolarDTO = JsonSerializer.Deserialize<DolarDTO>(json);
                return oDolarDTO.venta.valor;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
