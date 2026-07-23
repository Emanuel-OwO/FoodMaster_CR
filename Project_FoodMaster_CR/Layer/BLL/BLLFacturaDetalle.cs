using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLFacturaDetalle : IBLLFacturaDetalle
    {
        private readonly IDALFacturaDetalle _dalDetalle;

        public BLLFacturaDetalle()
        {
            _dalDetalle = new DALFacturaDetalle();
        }

        public decimal CalcularSubTotal(int cantidad, decimal precio)
        {
            if (cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            if (precio <= 0)
                throw new Exception("El precio debe ser mayor a cero.");

            return cantidad * precio;
        }

        public bool DeleteByFactura(int pIdFactura)
        {
            if (pIdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            return _dalDetalle.DeleteByFactura(pIdFactura);
        }

        public List<FacturaDetalle> GetByFactura(int pIdFactura)
        {
            if (pIdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            return _dalDetalle.GetByFactura(pIdFactura);
        }

        public int Save(FacturaDetalle facturaDetalle)
        {
            if (facturaDetalle == null)
                throw new Exception("El detalle no puede ser nulo.");

            if (facturaDetalle.IdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            if (facturaDetalle.IdProducto <= 0)
                throw new Exception("Debe seleccionar un producto válido.");

            if (facturaDetalle.Cantidad <= 0)
                throw new Exception("La cantidad debe ser mayor a cero.");

            if (facturaDetalle.Precio <= 0)
                throw new Exception("El precio debe ser mayor a cero.");

            if (facturaDetalle.Subtotal <= 0)
                throw new Exception("El subtotal debe ser mayor a cero.");

            return _dalDetalle.Insert(facturaDetalle);
        }
    }
}
