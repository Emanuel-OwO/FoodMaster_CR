using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Utilitarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Reportes
{
    public partial class frmReportesFacturas : Form
    {
        private IBLLReporte _bllReporte = new BLLReportes();

        public frmReportesFacturas()
        {
            InitializeComponent();
        }

        private void frmReportesFacturas_Load(object sender, EventArgs e)
        {

        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                var lista = _bllReporte.GetFacturasPorFecha(
                    dtpFechaInicial.Value,
                    dtpFechaFinal.Value
                );

                if (lista.Count == 0)
                {
                    MessageBox.Show("No hay datos para ese rango.");
                    return;
                }

                decimal total = lista.Sum(x => x.TotalColones);

                FacturaReportePdfService pdf = new FacturaReportePdfService();
                pdf.GenerarPdf(lista, total, dtpFechaInicial.Value, dtpFechaFinal.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
