using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IDAL
{
    public interface IDALTipoProducto
    {
        void CREATE(TipoProducto tipo);
        void UPDATE(TipoProducto tipo);
        void DELETE(int id);
        List<TipoProducto> SelectAll();
        TipoProducto SelectById(int id);
    }
}
