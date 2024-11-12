console.log("El archivo login.js está cargado correctamente.");

// Manejador para el formulario de inicio de sesión
document.getElementById('loginForm').addEventListener('submit', async function (e) {
    e.preventDefault(); // Evitar el envío del formulario

    const correoElectronico = document.getElementById('correoElectronico').value;
    const password = document.getElementById('password').value;

    if (!correoElectronico || !password) {
        alert("Por favor, complete ambos campos antes de iniciar sesión.");
        return;
    }

    const requestData = {
        CorreoElectronico: correoElectronico,
        Password: password
    };

    try {
        console.log("Iniciando sesión con los datos:", requestData);
        const response = await fetch('http://localhost:5085/api/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            const result = await response.json();
            console.log('Respuesta del servidor:', result);

            if (result.role === "Empleado") {
                window.location.href = "/vista/empleado.html";
            } else if (result.role === "Cliente") {
                window.location.href = "/vista/cliente.html";
            } else {
                console.log("Rol desconocido:", result.role);
            }
        } else {
            console.log("Error: Credenciales inválidas");
            alert("Credenciales inválidas");
        }
    } catch (error) {
        console.error("Error al realizar la solicitud:", error);
        alert("Error al conectarse con el servidor.");
    }
});

// Manejador para el botón de registro
document.getElementById('signUpBtn').addEventListener('click', async function (e) {
    e.preventDefault(); // Evitar el envío predeterminado del formulario

    const nombre = document.getElementById('nombre').value;
    const apellido = document.getElementById('apellido').value;
    const sexo = document.getElementById('sexo').value;
    const fechaNacimiento = document.getElementById('fechaNacimiento').value;
    const telefono = document.getElementById('telefono').value;
    const correoElectronico = document.getElementById('correoElectronicoSignup').value;
    const direccion = document.getElementById('direccion').value;
    const password = document.getElementById('password-signup').value;

    if (!nombre || !apellido || !sexo || !correoElectronico || !password) {
        alert("Por favor, complete todos los campos obligatorios.");
        return;
    }

    const requestData = {
        Nombre: nombre,
        Apellido: apellido,
        Sexo: sexo,
        FechaNacimiento: fechaNacimiento || null,
        Telefono: telefono,
        CorreoElectronico: correoElectronico,
        Direccion: direccion,
        Password: password
    };

    try {
        console.log("Intentando registrar con los datos:", requestData);

        // Enviar la solicitud POST a la API de registro
        const response = await fetch('http://localhost:5085/api/cliente/registrar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(requestData)
        });

        if (response.ok) {
            console.log("Registro exitoso");
            alert("Registro exitoso. Ahora puedes iniciar sesión.");
            document.getElementById('goLeft').click(); // Redirigir al login después del registro
        } else {
            const errorResponse = await response.text();
            console.error("Error en la respuesta de registro:", errorResponse);
            alert("Error al registrar. Inténtalo de nuevo.");
        }
    } catch (error) {
        console.error("Error al realizar la solicitud de registro:", error);
        alert("Error al conectarse con el servidor.");
    }
});
