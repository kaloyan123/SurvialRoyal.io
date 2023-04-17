var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/loginuser").build();
function start(connection) {
    const starting = async () => {
        try {
            await connection.start();
        } catch (e) {
            console.log(e);
        }
    };
    return starting().then(() => {startGame()});
}

start(connection);
