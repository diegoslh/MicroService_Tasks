using Models;
using Models.DTOs;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TareaRepository(ISqlServerConnection connection) : ITareaRepository
    {
        public async Task<IEnumerable<TareaDto>> ObtenerFullPayloadTareas(Dictionary<string, object?> parametros)
        {
            var query = @"
                SELECT 
	                t.tar_idTareaPk AS Id,
	                t.tar_titulo AS Titulo,
	                ISNULL(t.tar_descripcion, 'N/A') AS Descripcion,
	                FORMAT(t.tar_fechaCreacion, 'yyyy-MM-dd HH:mm') AS FechaCreacion,
	                FORMAT(t.tar_fechaLimite, 'yyyy-MM-dd HH:mm') AS FechaLimite,
	                DATEDIFF(DAY, GETDATE(), t.tar_fechaLimite) AS DiasRestantes,
	                CONCAT(c.col_nombre, ' (', c.col_email, ')') AS Colaborador,
	                CASE 
		                WHEN DATEDIFF(DAY, GETDATE(), t.tar_fechaLimite) < 0 THEN 'Vencida'
		                WHEN DATEDIFF(DAY, GETDATE(), t.tar_fechaLimite) <= 3 THEN 'Urgente'
		                ELSE 'Normal'
	                END AS Prioridad,
	                e.est_nombre AS EstadoTarea,
	                CASE 
		                WHEN t.tar_estado = 1 
			                THEN 'Activa'
			                ELSE 'Inactiva'
	                END AS EstadoRegistro
                FROM Tc_TblTarea t
                LEFT JOIN Tc_TblColaborador c ON c.col_idColaboradorPk = t.tar_colaboradorFk
                LEFT JOIN Tc_TblDicEstadoTarea e ON e.est_idEstadoPk = t.tar_estadoFk
                WHERE (@soloActivas IS NULL OR t.tar_estado = CASE WHEN @soloActivas = 1 THEN 1 ELSE 0 END)
                ORDER BY t.tar_fechaLimite ASC;
            ";

            return await connection.ExecuteQuerySqlServerDb<TareaDto>(query, parametros);
        }

        public async Task<IEnumerable<TareaDto>> ObtenerTareasActivas()
        {
            var query = @"
                SELECT 
	                t.tar_idTareaPk AS Id,
	                t.tar_titulo AS Titulo,
	                ISNULL(t.tar_descripcion, 'N/A') AS Descripcion,
	                t.tar_fechaCreacion AS FechaCreacion,
	                t.tar_fechaLimite AS FechaLimite,
	                c.col_nombre AS Colaborador,
	                e.est_nombre AS EstadoTarea
                FROM Tc_TblTarea t
                LEFT JOIN Tc_TblColaborador c ON c.col_idColaboradorPk = t.tar_colaboradorFk
                LEFT JOIN Tc_TblDicEstadoTarea e ON e.est_idEstadoPk = t.tar_estadoFk
                WHERE t.tar_estado = 1
                ORDER BY t.tar_estadoFk
            ";

            return await connection.ExecuteQuerySqlServerDb<TareaDto>(query, null);
        }

        public async Task CrearTareaAsync(TblTarea tarea)
        {

            var query = @"
                INSERT INTO Tc_TblTarea (
                    tar_titulo,
                    tar_descripcion,
                    tar_fechaCreacion,
                    tar_fechaLimite,
                    tar_colaboradorFk,
                    tar_estadoFk,
                    tar_estado
                )
                VALUES (
                    @Titulo,
                    @Descripcion,
                    @FechaCreacion,
                    @FechaLimite,
                    @ColaboradorFk,
                    @EstadoTareaFk,
                    @Estado
                );
            ";

            var parametros = new Dictionary<string, object?>
            {
                { "@Titulo", tarea.Titulo },
                { "@Descripcion", tarea.Descripcion ?? (object)DBNull.Value },
                { "@FechaCreacion", tarea.FechaCreacion },
                { "@FechaLimite", tarea.FechaLimite },
                { "@ColaboradorFk", tarea.ColaboradorFk },
                { "@EstadoTareaFk", tarea.EstadoTareaFk },
                { "@Estado", tarea.Estado }
            };

            await connection.ExecuteNonQuerySqlServerDb(query, parametros);

        }

        public async Task<bool> ExisteTareaAsync(int id)
        {
            var query = @"
                SELECT COUNT(1) AS CountValue
                FROM Tc_TblTarea 
                WHERE tar_idTareaPk = @Id
            ";

            var parametros = new Dictionary<string, object>
            {
                { "@Id", id }
            };

            var resultado = await connection.ExecuteQuerySqlServerDb<CountDto>(query, parametros);
            //return Convert.ToInt32(resultado) > 0;
            return resultado?.FirstOrDefault()?.CountValue > 0;
        }

        public async Task ActualizarTareaAsync(TblTarea tarea)
        {
            var query = @"
                UPDATE Tc_TblTarea
                SET 
                    tar_titulo = @Titulo,
                    tar_descripcion = @Descripcion,
                    tar_fechaCreacion = @FechaCreacion,
                    tar_fechaLimite = @FechaLimite,
                    tar_colaboradorFk = @ColaboradorFk,
                    tar_estadoFk = @EstadoTareaFk,
                    tar_estado = @Estado
                WHERE tar_idTareaPk = @IdTareaPk
            ";

            var parametros = new Dictionary<string, object?>
            {
                { "@IdTareaPk", tarea.IdTareaPk },
                { "@Titulo", tarea.Titulo },
                { "@Descripcion", tarea.Descripcion ?? (object)DBNull.Value },
                { "@FechaCreacion", tarea.FechaCreacion },
                { "@FechaLimite", tarea.FechaLimite },
                { "@ColaboradorFk", tarea.ColaboradorFk },
                { "@EstadoTareaFk", tarea.EstadoTareaFk },
                { "@Estado", tarea.Estado }
            };

            await connection.ExecuteNonQuerySqlServerDb(query, parametros);
        }


        public async Task ActualizarEstadoTareaAsync(int id, bool estado)
        {
            var query = @"
                UPDATE Tc_TblTarea
                SET tar_estado = @Estado
                WHERE tar_idTareaPk = @Id
            ";

            var parametros = new Dictionary<string, object>
            {
                { "@Id", id },
                { "@Estado", estado }
            };

            await connection.ExecuteNonQuerySqlServerDb(query, parametros);
        }

        public async Task EliminarTareaAsync(int id)
        {
            var query = @"
                DELETE FROM Tc_TblTarea
                WHERE tar_idTareaPk = @Id
            ";

            var parametros = new Dictionary<string, object>
            {
                { "@Id", id }
            };

            await connection.ExecuteNonQuerySqlServerDb(query, parametros);
        }

    }
}
