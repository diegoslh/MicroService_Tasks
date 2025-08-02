import { useState } from "react";
import { API_URL } from "../../config.js";

function ModalCrearTarea({ idModal, onUpdated }) {
  const token = localStorage.getItem("JWT_TOKEN");
  
  const [formData, setFormData] = useState({
    titulo: "",
    descripcion: null,
    fechaLimite: "",
    colaboradorFk: "",
    estadoTareaFk: "",
  });

  const [colaboradores, setColaboradores] = useState([]);
  const [estadoCargado, setEstadoCargado] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!formData.titulo || !formData.fechaLimite || !formData.colaboradorFk) {
      alert("Por favor completa todos los campos obligatorios");
      return;
    }

    const payload = {
      titulo: formData.titulo,
      descripcion: formData.descripcion,
      fechaCreacion: new Date().toISOString(),
      fechaLimite: new Date(formData.fechaLimite).toISOString(),
      colaboradorFk: parseInt(formData.colaboradorFk),
      estadoTareaFk: parseInt(formData.estadoTareaFk),
      estado: true,
    };
    console.log("Payload:", payload);

    try {
      const res = await fetch(`${API_URL}/Tarea`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(payload),
      });

      if (!res.ok) {
        const errorText = await res.text();
        console.error("Error al crear la tarea:", errorText);
        throw new Error(
          JSON.parse(errorText).mensaje || "Error al crear la tarea"
        );
      }

      alert("✅ ¡Tarea creada exitosamente!");
      document.getElementById(idModal).querySelector("form").reset();
      if (onUpdated) onUpdated();
    } catch (error) {
      alert(error || "❌ Error al crear la tarea");
    }
  };

  const cargarDatosModal = async () => {
    const token = localStorage.getItem("JWT_TOKEN");

    const headers = {
      "Authorization": `Bearer ${token}`,
      "Accept": "application/json",
    };
    try {
      // Cargar colaboradores
      const colabRes = await fetch(`${API_URL}/Usuario/Colaboradores`, { headers });
      const colabData = await colabRes.json();
      setColaboradores(colabData);

      // Cargar estado "Pendiente"
      const estadoRes = await fetch(`${API_URL}/EstadosTarea`, { headers });
      const estadoData = await estadoRes.json();
      const pendiente = estadoData.find(
        (e) => e.estadoTarea.toLowerCase() === "pendiente"
      );

      if (pendiente) {
        setFormData((prev) => ({
          ...prev,
          estadoTareaFk: pendiente.id,
        }));
        setEstadoCargado(true);
      }
    } catch (error) {
      console.error("Error cargando datos del modal:", error);
    }
  };

  return (
    <>
      <button
        className="btn btn-outline-primary"
        type="button"
        data-bs-toggle="modal"
        data-bs-target={`#${idModal}`}
        onClick={cargarDatosModal}
        disabled={!token}
      >
        <span>Crear Tarea</span>
      </button>

      <article className="modal fade" tabIndex="-1" id={idModal}>
        <div className="modal-dialog">
          <form className="modal-content" onSubmit={handleSubmit}>
            <header className="modal-header">
              <h4 className="modal-title w-100 text-center text-muted">Crear Tarea</h4>
            </header>

            <section className="modal-body">
              <div className="mb-3">
                <label className="form-label">
                  Título <code>*</code>
                </label>
                <input
                  type="text"
                  name="titulo"
                  maxLength="100"
                  className="form-control"
                  onChange={handleChange}
                  required
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Descripción</label>
                <textarea
                  name="descripcion"
                  className="form-control"
                  rows="2"
                  onChange={handleChange}
                ></textarea>
              </div>

              <div className="mb-3">
                <label className="form-label">Fecha Creación</label>
                <input
                  type="text"
                  className="form-control"
                  value={new Date().toLocaleDateString("es-ES")}
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
                  onChange={handleChange}
                  required
                >
                  <option value="" hidden>
                    Seleccione un colaborador
                  </option>
                  {colaboradores.map((col) => (
                    <option key={col.id} value={col.id}>
                      {col.nombre}
                    </option>
                  ))}
                </select>
              </div>

              <div className="mb-0">
                <label className="form-label">Estado de Tarea</label>
                <input
                  type="text"
                  className="form-control"
                  value="Pendiente"
                  disabled
                  readOnly
                />
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
              <button
                type="submit"
                className="btn btn-primary"
                disabled={!estadoCargado}
              >
                Crear Tarea
              </button>
            </footer>
          </form>
        </div>
      </article>
    </>
  );
}

export default ModalCrearTarea;
