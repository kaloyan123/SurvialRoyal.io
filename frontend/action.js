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

connection.on("startCharacters", function (id) {
   if(playerid<0){
    myGamePieces[0].SetId(id);
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
            myGamePiece.Draw();
        }
    })
    if(!isplayer){
        myGamePieces.push(new playercomponent(x, y, "red", playerWidth, playerHeight));
        myGamePieces[myGamePieces.length-1].SetId(id);
        isplayer=false;
    }
}
});
start(connection);

var myGamePieces = [];
var playerCodinateX = Math.floor(Math.random()*1000), playerCodinateY = Math.floor(Math.random()*700), 
playerWidth=30,playerHeight=30;
var characters = [] , numb=0;
var playerid=-1 , isplayer=false;

function startGame() {
    myGamePieces.push(new playercomponent(playerCodinateX, playerHeight, "blue", playerWidth, playerHeight));

    connection.invoke("InitiatePlayers").catch(function (err) {
        return console.error(err.toString());
    });
    
    myGameArea.start();
}


var myGameArea = {
    canvas : document.createElement("canvas"),
    start : function() {
        this.canvas.width = 1300;
        this.canvas.height = 800;
        this.context = this.canvas.getContext("2d");
        document.body.insertBefore(this.canvas, document.body.childNodes[0]);
        this.interval = setInterval(updateGameArea, 20);

        window.addEventListener('keydown', function (e) {
            myGameArea.keys = (myGameArea.keys || []);
            myGameArea.keys[e.keyCode] = (e.type == "keydown");
        })
        window.addEventListener('keyup', function (e) {
            myGameArea.keys[e.keyCode] = (e.type == "keydown");
        })
    },
    clear : function() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
}
  
function playercomponent(x, y, color, width, height) {
    this.width = width;
    this.height = height;
    this.speedX = 0;
    this.speedY = 0;
    this.x = x;
    this.y = y;   
    // this.id =  id; 
    
    this.SetId = function(id) {
        this.id =  id; 
    }

    this.Draw = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(this.x, this.y, this.width, this.height);
    }  
    this.newPosUpdate = function() {
        this.x += this.speedX;
        this.y += this.speedY;  

        playerCodinateX = this.x;
        playerCodinateY = this.y;

        connection.invoke("UpdatePlayers", this.x, this.y, playerid).catch(function (err) {
            return console.error(err.toString());
        });
    }   
}

function updateGameArea() {
    myGameArea.clear();

    myGamePieces.forEach(myGamePiece=>{
        if(playerid==myGamePiece.id){

            myGamePiece.speedX = 0;
            myGamePiece.speedY = 0;    
            if (myGameArea.keys && myGameArea.keys[37]) {myGamePiece.speedX = -2; }
            if (myGameArea.keys && myGameArea.keys[39]) {myGamePiece.speedX = 2; }
            if (myGameArea.keys && myGameArea.keys[38]) {myGamePiece.speedY = -2; }
            if (myGameArea.keys && myGameArea.keys[40]) {myGamePiece.speedY = 2; }
            
            myGamePiece.newPosUpdate(); 
        }
        myGamePiece.Draw();
        
    })
}


/*
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

<div style="text-align:center;width:480px;">
        <button onmousedown="moveup()" onmouseup="clearmove()" ontouchstart="moveup()">UP</button><br><br>
        <button onmousedown="moveleft()" onmouseup="clearmove()" ontouchstart="moveleft()">LEFT</button>
        <button onmousedown="moveright()" onmouseup="clearmove()" ontouchstart="moveright()">RIGHT</button><br><br>
        <button onmousedown="movedown()" onmouseup="clearmove()" ontouchstart="movedown()">DOWN</button>
</div>
*/