var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/loginuser").build();
function start(connection) {
    const starting = async () => {
        try {
            await connection.start();
        } catch (e) {
            console.log(e);
        }
    };
    return starting().then(() => {First()});
}

connection.on("logtest", function () 
{
    console.log("working");
});

connection.on("ReceiveValues", function (id, name, TimesDied , HighestScore) {
 console.log(id, name, TimesDied , HighestScore);
    document.getElementById("name").defaultValue = name;
    document.getElementById("TimesDied").defaultValue = TimesDied;
    document.getElementById("HighestScore").defaultValue = HighestScore;

});

start(connection);

var connectiontid = sessionStorage.getItem("connectionid");
var playername = sessionStorage.getItem("connectionname");

function First() {
    connection.invoke("Account", parseInt(connectiontid)).catch(function (err) {
        return console.error(err.toString());

    });
}