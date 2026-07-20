using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
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
    public class DALTipoProducto : IDALTipoProducto
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");

        public void CREATE(TipoProducto tipo)
        {
            string sql = @"INSERT INTO TipoProducto (Descripcion, Estado)
               VALUES (@Descripcion, @Estado)";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@Descripcion", tipo.Descripcion);
                command.Parameters.AddWithValue("@Estado", tipo.Estado);

                command.CommandText = sql;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error("Error CREATE TipoProducto", er);
                throw;
            }
        }

        public void DELETE(int id)
        {
            string sql = @"DELETE FROM TipoProducto WHERE IdTipoProducto=@Id";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@Id", id);
                command.CommandText = sql;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error("Error DELETE TipoProducto", er);
                throw;
            }
        }

        public List<TipoProducto> SelectAll()
        {
            List<TipoProducto> lista = new List<TipoProducto>();
            DataSet ds = null;

            string sql = @"SELECT IdTipoProducto, Descripcion, Estado
               FROM TipoProducto";

            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandText = sql;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "TipoProducto");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TipoProducto t = new TipoProducto()
                    {
                        IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };

                    lista.Add(t);
                }

                return lista;
            }
            catch (SqlException er)
            {
                _log.Error("Error SelectAll TipoProducto", er);
                throw;
            }
        }

        public TipoProducto SelectById(int id)
        {
            TipoProducto t = null;
            DataSet ds = null;

            string sql = @"SELECT IdTipoProducto, Descripcion, Estado
               FROM TipoProducto
               WHERE IdTipoProducto=@Id";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@Id", id);
                command.CommandText = sql;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "TipoProducto");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    t = new TipoProducto()
                    {
                        IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                        Descripcion = dr["Descripcion"].ToString(),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }

                return t;
            }
            catch (SqlException er)
            {
                _log.Error("Error SelectById TipoDispositivo", er);
                throw;
            }
        }

        public void UPDATE(TipoProducto tipo)
        {
            string sql = @"UPDATE TipoProducto
                   SET Descripcion=@Descripcion,
                       Estado=@Estado
                   WHERE IdTipoProducto=@Id";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@Id", tipo.IdTipoProducto);
                command.Parameters.AddWithValue("@Descripcion", tipo.Descripcion);
                command.Parameters.AddWithValue("@Estado", tipo.Estado);

                command.CommandText = sql;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error("Error UPDATE TipoProducto", er);
                throw;
            }
        }
    }
}
