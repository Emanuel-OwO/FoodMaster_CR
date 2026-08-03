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
    public class DALUsuario : IDALUsuario
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");
        public void DELETE(int idUsuario)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_DELETE_Usuario_ByID");
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    command.CommandType = CommandType.StoredProcedure;
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error DELETE Usuario", er);
                MessageBox.Show(er.Message);
            }
        }

        public void INSERT(Usuario usuario)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_INSERT_Usuario");
                    command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Clave", usuario.Clave);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@PrimerApellido", usuario.PrimerApellido);
                    command.Parameters.AddWithValue("@SegundoApellido",
                        (object)usuario.SegundoApellido ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdPerfil", usuario.IdPerfil);
                    command.CommandType = CommandType.StoredProcedure;
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error CREATE Usuario", er);
                MessageBox.Show(er.Message);
            }
        }

        public Usuario LOGIN(string nombreUsuario, string clave)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_LOGIN_Usuario");
                    command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    command.Parameters.AddWithValue("@Clave", clave);
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Usuario");

                    if (ds.Tables[0].Rows.Count == 0) return null;
                    DataRow dr = ds.Tables[0].Rows[0];
                    return new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        PrimerApellido = dr["PrimerApellido"].ToString(),
                        SegundoApellido = dr["SegundoApellido"] == DBNull.Value
                            ? null : dr["SegundoApellido"].ToString(),
                        IdPerfil = Convert.ToInt32(dr["IdPerfil"]),
                        Estado = true   
                    };
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public List<Usuario> SELECT_ALL()
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Usuario_All");
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Usuario");

                    var lista = new List<Usuario>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreUsuario = dr["NombreUsuario"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            PrimerApellido = dr["PrimerApellido"].ToString(),
                            SegundoApellido = dr["SegundoApellido"] == DBNull.Value
                                ? null : dr["SegundoApellido"].ToString(),
                            IdPerfil = Convert.ToInt32(dr["IdPerfil"]),
                            Estado = Convert.ToBoolean(dr["Estado"])
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error SelectAll Usuario", er);
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public DataTable SELECT_ALL_PERFILES()
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Perfil_All");
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Perfiles");
                    return ds.Tables[0];
                }
            }
            catch (Exception er) 
            { _log.Error("Error SELECT_ALL_PERFILES", er); throw; 

            }
        }

        public Usuario SELECT_BY_ID(int idUsuario)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Usuario_ByID");
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Usuario");

                    if (ds.Tables[0].Rows.Count == 0) return null;
                    DataRow dr = ds.Tables[0].Rows[0];
                    return new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        Clave = dr["Clave"].ToString(),
                        Nombre = dr["Nombre"].ToString(),
                        PrimerApellido = dr["PrimerApellido"].ToString(),
                        SegundoApellido = dr["SegundoApellido"] == DBNull.Value
                            ? null : dr["SegundoApellido"].ToString(),
                        IdPerfil = Convert.ToInt32(dr["IdPerfil"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }
            }
            catch (Exception er)
            {
                _log.Error("Error SelectById Usuario", er);
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public void UPDATE(Usuario usuario)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_UPDATE_Usuario");
                    command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@PrimerApellido", usuario.PrimerApellido);
                    command.Parameters.AddWithValue("@SegundoApellido",
                        (object)usuario.SegundoApellido ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IdPerfil", usuario.IdPerfil);
                    command.Parameters.AddWithValue("@Estado", usuario.Estado);
                    command.CommandType = CommandType.StoredProcedure;
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error UPDATE Usuario", er);
                MessageBox.Show(er.Message);
            }
        }
    }
}
