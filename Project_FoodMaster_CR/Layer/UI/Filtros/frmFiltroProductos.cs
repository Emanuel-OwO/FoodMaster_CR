using appFoodMaster_CR.Extensiones;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Filtros
{
    public partial class frmFiltroProductos : Form
    {

        private static readonly ILog _myLogControlEventos =
          log4net.LogManager.GetLogger("MyControlEventos");

        public Producto producto { get; set; }
        
        public frmFiltroProductos()
        {
            InitializeComponent();
        }

        private void frmFiltroProductos_Load(object sender, EventArgs e)
        {
            ConfigurarGridProducto();
            CargarProductos("%%");
        }

        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            IBLLProducto bLLProducto = new BLLProducto();

            try
            {
                string filtro = txtBuscarProducto.Text;
                filtro = "%" + filtro.Replace(' ', '%') + "%";

                dgvDatos.AutoGenerateColumns = true;

                dgvDatos.DataSource = bLLProducto.Get_By_Filter(filtro);


                if (dgvDatos.Columns["Foto"] != null)
                    dgvDatos.Columns["Foto"].Visible = false;
            }
            catch (Exception er)
            {
                string msg = "";
                _myLogControlEventos.ErrorFormat("Error {0}", msg.ToExceptionDetail(er, MethodBase.GetCurrentMethod()));
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigurarGridProducto()
        {
            dgvDatos.AutoGenerateColumns = false;
         dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", DataPropertyName = "IdProducto", HeaderText = "Id", Width = 40 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CodigoInterno", DataPropertyName = "CodigoInterno", HeaderText = "Código", Width = 70 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descripcion", DataPropertyName = "Descripcion", HeaderText = "Descripcion", Width = 130 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Precio", DataPropertyName = "Precio", HeaderText = "Precio", Width = 90 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CantidadStock", DataPropertyName = "CantidadStock", HeaderText = "Stock", Width = 55 });
        }

        private void CargarProductos(string filtro)
        {
            try
            {
                IBLLProducto bLLProducto = new BLLProducto();

                dgvDatos.AutoGenerateColumns = false;
                dgvDatos.Columns.Clear();

                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "IdProducto", DataPropertyName = "IdProducto", HeaderText = "Id", Width = 40 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CodigoInterno", DataPropertyName = "CodigoInterno", HeaderText = "Código", Width = 70 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Descripcion", DataPropertyName = "Descripcion", HeaderText = "Descripcion", Width = 130 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "Precio", DataPropertyName = "Precio", HeaderText = "Precio", Width = 90 });
                dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { Name = "CantidadStock", DataPropertyName = "CantidadStock", HeaderText = "Stock", Width = 55 });

                dgvDatos.DataSource = bLLProducto.Get_By_Filter(filtro);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando productos: " + ex.Message);
            }
        }

        private void dgvDatos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //this.dgvDatos.SelectionMode =   DataGridViewSelectionMode.FullRowSelect;
                if (dgvDatos.RowCount > 0 && dgvDatos.SelectedRows.Count > 0)
                {
                    if (dgvDatos.CurrentCell.Selected)
                    {
                        producto = dgvDatos.SelectedRows[0].DataBoundItem as Producto;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception er)
            {
                string msg = "";
                _myLogControlEventos.ErrorFormat("Error {0}", msg.ToExceptionDetail(er, MethodBase.GetCurrentMethod()));
                MessageBox.Show("Se ha producido el siguiente error: " + er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void dgvDatos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvDatos.CurrentRow != null)
                {
                    producto = dgvDatos.CurrentRow.DataBoundItem as Producto;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }
    }
}
