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
    public class DALTarjeta : IDALTarjeta
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");

        public List<Tarjeta> GetAll()
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Tarjeta_All");
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Tarjeta");

                    var lista = new List<Tarjeta>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Tarjeta
                        {
                            IdTarjeta = Convert.ToInt32(dr["IdTarjeta"]),
                            Descripcion = dr["Descripcion"].ToString()
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error GetAll Tarjeta", er);
                throw;
            }
        }

        public int Insert(Tarjeta tarjeta)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_INSERT_Tarjeta");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Descripcion", tarjeta.Descripcion);

                    var outId = new SqlParameter("@IdTarjeta", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outId);

                    db.ExecuteNonQuery(command);

                    return Convert.ToInt32(outId.Value);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error Insert Tarjeta", er);
                throw;
            }
        }
    }
}
