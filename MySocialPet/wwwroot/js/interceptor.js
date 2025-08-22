document.addEventListener("DOMContentLoaded", function () {
    const loader = document.getElementById("globalLoader");
    if (!loader) return;

    let timer = null;
    const FAILSAFE_MS = 3000; // 10s

    function show() {
        clearTimeout(timer);
        loader.style.display = "flex";
        // Auto-ocultar por si algo se queda colgado
        timer = setTimeout(() => {
            loader.style.display = "none";
            // Si tienes toasts, podrías avisar aquí:
            // if (window.toastError) toastError("Tiempo de espera", "La operación tardó demasiado");
        }, FAILSAFE_MS);
    }

    function hide() {
        clearTimeout(timer);
        loader.style.display = "none";
    }

    // Al enviar formularios (misma lógica que tenías)
    document.querySelectorAll("form").forEach(form => {
        form.addEventListener("submit", () => {
            show();
        });
    });

    // Al hacer click en links que no son anchors (#) (misma lógica que tenías)
    document.querySelectorAll("a[href]").forEach(link => {
        link.addEventListener("click", () => {
            const href = link.getAttribute("href") || "";
            if (href.startsWith("#")) return;
            show();
        });
    });

    // Ocultar loader al terminar de cargar/volver del caché
    window.addEventListener("pageshow", hide);
    // Y también si se navega fuera o hay errores no capturados
    window.addEventListener("pagehide", hide);
    window.addEventListener("error", hide);
    window.addEventListener("unhandledrejection", hide);

    // Opcional: helpers globales por si quieres llamarlos manualmente
    window.showGlobalLoader = show;
    window.hideGlobalLoader = hide;
});