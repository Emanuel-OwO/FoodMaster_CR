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
    public partial class frmReporteProductos : Form
    {

        private IBLLReporte _bllReporte = new BLLReportes();
        private BLLProducto _bllProducto = new BLLProducto();
        private BLLTipoProducto _bllTipoProducto = new BLLTipoProducto();

        public frmReporteProductos()
        {
            InitializeComponent();
        }

        private void frmReporteProductos_Load(object sender, EventArgs e)
        {
            cboProducto.DataSource = _bllProducto.SelectAll();
            cboProducto.DisplayMember = "Descripcion";
            cboProducto.ValueMember = "IdProducto";
            cboProducto.SelectedIndex = -1;

            cboTipoProducto.DataSource = _bllTipoProducto.SELECTALL();
            cboTipoProducto.DisplayMember = "Descripcion";
            cboTipoProducto.ValueMember = "IdTipoProducto";
            cboTipoProducto.SelectedIndex = -1;

            ActualizarEstadoFiltroProductos();

            rdbPorProducto.Checked = true;
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                int? IdProducto = cboProducto.SelectedIndex >= 0 ? Convert.ToInt32(cboProducto.SelectedValue) : (int?)null;
                int? IdTipoProducto = cboTipoProducto.SelectedIndex >= 0 ? Convert.ToInt32(cboTipoProducto.SelectedValue) : (int?)null;
                string Descripcion = txtDescripcion.Text.Trim();

                var lista = _bllReporte.GetProductosVendidos(IdProducto, Descripcion, IdTipoProducto);

                if (lista == null || lista.Count == 0)
                {
                    MessageBox.Show("No hay productos vendidos con esos filtros.");
                    return;
                }

                ProductoVendidoReportePdfService pdf = new ProductoVendidoReportePdfService();
                pdf.GenerarPdf(lista, cboProducto.Text, Descripcion, cboTipoProducto.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar reporte: " + ex.Message);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rdbPorProducto_CheckedChanged(object sender, EventArgs e)
        {
            ActualizarEstadoFiltroProductos();
        }
        private void ActualizarEstadoFiltroProductos()
        {
            bool habilitar = rdbPorProducto.Checked;

            cboProducto.Enabled = habilitar;
            cboTipoProducto.Enabled = habilitar;
            txtDescripcion.Enabled = habilitar;

            if (!habilitar)
            {
                cboProducto.SelectedIndex = -1;
                cboTipoProducto.SelectedIndex = -1;
                txtDescripcion.Text = "";
            }
        }
    }
}
