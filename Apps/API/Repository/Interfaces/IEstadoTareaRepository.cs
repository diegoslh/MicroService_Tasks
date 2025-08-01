﻿using Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IEstadoTareaRepository
    {
        Task<IEnumerable<EstadoTareaDto>> ObtenerEstadosActivosAsync();
    }
}
