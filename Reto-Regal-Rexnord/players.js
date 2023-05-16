const token = localStorage.getItem('token');
if (!token) {
    window.location.href = 'login.html';
}

const api = axios.create({
    baseURL: 'http://127.0.0.1:8000/api/',
    headers: { Authorization: `Token ${token}` },
});

async function loadPlayers() {
    try {
        const players = await api.get('users/');
        const playerTableBody = document.getElementById('playerTableBody');

        players.data.forEach((player, index) => {
            const row = document.createElement('tr');

            const numberCell = document.createElement('th');
            numberCell.setAttribute('scope', 'row');
            numberCell.textContent = index + 1;
            row.appendChild(numberCell);

            const firstNameCell = document.createElement('td');
            firstNameCell.textContent = player.first_name;
            row.appendChild(firstNameCell);

            // Agrega la celda del email en lugar del apellido
            const emailCell = document.createElement('td');
            emailCell.textContent = player.email;
            row.appendChild(emailCell);

            const scoreCell = document.createElement('td');
            scoreCell.textContent = player.total_score;
            row.appendChild(scoreCell);

            playerTableBody.appendChild(row);
        });
    } catch (error) {
        alert('Error al cargar los datos');
    }
}

async function logout() {
    try {
        await api.post('users/log_out/');
        localStorage.removeItem('token');
        window.location.href = 'login.html';
    } catch (error) {
        console.error('Error al cerrar sesión:', error);
    }
}

// Carga la lista de jugadores al cargar la página
loadPlayers();
