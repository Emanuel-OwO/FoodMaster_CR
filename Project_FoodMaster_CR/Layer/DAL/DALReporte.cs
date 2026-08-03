using appFoodMaster_CR.Layer.DTO;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.DAL
{
    public class DALReporte : IDALReporte
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");
        public List<Cliente> GetClientesReporte()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_REPORTE_Clientes");
                    command.CommandType = CommandType.StoredProcedure;

                    DataSet ds = db.ExecuteReader(command, "query");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Cliente cliente = new Cliente
                        {
                            IdCliente = Convert.ToInt32(row["IdCliente"]),
                            Identificacion = row["Identificacion"].ToString(),
                            Nombre = row["Nombre"].ToString(),
                            PrimerApellido = row["PrimerApellido"].ToString(),
                            SegundoApellido = row["SegundoApellido"].ToString(),
                            Telefono = row["Telefono"].ToString(),
                            Correo = row["Correo"].ToString(),
                            Direccion = row["Direccion"].ToString(),
                            IdProvincia = Convert.ToInt32(row["Provincia"].ToString()),
                            Fotografia = row["Fotografia"] != DBNull.Value ? (byte[])row["Fotografia"] : null
                        };

                        lista.Add(cliente);
                    }
                }
            }
            catch (Exception er)
            {
                _log.Error("Error GetClientesReporte", er);
                throw;
            }

            return lista;
        }

        public List<FacturaReporteDTO> GetFacturasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<FacturaReporteDTO> lista = new List<FacturaReporteDTO>();

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_REPORTE_FacturasPorFecha");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

                    DataSet ds = db.ExecuteReader(command, "query");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lista.Add(new FacturaReporteDTO
                        {
                            IdFactura = Convert.ToInt32(row["IdFactura"]),
                            NumeroFactura = row["NumeroFactura"].ToString(),
                            Fecha = Convert.ToDateTime(row["Fecha"]),
                            Cliente = row["Cliente"].ToString(),
                            Usuario = row["Usuario"].ToString(),
                            TipoPago = row["TipoPago"].ToString(),
                            TotalColones = Convert.ToDecimal(row["TotalColones"]),
                            Estado = row["Estado"].ToString()
                        });
                    }
                }
            }
            catch (Exception er)
            {
                _log.Error("Error GetFacturasPorFecha", er);
                throw;
            }

            return lista;
        }

        public List<ProductoVendidoReporteDTO> GetProductosVendidos(int? IdProducto, string Descripcion, int? IdTipoProducto)
        {
            List<ProductoVendidoReporteDTO> lista = new List<ProductoVendidoReporteDTO>();

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_REPORTE_ProductosVendidosFiltrado");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdProducto", (object)IdProducto ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Descripcion", string.IsNullOrWhiteSpace(Descripcion) ? (object)DBNull.Value : Descripcion);
                    command.Parameters.AddWithValue("@IdTipoProducto", (object)IdTipoProducto ?? DBNull.Value);

                    DataSet ds = db.ExecuteReader(command, "query");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        ProductoVendidoReporteDTO item = new ProductoVendidoReporteDTO
                        {
                            IdProducto = Convert.ToInt32(row["IdProducto"]),
                            CodigoInterno = row["CodigoInterno"].ToString(),
                            Descripcion = row["Descripcion"].ToString(),
                            TipoProducto = row["TipoDispositivo"].ToString(),
                            Precio = Convert.ToDecimal(row["Precio"]),
                            CantidadVendida = Convert.ToInt32(row["CantidadVendida"]),
                            Fotografia = row["Fotografia"] != DBNull.Value ? (byte[])row["Fotografia"] : null
                        };

                        lista.Add(item);
                    }
                }
            }
            catch (Exception er)
            {
                _log.Error("Error GetProductosVendidos", er);
                throw;
            }

            return lista;
        }

        public List<VentasPorFechaDTO> GetVentasPorFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<VentasPorFechaDTO> lista = new List<VentasPorFechaDTO>();

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_REPORTE_VentasPorFecha");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FechaInicial", fechaInicial);
                    command.Parameters.AddWithValue("@FechaFinal", fechaFinal);

                    DataSet ds = db.ExecuteReader(command, "query");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        lista.Add(new VentasPorFechaDTO
                        {
                            FechaVenta = Convert.ToDateTime(row["FechaVenta"]),
                            TotalVenta = Convert.ToDecimal(row["TotalVenta"])
                        });
                    }
                }
            }
            catch (Exception er)
            {
                _log.Error("Error GetVentasPorFecha", er);
                throw;
            }

            return lista;
        }
    }
}
