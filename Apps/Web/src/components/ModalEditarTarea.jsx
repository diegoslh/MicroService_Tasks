import { useEffect, useState } from "react";
import { API_URL } from "../../config.js";

function ModalEditarTarea({ tarea, onUpdated }) {
  const [formData, setFormData] = useState({
    titulo: "",
    descripcion: null,
    fechaLimite: "",
    colaboradorFk: "",
    estadoTareaFk: "",
  });

  const [colaboradores, setColaboradores] = useState([]);
  const [estados, setEstados] = useState([]);

  // ID único para cada modal
  const modalId = `modalEditarTarea-${tarea.id}`;

  useEffect(() => {
    if (tarea?.id) {
      setFormData({
        titulo: tarea.titulo || "",
        descripcion: tarea.descripcion || "",
        fechaLimite: tarea.fechaLimite?.split("T")[0] || "",
        colaboradorFk: tarea.colaboradorFk || "",
        estadoTareaFk: tarea.estadoTareaFk || "",
      });

      // Cargar colaboradores
      fetch(`${API_URL}/Usuario/Colaboradores`)
        .then((res) => res.json())
        .then(setColaboradores)
        .catch((err) => console.error("Error cargando colaboradores", err));

      // Cargar estados
      fetch(`${API_URL}/EstadosTarea`)
        .then((res) => res.json())
        .then(setEstados)
        .catch((err) => console.error("Error cargando estados", err));
    }
  }, [tarea]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      titulo: formData.titulo,
      descripcion: formData.descripcion,
      fechaCreacion: tarea.fechaCreacion,
      fechaLimite: new Date(formData.fechaLimite).toISOString(),
      colaboradorFk: parseInt(formData.colaboradorFk),
      estadoTareaFk: parseInt(formData.estadoTareaFk),
      estado: tarea.estado,
    };

    try {
      const res = await fetch(`${API_URL}/Tarea/${tarea.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const errorText = await res.text();
        console.error("Error al crear la tarea:", errorText);
        throw new Error(
          JSON.parse(errorText).mensaje || "Error al crear la tarea"
        );
      }

      alert("✅ ¡Tarea actualizada!");
      document.getElementById(modalId).querySelector("form").reset();
      if (onUpdated) onUpdated();
    } catch (error) {
      console.error(error);
      alert(error || "❌ Error al actualizar la tarea");
    }
  };

  return (
    <>
      <button
        className="btn btn-warning"
        data-bs-toggle="modal"
        data-bs-target={`#${modalId}`}
      >
        Editar
      </button>

      <article className="modal fade" id={modalId} tabIndex="-1">
        <div className="modal-dialog">
          <form className="modal-content" onSubmit={handleSubmit}>
            <header className="modal-header">
              <h4 className="modal-title w-100 text-center">Editar Tarea</h4>
            </header>

            <section className="modal-body">
              <div className="mb-3">
                <label className="form-label">
                  Título <code>*</code>
                </label>
                <input
                  type="text"
                  name="titulo"
                  className="form-control"
                  value={formData.titulo}
                  onChange={handleChange}
                  maxLength="100"
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Descripción</label>
                <textarea
                  name="descripcion"
                  className="form-control"
                  rows="2"
                  value={formData.descripcion}
                  onChange={handleChange}
                ></textarea>
              </div>

              <div className="mb-3">
                <label className="form-label">Fecha Creación</label>
                <input
                  type="text"
                  className="form-control"
                  value={
                    tarea.fechaCreacion
                      ? new Date(tarea.fechaCreacion).toLocaleDateString(
                          "es-ES"
                        )
                      : ""
                  }
                  disabled
                  readOnly
                />
              </div>

              <div className="mb-3">
                <label className="form-label">
                  Fecha Límite <code>*</code>
                </label>
                <input
                  type="date"
                  name="fechaLimite"
                  className="form-control"
                  value={formData.fechaLimite}
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label">
                  Colaborador <code>*</code>
                </label>
                <select
                  name="colaboradorFk"
                  className="form-select"
                  value={formData.colaboradorFk}
                  onChange={handleChange}
                  required
                >
                  <option value="" hidden>
                    Seleccione un colaborador
                  </option>
                  {colaboradores.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.nombre}
                    </option>
                  ))}
                </select>
              </div>

              <div className="mb-3">
                <label className="form-label">
                  Estado de Tarea <code>*</code>
                </label>
                <select
                  name="estadoTareaFk"
                  className="form-select"
                  value={formData.estadoTareaFk}
                  onChange={handleChange}
                  required
                >
                  <option value="" hidden>
                    Seleccione un estado
                  </option>
                  {estados.map((e) => (
                    <option key={e.id} value={e.id}>
                      {e.estadoTarea}
                    </option>
                  ))}
                </select>
              </div>
            </section>

            <footer className="modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                data-bs-dismiss="modal"
              >
                Cancelar
              </button>
              <button type="submit" className="btn btn-success">
                Actualizar
              </button>
            </footer>
          </form>
        </div>
      </article>
    </>
  );
}

export default ModalEditarTarea;
