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

connection.on("logtest", function () {
    console.log("working");
});

connection.on("ReciveLogins", function (id, name) {

    console.log(id,name);

    sessionStorage.setItem("connectionid", id);

    sessionStorage.setItem("connectionname", name);

    window.location.href = "index.html";
});

start(connection);


function First() {
    connection.invoke("LogSmt").catch(function (err) {
        return console.error(err.toString());
    });
}

function SubmitLogin() {

    var name = document.getElementById('name').value;
    
    var password = document.getElementById('password').value;

    // console.log(name,password);

    connection.invoke("Login", name, password).catch(function (err) {
        return console.error(err.toString());
    });

}

function SubmitRegister() {

    var name = document.getElementById('name').value;
    
    var password = document.getElementById('password').value;

 //   console.log(name,password);

    connection.invoke("Register", name, password).catch(function (err) {
        return console.error(err.toString());
    });

}

// onclick="window.location.href='game.html';"  
// http://localhost:5500/
