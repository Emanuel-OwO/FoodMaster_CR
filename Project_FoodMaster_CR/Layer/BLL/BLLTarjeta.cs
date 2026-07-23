using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLTarjeta : IBLLTarjeta
    {
        private readonly IDALTarjeta _dalTarjeta;
        public BLLTarjeta(IDALTarjeta dalTarjeta)
        {
            _dalTarjeta = dalTarjeta;
        }
        public int Save(Tarjeta tarjeta)
        {

            if (tarjeta == null)
                throw new Exception("La tarjeta no puede ser nula.");

            if (string.IsNullOrWhiteSpace(tarjeta.Descripcion))
                throw new Exception("Debe indicar la descripción de la tarjeta (VISA, MASTERCARD, etc.)");

            return _dalTarjeta.Insert(tarjeta);
        }   
    }
}
