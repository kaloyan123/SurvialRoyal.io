var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:8000/drawDotHub").build();
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

connection.on("startCharacters", function (x, y, id) {
    myGamePieces.push(new playercomponent(x, y, "red", playerWidth, playerHeight, id));
    console.log(id);
    if(myGamePieces.length==1){
        playerid=id;
    }
    console.log('joined');
});
connection.on("drawCharacters", function (x, y, id) {
    if(playerid!=id){
    myGamePieces.forEach(myGamePiece=>{
        if(myGamePiece.id==id){    
            isplayer=true;
            
            myGamePiece.x = x;
            myGamePiece.y = y;   
            myGamePiece.update();
        }
    })
    if(!isplayer){
        myGamePieces.push(new playercomponent(x, y, "red", playerWidth, playerHeight, id));
        isplayer=false;
    }
}
});
start(connection);

var myGamePieces = [];
var playerCodinateX = 10, playerCodinateY = 120, playerWidth=30,playerHeight=30;
var characters = [];
var playerid=-1 , isplayer=false;

function startGame() {
    connection.invoke("InitiatePlayers", playerCodinateX, playerCodinateY).catch(function (err) {
        return console.error(err.toString());
    });

    myGameArea.start();
}


var myGameArea = {
    canvas : document.createElement("canvas"),
    start : function() {
        this.canvas.width = 480;
        this.canvas.height = 270;
        this.context = this.canvas.getContext("2d");
        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
        this.interval = setInterval(updateGameArea, 20);
    },
    clear : function() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
}
  
function playercomponent(x, y, color, width, height, id) {
    this.width = width;
    this.height = height;
    this.speedX = 0;
    this.speedY = 0;
    this.x = x;
    this.y = y;   
    this.id =  id; 

    this.update = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(this.x, this.y, this.width, this.height);
        if(this.id==playerid){
            playerCodinateX = this.x;
            playerCodinateY = this.y;
        }
    }  
    this.newPosUpdate = function() {
        this.x += this.speedX;
        this.y += this.speedY;  

        connection.invoke("UpdatePlayers", this.x, this.y, playerid).catch(function (err) {
            return console.error(err.toString());
        });
    }   
}

function updateGameArea() {
    myGameArea.clear();
    
    myGamePieces.forEach(myGamePiece=>{

        if(playerid==myGamePiece.id){
            myGamePiece.newPosUpdate(); 
        }
        myGamePiece.update();
        
    })
}



function moveup() {
    myGamePieces[0].speedY = -1; 
}

function movedown() {
    myGamePieces[0].speedY = 1; 
}

function moveleft() {
    myGamePieces[0].speedX = -1; 
}

function moveright() {
    myGamePieces[0].speedX = 1; 
}

function clearmove() {
    myGamePieces[0].speedX = 0; 
    myGamePieces[0].speedY = 0; 
}