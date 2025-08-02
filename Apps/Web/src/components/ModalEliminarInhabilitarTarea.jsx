import React from "react";
import { API_URL } from "../../config.js";

function ModalEliminarInhabilitarTarea({ tareaId, onUpdated }) {
  const token = localStorage.getItem("JWT_TOKEN");
  const modalId = "modalEliminarInhabilitar";

  const handleInhabilitar = async () => {
    try {
      const res = await fetch(`${API_URL}/Tarea/${tareaId}/estado`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json", "Authorization": `Bearer ${token}` },
        body: JSON.stringify(false),
      });

      if (!res.ok) {
        const errorText = await res.text();
        console.error("Error al inhabilitar la tarea:", errorText);
        throw new Error(
          JSON.parse(errorText).mensaje || "Error al inhabilitar la tarea"
        );
      }

      alert("✅ Tarea inhabilitada exitosamente.");
      if (onUpdated) onUpdated();
    } catch (error) {
      console.error(error);
      alert("❌ Error al inhabilitar la tarea");
    }
  };

  const handleEliminar = async () => {
    try {
      const res = await fetch(`${API_URL}/Tarea/${tareaId}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` },
      });

      if (!res.ok) {
        const errorText = await res.text();
        console.error("Error al eliminar la tarea:", errorText);
        throw new Error(
          JSON.parse(errorText).mensaje || "Error al eliminar la tarea"
        );
      }

      alert("✅ Tarea eliminada correctamente.");
      if (onUpdated) onUpdated();
    } catch (error) {
      console.error(error);
      alert(error || "❌ Error al eliminar la tarea");
    }
  };

  return (
    <article className="modal fade" id={modalId} tabIndex="-1">
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-body text-center">
            <p className="fs-5">
              ¿Qué deseas hacer con la tarea <strong>ID #{tareaId}</strong>?
            </p>
            <p className="text-danger">⚠ Esta acción no se puede deshacer</p>
          </div>
          <footer className="modal-footer">
            <button
              type="button"
              className="btn btn-light"
              data-bs-dismiss="modal"
            >
              Cancelar
            </button>
            <button
              type="button"
              className="btn btn-warning"
              data-bs-dismiss="modal"
              onClick={handleInhabilitar}
            >
              Inhabilitar
            </button>
            <button
              type="button"
              className="btn btn-danger"
              data-bs-dismiss="modal"
              onClick={handleEliminar}
            >
              Eliminar
            </button>
          </footer>
        </div>
      </div>
    </article>
  );
}

export default ModalEliminarInhabilitarTarea;
