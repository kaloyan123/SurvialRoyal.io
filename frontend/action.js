var connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/drawDotHub").build();
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
connection.on("drawCharacters", function (x, y, id, servertiks) {
    if(playerid!=id){
        myGamePieces.forEach(myGamePiece=>{
            if(myGamePiece.id==id){    
                isplayer=true;
                
                myGamePiece.x = x;
                myGamePiece.y = y;   
            }
        })
        if(!isplayer){
            myGamePieces.push(new playercomponent(x, y, "red", playerWidth, playerHeight));
            myGamePieces[myGamePieces.length-1].SetId(id);
            isplayer=false;
        }
    
    }
   // console.log(servertiks);

});

connection.on("Attacked", function (id) {
    
    if(playerid==id){
     console.log('attacked');
    }
 });

 connection.on("drawObjects", function (Xes,Yes,Ids,Types) {
    for(i=0;i<Xes.length;i++){
        if(Types[i]=="tree"){
            stationObjects.push(new othercomponent(Xes[i], Yes[i], "green", 100, 100, Ids[i], Types[i]));
        }else{
            stationObjects.push(new othercomponent(Xes[i], Yes[i], "gray", 100, 100, Ids[i], Types[i]));
        }
        console.log("object drawn");
    }
    

    /*
    console.log("sdlojk");
    Xes.forEach(x => {
        console.log(x);
   });
   */
});

connection.on("drawEnteties", function (Xes,Yes,Ids,Types) {
    
    mobileEntities.forEach(mobileEntity=>{
       
        mobileEntity.x = Xes[numbr];
        mobileEntity.y = Yes[numbr];   
        //console.log(mobileEntity.x, Xes[numbr],"   ",mobileEntity.y, Yes[numbr]);
        //console.log(mobileEntity.x,mobileEntity.y);
       // console.log(numbr);
        numbr++;
    })
    //console.log(mobileEntities.length);
    if(numbr<Xes.length){
        for(i=numbr;i<Xes.length;i++){
            if(Types[i]=="rabit"){
                mobileEntities.push(new othercomponent(Xes[i], Yes[i], "yellow", 100, 100, Ids[i], Types[i]));
            }else{
                mobileEntities.push(new othercomponent(Xes[i], Yes[i], "yellow", 100, 100, Ids[i], Types[i]));
            }
            console.log("entity drawn");
        }
        
    }
    numbr=0;

});


start(connection);

var myGamePieces = [], //players
stationObjects= [], mobileEntities = [],background,playrsprites = [];
var playerCodinateX = Math.floor(Math.random()*1000), playerCodinateY = Math.floor(Math.random()*700), 
playerWidth=50,playerHeight=50,playerspeed=5;
var centerX= 600, centerY=300, cameraX = playerCodinateX-centerX, cameraY = playerCodinateY-centerY,
canvassezeX=1300,canvassezeY=800,mapX = -1900,mapY = -1100,mapendX=2600,mapendY=1600;
var playerid=-1 , isplayer=false ,numbr=0;


//  * * *
//  * * *
//  * * *
function startGame() {
    myGamePieces.push(new playercomponent(playerCodinateX, playerCodinateY, "blue", playerWidth, playerHeight));
    
    playrsprites.push(new Sprite({x:playerCodinateX,y:playerCodinateY,width:playerWidth,height:playerHeight,
        imgSrc:'./image/playrsircle.png'}));

   //otherentety = new othercomponent(100, 100, "gray", playerWidth, playerHeight);

   background = new Sprite({x:mapX, y:mapY, width:mapendX-mapX, height:mapendY-mapY, imgSrc:'./image/map_grass.png'})

    connection.invoke("InitiatePlayers",playerCodinateX ,playerHeight).catch(function (err) {
        return console.error(err.toString());
    });

    connection.invoke("GetImobileObj").catch(function (err) {
        return console.error(err.toString());
    });

   // mobileEntities.push(new othercomponent(100, 100, "yellow", 100, 100, 0, "rabit"));
    
    myGameArea.start();
}


var myGameArea = {
    canvas : document.createElement("canvas"),
    start : function() {
        this.canvas.width = canvassezeX;
        this.canvas.height = canvassezeY;
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

        window.addEventListener('click', function (e) {
            console.log("sdd");
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

    this.Drawrl = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(centerX, centerY, this.width, this.height);
    }  
    this.Draw = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(this.x-cameraX, this.y-cameraY, this.width, this.height);
    }  
    this.newPosUpdate = function() {
        this.x += this.speedX;
        this.y += this.speedY;  

        playerCodinateX = this.x;
        playerCodinateY = this.y;

        cameraX = playerCodinateX-centerX;
        cameraY = playerCodinateY-centerY;

        connection.invoke("UpdatePlayers", this.x, this.y, playerid).catch(function (err) {
            return console.error(err.toString());
        });
    }   
}
function othercomponent(x, y, color, width, height, id , type) {
    this.width = width;
    this.height = height;
    this.x = x;
    this.y = y;   
    this.id = id;   
    this.type = type; 

    this.Draw = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(this.x - cameraX, this.y - cameraY, this.width, this.height);
    }  
    
}

class Sprite{
    constructor({x,y,width,height,imgSrc}){
        this.x=x;
        this.y=y;
        this.width = width;
        this.height = height;
        this.image = new Image();
        this.image.src = imgSrc;
    }

    draw(){
        myGameArea.context.drawImage(this.image,this.x-cameraX,this.y-cameraY,this.width,this.height);
    }
    drawrl(){
        myGameArea.context.drawImage(this.image,centerX,centerY,this.width,this.height);
    }
    update(newx,newy){
        this.x=newx;
        this.y=newy;
    }
}

function updateGameArea() {
    myGameArea.clear();
    background.draw();


    myGamePieces.forEach(myGamePiece=>{
        if(playerid==myGamePiece.id){

            myGamePiece.speedX = 0;
            myGamePiece.speedY = 0;    
            if (myGameArea.keys && myGameArea.keys[37]) {myGamePiece.speedX = -playerspeed; }
            if (myGameArea.keys && myGameArea.keys[39]) {myGamePiece.speedX = playerspeed; }
            if (myGameArea.keys && myGameArea.keys[38]) {myGamePiece.speedY = -playerspeed; }
            if (myGameArea.keys && myGameArea.keys[40]) {myGamePiece.speedY = playerspeed; }
            
            if (myGameArea.keys && myGameArea.keys[65]) {myGamePiece.speedX = -playerspeed; }
            if (myGameArea.keys && myGameArea.keys[68]) {myGamePiece.speedX = playerspeed; }
            if (myGameArea.keys && myGameArea.keys[87]) {myGamePiece.speedY = -playerspeed; }
            if (myGameArea.keys && myGameArea.keys[83]) {myGamePiece.speedY = playerspeed; }

            myGamePiece.newPosUpdate(); 
            
            //playrsprite[0].update(playerCodinateX,playerCodinateY);

            myGamePiece.Drawrl();

            playrsprites[0].drawrl(playerCodinateX,playerCodinateY);
        }else{
        myGamePiece.Draw();
        }
       // playrsprite.draw();

    })
    
    connection.invoke("GetMobileEntity").catch(function (err) {
        return console.error(err.toString());
    });
    
    
    mobileEntities.forEach(otherentety=>{
        //otherentety.x=otherentety.x+0.5;
        otherentety.Draw();
    })

    stationObjects.forEach(otherentety=>{
        otherentety.Draw();
    })
    
}
function onmousemove (e) {
    console.log("sd");
    /*
    var dx = e.pageX - centerX;
    var dy = e.pageY - centerY;
    var theta = Math.atan2(dy, dx);
    drawArrow(theta);
    */
};

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
        <button onmousedown="moveright()" onmouseup="clearmove()" ontouchstart="moveright()">RIGHT
        </button><br><br>
        <button onmousedown="movedown()" onmouseup="clearmove()" ontouchstart="movedown()">DOWN</button>
</div>
*/