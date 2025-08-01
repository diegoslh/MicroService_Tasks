import { useEffect, useState } from "react";
import { API_URL } from "../../config.js";
import ModalCrearTarea from "../components/ModalCrearTarea.jsx";
import ModalEditarTarea from "../components/ModalEditarTarea.jsx";
import ModalEliminarInhabilitarTarea from "../components/ModalEliminarInhabilitarTarea.jsx";
import DeleteIcon from "../assets/icons/delete.png";

function HomeContent() {
  const [tareas, setTareas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [cargarTareas, setCargarTareas] = useState(false);
  const [tareaSeleccionadaId, setTareaSeleccionadaId] = useState(null);

  useEffect(() => {
    fetch(`${API_URL}/Tarea?fullPayload=true`)
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        // setTareas(data);
        setTareas(data.sort((a, b) => b.id - a.id));
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error al obtener tareas:", err);
        setError("Error al cargar los datos.");
        setLoading(false);
      });
  }, [cargarTareas]);

  return (
    <div className="container mt-4">
      <h2 className="text-center mb-4">Balance Tareas Colaboradores</h2>
      <div className="d-flex justify-content-end mb-3">
        <ModalCrearTarea
          idModal="modalCrearTarea"
          onUpdated={() => setCargarTareas(!cargarTareas)}
        />
      </div>

      {loading && <div className="alert alert-info">Cargando tareas...</div>}
      {error && <div className="alert alert-danger">{error}</div>}

      {!loading && !error && (
        <div className="table-responsive">
          <table className="table table-striped table-bordered table-hover">
            <thead className="table-dark">
              <tr>
                <th>ID</th>
                <th>Título</th>
                <th>Descripción</th>
                <th>Fecha Creación</th>
                <th>Fecha Límite</th>
                <th>Días Restantes</th>
                <th>Prioridad</th>
                {/* <th>ColaboradorFk</th> */}
                <th>Colaborador</th>
                {/* <th>EstadoFk</th> */}
                <th>Estado</th>
                <th>Registro</th>
                <th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {tareas.map((tarea) => (
                <tr key={tarea.id}>
                  <td>{tarea.id}</td>
                  <td>{tarea.titulo}</td>
                  <td>{tarea.descripcion}</td>
                  <td>{tarea.fechaCreacion.split("T")[0]}</td>
                  <td>{tarea.fechaLimite.split("T")[0]}</td>
                  <td>
                    {tarea.diasRestantes < 0
                      ? "--"
                      : `${tarea.diasRestantes} días`}
                  </td>
                  <td>{tarea.prioridad}</td>
                  {/* <td>{tarea.colaboradorFk}</td> */}
                  <td>{tarea.colaborador}</td>
                  {/* <td>{tarea.estadoTareaFk}</td> */}
                  <td>{tarea.estadoTarea}</td>
                  <td>{tarea.estadoRegistro}</td>
                  <td>
                    <ModalEditarTarea
                      tarea={tarea}
                      onUpdated={() => setCargarTareas(!cargarTareas)}
                    />
                    {tarea.estadoRegistro === "Activa" && (
                      <button
                        className="btn btn-danger-outline"
                        data-bs-toggle="modal"
                        title="Eliminar/Inhabilitar Registro"
                        data-bs-target="#modalEliminarInhabilitar"
                        onClick={() => setTareaSeleccionadaId(tarea.id)}
                      >
                        <img src={DeleteIcon} alt="Eliminar" width="20" />
                      </button>
                    )}
                  </td>
                </tr>
              ))}
              <ModalEliminarInhabilitarTarea
                tareaId={tareaSeleccionadaId}
                onUpdated={() => setCargarTareas(!cargarTareas)}
              />
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

export default HomeContent;
