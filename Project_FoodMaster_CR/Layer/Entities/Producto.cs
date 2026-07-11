using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Entities
{
    public class Producto
    {
        public int IdProducto { set; get; }
        public int IdTipoProducto { set; get; }
        public string CodigoInterno { set; get; }
        public string CodigoBarras { set; get; }
        public string Descripcion { set; get; }
        public double Peso { set; get; }
        public int ContenidoCalorico { set; get; }
        public byte[] Foto { set; get; }
        public byte[] DocumentoEspecificaciones { set; get; }
        public int CantidadStock { set; get; }
        public double Precio { set; get; }
        public bool Estado { set; get; }
    }
}
