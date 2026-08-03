using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
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
    public partial class frmReporteClientes : Form
    {
        private IBLLReporte _bllReporte = new BLLReportes();
        public frmReporteClientes()
        {
            InitializeComponent();
        }

        private void frmReporteClientes_Load(object sender, EventArgs e)
        {
            ActualizarEstadoFiltro();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {

                // Validaciones de los radio buttons ANTES de consultar la base
                if (rdbOrdenadoCedula.Checked && string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    MessageBox.Show("Necesitas escribir la cédula del cliente.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFiltro.Focus();
                    return;
                }

                if (rdbOrdenadoNombre.Checked && string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    MessageBox.Show("Necesitas escribir el nombre del cliente.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFiltro.Focus();
                    return;
                }


                List<Cliente> lista = _bllReporte.GetClientesReporte();

                if (lista == null || lista.Count == 0)
                {
                    MessageBox.Show("No hay clientes para generar el reporte.");
                    return;
                }

                // Filtrar por lo escrito en el textbox (cédula o nombre), si hay algo escrito
                string filtro = txtFiltro.Text.Trim(); 
                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    lista = lista.Where(x =>
                        x.Identificacion.Contains(filtro) ||
                        x.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0
                    ).ToList();

                    if (lista.Count == 0)
                    {
                        MessageBox.Show("No se encontró ningún cliente con ese criterio.");
                        return;
                    }
                }

                // Ordenar el resultado (ya filtrado, o completo si no escribiste nada)
                if (rdbOrdenadoCedula.Checked)
                {
                    lista = lista.OrderBy(x => x.IdCliente).ToList();
                }
                else if (rdbOrdenadoNombre.Checked)
                {
                    lista = lista.OrderBy(x => x.Nombre).ToList();
                }

                ClienteReportePdfService pdf = new ClienteReportePdfService();
                pdf.GenerarPdf(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte de clientes: " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ActualizarEstadoFiltro()
        {
            if (rdbMostrarTodos.Checked)
            {
                txtFiltro.Text = "";
                txtFiltro.Enabled = false;
            }
            else
            {
                txtFiltro.Enabled = true;
            }
        }

        private void rdbOrdenadoCedula_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarEstadoFiltro();
        }
    }
}
