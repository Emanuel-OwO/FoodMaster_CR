using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Entities
{
    public class MovimientoInventario
    {
        public int IdMovimiento { set; get; }
        public int IdProducto { set; get; }
        public string TipoMovimiento { set; get; }
        public int Cantidad { set; get; }
        public DateTime Fecha { set; get; }
        public string Observaciones { set; get; }
        public int IdFactura { set; get; }
    }
}
