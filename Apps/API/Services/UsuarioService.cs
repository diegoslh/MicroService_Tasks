using Microsoft.IdentityModel.Tokens;
using Models.DTOs;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

            // Generar Token
            var token = GenerarJWT(usuario.Alias);
            usuario.JwtToken = token;

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

        private string GenerarJWT(string usuario)
        {
            //crear la informacion del usuario para token
            var userClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]!)); // TODO Error al importar IConfiguration
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("420F64E3-BBF1-4EB0-83E8-1312CFFFF9E4"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //crear detalle del token
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
