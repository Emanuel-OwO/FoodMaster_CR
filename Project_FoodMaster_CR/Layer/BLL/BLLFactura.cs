using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLFactura : IBLLFactura
    {
        private readonly IDALFactura _objDALFactura;

        public BLLFactura() 
        {
            _objDALFactura = new DALFactura();
        }


        public decimal CalcularIVA(decimal subTotal)
        {
            return subTotal * 0.13m;
        }

        public decimal CalcularSubTotal(List<FacturaDetalle> listaDetalle)
        {
            decimal subtotal = 0;
            if (listaDetalle != null)
            {
                foreach (FacturaDetalle item in listaDetalle)
                    subtotal += (decimal)item.Subtotal;
            }
            return subtotal;
        }

        public decimal CalcularTotalColones(decimal subTotal, decimal impuesto)
        {
            return subTotal + impuesto;
        }

        public decimal CalcularTotalDolares(decimal totalColones, decimal tipoCambio)
        {
            if (tipoCambio <= 0)
                throw new Exception("El tipo de cambio debe ser mayor a cero.");

            return totalColones / tipoCambio;
        }

        public bool Delete(int pIdFactura)
        {

            if (pIdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            return _objDALFactura.Delete(pIdFactura);
        }

        public List<Factura> GetAll()
        {
            return _objDALFactura.GetAll();
        }

        public Factura GetById(int pIdFactura)
        {

            if (pIdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            return _objDALFactura.GetById(pIdFactura);
        }

        public int Save(Factura factura)
        {
            if (factura == null)
                throw new Exception("La facturación no puede ser nula.");

            if (factura.IdCliente <= 0)
                throw new Exception("Debe seleccionar un cliente válido.");

            if (factura.IdUsuario <= 0)
                throw new Exception("Debe existir un usuario válido.");

            if (factura.TotalColones <= 0)
                throw new Exception("El total en colones debe ser mayor a cero.");

            factura.Estado = true;
            return _objDALFactura.Insert(factura);
        }

        public bool Update(Factura factura)
        {
            if (factura == null)
                throw new Exception("La facturación no puede ser nula.");

            if (factura.IdFactura <= 0)
                throw new Exception("IdFactura inválido.");

            return _objDALFactura.Update(factura);
        }

        public void UpdateNumFactura(int idFactura, string numFactura)
        {
            if (idFactura <= 0)
                throw new Exception("IdFactura inválido.");
            if (string.IsNullOrWhiteSpace(numFactura))
                throw new Exception("El número de factura no puede estar vacío.");

            _objDALFactura.UpDateNumFactura(idFactura, numFactura);
        }

        public void UpdateXMLFactura(int idFactura, string xmlFactura)
        {
            if (idFactura <= 0)
                throw new Exception("IdFactura inválido.");
            if (string.IsNullOrWhiteSpace(xmlFactura))
                throw new Exception("El XML de la factura está vacío.");

            _objDALFactura.UpDateXMLFactura(idFactura, xmlFactura);
        }
    }
}
