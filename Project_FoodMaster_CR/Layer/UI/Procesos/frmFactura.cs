using appFoodMaster_CR.Layer.BLL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
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
        private double _precioProductoSeleccionado = 0;
        private double _tipoCambioActual = 0;

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
            IniciarNuevaFactura();
            CargarTipoCambio();
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
        private void btnFacturar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_firmaClienteBytes == null || _firmaClienteBytes.Length == 0)
                    throw new Exception("Debe capturar la firma del cliente antes de facturar.");

                if (_idClienteSeleccionado <= 0)
                    throw new Exception("Debe seleccionar un cliente.");

                if (listaDetalle.Count == 0)
                    throw new Exception("Debe agregar al menos un producto.");

                // Verificar stock real en BD
                foreach (FacturaDetalle item in listaDetalle)
                {
                    Producto p = _bllProducto.SelectById(item.IdProducto);
                    if (p == null)
                        throw new Exception("No se encontró el producto con Id: " + item.IdProducto);
                    if (item.Cantidad > p.CantidadStock)
                        throw new Exception($"Stock insuficiente para '{p.CantidadStock}'. Disponible: {p.CantidadStock}.");
                }

                // Calcular totales
                decimal subTotal = _bllFactura.CalcularSubTotal(listaDetalle);
                decimal impuesto = _bllFactura.CalcularIVA(subTotal);
                decimal totalColones = _bllFactura.CalcularTotalColones(subTotal, impuesto);

                decimal tipoCambio = 530m;
                if (decimal.TryParse(txtDolar.Text.Replace(",", ""), out decimal tc) && tc > 0)
                    tipoCambio = tc;

                decimal totalDolares = _bllFactura.CalcularTotalDolares(totalColones, tipoCambio);

                // Resolver IdTarjeta según el texto ingresado (VISA/MASTERCARD)
                var tarjetas = new BLLTarjeta(new appFoodMaster_CR.Layer.DAL.DALTarjeta()).GetAll();
                var tarjetaMatch = tarjetas.FirstOrDefault(t =>
                    t.Descripcion.Equals(txtTipoTarjerta.Text.Trim(), StringComparison.OrdinalIgnoreCase));

                if (tarjetaMatch == null)
                    throw new Exception("El tipo de tarjeta indicado no es válido (use VISA o MASTERCARD).");

                // Tomar solo los últimos 4 dígitos de la tarjeta
                string ultimosDigitos = txtNroTarjeta.Text.Trim();
                ultimosDigitos = ultimosDigitos.Length >= 4
                    ? ultimosDigitos.Substring(ultimosDigitos.Length - 4)
                    : ultimosDigitos.PadLeft(4, '0');

                // Número de autorización simulado (no hay pasarela de pago real)
                string numeroAutorizacion = "AUT-" + DateTime.Now.ToString("HHmmssfff");

                //MessageBox.Show("ANTES DEL IF: " + _idClienteSeleccionado);
                // Armar cabecera
                Factura factura = new Factura
                {
                    Fecha = dtpFecha.Value,
                    IdCliente = _idClienteSeleccionado,
                    IdUsuario = Convert.ToInt32(Properties.Settings.Default.IdUsuario),
                    Subtotal = (double)subTotal,
                    PorcentajeImpuestoAplicado = 13.0,      // ver nota abajo
                    MontoImpuesto = (double)impuesto,
                    TipoCambio = (double)tipoCambio,
                    TotalColones = (double)totalColones,
                    TotalDolares = (double)totalDolares,
                    FirmaCliente = _firmaClienteBytes,
                    IdTarjeta = tarjetaMatch.IdTarjeta,
                    UltimosDigitosTarjeta = ultimosDigitos,
                    NumeroAutorizacion = numeroAutorizacion,
                    Estado = true
                };

                // Guardar cabecera — el SP genera el número y lo retorna
                int idFactura = _bllFactura.Save(factura);
                string numeroFinal = factura.NumeroFactura;
                factura.IdFactura = idFactura;   // ← necesario para el XML

                // Guardar detalles y rebajar stock
                foreach (FacturaDetalle item in listaDetalle)
                {
                    item.IdFactura = idFactura;
                    _bllDetalle.Save(item);

                    Producto prod = _bllProducto.SelectById(item.IdProducto);
                    prod.CantidadStock -= item.Cantidad;
                    _bllProducto.UPDATE(prod);
                }

                // Actualizar pantalla
                txtNumeroFactura.Text = numeroFinal;
                txtEstado.Text = "Guardada";

                // Generar y guardar XML
                string xmlGenerado = appFoodMaster_CR.Utilitarios.Util.FacturaXmlHelper.GenerarXml(
                     factura,
                     listaDetalle,
                     txtNombreCliente.Text.Trim(),
                     txtTipoTarjerta.Text.Trim()

                );

                string carpetaXml = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacturasXML");
                if (!System.IO.Directory.Exists(carpetaXml))
                    System.IO.Directory.CreateDirectory(carpetaXml);

                string rutaXml = System.IO.Path.Combine(carpetaXml, numeroFinal + ".xml");
                System.IO.File.WriteAllText(rutaXml, xmlGenerado, System.Text.Encoding.UTF8);
                _bllFactura.UpdateXMLFactura(idFactura, xmlGenerado);

                // Generar PDF con QR
                string contenidoQR =
                    "FoodMaster_CR\n\n" +
                    "Factura: " + numeroFinal + "\n" +
                    "Cliente: " + txtNombreCliente.Text + "\n" +
                    "Cédula: " + txtCedula.Text + "\n" +
                    "Fecha: " + dtpFecha.Value.ToString("dd/MM/yyyy") + "\n" +
                    "Total CRC: " + txtTotalColones.Text + "\n" +
                    "Pago: " + txtTipoTarjerta.Text;

                System.Drawing.Image qrImage = QuickResponse.QuickResponseGenerador(contenidoQR, 10);

                string rutaPdf = appFoodMaster_CR.Layer.Utilitarios.FacturaPdfService.GenerarPdfFactura(
                    factura,
                    listaDetalle,
                    txtNombreCliente.Text.Trim(),
                    txtCedula.Text.Trim(),
                    txtUsuario.Text.Trim(),
                    txtTipoTarjerta.Text.Trim(),
                    _firmaClienteBytes,
                    qrImage
                );

                // Enviar correo
                if (!string.IsNullOrWhiteSpace(correoClienteSeleccionado))
                {
                    try
                    {
                        appFoodMaster_CR.Utilitarios.EnviarCorreo correo = new appFoodMaster_CR.Utilitarios.EnviarCorreo();

                        string asunto = "Factura " + numeroFinal + " - FoodMaster_CR";
                        string body = "<h2>FoodMaster_CR</h2>" +
                                        "<p>Estimado cliente,</p>" +
                                        "<p>Adjuntamos su factura en formato PDF y XML.</p>" +
                                        "<p><b>Número de factura:</b> " + numeroFinal + "</p>" +
                                        "<p>Gracias por su compra.</p>";

                        correo.enviarCorreoGmail(body, correoClienteSeleccionado, asunto,
                            new List<string> { rutaPdf, rutaXml });
                    }
                    catch (Exception exCorreo)
                    {
                        MessageBox.Show(
                            "La factura se guardó, pero no se pudo enviar el correo:\n" + exCorreo.Message,
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("La factura se generó, pero el cliente no tiene correo registrado.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show("Factura guardada correctamente. Número: " + numeroFinal,
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                IniciarNuevaFactura();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al facturar: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // FIX: se usa el double guardado al cargar el tipo de cambio,
                // NO se vuelve a parsear txtDolar.Text (mismo bug que el precio).
                decimal tipoCambio = (decimal)_tipoCambioActual;
                decimal totalDolares = 0m;

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
            //txtNumeroFactura.Text = GenerarNumeroFactura();
            txtNumeroFactura.ReadOnly = true;
            txtNumeroFactura.ReadOnly = true;
            dtpFecha.Value = DateTime.Now;
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
        //private string GenerarNumeroFactura()
        //{
        //    return "FAC-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        //}
        private void CargarTipoCambio()
        {
            try
            {
                _tipoCambioActual = _bllDolar.GetVentaDolar();
                txtDolar.Text = _tipoCambioActual.ToString("N2");
            }
            catch (Exception ex)
            {
                _tipoCambioActual = 0;
                txtDolar.Text = "";
                MessageBox.Show(
                    "No se pudo obtener el tipo de cambio del dólar. " +
                    "Verifique su conexión a internet.\n\n" + ex.Message,
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtEstado_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
