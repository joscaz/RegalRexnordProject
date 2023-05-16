const token = localStorage.getItem('token');
if (!token) {
    window.location.href = 'login.html';
}

const barChart = document.getElementById('barChart').getContext('2d');
const lineChart = document.getElementById('lineChart').getContext('2d');

const api = axios.create({
    baseURL: 'http://127.0.0.1:8000/api/',
    headers: { Authorization: `Token ${token}` },
});

async function loadDashboard() {
    try {
        const players = await api.get('users/');
        const scores = await api.get('scores/');



        // Gráfica de barras
        const barChartData = {
            labels: players.data.map(player => player.first_name),
            datasets: [
                {
                    label: 'Total score',
                    data: players.data.map(player => {
                        return player.total_score;
                    }),
                    backgroundColor: 'rgba(75, 192, 192, 0.2)',
                    borderColor: 'rgba(75, 192, 192, 1)',
                    borderWidth: 1,
                },
            ],
        };

        const barChartConfig = {
            type: 'bar',
            data: barChartData,
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                },
            },
        };

        new Chart(barChart, barChartConfig);

        // Gráfica de líneas
        const lineChartData = {
            labels: players.data.map(player => player.first_name),
            datasets: [
                {
                    label: 'Average Score',
                    data: players.data.map(player => {
                        return player.average_score;
                    }),
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderColor: 'rgba(255, 99, 132, 1)',
                    borderWidth: 1,
                    fill: false,
                },
            ],
        };

        const lineChartConfig = {
            type: 'line',
            data: lineChartData,
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                    },
                },
            },
        };

        new Chart(lineChart, lineChartConfig);

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


async function loadLastLoginTable() {
    try {
        const players = await api.get('users/');
        const lastLoginTableBody = document.getElementById('lastLoginTableBody');

        players.data.forEach((player, index) => {
            const row = document.createElement('tr');

            const numberCell = document.createElement('th');
            numberCell.setAttribute('scope', 'row');
            numberCell.textContent = index + 1;
            row.appendChild(numberCell);

            const playerNameCell = document.createElement('td');
            const playerNameSpan = document.createElement('span');
            playerNameSpan.textContent = player.first_name;
            playerNameCell.appendChild(playerNameSpan);

            const playerEmailSpan = document.createElement('span');
            playerEmailSpan.textContent = player.email;
            playerEmailSpan.style.marginLeft = '10px'; // Agrega un margen a la izquierda del correo electrónico
            playerNameCell.appendChild(playerEmailSpan);

            row.appendChild(playerNameCell);

            const lastLoginCell = document.createElement('td');
            lastLoginCell.textContent = player.last_login;
            row.appendChild(lastLoginCell);

            lastLoginTableBody.appendChild(row);
        });
    } catch (error) {
        alert('Error al cargar los datos de la tabla');
    }
}


// Carga el dashboard al cargar la página
loadDashboard();
// Carga la tabla de últimos inicios de sesión
loadLastLoginTable();