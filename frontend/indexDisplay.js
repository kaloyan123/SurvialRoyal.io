
var connectiontid = sessionStorage.getItem("connectionid");    
var playername = sessionStorage.getItem("connectionname");

if(connectiontid==null){
    document.getElementById("name").defaultValue = "login to get username";
}else{
    document.getElementById("name").defaultValue = playername;
}

function joinGame(){
    if(connectiontid==null){
        sessionStorage.setItem("connectionname", "Player");
    }

    window.location.href="game.html"
}

// -success