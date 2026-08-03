using appFoodMaster_CR.Layer.UI.Mantenimientos;
using appFoodMaster_CR.Layer.UI.Procesos;
using appFoodMaster_CR.Layer.UI.Reportes;
using appFoodMaster_CR.Layer.UI.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.UI.Login
{
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            try
            {
                // MessageBox.Show("Rol recibido: " + appFoodMaster_CR.Properties.Settings.Default.RolId);
                CargarStatusStrip();
                Seguridad();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el formulario principal: " + ex.Message);
            }
        }
        private void Seguridad()
        {
            List<string> menus = new List<string>();

            // Desactiva todos los menús principales
            foreach (ToolStripItem opcionMenu in this.menuStrip1.Items)
            {

                opcionMenu.Enabled = false;
            }

            // Menús que siempre estarán habilitados para todos
            menus.Add("toolStripMenuItemAcercaEmpresa");
            menus.Add("toolStripMenuItemSalir");
            menus.Add("toolStripMenuCambiarUsuario");
            menus.Add("toolStripMenuManualUsuario");

            string rol = appFoodMaster_CR.Properties.Settings.Default.RolId.Trim();

            // ADMINISTRADOR
            if (rol == "1")
            {
                menus.Add("toolStripMenuItemMantenimientos");
                menus.Add("toolStripMenuItemProcesos");
                menus.Add("toolStripMenuItemReportes");
                menus.Add("toolStripMenuItemAdministracion");
            }
            // VENDEDOR
            else if (rol == "2")
            {
                menus.Add("toolStripMenuItemMantenimientos");
                menus.Add("toolStripMenuItemProcesos");
                menus.Add("toolStripMenuItemReportes");
            }
            // REPORTES
            else if (rol == "3")
            {
                menus.Add("toolStripMenuItemReportes");
            }

            // Habilitar menús permitidos
            foreach (ToolStripItem opcionMenu in this.menuStrip1.Items)
            {
                if (menus.Contains(opcionMenu.Name))
                {
                    opcionMenu.Enabled = true;
                }
            }
        }

        private void CargarStatusStrip()
        {
            try
            {
                string usuario = appFoodMaster_CR.Properties.Settings.Default.Login;
                string rolId = appFoodMaster_CR.Properties.Settings.Default.RolId;
                string nombreRol = "";

                if (rolId == "1")
                    nombreRol = "Administrador";
                else if (rolId == "2")
                    nombreRol = "Vendedor";
                else if (rolId == "3")
                    nombreRol = "Reportes";
                else
                    nombreRol = "Sin rol";

                toolStripStatusEstado.Text = "Usuario: " + usuario;
                toolStripStatusRol.Text = "Rol: " + nombreRol;
                toolStripStatusFecha.Text = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la barra de estado: " + ex.Message);
            }
        }

        private void toolStripMenuItemMantenimientoCliente_Click(object sender, EventArgs e)
        {
            frmMantenimientoCliente frmMantenimientoCliente = new frmMantenimientoCliente();
            frmMantenimientoCliente.MdiParent = this;
            frmMantenimientoCliente.Show();
        }

        private void toolStripMenuItemMantenimientoProducto_Click(object sender, EventArgs e)
        {
            frmMantenimientoProducto frmMantenimientoProducto = new frmMantenimientoProducto();
            frmMantenimientoProducto.MdiParent = this;
            frmMantenimientoProducto.Show();
        }

        private void toolStripMenuItemMantenimientoTipoProducto_Click(object sender, EventArgs e)
        {
            frmMantenimientoTipoProducto frmMantenimientoTipoProducto = new frmMantenimientoTipoProducto();
            frmMantenimientoTipoProducto.MdiParent = this;
            frmMantenimientoTipoProducto.Show();
        }

        private void toolStripMenuItemMantenimientoImpuesto_Click(object sender, EventArgs e)
        {
            frmMantenimientoImpuesto frmMantenimientoImpuesto = new frmMantenimientoImpuesto();
            frmMantenimientoImpuesto.MdiParent = this;
            frmMantenimientoImpuesto.Show();
        }

        private void toolStripMenuItemFactura_Click(object sender, EventArgs e)
        {

            frmFactura frmFactura = new frmFactura();
            frmFactura.MdiParent = this;
            frmFactura.Show();
        }

        private void toolStripMenuCambiarUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                // Cerrar todas las ventanas hijas abiertas
                foreach (Form child in this.MdiChildren)
                {
                    child.Close();
                }

                // Deshabilitar todos los menús principales
                foreach (ToolStripItem opcionMenu in this.menuStrip1.Items)
                {
                    opcionMenu.Enabled = false;
                }

                // Dejar activos solo los que siempre deben verse
                List<string> menus = new List<string>();
                menus.Add("acercaDeToolStripMenuItem");
                menus.Add("salirToolStripMenuItem");
                menus.Add("cambiarUsuarioToolStripMenuItem");
                menus.Add("manualDelUsuarioToolStripMenuItem");

                foreach (ToolStripItem opcionMenu in this.menuStrip1.Items)
                {
                    if (menus.Contains(opcionMenu.Name))
                    {
                        opcionMenu.Enabled = true;
                    }
                }

                // Limpiar sesión actual
                appFoodMaster_CR.Properties.Settings.Default.Login = string.Empty;
                appFoodMaster_CR.Properties.Settings.Default.Nombre = string.Empty;
                appFoodMaster_CR.Properties.Settings.Default.RolId = string.Empty;
                appFoodMaster_CR.Properties.Settings.Default.Save();

                // Volver a mostrar login
                using (frmLogin ofrmLogin = new frmLogin())
                {
                    ofrmLogin.ShowDialog();

                    if (ofrmLogin.DialogResult == DialogResult.OK)
                    {
                        Seguridad();
                        CargarStatusStrip();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Se produjo un error al cambiar de usuario: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToolStripMenuItemUsuarios_Click(object sender, EventArgs e)
        {
            frmUsuario frmUsuario = new frmUsuario();
            frmUsuario.MdiParent = this;
            frmUsuario.Show();
        }

        private void toolStripMenuReporteFactura_Click(object sender, EventArgs e)
        {
            frmReportesFacturas frmReportesFacturas = new frmReportesFacturas();
            frmReportesFacturas.MdiParent = this;
            frmReportesFacturas.Show();
        }

        private void toolStripMenuReporteCliente_Click(object sender, EventArgs e)
        {
            frmReporteClientes frmReporteClientes = new frmReporteClientes();
            frmReporteClientes.MdiParent = this;
            frmReporteClientes.Show();
        }

        private void toolStripMenuReporteProductos_Click(object sender, EventArgs e)
        {
            frmReporteProductos frmReporteProductos = new frmReporteProductos();
            frmReporteProductos.MdiParent = this;
            frmReporteProductos.Show();
        }

        private void graficoToolStripMenuItoolStripMenuGrafico_Click(object sender, EventArgs e)
        {
            frmReporteGrafico frmReporteGrafico = new frmReporteGrafico();
            frmReporteGrafico.MdiParent = this;
            frmReporteGrafico.Show();
        }

        private void toolStripMenuItemSalir_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show(
        "¿Realmente desea salir?",
        "Confirmación",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void toolStripMenuItemAcercaEmpresa_Click(object sender, EventArgs e)
        {
             AboutEmpresa aboutEmpresa = new AboutEmpresa();
            aboutEmpresa.MdiParent = this;
            aboutEmpresa.Show();
        }
    }
}
