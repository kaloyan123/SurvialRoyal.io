function playercomponent(x, y, color, width, height, health) {
    this.width = width;
    this.height = height;
    this.speedX = 0;
    this.speedY = 0;
    this.x = x;
    this.y = y;
    this.health = health;   
    this.wood = 0;
    this.stone = 0;
    this.points = 0;
    this.centerx = centerX + this.width / 2;;
    this.centery = centerY + this.width / 2;;

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
    this.DrawNamerl = function() {
        ctx = myGameArea.context;

        ctx.font = "15px Arial";
        ctx.fillStyle = "black";
        ctx.textAlign = "center";
    
        ctx.fillText(playername, centerX+25, centerY-5);
    } 
    this.DrawAttavkBoxrl = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = "red";

        // general up
        if(playerAngle < 0.8 && playerAngle>-0.8){
            ctx.fillRect(centerX-playerReach, centerY-playerReach, this.width+(playerReach*2), this.height+(playerReach*2) -75);
          //  console.log("general up");
        }

        // general right
        if(playerAngle > 0.8 && playerAngle<2.2){
            ctx.fillRect(this.centerx, centerY-playerReach, this.width+(playerReach*2)-75, this.height+(playerReach*2));
          //  console.log("general right");
        }

        // general down
        if(playerAngle > 2.2 && playerAngle<3.8){
            ctx.fillRect(centerX-playerReach, this.centery, this.width+(playerReach*2), this.height+(playerReach*2)-75);
          //  console.log("general down");
        }

        // general left
        if(playerAngle>3.8 || playerAngle < -0.8 ){
            ctx.fillRect(centerX-playerReach, centerY-playerReach, this.width+(playerReach*2)-75, this.height+(playerReach*2));
          //  console.log("general left");
        }
        
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

        connection.invoke("UpdatePlayers", this.x, this.y, playerid, playerAngle).catch(function (err) {
            return console.error(err.toString());
        });
    }
    this.Drawformap = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(canvassezeX-200 + (this.x/ 20), canvassezeY-200 + (this.y/ 12), 5, 5);
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

    this.Drawformap = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(canvassezeX-200 + (this.x/ 20), canvassezeY-200 + (this.y/ 12), 5, 5);
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
    this.Drawformap = function() {
        ctx = myGameArea.context;
        ctx.fillStyle = color;
        ctx.fillRect(canvassezeX-200 + (this.x/ 20), canvassezeY-200 + (this.y/ 12), 5, 5);
    }
    
}

function toolcomponent(id, type, tier) {
    this.id = id;
    this.type = type; 
    this.tier = tier; 
    //1 -- wood
    //2 -- stone
    //3 -- iron?
    this.specialharvest = 0;
    if(this.tier=1){
        this.specialharvest = 2;
    }
    this.extradamage = 0;
    if(this.tier=1){
        this.extradamage = 10;
    }

    this.x=0;
    this.y=0;
    this.width = 0;
    this.height = 0;

    this.costin_wood = 0;
    this.costin_stone = 0;

    this.SetParameters = function(x,y,width,height,woodcost,stonecost) {
        this.x=x;
        this.y=y;
        this.width = width;
        this.height = height;

        this.costin_wood = woodcost;
        this.costin_stone = stonecost;
    }
}

class Sprite{
    constructor({x,y,width,height,spriteofsetx,spriteofsety,imgSrc}){
        this.x=x;
        this.y=y;
        this.width = width;
        this.height = height;
        this.image = new Image();
        this.image.src = imgSrc;
        this.id =  0; 
        this.angle = 0;
        this.spriteofsetx = spriteofsetx;
        this.spriteofsety = spriteofsety;
    }
    setId(id){
       // console.log(id);
        this.id = id;   
    }

    draw(){
        myGameArea.context.drawImage(this.image,this.x-cameraX,this.y-cameraY,this.width,this.height);
    }
    absolutedraw(){
        myGameArea.context.drawImage(this.image,this.x,this.y,this.width,this.height);
    }
    drawformap(){
        myGameArea.context.drawImage(this.image,canvassezeX-200,canvassezeY-200,200,200);
    }
    drawformap_(){
        myGameArea.context.drawImage(this.image,canvassezeX-200 -2,canvassezeY-200 -2,200,200);
    }
    
    backgrounddraw(){
        ctx = myGameArea.context;
        ctx.fillStyle = "black";
        ctx.globalAlpha = 0.2;
        ctx.fillRect(this.x , this.y , this.width, this.height);
        ctx.globalAlpha = 1;
    }
    backgrounddrawselect(){
        ctx = myGameArea.context;
        ctx.fillStyle = "red";
        ctx.globalAlpha = 0.2;
        ctx.fillRect(this.x , this.y , this.width, this.height);
        ctx.globalAlpha = 1;
    }

    rotationdraw(){
        ctx = myGameArea.context;

        this.centerx = (this.x-cameraX + this.width / 2) +10;
        this.centery = (this.y-cameraY + this.height / 2) +10;

        ctx.translate(this.centerx, this.centery);
        ctx.rotate(this.angle);

        ctx.translate(-this.centerx, -this.centery);
        myGameArea.context.drawImage(this.image,this.x-cameraX-5,this.y-cameraY-0,this.width+30,this.height+20);
        ctx.setTransform(1, 0, 0, 1, 0, 0);   
    }
    
    rotationdrawrl(){
        ctx = myGameArea.context;

        this.centerx = centerX + this.width / 2;
        this.centery = centerY + this.height / 2;

        this.angle = Math.atan2(mousey - this.centery, mousex - this.centerx) + (Math.PI/2);

        playerAngle=this.angle;

        ctx.translate(this.centerx, this.centery);
        ctx.rotate(this.angle);

        ctx.translate(-this.centerx, -this.centery);
        myGameArea.context.drawImage(this.image,centerX-15,centerY-10,this.width+30,this.height+20);
        ctx.setTransform(1, 0, 0, 1, 0, 0);   
   }
}