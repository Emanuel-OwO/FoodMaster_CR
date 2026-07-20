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
    public class DALImpuesto : IDALImpuesto
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");

        public void DELETE(DateTime fecha)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_DELETE_Impuesto_ByID");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Fecha", fecha.Date);
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error DELETE Impuesto", er);
                MessageBox.Show(er.Message);
            }
        }

        public List<Impuesto> Get_By_Filter(string filtro)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SEARCH_Impuesto");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Filtro", filtro ?? "");
                    var ds = db.ExecuteReader(command, "Impuesto");

                    var lista = new List<Impuesto>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Impuesto
                        {
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Porcentaje = Convert.ToDouble(dr["Porcentaje"])
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error Get_By_Filter Impuesto", er);
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public void INSERT(Impuesto impuesto)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_INSERT_Impuesto");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Fecha", impuesto.Fecha.Date);
                    command.Parameters.AddWithValue("@Porcentaje", impuesto.Porcentaje);
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error INSERT Impuesto", er);
                MessageBox.Show(er.Message);
            }
        }

        public List<Impuesto> SELECTALL()
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_Impuesto_All");
                    command.CommandType = CommandType.StoredProcedure;
                    var ds = db.ExecuteReader(command, "Impuesto");

                    var lista = new List<Impuesto>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new Impuesto
                        {
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                            Porcentaje = Convert.ToDouble(dr["Porcentaje"])
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error SELECTALL Impuesto", er);
                MessageBox.Show(er.Message);
                return null;
            }
        }

        public void UPDATE(Impuesto impuesto)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_UPDATE_Impuesto");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Fecha", impuesto.Fecha.Date);
                    command.Parameters.AddWithValue("@Porcentaje", impuesto.Porcentaje);
                    db.ExecuteNonQuery(command);
                }
            }
            catch (Exception er)
            {
                _log.Error("Error UPDATE Impuesto", er);
                MessageBox.Show(er.Message);
            }
        }
    }
}
