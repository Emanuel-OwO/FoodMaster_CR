using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IBLL
{
    public interface IBLLTarjeta
    {
        int Save(Tarjeta tarjeta);
        List<Tarjeta> GetAll();
    }
}
