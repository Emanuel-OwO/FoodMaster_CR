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

namespace appFoodMaster_CR.Layer.UI
{
    public partial class frmLogin : Form
    {

        private int contador = 0;
        private IBLLUsuario _objBLLUsuario = new BLLUsuario();
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                this.AcceptButton = btnAceptar;
                this.CancelButton = btnSalir;
                txtPassword.UseSystemPasswordChar = true;
                txtUsuario.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario: " + ex.Message);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                epError.Clear();

                if (string.IsNullOrWhiteSpace(txtUsuario.Text))
                {
                    epError.SetError(txtUsuario, "Usuario requerido");
                    txtUsuario.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    epError.SetError(txtPassword, "Contraseña requerida");
                    txtPassword.Focus();
                    return;
                }

                Usuario oUsuario = _objBLLUsuario.LOGIN(
                   txtUsuario.Text.Trim(),
                   txtPassword  .Text.Trim());



                if (oUsuario == null)
                {
                    contador++;
                    MessageBox.Show("Error en el acceso", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (contador == 3)
                    {
                        MessageBox.Show("Se equivocó en 3 ocasiones, el sistema se cerrará por seguridad.",
                            "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.DialogResult = DialogResult.Cancel;
                        Application.Exit();
                    }

                    return;
                }

                Properties.Settings.Default.Login = oUsuario.NombreUsuario;
                Properties.Settings.Default.Nombre = oUsuario.Nombre + " " + oUsuario.PrimerApellido;
                Properties.Settings.Default.RolId = oUsuario.IdPerfil.ToString();
                Properties.Settings.Default.Save();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void chkMostrarContrasena_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrarContrasena.Checked)
            {
                txtPassword.UseSystemPasswordChar = false; // mostrar
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true; // ocultar
            }
        }
    }
}
