using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.Entities
{
    public class Factura
    {
        public int IdFactura { set; get; }
        public string NumeroFactura { set; get; }
        public DateTime Fecha { set; get; }
        public string IdCliente { set; get; }
        public int IdUsuario { set; get; }
        public double Subtotal { set; get; }
        public double PorcentajeImpuestoAplicado { set; get; }
        public double MontoImpuesto { set; get; }
        public double TipoCambio { set; get; }
        public double TotalColones { set; get; }
        public double TotalDolares { set; get; }
        public string XMLFactura { set; get; }
        public byte[] FirmaCliente { set; get; }
        public int IdTarjeta { set; get; }
        public string UltimosDigitosTarjeta { set; get; }
        public string NumeroAutorizacion { set; get; }
        public bool Estado { set; get; }
    }
}
