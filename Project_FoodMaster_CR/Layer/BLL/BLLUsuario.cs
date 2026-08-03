using appFoodMaster_CR.Layer.DAL;
using appFoodMaster_CR.Layer.Entities;
using appFoodMaster_CR.Layer.Interfaces.IBLL;
using appFoodMaster_CR.Layer.Interfaces.IDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appFoodMaster_CR.Layer.BLL
{
    public class BLLUsuario : IBLLUsuario
    {
        private IDALUsuario _objdALUsuario = new DALUsuario();
        public void CREATE(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre de usuario es obligatorio");
            if (string.IsNullOrWhiteSpace(usuario.Clave))
                throw new Exception("La clave es obligatoria");
            if (usuario.Clave.Length < 6)
                throw new Exception("La clave debe tener al menos 6 caracteres");

 
            usuario.Clave = Cryptography.EncrypthAES(usuario.Clave);
            _objdALUsuario.INSERT(usuario);
        }

        public void DELETE(int idUsuario)
        {
            _objdALUsuario.DELETE(idUsuario);
        }

        public Usuario LOGIN(string nombreUsuario, string clave)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                throw new Exception("El nombre de usuario es obligatorio.");
            if (string.IsNullOrWhiteSpace(clave))
                throw new Exception("La contraseña es obligatoria.");

         
            string claveEncriptada = Cryptography.EncrypthAES(clave);
            return _objdALUsuario.LOGIN(nombreUsuario.Trim(), claveEncriptada);
        }

        public List<Usuario> SELECT_ALL()
        {
            return _objdALUsuario.SELECT_ALL();
        }

        public DataTable SELECT_ALL_PERFILES()
        {
            return _objdALUsuario.SELECT_ALL_PERFILES();
        }

        public Usuario SELECT_BY_ID(int idUsuario)
        {
            return _objdALUsuario.SELECT_BY_ID(idUsuario);
        }

        public void UPDATE(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreUsuario))
                throw new Exception("El nombre de usuario es obligatorio");

         
            if (!string.IsNullOrWhiteSpace(usuario.Clave))
                usuario.Clave = Cryptography.EncrypthAES(usuario.Clave);
            else
                usuario.Clave = _objdALUsuario.SELECT_BY_ID(usuario.IdUsuario).Clave; 

            _objdALUsuario.UPDATE(usuario);
        }
    }
}
