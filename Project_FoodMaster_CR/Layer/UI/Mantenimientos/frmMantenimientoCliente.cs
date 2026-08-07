using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using log4net;
using Project_CatTech.Layer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Mantenimientos
{
    public partial class frmMantenimientoCliente : Form
    {
        private static readonly ILog _myLogControlEventos = log4net.LogManager.GetLogger("MyControlEventos");
        private ErrorProvider oErrorProvider = new ErrorProvider();
        BLLCliente clienteBLL = new BLLCliente();
        ErrorProvider erp = new ErrorProvider();

        public frmMantenimientoCliente()
        {
            InitializeComponent();
        }

        private void frmMantenimientoCliente_Load(object sender, EventArgs e)
        {

            CargarProvincias();
            txtIdentificacion.TextChanged += txtTipoID_TextChanged;
            CargarUsuarios();

            rdoActivo.Checked = true;
            rdoMasculino.Checked = true;
        }


        private void CargarUsuarios()
        {
            dgvDatos.DataSource = null;
            dgvDatos.DataSource = clienteBLL.SELECTALL()
            .OrderBy(x => x.IdCliente)
            .ToList();
            dgvDatos.Columns["Fotografia"].Visible = false;
        }
        private void CargarProvincias()
        {
            try
            {
                cmbProvincia.DataSource = null;

                var lista = new DALProvincia().GetProvinciasFromInternet();

                if (lista != null && lista.Count > 0)
                {
                    cmbProvincia.DataSource = lista;
                    cmbProvincia.DisplayMember = "Descripcion";
                    cmbProvincia.ValueMember = "IdProvincia";

                    cmbProvincia.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando provincias: " + ex.Message);
            }
        }
        private void DetectarTipoID()
        {

            string cedula = txtIdentificacion.Text.Trim().Replace("-", "");

            if (cedula.Length == 9 && cedula.All(char.IsDigit))
                txtTipoID.Text = "Nacional";
            else if ((cedula.Length == 11 || cedula.Length == 12) && cedula.All(char.IsDigit))
                txtTipoID.Text = "Extranjero";
            else if (cedula.Any(char.IsLetter))
                txtTipoID.Text = "Pasaporte";
            else
                txtTipoID.Text = "";
        }


        private void btnConsultar_Click(object sender, EventArgs e)
        {
            IBLLPadron bLLPadron = new BLLPadron();
            try
            {
                erp.Clear();

                if (string.IsNullOrEmpty(txtIdentificacion.Text))
                {
                    erp.SetError(txtIdentificacion, "Id Requerido");
                    txtIdentificacion.Focus();
                    return;
                }

                if (txtIdentificacion.Text.Trim().Length != 9)
                {
                    erp.SetError(txtIdentificacion, "Largo de la Cédula 9 digitos");
                    txtIdentificacion.Focus();
                    return;
                }


                // Se hace la validación que solo permita números en la cédula 

                PadronElectoral oPadronDTO = bLLPadron.GetById(txtIdentificacion.Text.Trim());


                string[] array = oPadronDTO.nombre.Split(' ');

                // 1 nombres y dos apellidos
                if (array.Length == 3)
                {
                    txtNombre.Text = array[0];
                    txtPrimerApellido.Text = array[1];
                    txtSegundoApellido.Text = array[2];
                }

                // 2 nombres y dos apellidos
                if (array.Length == 4)
                {
                    txtNombre.Text = array[0] + " " + array[1];
                    txtPrimerApellido.Text = array[2];
                    txtSegundoApellido.Text = array[3];
                }

                // Ejemplo con varios nombres. 203960070 - ANTONIO MARIA DE LA TRINIDAD RODRIGUEZ CHAVES 
                // 2 nombres y dos apellidos
                // Nota: No se valida apellidos compuestos por ejemplo Maria de la O
                if (array.Length > 4)
                {
                    txtNombre.Text = array[0] + " " + array[1];
                    txtPrimerApellido.Text = array[array.Length - 2];
                    txtSegundoApellido.Text = array[array.Length - 1];
                }


            }
            catch (Exception er)
            {
                _myLogControlEventos.ErrorFormat("Error en {0}: {1}", MethodBase.GetCurrentMethod().Name, er.ToString());

                MessageBox.Show(
                    $"Se ha producido el siguiente error:\n{er.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void CargarDatos()
        {
            if (dgvDatos.CurrentRow == null) return;

            var row = dgvDatos.CurrentRow;

            txtIdentificacion.Text = row.Cells["Identificacion"].Value?.ToString() ?? "";
            txtTipoID.Text = row.Cells["TipoIdentificacion"].Value?.ToString() ?? "";
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
            txtPrimerApellido.Text = row.Cells["PrimerApellido"].Value?.ToString() ?? "";
            txtSegundoApellido.Text = row.Cells["SegundoApellido"].Value?.ToString() ?? "";


            if (row.Cells["IdProvincia"].Value != null)
            {
                cmbProvincia.SelectedValue = row.Cells["IdProvincia"].Value;
            }

            mskTelefono.Text = row.Cells["Telefono"].Value?.ToString() ?? "";
            txtCorreo.Text = row.Cells["Correo"].Value?.ToString() ?? "";
            txtDescripcion.Text = row.Cells["Direccion"].Value?.ToString() ?? "";


            if (row.Cells["Fotografia"].Value != null)
            {
                byte[] fotoBytes = row.Cells["Fotografia"].Value as byte[];

                if (fotoBytes != null && fotoBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(fotoBytes))
                    {
                        pctFoto.Image = Image.FromStream(ms);
                        pctFoto.SizeMode = PictureBoxSizeMode.StretchImage;
                        pctFoto.Tag = fotoBytes;
                    }
                }
            }
            else
            {
                pctFoto.Image = null;
                pctFoto.Tag = null;
            }


            bool estado = Convert.ToBoolean(row.Cells["Estado"].Value ?? false);
            rdoActivo.Checked = estado;
            rdoInactivo.Checked = !estado;


            string sexo = row.Cells["Sexo"].Value?.ToString() ?? "";
            rdoMasculino.Checked = sexo == "M";
            rdoFemenino.Checked = sexo == "F";
        }

        private void txtTipoID_TextChanged(object sender, EventArgs e)
        {
            DetectarTipoID();
        }

        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                Cliente cliente = new Cliente();

                cliente.IdCliente = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdCliente"].Value);


                cliente.TipoIdentificacion = txtTipoID.Text;
                cliente.Identificacion = txtIdentificacion.Text;
                cliente.Nombre = txtNombre.Text;
                cliente.PrimerApellido = txtPrimerApellido.Text;
                cliente.SegundoApellido = txtSegundoApellido.Text;
                cliente.IdProvincia = Convert.ToInt32(cmbProvincia.SelectedValue);
                cliente.Telefono = mskTelefono.Text.Trim().Replace("-", "");
                cliente.Correo = txtCorreo.Text.Trim();
                cliente.Direccion = txtDescripcion.Text;
                cliente.Fotografia = (byte[])pctFoto.Tag;
                cliente.Estado = rdoActivo.Checked;

                if (rdoMasculino.Checked)
                    cliente.Sexo = "M";
                else if (rdoFemenino.Checked)
                    cliente.Sexo = "F";
                else
                {
                    MessageBox.Show("Debe seleccionar el sexo");
                    return;
                }

                BLLCliente oClienteBLL = new BLLCliente();
                oClienteBLL.UPDATE(cliente);

                MessageBox.Show("Cliente actualizado correctamente");
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Boton Agregar
            Cliente cliente = new Cliente();

            cliente.Identificacion = txtIdentificacion.Text;
            cliente.TipoIdentificacion = txtTipoID.Text;
            cliente.Nombre = txtNombre.Text;
            cliente.PrimerApellido = txtPrimerApellido.Text;
            cliente.SegundoApellido = txtSegundoApellido.Text;
            cliente.IdProvincia = Convert.ToInt32(cmbProvincia.SelectedValue);
            cliente.Telefono = mskTelefono.Text.Trim().Replace("-", "");
            cliente.Correo = txtCorreo.Text;
            cliente.Direccion = txtDescripcion.Text;
            cliente.Fotografia = (byte[])pctFoto.Tag;
            cliente.Estado = rdoActivo.Checked;

            if (rdoMasculino.Checked)
                cliente.Sexo = "M";
            else if (rdoFemenino.Checked)
                cliente.Sexo = "F";
            else
            {
                MessageBox.Show("Debe seleccionar el sexo");
                return;
            }

            BLLCliente oClienteBLL = new BLLCliente();
            oClienteBLL.INSERT(cliente);

            MessageBox.Show("Cliente agregado correctamente");

            CargarUsuarios();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvDatos.CurrentRow.Cells["IdCliente"].Value);

                BLLCliente clienteBLL = new BLLCliente();
                clienteBLL.DELETE(id);

                MessageBox.Show("Cliente eliminado correctamente");

                CargarUsuarios();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente");
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
        private void LimpiarCampos()
        {
            txtIdentificacion.Clear();
            txtTipoID.Clear();
            txtNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            txtCorreo.Clear();
            txtDescripcion.Clear();

            mskTelefono.Clear();

            dtpFechaNacimiento.Value = DateTime.Now;


            pctFoto.Image = null;
            pctFoto.Tag = null;


            rdoActivo.Checked = true;
            rdoInactivo.Checked = false;


            rdoMasculino.Checked = true;
            rdoFemenino.Checked = false;

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
                this.oErrorProvider.SetError(this.pctFoto, msg.ToString());
            }
        }
    }
}
