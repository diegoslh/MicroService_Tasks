using Models.DTOs;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EstadoTareaRepository(ISqlServerConnection connection) : IEstadoTareaRepository
    {
        public async Task<IEnumerable<EstadoTareaDto>> ObtenerEstadosActivosAsync()
        {
            var query = @"
            SELECT 
                est_idEstadoPk AS Id,
                est_nombre AS EstadoTarea
            FROM Tc_TblDicEstadoTarea
            WHERE est_estado = 1;
        ";

            return await connection.ExecuteQuerySqlServerDb<EstadoTareaDto>(query, null);
        }
    }
}
