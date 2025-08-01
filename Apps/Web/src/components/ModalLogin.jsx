import { API_URL } from "../../config.js";
import { useState } from "react";

function ModalLogin() {
  const [formData, setFormData] = useState({
    usuario: "",
    clave: "",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const { usuario, clave } = formData;

    try {
      const res = await fetch(`${API_URL}/Usuario/Autenticar?usuario=${encodeURIComponent(usuario)}&clave=${encodeURIComponent(clave)}`, {
        method: "POST",
        headers: {
          "Accept": "*/*",
        },
        body: "",
      });

      if (!res.ok) {
        const err = await res.text();
        alert(`❌ Error de autenticación: ${err}`);
        return;
      }

      const data = await res.json();
      console.log("Login response:", data);
      if (!data || !data.data.token) {
        alert("❌ Credenciales inválidas o token no recibido.");
        return;
      }

      // Guardar datos usuario en localStorage
      localStorage.setItem("JWT_TOKEN", data.data.token);
      localStorage.setItem("ALIAS", data.data.alias);
      localStorage.setItem("ROL", data.data.rol);

      window.location.reload();
      alert("✅ Inicio de sesión exitoso.");
    } catch (error) {
      console.error("❌ Error al iniciar sesión:", error);
      alert("❌ Error al intentar iniciar sesión.");
    }
  };

  return (
    <>
      <button
        className="button_auth"
        type="button"
        data-bs-toggle="modal"
        data-bs-target="#loginModal">
        <span>Login</span>
      </button>

      <article className="modal" tabIndex="-1" id="loginModal">
        <div className="modal-dialog modal-dialog-centered modal-sm">
          <form className="modal-content" onSubmit={handleSubmit}>
            <header className="modal-header border-0">
              <h4 className="modal-title w-100 text-center">Bienvenido</h4>
            </header>

            <section className="modal-body">
              <div className="form-floating mb-3">
                <input
                  type="text"
                  className="form-control"
                  id="usuario"
                  name="usuario"
                  onChange={handleChange}
                  required
                  autoFocus
                />
                <label htmlFor="usuario">Usuario</label>
              </div>

              <div className="form-floating">
                <input
                  type="password"
                  className="form-control"
                  id="clave"
                  name="clave"
                  onChange={handleChange}
                  required
                />
                <label htmlFor="clave">Contraseña</label>
              </div>
            </section>

            <code className=" text-center">Credenciales en database.sql</code>

            <footer className="modal-footer border-0">
              <button type="submit" className="btn btn-outline-success mx-auto">
                Iniciar Sesión
              </button>
            </footer>
          </form>
        </div>
      </article>
    </>
  );
}

export default ModalLogin;
