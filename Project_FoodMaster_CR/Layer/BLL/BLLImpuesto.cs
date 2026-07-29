using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLImpuesto : IBLLImpuesto
    {
        private IDALImpuesto _objDALImpuesto = new DALImpuesto();
        public void DELETE(DateTime fecha)
        {
            _objDALImpuesto.DELETE(fecha);
        }

        public List<Impuesto> Get_By_Filter(string filtro)
        {
            return _objDALImpuesto.Get_By_Filter(filtro);
        }

        public void INSERT(Impuesto impuesto)
        {
            if (impuesto.Porcentaje <= 0)
                throw new Exception("El porcentaje debe ser mayor a 0");
            if (impuesto.Porcentaje > 100)
                throw new Exception("El porcentaje no puede ser mayor a 100");

            // No permitir dos registros con la misma fecha (Fecha es la PK)
            var existentes = _objDALImpuesto.SELECTALL();
            if (existentes != null && existentes.Any(i => i.Fecha.Date == impuesto.Fecha.Date))
                throw new Exception("Ya existe un porcentaje de impuesto registrado para esa fecha");

            _objDALImpuesto.INSERT(impuesto);
        }

        public Impuesto ObtenerVigente(DateTime? fecha)
        {
            var impuesto = _objDALImpuesto.ObtenerVigente(fecha);
            if (impuesto == null)
                throw new Exception("No hay un porcentaje de impuesto configurado. " +
                    "Registre uno en el mantenimiento de Impuesto antes de facturar.");
            return impuesto;
        }

        public List<Impuesto> SELECTALL()
        {
            return _objDALImpuesto.SELECTALL();
        }

        public void UPDATE(Impuesto impuesto)
        {
            if (impuesto.Porcentaje <= 0)
                throw new Exception("El porcentaje debe ser mayor a 0");
            if (impuesto.Porcentaje > 100)
                throw new Exception("El porcentaje no puede ser mayor a 100");

            _objDALImpuesto.UPDATE(impuesto);
        }
    }


}
