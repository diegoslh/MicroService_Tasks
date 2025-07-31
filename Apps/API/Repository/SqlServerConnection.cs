using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Repository.Interfaces;

namespace Repository
{
    public class SqlServerConnection(string connectionString) : ISqlServerConnection
    {
        private readonly string _connectionString = connectionString;

        public async Task<List<T>> ExecuteQuerySqlServerDb<T>(string query, Dictionary<string, object>? parameters)
            where T : new()
        {
            /// <summary>
            /// Este método tiene la estructura necesaria para realizar una consulta
            /// a la base de datos de SQL Server por medio de ADO.NET
            /// </summary>
            List<T> resultList = new List<T>();

            await using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                await using var command = new SqlCommand(query, connection);

                // Se agregan los parametros de manera segura para evitar SQL Injection con interpolacion de strings
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    // Crea la instancia de la clase T (genérica)
                    T obj = new T();

                    // Asigna los valores de las columnas a las propiedades de la clase
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        /// <summary>
                        ///Esta sección usa Reflection, que es la capacidad del programa para inspeccionar, modificar y ejecutar su propia estructura en tiempo de ejecución.
                        ///Esto incluye clases, propiedades, métodos y ensamblados, sin conocerlos en tiempo de compilación.
                        ///Para este caso, se usa para asignar los valores de las columnas a las propiedades de la clase genérica T.
                        /// </summary>
                        // Verifica si la propiedad existe en la clase T (genérica)
                        var prop = typeof(T).GetProperty(reader.GetName(i), BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                        // Si la propiedad existe y el valor no es DBNull, asigna el valor
                        if (prop != null && reader[i] != DBNull.Value)
                        {
                            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                            var safeValue = Convert.ChangeType(reader[i], targetType);
                            prop.SetValue(obj, safeValue);
                        }
                    }

                    // Agrega el objeto a la lista de resultados
                    resultList.Add(obj);
                }
            }
            return resultList;
        }
    }
}
