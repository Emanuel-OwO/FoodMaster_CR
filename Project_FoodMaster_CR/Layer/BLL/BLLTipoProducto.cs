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
    public class BLLTipoProducto : IBLLTipoProducto
    {
        private IDALTipoProducto _objDALTipoProducto = new DALTipoProducto();

        public void CREATE(TipoProducto tipo)
        {
            if (!string.IsNullOrEmpty(tipo.Descripcion))
            {
                if (Existe(tipo.Descripcion))
                {
                    _objDALTipoProducto.UPDATE(tipo);
                }
                else
                {
                    _objDALTipoProducto.CREATE(tipo);
                }
            }
        }

        public void DELETE(int id)
        {
            if (id > 0)
            {
                _objDALTipoProducto.DELETE(id);
            }
        }

        public List<TipoProducto> SELECTALL()
        {
            return _objDALTipoProducto.SelectAll();
        }

        public TipoProducto SELECTBYID(int id)
        {
            return _objDALTipoProducto.SelectById(id);
        }

        public void UPDATE(TipoProducto tipo)
        {
            if (tipo.IdTipoProducto > 0)
            {
                _objDALTipoProducto.UPDATE(tipo);
            }
        }

        private bool Existe(string descripcion)
        {
            var lista = _objDALTipoProducto.SelectAll();

            if (lista == null) return false;

            foreach (var t in lista)
            {
                if (t.Descripcion == descripcion)
                    return true;
            }

            return false;
        }
    }
}
