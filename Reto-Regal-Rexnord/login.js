document.getElementById('login-form').addEventListener('submit', async (e) => {
    e.preventDefault();

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const data = JSON.stringify({
        email: email,
        password: password
    });

    try {
        const response = await axios.post('http://127.0.0.1:8000/api/users/log_in/', data, { headers: { "Content-Type": "application/json" } });
        const token = response.data.token;

        if (token) {
            localStorage.setItem('token', token);
            window.location.href = 'dashboard.html';
        } else {
            alert('Error al iniciar sesión. Por favor, verifica tus credenciales.');
        }
    } catch (error) {
        console.error(error);
        alert('Error al iniciar sesión. Por favor, verifica tus credenciales.');
    }
});
