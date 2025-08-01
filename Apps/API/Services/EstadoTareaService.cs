using Models.DTOs;
using Repository.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EstadoTareaService(IEstadoTareaRepository estadoTareaRepository) : IEstadoTareaService
    {
        public async Task<IEnumerable<EstadoTareaDto>> ListarEstadosActivosAsync()
        {
            return await estadoTareaRepository.ObtenerEstadosActivosAsync();
        }
    }
}
