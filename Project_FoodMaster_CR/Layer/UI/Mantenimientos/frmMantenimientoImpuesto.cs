using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Mantenimientos
{
    public partial class frmMantenimientoImpuesto : Form
    {
        private static readonly ILog _myLogControlEventos = log4net.LogManager.GetLogger("MyControlEventos");
       private IBLLImpuesto _objBLLImpuesto = new BLLImpuesto();

        public frmMantenimientoImpuesto()
        {
            InitializeComponent();
        }

        private void frmMantenimientoImpuesto_Load(object sender, EventArgs e)
        {
            CargarDatos();
            
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Impuesto impuesto = new Impuesto();

                impuesto.Fecha = dtpFechaImpuesto.Value.Date;

                if (!double.TryParse(txtPorcentaje.Text.Replace(",", "."),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double porcentaje))
                {
                    MessageBox.Show("Porcentaje inválido");
                    return;
                }

                impuesto.Porcentaje = porcentaje;

                _objBLLImpuesto.INSERT(impuesto);

                MessageBox.Show("Impuesto guardado correctamente");

                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDatos.CurrentRow == null)
                {
                    MessageBox.Show("Seleccione un registro");
                    return;
                }

                Impuesto impuesto = new Impuesto();

                impuesto.Fecha = Convert.ToDateTime(dgvDatos.CurrentRow.Cells["Fecha"].Value);

                if (!double.TryParse(txtPorcentaje.Text.Replace(",", "."),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double porcentaje))
                {
                    MessageBox.Show("Porcentaje inválido");
                    return;
                }

                impuesto.Porcentaje = porcentaje;

                _objBLLImpuesto.UPDATE(impuesto);

                MessageBox.Show("Impuesto actualizado correctamente");

                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Validar que se haya seleccionado un impuesto
            if (dgvDatos.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Debe seleccionar un registro para poder eliminarlo.",
                    "Registro no seleccionado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                DateTime fecha = Convert.ToDateTime(
                    dgvDatos.SelectedRows[0].Cells["Fecha"].Value);

                // Confirmar eliminación
                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro de que desea eliminar el impuesto seleccionado?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                {
                    return;
                }

                _objBLLImpuesto.DELETE(fecha);

                MessageBox.Show(
                    "Impuesto eliminado correctamente.",
                    "Impuesto eliminado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar el impuesto: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            CargarDesdeGrid();
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarDesdeGrid();
        }

        private void CargarDatos()
        {
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = _objBLLImpuesto.SELECTALL(); ;
        }

        private void LimpiarCampos()
        {

            txtPorcentaje.Clear();
            dtpFechaImpuesto.Value = DateTime.Now;

        }

        private void CargarDesdeGrid()
        {
            if (dgvDatos.CurrentRow == null) return;

            var row = dgvDatos.CurrentRow;

            dtpFechaImpuesto.Value = Convert.ToDateTime(row.Cells["Fecha"].Value);
            txtPorcentaje.Text = row.Cells["Porcentaje"].Value?.ToString();
        }



    }
}
