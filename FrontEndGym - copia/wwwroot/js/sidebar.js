document.addEventListener("mousemove", function (event) {
    const sidebar = document.querySelector(".nav-scrollable");
    const mainContent = document.querySelector(".main-content");
    const navLinks = document.querySelectorAll(".nav-scrollable .nav-link");

    if (sidebar && mainContent) {
        if (event.clientX < 50) { // Mostrar sidebar
            sidebar.classList.add("visible");
            mainContent.classList.add("sidebar-visible");
            navLinks.forEach(link => link.classList.add("link-visible"));
        } else if (event.clientX > 300) { // Ocultar sidebar
            sidebar.classList.remove("visible");
            mainContent.classList.remove("sidebar-visible");
            navLinks.forEach(link => link.classList.remove("link-visible"));
        }
    }
});
