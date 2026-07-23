
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
    public class DALFacturaDetalle : IDALFacturaDetalle
    {
        private static readonly ILog _log = LogManager.GetLogger("MyControlEventos");
        public bool DeleteByFactura(int pIdFactura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_DELETE_FacturaDetalle_ByFactura");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdFactura", pIdFactura);
                    db.ExecuteNonQuery(command);
                }
                return true;
            }
            catch (Exception er)
            {
                _log.Error("Error DELETE FacturaDetalle", er);
                throw;
            }
        }

        public List<FacturaDetalle> GetByFactura(int pIdFactura)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_SELECT_FacturaDetalle_ByFactura");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdFactura", pIdFactura);
                    var ds = db.ExecuteReader(command, "FacturaDetalle");

                    var lista = new List<FacturaDetalle>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        lista.Add(new FacturaDetalle
                        {
                            IdDetalle = Convert.ToInt32(dr["IdDetalle"]),
                            IdFactura = Convert.ToInt32(dr["IdFactura"]),
                            IdProducto = Convert.ToInt32(dr["IdProducto"]),
                            Cantidad = Convert.ToInt32(dr["Cantidad"]),
                            Precio = Convert.ToDouble(dr["Precio"]),
                            Subtotal = Convert.ToDouble(dr["Subtotal"])
                        });
                    }
                    return lista;
                }
            }
            catch (Exception er)
            {
                _log.Error("Error SELECT FacturaDetalle", er);
                throw;
            }
        }

        public int Insert(FacturaDetalle facturaDetalle)
        {
            try
            {
                using (var db = FactoryDatabase.CreateDataBase(FactoryConexion.CreateConnection()))
                {
                    var command = new SqlCommand("usp_PROCESS_VentaDetalle");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@IdFactura", facturaDetalle.IdFactura);
                    command.Parameters.AddWithValue("@IdProducto", facturaDetalle.IdProducto);
                    command.Parameters.AddWithValue("@Cantidad", facturaDetalle.Cantidad);
                    command.Parameters.Add("@Precio", SqlDbType.Decimal).Value = (decimal)facturaDetalle.Precio;

                    db.ExecuteNonQuery(command);

                    return 1; 
                }
            }
            catch (Exception er)
            {
                _log.Error("Error INSERT FacturaDetalle (venta)", er);
                throw;
            }
        }
    }
}
