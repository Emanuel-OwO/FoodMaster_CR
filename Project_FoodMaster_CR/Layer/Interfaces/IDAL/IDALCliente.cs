using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IDAL
{
    public interface IDALCliente
    {
        void INSERT(Cliente cliente);
        void UPDATE(Cliente cliente);
        void DELETE(int idCliente);
        List<Cliente> SelectAll();
        Cliente SelectById(int idCliente);
        List<Cliente> Get_By_Filter(string pDescripcion);
    }
}
