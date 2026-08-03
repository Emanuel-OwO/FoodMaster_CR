using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.DAL
{
    public class DALFactura : IDALFactura
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");
        public bool Delete(int idFactura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_DELETE_Factura_ByID");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdFactura", idFactura);
                    db.ExecuteNonQuery(command);
                }
                return true;
            }
            catch (Exception er)
            {
                _log.Error("Error DELETE Factura", er);
                throw;
            }
        }

        public List<Factura> GetAll()
        {
            List<Factura> lista = new List<Factura>();

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Factura_All");
                    command.CommandType = CommandType.StoredProcedure;

                    var ds = db.ExecuteReader(command, "Factura");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Factura
                        {
                            NumeroFactura = dr["NumeroFactura"].ToString(),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            Subtotal = Convert.ToDouble(dr["SubTotal"]),
                            MontoImpuesto = Convert.ToDouble(dr["Impuesto"]),
                            TotalColones = Convert.ToDouble(dr["TotalColones"]),
                            TotalDolares = Convert.ToDouble(dr["TotalDolares"]),
                            Estado = Convert.ToBoolean(dr["Estado"])
                        });
                    }
                }
            }
            catch (Exception er) { _log.Error("Error GET_ALL Factura", er); throw; }

            return lista;
        }

        public Factura GetById(int idFactura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("sp_Factura_ObtenerPorId");
                    command.Parameters.AddWithValue("@IdFactura", idFactura);
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Factura");

                    if (ds.Tables[0].Rows.Count == 0) return null;
                    DataRow dr = ds.Tables[0].Rows[0];

                    return new Factura
                    {
                        IdFactura = Convert.ToInt32(dr["IdFactura"]),
                        NumeroFactura = dr["NumeroFactura"].ToString(),
                        Fecha = Convert.ToDateTime(dr["Fecha"]),
                        IdCliente = Convert.ToInt32(dr["IdCliente"]),
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        Subtotal = Convert.ToDouble(dr["SubTotal"]),
                        MontoImpuesto = Convert.ToDouble(dr["Impuesto"]),
                        TotalColones = Convert.ToDouble(dr["TotalColones"]),
                        TotalDolares = Convert.ToDouble(dr["TotalDolares"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }
            }
            catch (Exception er) { _log.Error("Error SELECT_BY_ID Factura", er); throw; }
        }

        public int Insert(Factura factura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_INSERT_Factura");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCliente", factura.IdCliente);
                    command.Parameters.AddWithValue("@IdUsuario", factura.IdUsuario);
                    command.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = (decimal)factura.Subtotal;
                    command.Parameters.Add("@PorcentajeImpuestoAplicado", SqlDbType.Decimal).Value = (decimal)factura.PorcentajeImpuestoAplicado;
                    command.Parameters.Add("@MontoImpuesto", SqlDbType.Decimal).Value = (decimal)factura.MontoImpuesto;
                    command.Parameters.Add("@TipoCambio", SqlDbType.Decimal).Value = (decimal)factura.TipoCambio;
                    command.Parameters.Add("@TotalColones", SqlDbType.Decimal).Value = (decimal)factura.TotalColones;
                    command.Parameters.Add("@TotalDolares", SqlDbType.Decimal).Value = (decimal)factura.TotalDolares;
                    command.Parameters.AddWithValue("@XMLFactura",
                        (object)factura.XMLFactura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FirmaCliente",
                        (object)factura.FirmaCliente ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdTarjeta", factura.IdTarjeta);
                    command.Parameters.AddWithValue("@UltimosDigitosTarjeta", factura.UltimosDigitosTarjeta);
                    command.Parameters.AddWithValue("@NumeroAutorizacion", factura.NumeroAutorizacion);

                    var outNumero = new SqlParameter("@NumeroFactura", SqlDbType.VarChar, 20)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outNumero);

                    var outId = new SqlParameter("@IdFactura", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outId);

                    db.ExecuteNonQuery(command);

                    factura.NumeroFactura = outNumero.Value.ToString();
                    factura.IdFactura = Convert.ToInt32(outId.Value);
                    return factura.IdFactura;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error INSERT Factura", er);
                throw;
            }
        }

        public bool Update(Factura factura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_UPDATE_Factura");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdFactura", factura.IdFactura);
                    command.Parameters.AddWithValue("@NumeroFactura", factura.NumeroFactura);
                    command.Parameters.AddWithValue("@IdCliente", factura.IdCliente);
                    command.Parameters.AddWithValue("@IdUsuario", factura.IdUsuario);
                    command.Parameters.Add("@Subtotal", SqlDbType.Decimal).Value = (decimal)factura.Subtotal;
                    command.Parameters.Add("@PorcentajeImpuestoAplicado", SqlDbType.Decimal).Value = (decimal)factura.PorcentajeImpuestoAplicado;
                    command.Parameters.Add("@MontoImpuesto", SqlDbType.Decimal).Value = (decimal)factura.MontoImpuesto;
                    command.Parameters.Add("@TipoCambio", SqlDbType.Decimal).Value = (decimal)factura.TipoCambio;
                    command.Parameters.Add("@TotalColones", SqlDbType.Decimal).Value = (decimal)factura.TotalColones;
                    command.Parameters.Add("@TotalDolares", SqlDbType.Decimal).Value = (decimal)factura.TotalDolares;
                    command.Parameters.AddWithValue("@XMLFactura",
                        (object)factura.XMLFactura ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FirmaCliente",
                        (object)factura.FirmaCliente ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdTarjeta", factura.IdTarjeta);
                    command.Parameters.AddWithValue("@UltimosDigitosTarjeta", factura.UltimosDigitosTarjeta);
                    command.Parameters.AddWithValue("@NumeroAutorizacion", factura.NumeroAutorizacion);
                    command.Parameters.AddWithValue("@Estado", factura.Estado);

                    db.ExecuteNonQuery(command);
                }
                return true;
            }
            catch (Exception er)
            {
                _log.Error("Error UPDATE Factura", er);
                throw;
            }
        }

        public void UpDateNumFactura(int idFactura, string numeroFactura)
        {

            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var cmd = new SqlCommand("usp_UPDATE_NumeroFactura");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = idFactura;
                    cmd.Parameters.Add("@NumeroFactura", SqlDbType.VarChar, 50).Value = numeroFactura;


                    db.ExecuteNonQuery(cmd);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error UPDATE NumeroFactura", er);
                throw;
            }
        }

        public void UpDateXMLFactura(int idFactura, string xmlFactura)
        {
            using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("usp_UPDATE_XMLFactura"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@IdFactura", SqlDbType.Int).Value = idFactura;
                    cmd.Parameters.Add("@XMLFactura", SqlDbType.Xml).Value = xmlFactura;

                    db.ExecuteNonQuery(cmd); 
                }
            }
        }
    }
}
