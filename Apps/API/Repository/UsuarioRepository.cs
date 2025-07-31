using Models.DTOs;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsuarioRepository(ISqlServerConnection connection) : IUsuarioRepository
    {
        public async Task<IEnumerable<ColaboradorDto>> ObtenerColaboradoresActivosAsync()
        {
            var query = @"
                SELECT 
	                col_idColaboradorPk AS Id, 
	                col_nombre AS Nombre
                FROM Tc_TblColaborador
                WHERE col_estado = 1
            ";

            return await connection.ExecuteQuerySqlServerDb<ColaboradorDto>(query, null);
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerUsuarioPorAlias(string alias)
        {
            var query = @"
                SELECT 
                    u.us_idUsuarioPkFK as Id,
	                u.us_alias AS Alias,
	                r.rol_nombreRol AS Rol,
	                u.us_contrasena AS Clave
                FROM Tc_TblUsuario u
                LEFT JOIN Tc_TblDicRol r ON r.rol_idRolPk=u.us_rolFk
                WHERE u.us_alias = @alias
            ";

            var parameters = new Dictionary<string, object>
            {
                { "@alias", alias }
            };

            return await connection.ExecuteQuerySqlServerDb<UsuarioDto>(query, parameters);
        }
    }
}
