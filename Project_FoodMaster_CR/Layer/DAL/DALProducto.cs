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
    public class DALProducto : IDALProducto
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");

        public void CREATE(Producto producto)
        {
            string msg = "";
            string sql = @"INSERT INTO Producto 
(IdTipoProducto, CodigoInterno, CodigoBarras, Descripcion, Peso, ContenidoCalorico,
 DocumentoEspecificaciones, Foto, CantidadStock, Precio, Estado)
VALUES 
(@IdTipoProducto, @CodigoInterno, @CodigoBarras, @Descripcion, @Peso, @ContenidoCalorico,
 @Documento, @Foto, @CantidadStock, @Precio, @Estado)";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                command.Parameters.AddWithValue("@CodigoInterno", producto.CodigoInterno);
                command.Parameters.AddWithValue("@CodigoBarras", producto.CodigoBarras ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Peso", producto.Peso);
                command.Parameters.AddWithValue("@ContenidoCalorico", producto.ContenidoCalorico);
                command.Parameters.AddWithValue("@Documento", producto.DocumentoEspecificaciones ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Foto", producto.Foto ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Estado", producto.Estado);

                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error($"Error {msg}", er);
                throw;
            }
        }

        public void DELETE(int id)
        {
            string msg = "";
            string sql = @"UPDATE Producto SET Estado = 0 WHERE IdProducto = @IdProducto";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@IdProducto", id);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error($"Error {msg}", er);
                throw;
            }
        }

        public List<Producto> Get_By_Filter(string filtro)
        {
            List<Producto> lista = new List<Producto>();
            DataSet ds = null;

            string sql = @"SELECT IdProducto, CodigoInterno, Descripcion, Precio, CantidadStock
                   FROM Producto
                   WHERE Estado = 1
                   AND (CodigoInterno LIKE @Filtro OR Descripcion LIKE @Filtro)";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@Filtro", filtro);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "Producto");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lista.Add(new Producto()
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        CodigoInterno = dr["CodigoInterno"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Precio = Convert.ToDouble(dr["Precio"]),
                        CantidadStock = Convert.ToInt32(dr["CantidadStock"])
                    });
                }

                return lista;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Producto> SelectAll()
        {
            string msg = "";
            DataSet ds = null;
            List<Producto> lista = new List<Producto>();

            string sql = @"SELECT IdProducto, IdTipoProducto, CodigoInterno, CodigoBarras,
               Descripcion, Peso, ContenidoCalorico,
               DocumentoEspecificaciones, Foto,
               CantidadStock, Precio, Estado
FROM Producto
WHERE Estado = 1";

            SqlCommand command = new SqlCommand();

            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "Producto");
                }

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Producto p = new Producto()
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                        CodigoInterno = dr["CodigoInterno"].ToString(),
                        CodigoBarras = dr["CodigoBarras"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Peso = Convert.ToDouble(dr["Peso"]),
                        ContenidoCalorico = Convert.ToInt32(dr["ContenidoCalorico"]),
                        DocumentoEspecificaciones = dr["DocumentoEspecificaciones"] == DBNull.Value ? null : (byte[])dr["DocumentoEspecificaciones"],
                        Foto = dr["Foto"] == DBNull.Value ? null : (byte[])dr["Foto"],
                        CantidadStock = Convert.ToInt32(dr["CantidadStock"]),
                        Precio = Convert.ToDouble(dr["Precio"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };

                    lista.Add(p);
                }

                return lista;
            }
            catch (SqlException er)
            {
                _log.Error($"Error {msg}", er);
                throw;
            }
        }

        public Producto SelectById(int id)
        {
            string msg = "";
            DataSet ds = null;
            Producto p = null;

            string sql = @"SELECT IdProducto, IdTipoProducto, CodigoInterno, CodigoBarras,
               Descripcion, Peso, ContenidoCalorico,
               DocumentoEspecificaciones, Foto,
               CantidadStock, Precio, Estado
               FROM Producto
               WHERE IdProducto = @IdProducto";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@IdProducto", id);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    ds = db.ExecuteReader(command, "Producto");
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    p = new Producto()
                    {
                        IdProducto = Convert.ToInt32(dr["IdProducto"]),
                        IdTipoProducto = Convert.ToInt32(dr["IdTipoProducto"]),
                        CodigoInterno = dr["CodigoInterno"].ToString(),
                        CodigoBarras = dr["CodigoBarras"].ToString(),
                        Descripcion = dr["Descripcion"].ToString(),
                        Peso = Convert.ToDouble(dr["Peso"]),
                        ContenidoCalorico = Convert.ToInt32(dr["ContenidoCalorico"]),
                        DocumentoEspecificaciones = dr["DocumentoEspecificaciones"] == DBNull.Value ? null : (byte[])dr["DocumentoEspecificaciones"],
                        Foto = dr["Foto"] == DBNull.Value ? null : (byte[])dr["Foto"],
                        CantidadStock = Convert.ToInt32(dr["CantidadStock"]),
                        Precio = Convert.ToDouble(dr["Precio"]),
                        Estado = Convert.ToBoolean(dr["Estado"])
                    };
                }

                return p;
            }
            catch (SqlException er)
            {
                _log.Error($"Error {msg}", er);
                throw;
            }
        }

        public void UPDATE(Producto producto)
        {
            string msg = "";
            string sql = @"UPDATE Producto SET
        IdTipoProducto = @IdTipoProducto,
        CodigoInterno = @CodigoInterno,
        CodigoBarras = @CodigoBarras,
        Descripcion = @Descripcion,
        Peso = @Peso,
        ContenidoCalorico = @ContenidoCalorico,
        DocumentoEspecificaciones = @Documento,
        Foto = @Foto,
        CantidadStock = @CantidadStock,
        Precio = @Precio,
        Estado = @Estado
    WHERE IdProducto = @IdProducto";

            SqlCommand command = new SqlCommand();

            try
            {
                command.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                command.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                command.Parameters.AddWithValue("@CodigoInterno", producto.CodigoInterno);
                command.Parameters.AddWithValue("@CodigoBarras", producto.CodigoBarras ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Peso", producto.Peso);
                command.Parameters.AddWithValue("@ContenidoCalorico", producto.ContenidoCalorico);
                command.Parameters.AddWithValue("@Documento", producto.DocumentoEspecificaciones ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Foto", producto.Foto ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@CantidadStock", producto.CantidadStock);
                command.Parameters.AddWithValue("@Precio", producto.Precio);
                command.Parameters.AddWithValue("@Estado", producto.Estado);

                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                using (IDataBase db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    db.ExecuteNonQuery(command, IsolationLevel.ReadCommitted);
                }
            }
            catch (SqlException er)
            {
                _log.Error($"Error {msg}", er);
                throw;
            }
        }
    }
}
