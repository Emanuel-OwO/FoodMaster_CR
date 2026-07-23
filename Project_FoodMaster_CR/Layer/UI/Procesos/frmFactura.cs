using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.UI.Filtros;
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

namespace appFoodMaster_CR.Layer.UI.Procesos
{
    public partial class frmFactura : Form
    {

        private string correoClienteSeleccionado = "";
        private readonly BLLFactura _bllFactura = new BLLFactura();
        private readonly BLLFacturaDetalle _bllDetalle = new BLLFacturaDetalle();
        private readonly BLLProducto _bllProducto = new BLLProducto();
        private readonly BLLDolar _bllDolar = new BLLDolar();

        private List<FacturaDetalle> listaDetalle = new List<FacturaDetalle>();

        private int _idClienteSeleccionado = 0;
        private int _idProductoSeleccionado = 0;
        private int _stockProductoSeleccionado = 0;
        private string _codigoProductoSeleccionado = "";

        private byte[] _firmaClienteBytes = null;
        private Producto producto;
        private Cliente clienteSelect;

        public frmFactura()
        {
            InitializeComponent();
        }

        private void frmFactura_Load(object sender, EventArgs e)
        {
            ConfigurarGrid();
            txtUsuario.Text = Properties.Settings.Default.Login + " - " + Properties.Settings.Default.Nombre;
           

        }

        private void ConfigurarGrid()
        {
            dgvDatos.Columns.Clear();
            dgvDatos.AutoGenerateColumns = false;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.ReadOnly = true;
            dgvDatos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDatos.MultiSelect = false;

            dgvDatos.Columns.Add("IdProducto", "IdProducto");
            dgvDatos.Columns["IdProducto"].Visible = false;

            dgvDatos.Columns.Add("NumeroProducto", "Número Producto");
            dgvDatos.Columns.Add("NombreProducto", "Producto");
            dgvDatos.Columns.Add("Precio", "Precio");
            dgvDatos.Columns.Add("Cantidad", "Cantidad");
            dgvDatos.Columns.Add("Subtotal", "Subtotal");
        }

        private void btnNuevaFactura_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            IniciarNuevaFactura();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDatos.CurrentRow == null)
                    throw new Exception("Debe seleccionar una línea del detalle.");

                int index = dgvDatos.CurrentRow.Index;

                if (index < 0 || index >= listaDetalle.Count)
                    throw new Exception("No se pudo identificar la línea seleccionada.");

                DialogResult respuesta = MessageBox.Show(
                    "¿Desea eliminar esta línea del detalle?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (respuesta == DialogResult.Yes)
                {
                    listaDetalle.RemoveAt(index);
                    dgvDatos.Rows.RemoveAt(index);
                    CalcularTotales();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFiltroCliente_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmFiltroClientes frm = new frmFiltroClientes())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Cliente c = frm.cliente;
                        _idClienteSeleccionado = c.IdCliente;
                        correoClienteSeleccionado = c.Correo ?? ""; 

                        txtNombreCliente.Text = c.Nombre + " "
                                              + c.PrimerApellido + " "
                                              + c.SegundoApellido;
                        txtCedula.Text = c.Identificacion;
                        txtCelular.Text = c.Telefono;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar cliente: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFiltroProducto_Click(object sender, EventArgs e)
        {
            try
            {
                using (frmFiltroProductos frm = new frmFiltroProductos())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Producto p = frm.producto;
                        _idProductoSeleccionado = p.IdProducto;
                        _stockProductoSeleccionado = p.CantidadStock;
                        _codigoProductoSeleccionado = p.CodigoInterno;

                        txtProducto.Text = p.Descripcion.ToString();
                        txtPrecio.Text = p.Precio.ToString("N2");
                        txtCantidad.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar producto: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFirmar_Click(object sender, EventArgs e)
        {
            frmFirma frm = new frmFirma();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                _firmaClienteBytes = frm.FirmaBytes;

                using (MemoryStream ms = new MemoryStream(_firmaClienteBytes))
                {
                    picFirma.Image = Image.FromStream(ms);
                }
            }
        }

        private void btnCalcularFactura_Click(object sender, EventArgs e)
        {
            try
            {
                if (_idClienteSeleccionado <= 0)
                    throw new Exception("Debe seleccionar un cliente.");

                if (_idProductoSeleccionado <= 0)
                    throw new Exception("Debe seleccionar un producto.");

                if (string.IsNullOrWhiteSpace(txtCantidad.Text))
                    throw new Exception("Debe indicar la cantidad.");

                int cantidad = Convert.ToInt32(txtCantidad.Text);
                double precio = Convert.ToDouble(txtPrecio.Text.Replace(",", ""));

                if (cantidad <= 0)
                    throw new Exception("La cantidad debe ser mayor a cero.");

                if (cantidad > _stockProductoSeleccionado)
                    throw new Exception($"No hay stock suficiente. Disponible: {_stockProductoSeleccionado}.");

                // Verificar que la cantidad total acumulada no supere el stock
                int yaAgregado = listaDetalle
                    .Where(x => x.IdProducto == _idProductoSeleccionado)
                    .Sum(x => x.Cantidad);

                if ((yaAgregado + cantidad) > _stockProductoSeleccionado)
                    throw new Exception("La cantidad total supera el stock disponible.");

                // Agregar una fila por unidad — igual que SweetTech
                for (int i = 0; i < cantidad; i++)
                {
                    FacturaDetalle detalle = new FacturaDetalle
                    {
                        IdProducto = _idProductoSeleccionado,
                        Cantidad = 1,
                        Precio = precio,
                        Subtotal = precio
                    };
                    listaDetalle.Add(detalle);

                    dgvDatos.Rows.Add(
                        _idProductoSeleccionado,
                        _codigoProductoSeleccionado,
                        txtProducto.Text,
                        precio.ToString("N2"),
                        1,
                        precio.ToString("N2")
                    );
                }

                CalcularTotales();
                LimpiarProducto();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LimpiarProducto()
        {
            _idProductoSeleccionado = 0;
            _stockProductoSeleccionado = 0;
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";
        }

        private void LimpiarFormulario()
        {
            txtNombreCliente.Clear();
            txtCedula.Clear();
            txtCelular.Clear();
            txtProducto.Clear();
            txtPrecio.Clear();
            txtCantidad.Clear();

            txtTotalDolares.Clear();
            txtTotalColones.Clear();
            txtSubTotal.Clear();
            txtTotal.Clear();
            txtDolar.Clear();

            dgvDatos.DataSource = null;

            //_detalleFactura.Clear();
            clienteSelect = null;
            producto = null;

            picFirma.Image = null;
        }
        private void CalcularTotales()
        {
            try
            {
                decimal subtotal = _bllFactura.CalcularSubTotal(listaDetalle);
                decimal impuesto = _bllFactura.CalcularIVA(subtotal);
                decimal totalColones = _bllFactura.CalcularTotalColones(subtotal, impuesto);

                decimal tipoCambio = 1m;
                decimal totalDolares = 0m;

                if (decimal.TryParse(txtDolar.Text.Replace(",", ""), out decimal tc) && tc > 0)
                    tipoCambio = tc;

                if (tipoCambio > 0)
                    totalDolares = _bllFactura.CalcularTotalDolares(totalColones, tipoCambio);

                txtSubTotal.Text = subtotal.ToString("N2");
                txtImpreso.Text = impuesto.ToString("N2");
                txtTotal.Text = totalColones.ToString("N2");
                txtTotalColones.Text = totalColones.ToString("N2");
                txtTotalDolares.Text = totalDolares.ToString("N2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al calcular", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void IniciarNuevaFactura()
        {
            listaDetalle.Clear();
            dgvDatos.Rows.Clear();

            _idClienteSeleccionado = 0;
            _idProductoSeleccionado = 0;
            _stockProductoSeleccionado = 0;
            // Cabecera
            txtNumeroFactura.Text = "Pendiente";
            dtpFecha.Value = DateTime.Now;
            txtUsuario.Text = "";
            txtEstado.Text = "Activa";

            // Cliente
            txtNombreCliente.Text = "";
            txtCedula.Text = "";
            txtCelular.Text = "";

            // Producto a agregar
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtCantidad.Text = "";

            // Totales
            txtSubTotal.Text = "0.00";
            txtImpreso.Text = "0.00";
            txtTotal.Text = "0.00";
            txtTotalColones.Text = "0.00";
            txtTotalDolares.Text = "0.00";

            // Pago
            txtNroTarjeta.Text = "";
            txtTipoTarjerta.Text = "";


            // Firma
            picFirma.Image = null;

        }

        private void txtEstado_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
