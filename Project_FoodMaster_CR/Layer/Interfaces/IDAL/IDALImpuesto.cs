using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IDAL
{
    public interface IDALImpuesto
    {
        void INSERT(Impuesto impuesto);
        void UPDATE(Impuesto impuesto);
        void DELETE(DateTime fecha);
        List<Impuesto> SELECTALL();
        List<Impuesto> Get_By_Filter(string filtro);
        Impuesto ObtenerVigente(DateTime? fecha);
    }
}
