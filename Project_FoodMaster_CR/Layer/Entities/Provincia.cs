using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Entities
{
    public class Provincia
    {
        public int IdProvincia { set; get; }
        public string Descripcion { set; get; }

        public override string ToString() => IdProvincia + " " + Descripcion;
    }
}
