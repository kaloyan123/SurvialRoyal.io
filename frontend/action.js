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

 connection.on("RemoveTools", function (id) {
});

connection.on("logtest", function (id) {
    console.log("test");
});

 connection.on("addTools", function (id,type,tier) {

    if(playerid==id){

        var toolid = 0;
        //console.log(toolid);
        if(playertools.length>0){
            playertools.forEach(tool => {
                console.log(tool);
                toolid++;
            });
        }

        console.log("new tool " + toolid);
        
        playertools.push(new toolcomponent(toolid, type, tier));

        if(type =="pickaxe"){
            toolsprites.push(new Sprite({x:20+(playertools.length*40),y:700,width:30,height:30,
                imgSrc:'./image/pick_wood.png'}));
        }
        if(type =="axe"){
            toolsprites.push(new Sprite({x:20+(playertools.length*40),y:700,width:30,height:30,
                imgSrc:'./image/axe.png'}));
        }
        if(type =="sword"){
            toolsprites.push(new Sprite({x:20+(playertools.length*40),y:700,width:30,height:30,
                imgSrc:'./image/sword_wood.png'}));
        }
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

var connectiontid = sessionStorage.getItem("connectionid");    
var playername = sessionStorage.getItem("connectionname");

var players = [], stationObjects= [], mobileEntities = [], playertools = [], craftables = [],
background, backbackground, playrsprites = [], objectSprites=[], entitySprites=[], toolsprites = [], craftablessprites = [];
var mapX = 0, mapY = 0, mapHight = 4000, mapWidth = 2400, canvassezeX = 1300, canvassezeY = 800;
var playerCodinateX = Math.floor(Math.random()*mapHight-100), playerCodinateY = Math.floor(Math.random()*mapWidth-100), 
playerSize = 50, playerspeed = 5, playerHealth = 100, playerReach = 50, playerAngle = 0, equipedtool=0;
var centerX= 600, centerY = 300, cameraX = playerCodinateX-centerX, cameraY = playerCodinateY-centerY;
var playerid=-1 , isplayer=false ,numbr=0,trowtoolsdelay=0;


function startGame() {

    players.push(new playercomponent(playerCodinateX, playerCodinateY, "blue", playerSize, playerSize, playerHealth));
    playrsprites.push(new Sprite({x:playerCodinateX,y:playerCodinateY,width:playerSize,height:playerSize,
        imgSrc:'./image/Coolplayer.png'}));


   background = new Sprite({x:mapX, y:mapY, width:mapHight-mapX, height:mapWidth-mapY, imgSrc:'./image/map_grass.png'})
   backbackground = new Sprite({x:mapX-1000, y:mapY-1000, width:mapHight-mapX+2000, height:mapWidth-mapY+2000, imgSrc:'./image/mapVoid.png'})

   console.log(playername);

    connection.invoke("InitiatePlayers",playerCodinateX ,playerCodinateY ,playerHealth).catch(function (err) {
        return console.error(err.toString());
    });

    connection.invoke("GetImobileObj").catch(function (err) {
        return console.error(err.toString());
    });
    
    connection.invoke("LogSmt").catch(function (err) {
        return console.error(err.toString());
    });

    /*
    connection.invoke("CreateTool",playerid ,"pickaxe" ,1, 0, 0).catch(function (err) {
        return console.error(err.toString());
    });
    */

    craftables.push(new toolcomponent(0, "pickaxe", 1));
    craftables[0].SetParameters(20+((craftables.length-1)*40),20,30,30,10,10);
    craftables.push(new toolcomponent(1, "axe", 1));
    craftables[1].SetParameters(20+((craftables.length-1)*40),20,30,30,10,0);
    craftables.push(new toolcomponent(2, "sword", 1));
    craftables[2].SetParameters(20+((craftables.length-1)*40),20,30,30,10,0);

    craftablessprites.push(new Sprite({x:20+(craftablessprites.length*40),y:20,width:30,height:30,
        imgSrc:'./image/pick.png'}));
    craftablessprites.push(new Sprite({x:20+(craftablessprites.length*40),y:20,width:30,height:30,
        imgSrc:'./image/axe.png'}));
    craftablessprites.push(new Sprite({x:20+(craftablessprites.length*40),y:20,width:30,height:30,
        imgSrc:'./image/sword_wood.png'}));
   

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

            /*
            for(i=0;i<200;i++){
                console.log(myGameArea.keys[i]);
            }
            */
        })

        window.addEventListener('click', function (e) {
           // console.log(playerAngle);

            craftables.forEach(craftable=>{

                if(mousex >= craftable.x && mousex <= craftable.x+craftable.width && mousey >= craftable.y && 
                mousey <= craftable.y+craftable.height){

                    console.log(players[0].wood);
                    console.log(craftable.costin_wood);
                    console.log(players[0].stone);
                    console.log(craftable.costin_stone);

                    if(players[0].wood >= craftable.costin_wood && players[0].stone >= craftable.costin_stone){

                        players[0].wood = players[0].wood - craftable.costin_wood;
                        players[0].stone = players[0].stone - craftable.costin_stone;
                        
                        connection.invoke("CreateTool",playerid, craftable.type, craftable.tier, craftable.costin_wood, 
                        craftable.costin_stone).catch(function (err) {
                            return console.error(err.toString());
                        });

                    }
                }
            })

            numbr=1;
           // var harvest_wood=1,harvest_stone=1;
            var equipedtype="",equipedharvest=1, equipeddamage=1;
            playertools.forEach(tool =>{
                if(numbr==equipedtool){
                    equipedtype=tool.type;
                    equipedharvest= tool.specialharvest;
                    equipeddamage = tool.extradamage;
                    /*
                    if(tool.type == "pickaxe"){
                        harvest_wood = tool.specialharvest;
                    }
                    if(tool.type == "axe"){
                        harvest_stone = tool.specialharvest;
                    }
                    */
                   // console.log(tool.specialharvest);
                }
                numbr++;
            })
            numbr=0;

            console.log(equipedharvest);
            connection.invoke("PlayerAttack", playerCodinateX, playerCodinateY, playerid, playerAngle, 
            equipedtype, equipedharvest, equipeddamage).catch(function (err) {
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
           player.DrawNamerl();

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

    if (myGameArea.keys && myGameArea.keys[49]) {equipedtool=1 }
    if (myGameArea.keys && myGameArea.keys[50]) {equipedtool=2 }
    if (myGameArea.keys && myGameArea.keys[51]) {equipedtool=3 }
    if (myGameArea.keys && myGameArea.keys[52]) {equipedtool=4 }
    if (myGameArea.keys && myGameArea.keys[53]) {equipedtool=5 }
    if (myGameArea.keys && myGameArea.keys[54]) {equipedtool=6 }
    if (myGameArea.keys && myGameArea.keys[55]) {equipedtool=7 }
    if (myGameArea.keys && myGameArea.keys[56]) {equipedtool=8 }
    if (myGameArea.keys && myGameArea.keys[57]) {equipedtool=9 }

    if (myGameArea.keys && myGameArea.keys[81] && trowtoolsdelay>100) {
        numbr=1;
        var equipedid=0,equipedindex=-1;

        console.log(equipedtool);
        playertools.forEach(tool =>{
            if(numbr==equipedtool){
                console.log("equipedtool");
                
                console.log(numbr);
                equipedid=tool.id;
                equipedindex=numbr-1;
                console.log(equipedindex);
             }
            numbr++;
        })
        numbr=0;

        console.log(playerid, equipedid);
        connection.invoke("RemoveTool",playerid, equipedid).catch(function (err) {
            return console.error(err.toString());
        });

        playertools.splice(equipedindex, 1); // 2nd parameter means remove one item only
        toolsprites.splice(equipedindex, 1);

        toolsprites.forEach(toolprite =>{
            if(numbr>=equipedindex){
                toolprite.x = toolprite.x - 40;
             }
            numbr++;
        })
        numbr=0;

        console.log(playertools);

        trowtoolsdelay=0;
    }
    trowtoolsdelay++;


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

    numbr=1;
    toolsprites.forEach(toolsprite=>{

        if(numbr==equipedtool){
            toolsprite.backgrounddrawselect();
            
            toolsprite.absolutedraw();
        }else{
            toolsprite.backgrounddraw();
            
            toolsprite.absolutedraw();
        }
        numbr++;
    })
    numbr=0;

    craftablessprites.forEach(craftable=>{
        craftable.backgrounddraw();

        craftable.absolutedraw();
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

function DrawItems(){
    ctx = myGameArea.context;
    ctx.fillStyle = "black";
   
    ctx.globalAlpha = 0.2;
    ctx.fillRect(canvassezeX-200, 0, 200, 200);
    ctx.globalAlpha = 1;

    var number = 0; 
    players.forEach(player=>{
        number +=11;
        ctx.fillText("player: "+player.id + "          has: " + player.points, canvassezeX-100, 10 + number);
    })
   
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

    connection.invoke("CreateTool",playerid ,"axe" ,1).catch(function (err) {
        return console.error(err.toString());
    });
    connection.invoke("CreateTool",playerid ,"sword" ,1).catch(function (err) {
        return console.error(err.toString());
    });
*/