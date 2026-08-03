using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.DTO
{
    public class ProductoVendidoReporteDTO
    {
        public int IdProducto { get; set; }
        public string CodigoInterno { get; set; }
        public string Descripcion { get; set; }
        public string TipoProducto { get; set; }
        public decimal Precio { get; set; }
        public int CantidadVendida { get; set; }
        public byte[] Fotografia { get; set; }
    }
}
