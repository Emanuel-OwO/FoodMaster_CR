using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.DTO
{
  

        public class DolarDTO
        {
            public Venta venta { get; set; }
            public Compra compra { get; set; }
        }

        public class Venta
        {
            public string fecha { get; set; }
            public float valor { get; set; }
        }

        public class Compra
        {
            public string fecha { get; set; }
            public float valor { get; set; }
        }

}
