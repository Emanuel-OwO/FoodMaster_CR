using appFoodMaster_CR.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IDAL
{
    public interface IDALProvincia
    {
        List<Provincia> GetAll();
        Provincia GetById(int pId);
        Provincia Save(Provincia pProvincia);
        Provincia Update(Provincia pProvincia);
        bool Delete(int pId);
    }
}
