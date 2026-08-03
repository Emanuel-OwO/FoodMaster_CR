using appFoodMaster_CR.Layer.DTO;
using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IDAL
{
    public interface IDALReporte
    {
        List<FacturaReporteDTO> GetFacturasPorFecha(DateTime fechaInicial, DateTime fechaFinal);
        List<Cliente> GetClientesReporte();
        List<ProductoVendidoReporteDTO> GetProductosVendidos(int? IdProducto, string Descripcion, int? IdTipoProducto);
        List<VentasPorFechaDTO> GetVentasPorFecha(DateTime fechaInicial, DateTime fechaFinal);
    }
}
