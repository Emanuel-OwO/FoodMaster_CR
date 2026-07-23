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
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.DAL
{
    public class DALCliente : IDALCliente
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");
      

        public void DELETE(int idCliente)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_DELETE_Cliente_ByID");
                    command.Parameters.AddWithValue("@IdCliente", idCliente);
                    command.CommandType = CommandType.StoredProcedure;
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error DELETE Cliente", er);
                MessageBox.Show(er.Message);
            }
        }

        public List<Cliente> Get_By_Filter(string pDescripcion)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SEARCH_Cliente");
                    command.Parameters.AddWithValue("@Filtro", pDescripcion ?? "");
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Cliente");

                    var lista = new List<Cliente>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Cliente
                        {
                            IdCliente = Convert.ToInt32(dr["IdCliente"]),
                            TipoIdentificacion = dr["TipoIdentificacion"].ToString(),
                            Identificacion = dr["Identificacion"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] == DBNull.Value
                                ? null : dr["SegundoApellido"].ToString(),
                            Sexo = dr["Sexo"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            Correo = dr["Correo"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"])
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error Get_By_Filter Cliente", er);
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public void INSERT(Cliente cliente)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_INSERT_Cliente");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TipoIdentificacion", cliente.TipoIdentificacion);
                    command.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@PrimerApellido", cliente.PrimerApellido);
                    command.Parameters.AddWithValue("@SegundoApellido",
                        (object)cliente.SegundoApellido ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono.Replace("-", ""));
                    command.Parameters.AddWithValue("@Correo", cliente.Correo);
                    command.Parameters.AddWithValue("@IdProvincia", cliente.IdProvincia);
                    command.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    command.Parameters.AddWithValue("@Fotografia",
                        (object)cliente.Fotografia ?? DBNull.Value);

                    var outId = new SqlParameter("@IdCliente", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outId);

                    db.ExecuteNonQuery(command);

                    // Se le asigna el Id generado al mismo objeto que se recibió
                    cliente.IdCliente = Convert.ToInt32(outId.Value);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error INSERT Cliente", er);
                MessageBox.Show(er.Message);
            }
        }

        public List<Cliente> SelectAll()
        {
            try
            {
                DataSet ds = null;

                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Cliente_All");
                    command.CommandType = CommandType.StoredProcedure;

                    ds = db.ExecuteReader(command, "Cliente");
                }

                List<Cliente> lista = new List<Cliente>();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Cliente cliente = new Cliente();

                        cliente.IdCliente = Convert.ToInt32(dr["IdCliente"]);
                        cliente.Identificacion = dr["Identificacion"].ToString();
                        cliente.TipoIdentificacion = dr["TipoIdentificacion"].ToString();
                        cliente.Nombre = dr["Nombre"].ToString();
                        cliente.Sexo = dr["Sexo"].ToString();
                        cliente.Telefono = dr["Telefono"].ToString();
                        cliente.Correo = dr["Correo"].ToString();
                        cliente.Direccion = dr["Direccion"].ToString();
                        cliente.IdProvincia = dr["IdProvincia"] != DBNull.Value ? Convert.ToInt32(dr["IdProvincia"]) : 0;
                        cliente.Estado = Convert.ToBoolean(dr["Estado"]);
                        cliente.PrimerApellido = dr["PrimerApellido"].ToString();
                        cliente.SegundoApellido = dr["SegundoApellido"].ToString();

                        if (dr["Fotografia"] != DBNull.Value)
                            cliente.Fotografia = (byte[])dr["Fotografia"];
                        else
                            cliente.Fotografia = null;

                        lista.Add(cliente);
                    }
                }

                return lista;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public Cliente SelectById(int idCliente)
        {
            try
            {
                DataSet ds = null;

                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Cliente_ByID");

                    command.Parameters.AddWithValue("@IdCliente", idCliente);
                    command.CommandType = CommandType.StoredProcedure;

                    ds = db.ExecuteReader(command, "Cliente");
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    Cliente cliente = new Cliente();

                    cliente.IdCliente = Convert.ToInt32(dt.Rows[0]["IdCliente"]);
                    cliente.Identificacion = dt.Rows[0]["Identificacion"].ToString();
                    cliente.Nombre = dt.Rows[0]["Nombre"].ToString();
                    cliente.Sexo = dt.Rows[0]["Sexo"].ToString();
                    cliente.Telefono = dt.Rows[0]["Telefono"].ToString();
                    cliente.Correo = dt.Rows[0]["Correo"].ToString();
                    cliente.Direccion = dt.Rows[0]["Direccion"].ToString();
                    cliente.IdProvincia = Convert.ToInt32(dt.Rows[0]["Provincia"]);
                    cliente.Estado = Convert.ToBoolean(dt.Rows[0]["Estado"]);
                    cliente.PrimerApellido = dt.Rows[0]["Apellido1"].ToString();
                    cliente.SegundoApellido = dt.Rows[0]["Apellido2"].ToString();

                    if (dt.Rows[0]["Fotografia"] != DBNull.Value)
                        cliente.Fotografia = (byte[])dt.Rows[0]["Fotografia"];
                    else
                        cliente.Fotografia = null;

                    return cliente;
                }

                return null;
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public void UPDATE(Cliente cliente)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_UPDATE_Cliente");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                    command.Parameters.AddWithValue("@TipoIdentificacion", cliente.TipoIdentificacion);
                    command.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@PrimerApellido", cliente.PrimerApellido);
                    command.Parameters.AddWithValue("@SegundoApellido",
                        (object)cliente.SegundoApellido ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono.Replace("-", ""));
                    command.Parameters.AddWithValue("@Correo", cliente.Correo);
                    command.Parameters.AddWithValue("@IdProvincia", cliente.IdProvincia);
                    command.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                    command.Parameters.AddWithValue("@Fotografia",
                        (object)cliente.Fotografia ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Estado", cliente.Estado);

                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error UPDATE Cliente", er);
                MessageBox.Show(er.Message);
            }
        }
    }
}
