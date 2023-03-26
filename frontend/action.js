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
    players[0].SetId(id);
    playrsprites[0].setId(id);
    playerid=id;
   }
    
    console.log('joined');
});
connection.on("drawCharacters", function (X, Y, Hp, Points, Wood, Stone, id, angle) { 
    if(playerid==id){
        players[0].health = Hp;
        players[0].points = Points[playerid];
        players[0].wood = Wood;
        players[0].stone = Stone;
    }
    else{
        players.forEach(player=>{
            if(player.id==id){    
                isplayer=true;
                
                player.x = X;
                player.y = Y;   
                player.health = Hp;
            }
            player.points = Points[player.id];
        })
        playrsprites.forEach(playrsprite=>{
            if(playrsprite.id==id){    
                playrsprite.x = X-10;
                playrsprite.y = Y-10;  
                
                playrsprite.angle = angle; 
            }
        })
        
        if(!isplayer){
            players.push(new playercomponent(X, Y, "red", playerSize, playerSize, Hp));

            playrsprites.push(new Sprite({x:X,y:Y,width:playerSize,height:playerSize,
                imgSrc:'./image/Coolplayer.png'}));

            players[players.length-1].SetId(id);
            playrsprites[players.length-1].setId(id);
            
        }
        isplayer=false;
        
    }
});

connection.on("Attacked", function (id) {
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
});

connection.on("drawEnteties", function (Xes,Yes,Hps,Ids,Types,Angles) {
    mobileEntities.forEach(mobileEntity=>{
        mobileEntity.x = Xes[numbr];
        mobileEntity.y = Yes[numbr];   
        mobileEntity.health = Hps[numbr];   
        numbr++;
    })
    numbr=0;
    entitySprites.forEach(entitySprite=>{
        entitySprite.x = Xes[numbr]-entitySprite.spriteofsetx;
        entitySprite.y = Yes[numbr]-entitySprite.spriteofsety;   
        entitySprite.angle = Angles[numbr];  
        numbr++;
    }) 
    if(numbr<Xes.length){ 
        for(i=numbr;i<Xes.length;i++){
            if(Types[i]=="rabit"){
                mobileEntities.push(new entitycomponent(Xes[i], Yes[i], "yellow", 30, 30, Ids[i], Types[i],Hps[i]));

                entitySprites.push(new Sprite({x:Xes[i]-5,y:Yes[i]-5,width:25,height:25,spriteofsetx:5,spriteofsety:5,
                    imgSrc:'./image/bunny.png'}));//bunnycrop.png
            }else if(Types[i]=="pig"){
                mobileEntities.push(new entitycomponent(Xes[i]-2, Yes[i]-2, "pink", 50, 50, Ids[i], Types[i],Hps[i]));


                entitySprites.push(new Sprite({x:Xes[i],y:Yes[i],width:35,height:55,spriteofsetx:0,spriteofsety:10,
                    imgSrc:'./image/Pig.png'}));
            }else if(Types[i]=="cow"){
                mobileEntities.push(new entitycomponent(Xes[i], Yes[i], "pink", 60, 60, Ids[i], Types[i],Hps[i]));


                entitySprites.push(new Sprite({x:Xes[i]-5,y:Yes[i],width:55,height:50,spriteofsetx:5,spriteofsety:10,
                    imgSrc:'./image/Bull.png'}));//Bull.webp   Bullcrop.png
            }else{
                mobileEntities.push(new entitycomponent(Xes[i], Yes[i], "red", 50, 50, Ids[i], Types[i],Hps[i]));


                entitySprites.push(new Sprite({x:Xes[i]-5,y:Yes[i],width:50,height:50,spriteofsetx:0,spriteofsety:0,
                    imgSrc:'./image/wolf.png'}));
            }
            console.log("entity drawn");
        }
    }
    numbr=0;
});


start(connection);

var players = [], stationObjects= [], mobileEntities = [],
background, backbackground, playrsprites = [], objectSprites=[], entitySprites=[];
var mapX = 0, mapY = 0, mapHight = 4000, mapWidth = 2400, canvassezeX = 1300, canvassezeY = 800;
var playerCodinateX = Math.floor(Math.random()*mapHight-100), playerCodinateY = Math.floor(Math.random()*mapWidth-100), 
playerSize = 50, playerspeed = 5, playerHealth = 100, playerReach = 50, playerAngle = 0;
var centerX= 600, centerY = 300, cameraX = playerCodinateX-centerX, cameraY = playerCodinateY-centerY;
var playerid=-1 , isplayer=false ,numbr=0;


function startGame() {

    players.push(new playercomponent(playerCodinateX, playerCodinateY, "blue", playerSize, playerSize, playerHealth));

    
    playrsprites.push(new Sprite({x:playerCodinateX,y:playerCodinateY,width:playerSize,height:playerSize,
        imgSrc:'./image/Coolplayer.png'}));

   background = new Sprite({x:mapX, y:mapY, width:mapHight-mapX, height:mapWidth-mapY, imgSrc:'./image/map_grass.png'})

   backbackground = new Sprite({x:mapX-1000, y:mapY-1000, width:mapHight-mapX+2000, height:mapWidth-mapY+2000, imgSrc:'./image/mapVoid.png'})

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
            connection.invoke("PlayerAttack", playerCodinateX, playerCodinateY, playerid, playerAngle).catch(function (err) {
                return console.error(err.toString());
            });
            players[0].DrawAttavkBoxrl();
        })

        window.addEventListener("mousemove", function(e) {
            mousex = e.pageX;
            mousey = e.pageY;
            //console.log(mousex);
        });
    },
    clear : function() {
        this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
    }
}

function updateGameArea() {
    myGameArea.clear();
    backbackground.draw();
    background.draw();

    players.forEach(player=>{
        if(playerid==player.id){

            player.speedX = 0;
            player.speedY = 0;  

            if(player.x+50<=mapHight){
                if (myGameArea.keys && myGameArea.keys[39]) {player.speedX = playerspeed; }

                if (myGameArea.keys && myGameArea.keys[68]) {player.speedX = playerspeed; }
            }
            if(player.x>=mapX){
                if (myGameArea.keys && myGameArea.keys[37]) {player.speedX = -playerspeed; }
                
                if (myGameArea.keys && myGameArea.keys[65]) {player.speedX = -playerspeed; }
            }

            if(player.y>=mapY){
                if (myGameArea.keys && myGameArea.keys[38]) {player.speedY = -playerspeed; }
                
                if (myGameArea.keys && myGameArea.keys[87]) {player.speedY = -playerspeed; }
            }
            if(player.y+50<=mapWidth){
                if (myGameArea.keys && myGameArea.keys[40]) {player.speedY = playerspeed; }

                if (myGameArea.keys && myGameArea.keys[83]) {player.speedY = playerspeed; }
            }
           
            
            player.newPosUpdate(); 
           // player.Drawrl();
           player.DrawHealth();

           if(player.health<=0){
            window.location.href = "index.html";
           }
        }else{
            if(player.health>0){
                // player.Draw();
                player.DrawHealth();
            }
        }

    })
    playrsprites.forEach(playrsprite=>{
        if(playerid==playrsprite.id){ 
           playrsprite.rotationdrawrl();
        }else{
            playrsprite.rotationdraw();
        }
    })
    
    connection.invoke("GetMobileEntity").catch(function (err) {
        return console.error(err.toString());
    });
    
    
    mobileEntities.forEach(entity=>{
        if(entity.health>0){
           // entity.Draw();
            entity.DrawHealth();
        }
    })
    /*
    stationObjects.forEach(object=>{
       // object.Draw();
    })
    */

    entitySprites.forEach(entitySprite=>{
       // entitySprite.draw();
        
       entitySprite.rotationdraw();
    })

    objectSprites.forEach(objectSprite=>{
        objectSprite.draw();
    })

    
     Drawminimap();
     DrawLeaderboard();
     DrawExtentions();
}

function DrawExtentions(){
    ctx = myGameArea.context;
    ctx.font = "30px Arial";
    ctx.fillStyle = "white";
    ctx.textAlign = "center";

    ctx.fillText("points: " + players[0].points, canvassezeX-100, canvassezeY-210);
    ctx.fillText("wood: " + players[0].wood, canvassezeX-100, canvassezeY-260);
    ctx.fillText("stone: " + players[0].stone, canvassezeX-100, canvassezeY-310);

}

function DrawLeaderboard(){
    ctx = myGameArea.context;
    ctx.fillStyle = "black";
   
    ctx.globalAlpha = 0.2;
    ctx.fillRect(canvassezeX-200, 0, 200, 200);
    ctx.globalAlpha = 1;
    
    ctx.fillStyle = "white";
    ctx.textAlign = "center";
    ctx.font = "10px Arial";

    var number = 0; 
    players.forEach(player=>{
        number +=11;
        ctx.fillText("player: "+player.id + "          has: " + player.points, canvassezeX-100, 10 + number);
    })
   
}

function Drawminimap(){
    backbackground.drawformap_();
    background.drawformap();

    players.forEach(player=>{
        if(player.health>0){
            player.Drawformap();
        }
    })

    mobileEntities.forEach(entity=>{
        if(entity.health>0){
            entity.Drawformap();
        }
    })

    stationObjects.forEach(otherentety=>{
        otherentety.Drawformap();
     })
}

/*

function moveup() {
    players[0].speedY = -1; 
}

function movedown() {
    players[0].speedY = 1; 
}

function moveleft() {
    players[0].speedX = -1; 
}

function moveright() {
    players[0].speedX = 1; 
}

function clearmove() {
    players[0].speedX = 0; 
    players[0].speedY = 0; 
}

<div style="text-align:center;width:480px;">
        <button onmousedown="moveup()" onmouseup="clearmove()" ontouchstart="moveup()">UP</button><br><br>
        <button onmousedown="moveleft()" onmouseup="clearmove()" ontouchstart="moveleft()">LEFT</button>
        <button onmousedown="moveright()" onmouseup="clearmove()" ontouchstart="moveright()">RIGHT
        </button><br><br>
        <button onmousedown="movedown()" onmouseup="clearmove()" ontouchstart="movedown()">DOWN</button>
</div>

const btn = document.getElementById('btnValue');

btn.addEventListener('click', GetValue);

let value_ = 0;
function GetValue()
{

    value_ = document.getElementById('getMyValue').value;
    console.log(value_ );
}

<p></p>
              <p><form method="GET">
                <div class="row">
                  <div class="col">
                    <input type="text" class="form-control" placeholder="Name" id="getMyValue">
                  </div>
                </div>
              </form></p>


players.forEach(player=>{
        numbr++;
    })
    if(numbr<playerId.length){
        for(i=numbr;i<playerId.length;i++){
            players.push(new playercomponent(playerX[i], playerY[i], "red", 50, 50, 100));

            playrsprites.push(new Sprite({x:playerX[i],y:playerY[i],width:50,height:50, imgSrc:'./image/Coolplayer.png'}));
          
            players[players.length-1].SetId(playerId[i]);
            playrsprites[players.length-1].setId(playerId[i]);

            console.log("new player join");
        }
    }
    numbr=0;
              

    List<double> PlayerIds = new List<double>();
            this.loopCraete.curMap?.players.ForEach(player =>
            {
                double id = player.Id;
                PlayerIds.Add(id);
            });
    List<double> PlayerY = new List<double>();
            this.loopCraete.curMap?.players.ForEach(player =>
            {
                double y = player.Y;
                PlayerY.Add(y);
            });
    List<double> PlayerX = new List<double>();
            this.loopCraete.curMap?.players.ForEach(player =>
            {
                double x = player.X;
                PlayerX.Add(x);
            });



mobileEntities.ForEach(mobileEntity =>
    {
        mobileEntity
    });    
*/