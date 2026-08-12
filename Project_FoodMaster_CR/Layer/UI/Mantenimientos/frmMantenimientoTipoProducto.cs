using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Mantenimientos
{
    public partial class frmMantenimientoTipoProducto : Form
    {
        private static readonly ILog _myLogControlEventos = log4net.LogManager.GetLogger("MyControlEventos");

        IBLLTipoProducto _objBLLTipoProducto = new BLLTipoProducto();

        public frmMantenimientoTipoProducto()
        {
            InitializeComponent();
        }

        private void frmMantenimientoTipoProducto_Load(object sender, EventArgs e)
        {
            txtID_Dispositivo.ReadOnly = true;

            CargarDatos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show(
                        "No se puede agregar el tipo de producto porque falta la descripción.\n\n" +
                        "Por favor, ingrese una descripción.",
                        "Datos incompletos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                TipoProducto tipo = new TipoProducto();

                tipo.IdTipoProducto = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdTipoProducto"].Value);
                tipo.Descripcion = txtDescripcion.Text;
                tipo.Estado = rdoActivo.Checked;

                _objBLLTipoProducto.CREATE(tipo);

                MessageBox.Show("TipoProducto guardado correctamente");

                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {

                // Validar que la descripción tenga datos
                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show(
                        "No se puede actualizar el tipo de producto porque falta la descripción.\n\n" +
                        "Por favor, ingrese una descripción.",
                        "Datos incompletos",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                if (dgvDatos.CurrentRow == null)
                {
                    MessageBox.Show("Seleccione un registro");
                    return;
                }

                TipoProducto tipo = new TipoProducto();

                tipo.IdTipoProducto = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdTipoProducto"].Value);
                tipo.Descripcion = txtDescripcion.Text;
                tipo.Estado = rdoActivo.Checked;

                _objBLLTipoProducto.UPDATE(tipo);

                MessageBox.Show("TipoProducto actualizado correctamente");

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

            // Validar que se haya seleccionado un tipo de producto
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
                int id = Convert.ToInt32(
                    dgvDatos.SelectedRows[0].Cells["IdTipoProducto"].Value);

                // Confirmar eliminación
                DialogResult resultado = MessageBox.Show(
                    "¿Está seguro de que desea eliminar el tipo de producto seleccionado?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado != DialogResult.Yes)
                {
                    return;
                }

                _objBLLTipoProducto.DELETE(id);

                MessageBox.Show(
                    "Tipo de producto eliminado correctamente.",
                    "Tipo de producto eliminado",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al eliminar el tipo de producto: " + ex.Message,
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


        private void CargarDatos()
        {
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = _objBLLTipoProducto.SELECTALL(); ;
        }

        private void LimpiarCampos()
        {
            txtID_Dispositivo.Clear();
            txtDescripcion.Clear();

            rdoActivo.Checked = false;
            rdoInactivo.Checked = false;
        }
        private void CargarDesdeGrid()
        {
            if (dgvDatos.CurrentRow == null) return;

            var row = dgvDatos.CurrentRow;

            txtID_Dispositivo.Text = row.Cells["IdTipoProducto"].Value?.ToString();
            txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString() ?? "";

            bool estado = Convert.ToBoolean(row.Cells["Estado"].Value ?? false);
            rdoActivo.Checked = estado;
            rdoInactivo.Checked = !estado;
        }

        private void dgvDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            CargarDesdeGrid();
        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            CargarDesdeGrid();
        }
    }
}
