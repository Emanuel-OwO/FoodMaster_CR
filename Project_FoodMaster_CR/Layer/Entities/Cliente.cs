using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string TipoIdentificacion { get; set; }  
        public string Identificacion { get; set; }       
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int IdProvincia { get; set; }
        public string Direccion { get; set; }
        public byte[] Fotografia { get; set; }
        public bool Estado { get; set; }
    }
}
