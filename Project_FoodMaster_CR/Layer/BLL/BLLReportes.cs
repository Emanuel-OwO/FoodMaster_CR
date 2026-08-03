using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.DTO;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLReportes : IBLLReporte
    {
        private readonly IDALReporte _dal = new DALReporte();
        public List<Cliente> GetClientesReporte()
        {
            return _dal.GetClientesReporte();
        }

        public List<FacturaReporteDTO> GetFacturasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            if (fechaInicial > fechaFinal)
                throw new Exception("La fecha inicial no puede ser mayor que la final.");

            return _dal.GetFacturasPorFecha(fechaInicial, fechaFinal);
        }

        public List<ProductoVendidoReporteDTO> GetProductosVendidos(int? IdProducto, string Descripcion, int? IdTipoProducto)
        {
            return _dal.GetProductosVendidos(IdProducto, Descripcion, IdTipoProducto);
        }

        public List<VentasPorFechaDTO> GetVentasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            if (fechaInicial > fechaFinal)
                throw new Exception("La fecha inicial no puede ser mayor que la final.");

            return _dal.GetVentasPorFecha(fechaInicial, fechaFinal);
        }
    }
}
