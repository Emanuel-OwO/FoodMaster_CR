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
    public class BLLCliente : IBLLCliente
    {
        private IDALCliente _dALCliente = new DALCliente();
        public void DELETE(int id)
        {
            if (id > 0)
            {
                _dALCliente.DELETE(id);
            }
        }

        public List<Cliente> Get_By_Filter(string pDescripcion)
        {
            return _dALCliente.Get_By_Filter(pDescripcion);
        }

        public void INSERT(Cliente cliente)
        {
            if (!string.IsNullOrEmpty(cliente.Identificacion))
            {
                if (Existe(cliente.Identificacion))
                {
                    _dALCliente.UPDATE(cliente);
                }
                else
                {
                    _dALCliente.INSERT(cliente);
                }
            }
        }
        private bool Existe(string identificacion)
        {
            var lista = _dALCliente.SelectAll();

            if (lista == null) return false;

            foreach (var c in lista)
            {
                if (c.Identificacion == identificacion)
                    return true;
            }

            return false;
        }


        public List<Cliente> SELECTALL()
        {
            return _dALCliente.SelectAll();
        }

        public void UPDATE(Cliente cliente)
        {
            if (cliente.IdCliente > 0)
            {
                    _dALCliente.UPDATE(cliente);
            }
        }
    }
}
