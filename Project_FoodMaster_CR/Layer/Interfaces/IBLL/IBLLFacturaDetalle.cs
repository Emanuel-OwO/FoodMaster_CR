using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IBLL
{
    public interface IBLLFacturaDetalle
    {
        int Save(FacturaDetalle facturaDetalle);
        bool DeleteByFactura(int pIdFactura);
        List<FacturaDetalle> GetByFactura(int pIdFactura);
        decimal CalcularSubTotal(int cantidad, decimal precio);
    }
}
