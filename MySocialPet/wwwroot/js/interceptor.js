document.addEventListener("DOMContentLoaded", function () {
    const loader = document.getElementById("globalLoader");

    // Al enviar formularios
    document.querySelectorAll("form").forEach(form => {
    form.addEventListener("submit", () => {
        loader.style.display = "flex";
    });
    });

    // Al hacer click en links que no son anchors (#)
    document.querySelectorAll("a[href]").forEach(link => {
    link.addEventListener("click", e => {
        if (link.getAttribute("href").startsWith("#")) return;
        loader.style.display = "flex";
    });
    });

    // Ocultar loader al terminar de cargar la página
    window.addEventListener("pageshow", () => {
    loader.style.display = "none";
    });
});
