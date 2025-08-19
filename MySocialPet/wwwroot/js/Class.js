// /wwwroot/js/Class.js
document.addEventListener("DOMContentLoaded", function () {

    // --- Delegación global para formularios de borrado ---
    document.addEventListener("submit", async function (ev) {
        const form = ev.target.closest(".delete-photo-form");
        if (!form) return;

        ev.preventDefault();

        if (!confirm("¿Estás seguro de que quieres eliminar esta foto?")) return;

        try {
            // Preparamos el FormData con los datos del form
            const formData = new FormData(form);

            // Añadimos el AntiForgeryToken (uno por página es suficiente)
            const token = document.querySelector("input[name='__RequestVerificationToken']")?.value;
            if (token) {
                formData.append("__RequestVerificationToken", token);
            }

            // Leemos la URL del atributo data-delete-url
            const url = form.dataset.deleteUrl;
            if (!url) throw new Error("No se encontró la URL de borrado");

            const resp = await fetch(url, {
                method: "POST",
                body: formData,
                credentials: "same-origin",
                headers: { "X-Requested-With": "XMLHttpRequest" }
            });

            if (!resp.ok) {
                const text = await resp.text();
                throw new Error(`Error ${resp.status}: ${text.substring(0, 200)}`);
            }

            // ✅ Recargar la página completa después de borrar
            window.location.reload();

        } catch (err) {
            console.error("Error al eliminar la foto:", err);
            alert("No se pudo eliminar la foto. " + (err?.message || ""));
        }
    });

    // --- Configuración de Fancybox ---
    Fancybox.bind("[data-fancybox]", {
        Carousel: { infinite: true },
        Toolbar: {
            display: {
                left: ["infobar"],
                middle: [],
                right: ["slideshow", "fullscreen", "thumbs", "close"]
            }
        },
        caption: function (fancybox, carousel, slide) {
            return slide.caption || "";
        }
    });

});
