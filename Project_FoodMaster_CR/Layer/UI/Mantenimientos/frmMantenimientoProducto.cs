using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Mantenimientos
{
    public partial class frmMantenimientoProducto : Form
    {

        private static readonly ILog _myLogControlEventos = log4net.LogManager.GetLogger("MyControlEventos");
        private ErrorProvider _objErrorProvider = new ErrorProvider();
        private IBLLProducto _ojbBLLProducto = new BLLProducto();
        private IBLLTipoProducto _objBLLTipoProducto = new BLLTipoProducto();

        public frmMantenimientoProducto()
        {
            InitializeComponent();
        }

        private void frmMantenimientoProducto_Load(object sender, EventArgs e)
        {
            CargarDatos();
            CargarCombos();
            CargarDesdeGrid();
            rdoActivo.Enabled = true;
        }



      

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                Producto producto = new Producto();
                producto.CodigoInterno = txtCodigoInterno.Text;
                producto.CodigoBarras = txtCodigoBarras.Text;
                producto.Descripcion = txtCaracteristicas.Text;

                producto.IdTipoProducto = Convert.ToInt32(cmbTipoProducto.SelectedValue);

                producto.Peso = Convert.ToDouble(txtPeso.Text);
                producto.ContenidoCalorico = Convert.ToInt32(txtCalorias.Text);

                producto.Precio = Convert.ToDouble(txtPrecio.Text);
                //Hice la variable stock para evitar errores si escriben letras
                int stock = 0;
                int.TryParse(txtStock.Text, out stock);
                producto.CantidadStock = stock;

                producto.Estado = rdoActivo.Checked;

                producto.Foto = pctFoto.Tag != null ? (byte[])pctFoto.Tag : null;
                producto.DocumentoEspecificaciones = pctDocumento.Tag != null ? (byte[])pctDocumento.Tag : null;

                _ojbBLLProducto.CREATE(producto);

                MessageBox.Show("Producto guardado correctamente");

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
                    MessageBox.Show("Seleccione un producto");
                    return;
                }

                Producto producto = new Producto();

                producto.IdProducto = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdProducto"].Value);

                producto.CodigoInterno = txtCodigoInterno.Text;
                producto.CodigoBarras = txtCodigoBarras.Text;
                producto.Descripcion = txtCaracteristicas.Text;

                producto.IdTipoProducto = Convert.ToInt32(cmbTipoProducto.SelectedValue);

                producto.Peso = Convert.ToDouble(txtPeso.Text);
                producto.ContenidoCalorico = Convert.ToInt32(txtCalorias.Text);

                producto.Precio = Convert.ToDouble(txtPrecio.Text);
                producto.CantidadStock = Convert.ToInt32(txtStock.Text);
                producto.Estado = rdoActivo.Checked;

                producto.Foto = pctFoto.Tag != null ? (byte[])pctFoto.Tag : null;
                producto.DocumentoEspecificaciones = pctDocumento.Tag != null ? (byte[])pctDocumento.Tag : null;

                _ojbBLLProducto.UPDATE(producto);

                MessageBox.Show("Producto actualizado correctamente");

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
            if (dgvDatos.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdProducto"].Value);

                _ojbBLLProducto.DELETE(id);

                MessageBox.Show("Producto eliminado correctamente");

                CargarDatos();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un producto");
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
            dgvDatos.DataSource = _ojbBLLProducto.SelectAll();

            if (dgvDatos.Columns["Foto"] != null)
                dgvDatos.Columns["Foto"].Visible = false;

            if (dgvDatos.Columns["DocumentoEspecificaciones"] != null)
                dgvDatos.Columns["DocumentoEspecificaciones"].Visible = false;


        }

        private void LimpiarCampos()
        {
            txtCodigoInterno.Clear();
            txtCodigoBarras.Clear();
            txtCaracteristicas.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            txtStock.Text = "0";
            txtCalorias.Clear();
            txtPeso.Clear();

          

            rdoActivo.Checked = true;
            rdoInactivo.Checked = false;

            pctFoto.Image = null;
            pctFoto.Tag = null;

            pctDocumento.Image = null;
            pctDocumento.Tag = null;
        }

        private void CargarDesdeGrid()
        {
            if (dgvDatos.CurrentRow == null) return;

            var row = dgvDatos.CurrentRow;

            txtCodigoInterno.Text = row.Cells["CodigoInterno"].Value?.ToString();
            txtCodigoBarras.Text = row.Cells["CodigoBarras"].Value?.ToString();
            txtCaracteristicas.Text = row.Cells["Descripcion"].Value?.ToString();
            txtPrecio.Text = row.Cells["Precio"].Value?.ToString();
            txtStock.Text = row.Cells["CantidadStock"].Value?.ToString();
            txtPeso.Text = row.Cells["Peso"].Value?.ToString();
            txtCalorias.Text = row.Cells["ContenidoCalorico"].Value?.ToString();

            cmbTipoProducto.SelectedValue = row.Cells["IdTipoProducto"].Value;

            bool estado = Convert.ToBoolean(row.Cells["Estado"].Value ?? false);
            rdoActivo.Checked = estado;
            rdoInactivo.Checked = !estado;

            try
            {
                int idProducto = Convert.ToInt32(row.Cells["IdProducto"].Value);
                Producto p = _ojbBLLProducto.SelectById(idProducto);

                // Mostrar Foto
                if (p.Foto != null && p.Foto.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(p.Foto))
                    {
                        pctFoto.Image = Image.FromStream(ms);
                        pctFoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        pctFoto.Tag = p.Foto;
                    }
                }
                else
                {
                    pctFoto.Image = null;
                    pctFoto.Tag = null;
                }

                // Mostrar Documento (icono Word si tiene archivo)
                if (p.DocumentoEspecificaciones != null && p.DocumentoEspecificaciones.Length > 0)
                {
                    //   pctDocumento.Image = Project_CatTech.Properties.Resources.MSWordAcepted;
                    pctDocumento.Tag = p.DocumentoEspecificaciones;
                }
                else
                {
                    pctDocumento.Image = null;
                    pctDocumento.Tag = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando foto/documento: " + ex.Message);
            }
        }

        private void CargarCombos()
        {

            cmbTipoProducto.DataSource = null;
            cmbTipoProducto.DisplayMember = "Descripcion";
            cmbTipoProducto.ValueMember = "IdTipoProducto";
            cmbTipoProducto.DataSource = _objBLLTipoProducto.SELECTALL();//.Select(p => new { p.IdTipoProducto, p.Descripcion }).Distinct().ToList();

            cmbTipoProducto.SelectedIndex = -1;
        }

        private void pctFoto_Click(object sender, EventArgs e)
        {
            try
            {
                // Mostrar el Dialogo de archivos
                OpenFileDialog opt = new OpenFileDialog();
                // Parametros del dialogo
                opt.Title = "Seleccione la imagen";
                opt.SupportMultiDottedExtensions = true;
                opt.DefaultExt = "*.jpg";
                opt.Filter = "Archivos de Imagenes (*.jpg)|*.jpg| All files (*.*)|*.*";
                opt.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                opt.FileName = "";

                if (opt.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        this.pctFoto.ImageLocation = opt.FileName;
                        this.pctFoto.SizeMode = PictureBoxSizeMode.StretchImage;

                        byte[] cadenaBytes = File.ReadAllBytes(opt.FileName);

                        this.pctFoto.Tag = (byte[])cadenaBytes;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:  " + ex.Message);
                    }
                }
            }
            catch (Exception er)
            {
                StringBuilder msg = new StringBuilder();
                msg.AppendFormat("Message        {0}\n", er.Message);
                msg.AppendFormat("Source         {0}\n", er.Source);
                msg.AppendFormat("InnerException {0}\n", er.InnerException);
                msg.AppendFormat("StackTrace     {0}\n", er.StackTrace);
                msg.AppendFormat("TargetSite     {0}\n", er.TargetSite);
                this._objErrorProvider.SetError(this.pctFoto, msg.ToString());
            }
        }

        private void pctDocumento_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opt = new OpenFileDialog();
                opt.Title = "Seleccione el Documento";
                opt.SupportMultiDottedExtensions = true;
                opt.DefaultExt = "*.docx";
                opt.Filter = "Archivos (*.docx;*.pdf)|*.docx;*.pdf|All files (*.*)|*.*";
                opt.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                opt.FileName = "";

                if (opt.ShowDialog(this) == DialogResult.OK)
                {
                    byte[] cadenaBytes = File.ReadAllBytes(opt.FileName);
                    pctDocumento.Tag = cadenaBytes;

                    // aquí se  puede poner una imagen de recurso 
                    // pbDocumentacion.Image = Properties.Resources.MSWordAcepted;

                    pctDocumento.BackColor = Color.LightGray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se ha producido el siguiente error: " + ex.Message);
            }
        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                CargarDesdeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar producto: " + ex.Message);
            }
        }
    }
}
