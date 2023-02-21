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
    playrsprites[0].setId(id);
    playerid=id;
   }
    
    console.log('joined');
});
connection.on("drawCharacters", function (X, Y, Hp, id) {
    if(playerid==id){
        myGamePieces[0].health = Hp;
    }
    else{
        myGamePieces.forEach(myGamePiece=>{
            if(myGamePiece.id==id){    
                isplayer=true;
                
                myGamePiece.x = X;
                myGamePiece.y = Y;   
                myGamePiece.health = Hp;
            }
        })
        playrsprites.forEach(playrsprite=>{
            if(playrsprite.id==id){    
                playrsprite.x = X-10;
                playrsprite.y = Y-10;   
            }
        })
        if(!isplayer){
            myGamePieces.push(new playercomponent(X, Y, "red", playerWidth, playerHeight, Hp));

            playrsprites.push(new Sprite({x:X-10,y:Y-10,width:playerWidth+20,height:playerHeight+20,
                imgSrc:'./image/Coolplayer.webp'}));

            myGamePieces[myGamePieces.length-1].SetId(id);
            playrsprites[myGamePieces.length-1].setId(id);
            isplayer=false;
        }
    }
});

connection.on("Attacked", function (id) {
    
    if(playerid==id){
     console.log('attacked');
    }
 });

 connection.on("drawObjects", function (Xes,Yes,Ids,Types) {
    for(i=0;i<Xes.length;i++){
        if(Types[i]=="tree"){
            stationObjects.push(new objectcomponent(Xes[i], Yes[i], "green", 100, 100, Ids[i], Types[i]));

            objectSprites.push(new Sprite({x:Xes[i]-10,y:Yes[i]-10,width:120,height:120,
            imgSrc:'./image/tree.png'}));

        }else{
            stationObjects.push(new objectcomponent(Xes[i], Yes[i], "gray", 100, 100, Ids[i], Types[i]));

            objectSprites.push(new Sprite({x:Xes[i]-30,y:Yes[i]-35,width:160,height:160,
            imgSrc:'./image/rock.png'}));
        }
        console.log("object drawn");
    }
    /*
    Xes.forEach(x => {
        console.log(x);
   });
   */
});

connection.on("drawEnteties", function (Xes,Yes,Hps,Ids,Types) {
    
    mobileEntities.forEach(mobileEntity=>{
        mobileEntity.x = Xes[numbr];
        mobileEntity.y = Yes[numbr];   
        mobileEntity.health = Hps[numbr];   
        numbr++;
    })
    numbr=0;
    entitySprites.forEach(entitySprite=>{
        entitySprite.x = Xes[numbr];
        entitySprite.y = Yes[numbr];   
        numbr++;
    })
    if(numbr<Xes.length){
        for(i=numbr;i<Xes.length;i++){
            if(Types[i]=="rabit"){
                mobileEntities.push(new entitycomponent(Xes[i], Yes[i], "yellow", 30, 30, Ids[i], Types[i],Hps[i]));

                entitySprites.push(new Sprite({x:Xes[i],y:Yes[i],width:30,height:30,
                    imgSrc:'./image/bunny.png'}));
            }else{
                mobileEntities.push(new entitycomponent(Xes[i], Yes[i], "pink", 50, 50, Ids[i], Types[i],Hps[i]));

                entitySprites.push(new Sprite({x:Xes[i],y:Yes[i],width:50,height:50,
                    imgSrc:'./image/pig_maybe.png'}));
            }
            console.log("entity drawn");
        }
    }
    numbr=0;

});


start(connection);

var myGamePieces = [], //players
stationObjects= [], mobileEntities = [],background,playrsprites = [],objectSprites=[],entitySprites=[];
var playerCodinateX = Math.floor(Math.random()*1000), playerCodinateY = Math.floor(Math.random()*700), 
playerWidth=50,playerHeight=50,playerspeed=5,playerHealth=100,playerReach=50;
var centerX= 600, centerY=300, cameraX = playerCodinateX-centerX, cameraY = playerCodinateY-centerY,
canvassezeX=1300,canvassezeY=800,mapX = -1900,mapY = -1100,mapendX=2600,mapendY=1600;
var playerid=-1 , isplayer=false ,numbr=0;


//  * * *
//  * * *
//  * * *
function startGame() {
    myGamePieces.push(new playercomponent(playerCodinateX, playerCodinateY, "blue", playerWidth, playerHeight, playerHealth));
    
    playrsprites.push(new Sprite({x:playerCodinateX,y:playerCodinateY,width:playerWidth,height:playerHeight,
        imgSrc:'./image/Coolplayer.webp'}));

   background = new Sprite({x:mapX, y:mapY, width:mapendX-mapX, height:mapendY-mapY, imgSrc:'./image/map_grass.png'})

    connection.invoke("InitiatePlayers",playerCodinateX ,playerCodinateY ,playerHealth).catch(function (err) {
        return console.error(err.toString());
    });

    connection.invoke("GetImobileObj").catch(function (err) {
        return console.error(err.toString());
    });
    
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
            connection.invoke("PlayerAttack", playerCodinateX, playerCodinateY, playerid).catch(function (err) {
                return console.error(err.toString());
            });
            myGamePieces[0].DrawAttavkBoxrl();
        })
    },
    clear : function() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
}
  
function playercomponent(x, y, color, width, height, health) {
    this.width = width;
    this.height = height;
    this.speedX = 0;
    this.speedY = 0;
    this.x = x;
    this.y = y;
    this.health = health;   

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
    this.DrawAttavkBoxrl = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = "red";
        ctx.fillRect(this.x-cameraX-playerReach, this.y-cameraY-playerReach, this.width+(playerReach*2), this.height+(playerReach*2));
    }  
    this.DrawHealth = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = "black";
        ctx.fillRect(this.x-cameraX, this.y-cameraY+this.height+10, this.width, 10);
        ctx.fillStyle = "green";
        ctx.fillRect(this.x-cameraX, this.y-cameraY+this.height+10, this.health/2, 10);
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
function objectcomponent(x, y, color, width, height, id , type) {
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

function entitycomponent(x, y, color, width, height, id , type, health) {
    this.width = width;
    this.height = height;
    this.x = x;
    this.y = y;   
    this.id = id;   
    this.type = type; 
    this.health = health; 
    

    this.Draw = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(this.x - cameraX, this.y - cameraY, this.width, this.height);
    }  
    this.DrawHealth = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = "black";
        ctx.fillRect(this.x-cameraX, this.y-cameraY+this.height+10, this.width, 10);
        ctx.fillStyle = "green";
        ctx.fillRect(this.x-cameraX, this.y-cameraY+this.height+10, this.health/2, 10);
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
        this.id =  0; 
    }
    setId(id){
        this.id = id;   
    }

    draw(){
        myGameArea.context.drawImage(this.image,this.x-cameraX,this.y-cameraY,this.width,this.height);
    }
    drawrl(){
        myGameArea.context.drawImage(this.image,centerX-10,centerY-10,this.width+20,this.height+20);
    }
}

function updateGameArea() {
    myGameArea.clear();
    background.draw();
    //console.log(value_);


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

           // myGamePiece.Drawrl();
           myGamePiece.DrawHealth();
        }else{
        // myGamePiece.Draw();
        myGamePiece.DrawHealth();
        }

    })
    playrsprites.forEach(playrsprite=>{
        if(playerid==playrsprite.id){ 
           playrsprite.drawrl();
        }else{
            playrsprite.draw();
        }
    })
    
    connection.invoke("GetMobileEntity").catch(function (err) {
        return console.error(err.toString());
    });
    
    
    mobileEntities.forEach(otherentety=>{
        otherentety.Draw();
        otherentety.DrawHealth();
    })
    
    /*
    stationObjects.forEach(otherentety=>{
        otherentety.Draw();
    })
    */

    entitySprites.forEach(entitySprites=>{
        entitySprites.draw();
    })

    objectSprites.forEach(objectSprite=>{
        objectSprite.draw();
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