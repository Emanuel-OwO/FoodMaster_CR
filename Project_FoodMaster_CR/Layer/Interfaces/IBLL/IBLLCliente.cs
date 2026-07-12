using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IBLL
{
    public interface IBLLCliente
    {
        void INSERT(Cliente cliente);
        void UPDATE(Cliente cliente);
        void DELETE(int id);
        List<Cliente> SELECTALL();
        List<Cliente> Get_By_Filter(string pDescripcion);
    }
}
