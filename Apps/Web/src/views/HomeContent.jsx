import { use, useEffect, useState } from "react";
import { API_URL } from "../../config.js";
import ModalCrearTarea from "../components/ModalCrearTarea.jsx";
import ModalEditarTarea from "../components/ModalEditarTarea.jsx";
import ModalEliminarInhabilitarTarea from "../components/ModalEliminarInhabilitarTarea.jsx";
import DeleteIcon from "../assets/icons/delete.png";
import ModalLogin from "../components/ModalLogin.jsx";

function HomeContent() {
  const [tareas, setTareas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [cargarTareas, setCargarTareas] = useState(false);
  const [estadoTareas, setEstadoTareas] = useState(null);
  const [tareaSeleccionadaId, setTareaSeleccionadaId] = useState(null);
  const token = localStorage.getItem("JWT_TOKEN");

  useEffect(() => {
    fetch(
      `${API_URL}/Tarea?fullPayload=true${
        estadoTareas != null ? `&estado=${estadoTareas}` : ""
      }`
    )
      .then((response) => {
        if (!response.ok) {
          throw new Error(`Error: ${response.status}`);
        }
        return response.json();
      })
      .then((data) => {
        setTareas(data.sort((a, b) => b.id - a.id));
        setLoading(false);
      })
      .catch((err) => {
        console.error("Error al obtener tareas:", err);
        setError("Error al cargar los datos.");
        setLoading(false);
      });
  }, [cargarTareas, estadoTareas]);

  return (
    <div className="mx-2 my-4">
      <div style={{ position: "absolute", top: "20px", right: "20px" }}>
        {!token ? (
          <ModalLogin />
        ) : (
          <button
            className="btn btn-outline-danger border-2"
            onClick={() => {
              localStorage.removeItem("JWT_TOKEN");
              localStorage.removeItem("ALIAS");
              window.location.reload();
            }}
          >
            Cerrar Sesión
          </button>
        )}
      </div>

      <header className="mb-3">
        {token ? (
          <span className="text-success">
            Hola👋, {localStorage.getItem("ALIAS").toLowerCase()}
          </span>
        ) : (
          <span className="text-danger">
            🔔 <b className="">Bienvenido</b>, Por favor inicia sesión para
            habilitar las acciones.
          </span>
        )}

        <h2 className="text-center text-muted" style={{ fontWeight: 600 }}>
          Tareas Colaboradores
        </h2>
      </header>

      <div className="table-responsive bg-white p-4 pt- rounded shadow-sm">
        <div className="d-flex justify-content-between mb-3">
          <div
            class="btn-group btn-group-sm"
            role="group"
            aria-label="Basic outlined example"
          >
            <button
              type="button"
              class={
                estadoTareas === null
                  ? "btn btn-secondary"
                  : "btn btn-outline-secondary"
              }
              onClick={() => setEstadoTareas(null)}
            >
              Todos
            </button>
            <button
              type="button"
              class={
                estadoTareas === true
                  ? "btn btn-secondary"
                  : "btn btn-outline-secondary"
              }
              onClick={() => setEstadoTareas(true)}
            >
              Activos
            </button>
            <button
              type="button"
              class={
                estadoTareas === false
                  ? "btn btn-secondary"
                  : "btn btn-outline-secondary"
              }
              onClick={() => setEstadoTareas(false)}
            >
              Inactivos
            </button>
          </div>

          {token ? (
            <ModalCrearTarea
              idModal="modalCrearTarea"
              onUpdated={() => setCargarTareas(!cargarTareas)}
            />
          ) : null}
        </div>

        {loading && <div className="alert alert-info">Cargando tareas...</div>}
        {error && <div className="alert alert-danger">{error}</div>}

        {!loading && !error && (
          <table
            className="table table-striped table-bordered table-hover"
            style={{ fontSize: "0.9rem" }}
          >
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
                  <td>
                    <span
                      className={`badge bg-${tarea.prioridad.toLowerCase()}`}
                    >
                      {tarea.prioridad}
                    </span>
                  </td>
                  <td>{tarea.colaborador}</td>
                  <td>{tarea.estadoTarea}</td>
                  <td>{tarea.estadoRegistro}</td>
                  <td>
                    {token ? (
                      <div className="d-flex justify-content-center">
                        {tarea.estadoRegistro === "Activa" ? (
                          <>
                            <ModalEditarTarea
                              tarea={tarea}
                              onUpdated={() => setCargarTareas(!cargarTareas)}
                            />
                            <button
                              className="btn btn-outline-light"
                              data-bs-toggle="modal"
                              title="Eliminar/Inhabilitar Registro"
                              data-bs-target="#modalEliminarInhabilitar"
                              onClick={() => setTareaSeleccionadaId(tarea.id)}
                              disabled={!token}
                            >
                              <img src={DeleteIcon} alt="Eliminar" width="20" />
                            </button>
                          </>
                        ) : (
                          <span className="text-muted text-center">✖️</span>
                        )}
                      </div>
                    ) : (
                      <span className="text-muted text-center">🔒</span>
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
        )}
      </div>
    </div>
  );
}

export default HomeContent;
