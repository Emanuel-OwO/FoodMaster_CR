using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IBLL
{
    public interface IBLLProducto
    {
        void CREATE(Producto producto);
        void UPDATE(Producto producto);
        void DELETE(int id);
        List<Producto> SelectAll();
        Producto SelectById(int id);

        List<Producto> Get_By_Filter(string filtro);
    }
}
