using Models.DTOs;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
    {
        public async Task<IEnumerable<ColaboradorDto>> ObtenerColaboradoresAsync()
        {
            return await repository.ObtenerColaboradoresActivosAsync();
        }

        public async Task<UsuarioDto?> VerificarCredencialesUsuario(string alias, string clave)
        {
            // Obtener el usuario por alias
            var usuarios = await repository.ObtenerUsuarioPorAlias(alias);

            if (!usuarios.Any())
                return null; // Alias no existe

            // Verificar si la clave encriptada coincide con la almacenada
            var encryptedPassword = EncryptSHA256(clave);
            var usuario = usuarios.FirstOrDefault(u => u.Clave == encryptedPassword);

            return usuario; // Será null si la clave no coincide

        }

        private static string EncryptSHA256(string clave)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Computar el hash
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(clave));

                // Convertir el array de bytes a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
