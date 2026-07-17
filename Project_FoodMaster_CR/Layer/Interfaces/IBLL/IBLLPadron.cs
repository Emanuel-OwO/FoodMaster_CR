using Project_CatTech.Layer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Interfaces.IBLL
{
    public interface IBLLPadron
    {
        PadronElectoral GetById(string pId);
    }
}
