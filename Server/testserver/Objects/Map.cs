using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testserver.Models;

namespace testserver.Objects
{
    public class Map
    {
        // public bool IsDone { get; private set; } = false;

        // public string Winner { get; private set; } = null;

        Random rnd = new Random();

        public List<Player> players { get; set; } = new List<Player>();

        public List<StationryObj> imobileObjs { get; set; } = new List<StationryObj>();

        public List<ImobileEntity> imobileEntities { get; set; } = new List<ImobileEntity>();

        public List<MobileEntity> mobileEntities { get; set; } = new List<MobileEntity>();

        public int objectNumber = -1;
        public int entityNumber = -1;

        public int mapstartX = 0;
        public int mapstartY = 0;
        public int mapendX = 4000;
        public int mapendY = 2400;

        public int playerReach = 50;
        public int playerSize = 50;

        public int wolfSenseDistanse = 100;

        public int addAnimals = 0;

        bool islegalmoveX = true;
        bool islegalmoveX_ = true;
        bool islegalmoveY = true;
        bool islegalmoveY_ = true;

        public void CreatePlayer(int x, int y,int health, int id)
        {
            Player player = new Player() { X = x, Y = y, Hp = health, Id = id };
            players.Add(player);
        }
        public void CreateObj(int x, int y, string type)
        {
            objectNumber++;
            StationryObj imobileobject = new StationryObj() { X = x, Y = y, Id = objectNumber, Type = type };
            imobileObjs.Add(imobileobject);

            //  Console.WriteLine(imobileobject.Type);
        }
        public void CreateStructure(int id, int x, int y, string type, int structureid, int hp)
        {
            ImobileEntity imobileentity = new ImobileEntity() { X = x, Y = y, Heigth = 50, Width = 50, Hp = hp, Id = structureid,
                Type = type, CreatorId = id};
            imobileEntities.Add(imobileentity);

            //  Console.WriteLine(imobileobject.Type);
        }

        public void CreateEntity(int x, int y, int width, int heigth, double Hp, string type)
        {
            entityNumber++;
            MobileEntity mobileEntity = new MobileEntity() { X = x, Y = y,Width = width, Heigth = heigth, Hp = Hp, Id = entityNumber, Type = type };
            mobileEntities.Add(mobileEntity);

            // Console.WriteLine(mobileEntity.Type);
        }
        public void CreateItem(int playerid, string type, int tier, int woodcost, int stonecost, string kind)
        {
            if (kind == "tool")
            {
                players.ForEach(player =>
                {
                    if (player.Id == playerid)
                    {
                        player.Wood = player.Wood - woodcost;
                        player.Stone = player.Stone - stonecost;

                        int id = -1;
                        player.playertools.ForEach(tool => {
                            id++;
                        });
                        id++;

                        Item newtool = new Item() { Id = id, Type = type, Tier = tier, Kind = "tool", Copies = 1 };

                        player.playertools.Add(newtool);

                        //Console.WriteLine(newtool.Type);
                    }
                });
            }
            else
            {
                Console.WriteLine("here");
                players.ForEach(player =>
                {
                    if (player.Id == playerid)
                    {
                        player.Wood = player.Wood - woodcost;
                        player.Stone = player.Stone - stonecost;

                        bool hasthissructure = false;
                        player.playertools.ForEach(tool => {
                            if (tool.Type == type)
                            {
                                hasthissructure = true;
                                tool.Copies++;
                            }
                        });

                        if (hasthissructure) {}
                        else
                        {
                            int id = -1;
                            player.playertools.ForEach(tool => {
                                id++;
                            });
                            id++;

                            Item newtool = new Item() { Id = id, Type = type, Tier = tier, Kind = "tool" };

                            player.playertools.Add(newtool);

                            //Console.WriteLine(newtool.Type);
                        }
                    }
                });
            }
            
        }
        public void RemoveTool(int playerid, int toolid)
        {
            players.ForEach(player =>
            {
                if (player.Id == playerid)
                {
                    var numbr = 0;
                    var toolindex = 0;

                    player.playertools.ForEach(tool =>
                    {
                      //  Console.WriteLine(tool.Type);
                        if (tool.Id == toolid)
                        {
                            toolindex = numbr;
                        }
                        numbr++;
                    });
                   // Console.WriteLine("end of old list");

                    player.playertools.RemoveAt(toolindex);
                }
            });
        }

        public void CreateObjectByNumber(int number)
        {
            for (int i = 0; i < number; i++)
            {
                if (rnd.Next(0, 100) <= 50)
                {
                    CreateObj(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), "tree");
                }
                else
                {
                    CreateObj(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), "rock");
                }
            }
        }
        public void CreateEntityByNumber(int number)
        {
            for (int i = 0; i < number; i++)
            {
                int randm = rnd.Next(0, 100);

                if (randm <= 35)
                {
                    Console.WriteLine("rabit " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 30, 30, 60, "rabit");
                }
                else if (randm > 35 && randm <= 70)
                {
                    Console.WriteLine("pig " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 50, 50, 100, "pig");
                }
                else if (randm > 70 && randm <= 90)
                {
                    Console.WriteLine("cow " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 60, 60, 120, "cow");
                }
                else if (randm > 90)
                {
                    Console.WriteLine("wolf " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 50, 50, 100, "wolf");
                }
            }

            addAnimals = 0;
        }
         
        public void StartOfGame()
        {
            CreateObjectByNumber(15);

            CreateEntityByNumber(10);

            Console.WriteLine(imobileObjs.Count);
            Console.WriteLine(mobileEntities.Count);
        }

        public void Update()
        {

            mobileEntities.ForEach(mobileEntity =>
            {
                if (mobileEntity.Type == "rabit")
                {
                    if (mobileEntity.IsAlive)
                    {
                        if (mobileEntity.EntityTiks % 100 == 0)
                        {

                            if (rnd.Next(0, 100) <= 50)
                            {
                                mobileEntity.DirectionX = rnd.NextDouble() * ((0.2 - 1) * -1) + 0.1;
                            }
                            else
                            {
                                mobileEntity.DirectionX = (rnd.NextDouble() * ((0.2 - 1) * -1) + 0.1) * -1;

                            }

                            if (rnd.Next(0, 100) <= 50)
                            {
                                mobileEntity.DirectionY = rnd.NextDouble() * ((0.2 - 1) * -1) + 0.1;
                            }
                            else
                            {
                                mobileEntity.DirectionY = (rnd.NextDouble() * ((0.2 - 1) * -1) + 0.1) * -1;

                            }

                            //entity rotation
                            if (true)
                            {
                                //general up-right
                                if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 0.8;
                                    // Console.WriteLine("general up-right");

                                }
                                //general down-right
                                if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 2.2;
                                    //Console.WriteLine("general down-right");
                                }
                                //general down-left
                                if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 3.8;
                                    // Console.WriteLine("general down-left");
                                }
                                //general up-left
                                if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = -0.8;
                                    //  Console.WriteLine("general up-left");
                                }


                                // if speed by Y is low  \/ \/
                                if (mobileEntity.DirectionY <= 0.4 && mobileEntity.DirectionY > 0)
                                {
                                    //general up
                                    if (mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = -0.8;
                                        // Console.WriteLine("general up");
                                    }
                                    //general down
                                    if (mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 1.6;
                                        // Console.WriteLine("general down");
                                    }
                                }
                                if (mobileEntity.DirectionY >= -0.4 && mobileEntity.DirectionY < 0)
                                {
                                    //general up
                                    if (mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = -0.8;
                                        // Console.WriteLine("general up");
                                    }
                                    //general down
                                    if (mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 1.6;
                                        //  Console.WriteLine("general down");
                                    }
                                }

                                // if speed by X is low  \/ \/
                                if (mobileEntity.DirectionX <= 0.4 && mobileEntity.DirectionX > 0)
                                {
                                    //general left
                                    if (mobileEntity.DirectionY < 0)
                                    {
                                        mobileEntity.Angle = 0;
                                        //    Console.WriteLine("general left");
                                    }
                                    //general right
                                    if (mobileEntity.DirectionY > 0)
                                    {
                                        mobileEntity.Angle = 3.2;
                                        //      Console.WriteLine("general right");
                                    }
                                }
                                if (mobileEntity.DirectionX >= -0.4 && mobileEntity.DirectionX < 0)
                                {
                                    //general left
                                    if (mobileEntity.DirectionY < 0)
                                    {
                                        mobileEntity.Angle = 0;
                                        //       Console.WriteLine("general left");
                                    }
                                    //general right
                                    if (mobileEntity.DirectionY > 0)
                                    {
                                        mobileEntity.Angle = 3.2;
                                        //      Console.WriteLine("general right");
                                    }
                                }
                            }

                        }

                        

                    }
                }
                else if (mobileEntity.Type == "pig")
                {
                    if (mobileEntity.IsAlive)
                    {
                        if (mobileEntity.EntityTiks % 500 == 0)
                        {
                            mobileEntity.DirectionX = 0;
                            mobileEntity.DirectionY = 0;

                        }
                        if (mobileEntity.EntityTiks % 1000 == 0)
                        {
                            if (rnd.Next(0, 100) <= 50)
                            {
                                mobileEntity.DirectionX = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                            }
                            else
                            {
                                mobileEntity.DirectionX = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                            }
                            if (rnd.Next(0, 100) <= 50)
                            {
                                mobileEntity.DirectionY = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                            }
                            else
                            {
                                mobileEntity.DirectionY = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                            }
                            //entity rotation
                            if (true)
                            {
                                //general up-right
                                if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 0.8;
                                    // Console.WriteLine("general up-right");

                                }
                                //general down-right
                                if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 2.2;
                                    //Console.WriteLine("general down-right");
                                }
                                //general down-left
                                if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 3.8;
                                    // Console.WriteLine("general down-left");
                                }
                                //general up-left
                                if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = -0.8;
                                    //  Console.WriteLine("general up-left");
                                }

                                // if speed by Y is low  \/ \/
                                if (mobileEntity.DirectionY <= 0.2 && mobileEntity.DirectionY > 0)
                                {
                                    //general up
                                    if (mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = 4;
                                        //   Console.WriteLine("general up");
                                    }
                                    //general down
                                    if (mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 1.6;
                                        // Console.WriteLine("general down");
                                    }
                                }
                                if (mobileEntity.DirectionY >= -0.2 && mobileEntity.DirectionY < 0)
                                {
                                    //general up
                                    if (mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = 4;
                                        //  Console.WriteLine("general up");
                                    }
                                    //general down
                                    if (mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 1.6;
                                        //  Console.WriteLine("general down");
                                    }
                                }

                                // if speed by X is low  \/ \/
                                if (mobileEntity.DirectionX <= 0.2 && mobileEntity.DirectionX > 0)
                                {
                                    //general left
                                    if (mobileEntity.DirectionY < 0)
                                    {
                                        mobileEntity.Angle = 0;
                                        //  Console.WriteLine("general left");
                                    }
                                    //general right
                                    if (mobileEntity.DirectionY > 0)
                                    {
                                        mobileEntity.Angle = 3.2;
                                        // Console.WriteLine("general right");
                                    }
                                }
                                if (mobileEntity.DirectionX >= -0.2 && mobileEntity.DirectionX < 0)
                                {
                                    //general left
                                    if (mobileEntity.DirectionY < 0)
                                    {
                                        mobileEntity.Angle = 0;
                                        //  Console.WriteLine("general left");
                                    }
                                    //general right
                                    if (mobileEntity.DirectionY > 0)
                                    {
                                        mobileEntity.Angle = 3.2;
                                        // Console.WriteLine("general right");
                                    }
                                }
                            }

                            /*
                            Console.WriteLine(mobileEntity.DirectionX);
                            Console.WriteLine(mobileEntity.DirectionY);
                             Console.WriteLine(mobileEntity.Angle);
                            */
                        } 
                    }
                }
                else if (mobileEntity.Type == "cow")
                {
                    if (mobileEntity.IsAlive)
                    {
                        
                        if (mobileEntity.AgrivatedById < 0)
                        {
                            if (mobileEntity.EntityTiks % 500 == 0)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 0;

                            }
                            if (mobileEntity.EntityTiks % 1000 == 0)
                            {
                                if (rnd.Next(0, 100) <= 50)
                                {
                                    mobileEntity.DirectionX = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                                }
                                else
                                {
                                    mobileEntity.DirectionX = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                                }
                                if (rnd.Next(0, 100) <= 50)
                                {
                                    mobileEntity.DirectionY = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                                }
                                else
                                {
                                    mobileEntity.DirectionY = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                                }
                                //entity rotation
                                if (true)
                                {
                                    //general up-right
                                    if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 0.8;
                                        // Console.WriteLine("general up-right");

                                    }
                                    //general down-right
                                    if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 2.2;
                                        //Console.WriteLine("general down-right");
                                    }
                                    //general down-left
                                    if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = 3.8;
                                        // Console.WriteLine("general down-left");
                                    }
                                    //general up-left
                                    if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = -0.8;
                                        //  Console.WriteLine("general up-left");
                                    }

                                    // if speed by Y is low  \/ \/
                                    if (mobileEntity.DirectionY <= 0.2 && mobileEntity.DirectionY > 0)
                                    {
                                        //general up
                                        if (mobileEntity.DirectionX < 0)
                                        {
                                            mobileEntity.Angle = 4.5;
                                            //   Console.WriteLine("general up");
                                        }
                                        //general down
                                        if (mobileEntity.DirectionX > 0)
                                        {
                                            mobileEntity.Angle = 1.6;
                                            // Console.WriteLine("general down");
                                        }
                                    }
                                    if (mobileEntity.DirectionY >= -0.2 && mobileEntity.DirectionY < 0)
                                    {
                                        //general up
                                        if (mobileEntity.DirectionX < 0)
                                        {
                                            mobileEntity.Angle = 4.5;
                                            //  Console.WriteLine("general up");
                                        }
                                        //general down
                                        if (mobileEntity.DirectionX > 0)
                                        {
                                            mobileEntity.Angle = 1.6;
                                            //  Console.WriteLine("general down");
                                        }
                                    }

                                    // if speed by X is low  \/ \/
                                    if (mobileEntity.DirectionX <= 0.2 && mobileEntity.DirectionX > 0)
                                    {
                                        //general left
                                        if (mobileEntity.DirectionY < 0)
                                        {
                                            mobileEntity.Angle = 0;
                                            //  Console.WriteLine("general left");
                                        }
                                        //general right
                                        if (mobileEntity.DirectionY > 0)
                                        {
                                            mobileEntity.Angle = 3.2;
                                            // Console.WriteLine("general right");
                                        }
                                    }
                                    if (mobileEntity.DirectionX >= -0.2 && mobileEntity.DirectionX < 0)
                                    {
                                        //general left
                                        if (mobileEntity.DirectionY < 0)
                                        {
                                            mobileEntity.Angle = 0;
                                            //  Console.WriteLine("general left");
                                        }
                                        //general right
                                        if (mobileEntity.DirectionY > 0)
                                        {
                                            mobileEntity.Angle = 3.2;
                                            // Console.WriteLine("general right");
                                        }
                                    }
                                }

                                /*
                                Console.WriteLine(mobileEntity.DirectionX);
                                Console.WriteLine(mobileEntity.DirectionY);
                                 Console.WriteLine(mobileEntity.Angle);
                                */
                            }

                            
                        } 
                        else {
                            if (mobileEntity.AgrivatedBy == "player")
                            {
                                var obj = players.FirstOrDefault(x => x.Id == mobileEntity.AgrivatedById);

                                /*
                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = 0;
                                }
                                */

                                if (mobileEntity.X < obj.X) {
                                    mobileEntity.DirectionX = 1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 2.2;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = 0.8;
                                    }
                                }

                                if (mobileEntity.X > obj.X)
                                {
                                    mobileEntity.DirectionX = -1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 3.8;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = -0.8;
                                    }
                                }

                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = -1;
                                    mobileEntity.AgrivatedBy = "";
                                    //Console.WriteLine();
                                }
                            }
                            else
                            {
                                var obj = mobileEntities.FirstOrDefault(x => x.Id == mobileEntity.AgrivatedById);

                                /*
                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = 0;
                                }
                                */

                                if (mobileEntity.X < obj.X)
                                {
                                    mobileEntity.DirectionX = 1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 2.2;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = 0.8;
                                    }
                                }

                                if (mobileEntity.X > obj.X)
                                {
                                    mobileEntity.DirectionX = -1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 3.8;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = -0.8;
                                    }
                                }

                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = -1;
                                    mobileEntity.AgrivatedBy = "";
                                }
                            }

                            if (mobileEntity.EntityTiks % 120 == 0)
                            {
                                Attack(mobileEntity.X, mobileEntity.Y, mobileEntity.Type, mobileEntity.Id, 0, "no tool", 0, 0);
                            }
                        }

                    }
                }
                else if (mobileEntity.Type == "wolf")
                {
                    if (mobileEntity.IsAlive)
                    {
                       // Console.WriteLine(mobileEntity.AgrivatedById +" " + mobileEntity.Id);
                        if (mobileEntity.AgrivatedById < 0)
                        {
                            if (mobileEntity.EntityTiks % 500 == 0)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 0;

                            }
                            if (mobileEntity.EntityTiks % 1000 == 0)
                            {
                                if (rnd.Next(0, 100) <= 50)
                                {
                                    mobileEntity.DirectionX = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                                }
                                else
                                {
                                    mobileEntity.DirectionX = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                                }
                                if (rnd.Next(0, 100) <= 50)
                                {
                                    mobileEntity.DirectionY = rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1;
                                }
                                else
                                {
                                    mobileEntity.DirectionY = (rnd.NextDouble() * ((0.2 - 0.5) * -1) + 0.1) * -1;

                                }
                                //entity rotation
                                if (true)
                                {
                                    //general up-right
                                    if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 0.8;
                                        // Console.WriteLine("general up-right");

                                    }
                                    //general down-right
                                    if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                                    {
                                        mobileEntity.Angle = 2.2;
                                        //Console.WriteLine("general down-right");
                                    }
                                    //general down-left
                                    if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = 3.8;
                                        // Console.WriteLine("general down-left");
                                    }
                                    //general up-left
                                    if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                                    {
                                        mobileEntity.Angle = -0.8;
                                        //  Console.WriteLine("general up-left");
                                    }

                                    // if speed by Y is low  \/ \/
                                    if (mobileEntity.DirectionY <= 0.2 && mobileEntity.DirectionY > 0)
                                    {
                                        //general up
                                        if (mobileEntity.DirectionX < 0)
                                        {
                                            mobileEntity.Angle = 4;
                                            //   Console.WriteLine("general up");
                                        }
                                        //general down
                                        if (mobileEntity.DirectionX > 0)
                                        {
                                            mobileEntity.Angle = 1.6;
                                            // Console.WriteLine("general down");
                                        }
                                    }
                                    if (mobileEntity.DirectionY >= -0.2 && mobileEntity.DirectionY < 0)
                                    {
                                        //general up
                                        if (mobileEntity.DirectionX < 0)
                                        {
                                            mobileEntity.Angle = 4;
                                            //  Console.WriteLine("general up");
                                        }
                                        //general down
                                        if (mobileEntity.DirectionX > 0)
                                        {
                                            mobileEntity.Angle = 1.6;
                                            //  Console.WriteLine("general down");
                                        }
                                    }

                                    // if speed by X is low  \/ \/
                                    if (mobileEntity.DirectionX <= 0.2 && mobileEntity.DirectionX > 0)
                                    {
                                        //general left
                                        if (mobileEntity.DirectionY < 0)
                                        {
                                            mobileEntity.Angle = 0;
                                            //  Console.WriteLine("general left");
                                        }
                                        //general right
                                        if (mobileEntity.DirectionY > 0)
                                        {
                                            mobileEntity.Angle = 3.2;
                                            // Console.WriteLine("general right");
                                        }
                                    }
                                    if (mobileEntity.DirectionX >= -0.2 && mobileEntity.DirectionX < 0)
                                    {
                                        //general left
                                        if (mobileEntity.DirectionY < 0)
                                        {
                                            mobileEntity.Angle = 0;
                                            //  Console.WriteLine("general left");
                                        }
                                        //general right
                                        if (mobileEntity.DirectionY > 0)
                                        {
                                            mobileEntity.Angle = 3.2;
                                            // Console.WriteLine("general right");
                                        }
                                    }
                                }

                            }

                            
                            if (mobileEntity.EntityTiks % 240 == 0)
                            {
                                double senseboxX = mobileEntity.X - wolfSenseDistanse;
                                double sensekboxY = mobileEntity.Y - wolfSenseDistanse;
                                double senseboxHight = mobileEntity.X +50 + wolfSenseDistanse;
                                double senseboxWidth = mobileEntity.Y +50 + wolfSenseDistanse;

                                players.ForEach(player =>
                                {
                                    if (player.IsAlive == true) { 
                                        if (player.X + playerSize >= senseboxX && player.Y + playerSize >= sensekboxY && player.X <= senseboxHight &&
                                        player.Y <= senseboxWidth)
                                        {
                                            mobileEntity.AgrivatedBy = "player";
                                            mobileEntity.AgrivatedById = player.Id;
                                        }
                                    }

                                });

                                
                                mobileEntities.ForEach(otherEntity =>
                                {
                                    if (otherEntity.Id != mobileEntity.Id && otherEntity.IsAlive == true)
                                    {
                                        if (otherEntity.X + 50 >= senseboxX && otherEntity.Y + 50 >= sensekboxY && otherEntity.X <= senseboxHight &&
                                        otherEntity.Y <= senseboxWidth)
                                        {
                                            mobileEntity.AgrivatedBy = otherEntity.Type;
                                            mobileEntity.AgrivatedById = otherEntity.Id;
                                        }
                                    }
                                });
                                 
                                
                            }
                            
                        }
                        else
                        {

                            if (mobileEntity.AgrivatedBy == "player")
                            {
                                var obj = players.FirstOrDefault(x => x.Id == mobileEntity.AgrivatedById);

                                if (mobileEntity.X < obj.X)
                                {
                                    mobileEntity.DirectionX = 1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 2.2;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = 0.8;
                                    }
                                }

                                if (mobileEntity.X > obj.X)
                                {
                                    mobileEntity.DirectionX = -1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 3.8;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = -0.8;
                                    }
                                }

                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = -1;
                                    mobileEntity.AgrivatedBy = "";
                                }
                            }
                            else
                            {
                                var obj = mobileEntities.FirstOrDefault(x => x.Id == mobileEntity.AgrivatedById);

                                if (mobileEntity.X < obj.X)
                                {
                                    mobileEntity.DirectionX = 1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 2.2;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = 0.8;
                                    }
                                }

                                if (mobileEntity.X > obj.X)
                                {
                                    mobileEntity.DirectionX = -1;

                                    if (mobileEntity.Y < obj.Y)
                                    {
                                        mobileEntity.DirectionY = 1;
                                        mobileEntity.Angle = 3.8;
                                    }
                                    if (mobileEntity.Y > obj.Y)
                                    {
                                        mobileEntity.DirectionY = -1;
                                        mobileEntity.Angle = -0.8;
                                    }
                                }

                                if (obj.IsAlive == false)
                                {
                                    mobileEntity.AgrivatedById = -1;
                                    mobileEntity.AgrivatedBy = "";
                                }
                            }


                            if (mobileEntity.EntityTiks % 120 == 0)
                            {
                                Attack(mobileEntity.X, mobileEntity.Y, mobileEntity.Type, mobileEntity.Id, 0, "no tool", 0, 0);
                            }
                        }

                    }
                }

                //movement and check for movement
                if (true)
                {
                    islegalmoveX = true;
                    islegalmoveX_ = true;
                    islegalmoveY = true;
                    islegalmoveY_ = true;

                    if (mobileEntity.X + mobileEntity.Width > mapendX)
                    {
                        islegalmoveX = false;
                    }
                    if (mobileEntity.X < mapstartX)
                    {
                        islegalmoveX_ = false;
                    }
                    if (mobileEntity.Y + mobileEntity.Heigth > mapendY)
                    {
                        islegalmoveY = false;
                    }
                    if (mobileEntity.Y < mapstartY)
                    {
                        islegalmoveY_ = false;
                    }
                    imobileObjs.ForEach(Object =>
                    {
                        if (mobileEntity.X + mobileEntity.Width >= Object.X && mobileEntity.X <= Object.X + 100 &&
                        mobileEntity.Y + mobileEntity.Heigth >= Object.Y && mobileEntity.Y <= Object.Y + 100)
                        {

                            //  Console.WriteLine("work" + mobileEntity.Id);
                            if (mobileEntity.X < Object.X)
                            {
                                islegalmoveX = false;
                            }
                            if (mobileEntity.X + mobileEntity.Width > Object.X + 100)
                            {
                                islegalmoveX_ = false;
                            }
                            if (mobileEntity.Y < Object.Y)
                            {
                                islegalmoveY = false;
                            }
                            if (mobileEntity.Y + mobileEntity.Heigth > Object.Y + 100)
                            {
                                islegalmoveY_ = false;
                            }
                            // Console.WriteLine(mobileEntity.X + " " + mobileEntity.Y + " " + Object.X + " " + Object.Y);
                            //Console.WriteLine(islegalmoveX +""+ islegalmoveX_ + "" + islegalmoveY + "" + islegalmoveY_);
                        }
                    });
                    imobileEntities.ForEach(imobileEntity =>
                    {
                        if (imobileEntity.IsAlive)
                        {
                            if (mobileEntity.X + mobileEntity.Width >= imobileEntity.X &&
                            mobileEntity.X <= imobileEntity.X + imobileEntity.Width &&
                            mobileEntity.Y + mobileEntity.Heigth >= imobileEntity.Y &&
                            mobileEntity.Y <= imobileEntity.Y + imobileEntity.Heigth)
                            {

                                //  Console.WriteLine("work" + mobileEntity.Id);
                                if (mobileEntity.X < imobileEntity.X)
                                {
                                    islegalmoveX = false;
                                }
                                if (mobileEntity.X + mobileEntity.Width > imobileEntity.X + imobileEntity.Width)
                                {
                                    islegalmoveX_ = false;
                                }
                                if (mobileEntity.Y < imobileEntity.Y)
                                {
                                    islegalmoveY = false;
                                }
                                if (mobileEntity.Y + mobileEntity.Heigth > imobileEntity.Y + imobileEntity.Heigth)
                                {
                                    islegalmoveY_ = false;
                                }
                                // Console.WriteLine(mobileEntity.X + " " + mobileEntity.Y + " " + Object.X + " " + Object.Y);
                                //Console.WriteLine(islegalmoveX +""+ islegalmoveX_ + "" + islegalmoveY + "" + islegalmoveY_);
                            }
                        }
                    });

                    if (mobileEntity.DirectionX > 0)
                    {
                        if (islegalmoveX)
                        {
                            mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                        }
                    }
                    else
                    {
                        if (islegalmoveX_)
                        {
                            mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                        }
                    }
                    if (mobileEntity.DirectionY > 0)
                    {
                        if (islegalmoveY)
                        {
                            mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                        }
                    }
                    else
                    {
                        if (islegalmoveY_)
                        {
                            mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                        }
                    }
                }

                mobileEntity.EntityTiks++;
            });


            if (addAnimals > 0)
            {
               // Console.WriteLine(addAnimals);

                CreateEntityByNumber(addAnimals);

                addAnimals = 0;
            }

        }

        public void MovePlayer(int x, int y, int id)
        {
            players.ForEach(player =>
            {
                if (player.Id == id)
                {
                    player.X = x;
                    player.Y = y;
                }
            });
        }

        public void Attack(double x, double y, string type, int id, double angle, string tooltype, int toolharvest, int tooldamage)
        {
            double attackboxX = 0;
            double attackboxY = 0;
            double attackboxHight = 0;
            double attackboxWidth = 0;

            if (type == "player")
            {
                // general up
                if (angle <= 0.8 && angle >= -0.8)
                {
                    attackboxX = x - playerReach;
                    attackboxY = y - playerReach;
                    attackboxHight = x + playerSize + playerReach;
                    attackboxWidth = y + playerSize + playerReach - 75;

                }

                // general right
                if (angle > 0.8 && angle <= 2.2)
                {
                    attackboxX = x - playerReach + 75;
                    attackboxY = y - playerReach;
                    attackboxHight = x + playerSize + playerReach;
                    attackboxWidth = y + playerSize + playerReach;

                }

                // general down
                if (angle > 2.2 && angle <= 3.8)
                {
                    attackboxX = x - playerReach;
                    attackboxY = y - playerReach + 75;
                    attackboxHight = x + playerSize + playerReach;
                    attackboxWidth = y + playerSize + playerReach;

                }

                // general left
                if (angle > 3.8 || angle < -0.8)
                {
                    attackboxX = x - playerReach;
                    attackboxY = y - playerReach;
                    attackboxHight = x + playerSize + playerReach - 75;
                    attackboxWidth = y + playerSize + playerReach;

                }
            }
            else
            {
                attackboxX = x - playerReach;
                attackboxY = y - playerReach;
                attackboxHight = x + playerSize + playerReach;
                attackboxWidth = y + playerSize + playerReach;
            }


            List<int> attackedThingsId = new List<int>();
            players.ForEach(player =>
            {
                if (type == "player") {
                    if (player.Id == id)
                    {
                    }
                    else
                    {
                        if (player.X + playerSize >= attackboxX && player.Y + playerSize >= attackboxY && player.X <= attackboxHight &&
                        player.Y <= attackboxWidth)
                        {
                            if (tooltype == "sword") {
                                player.Hp -= 10;
                            }
                            else
                            {
                                player.Hp -= 5;
                            }
                            attackedThingsId.Add(player.Id);

                            if (player.Hp <= 0 && player.IsAlive)
                            {
                                player.IsAlive = false;
                                players[id].Points += 50;
                            }
                        }
                    }
                }
                else
                {
                    if (player.X + playerSize >= attackboxX && player.Y + playerSize >= attackboxY && player.X <= attackboxHight &&
                        player.Y <= attackboxWidth)
                    {
                        player.Hp -= 10;
                        attackedThingsId.Add(player.Id);

                        if (player.Hp <= 0 && player.IsAlive)
                        {
                            player.IsAlive = false;
                        }
                    }
                }
            });

            if (type == "player")
            {
                imobileObjs.ForEach(Object =>
                {
                    if (Object.X + playerSize >= attackboxX && Object.Y + playerSize >= attackboxY && Object.X <= attackboxHight &&
                    Object.Y <= attackboxWidth)
                    {
                        if (Object.Type == "tree")
                        { // Object.Type is tree/rock
                            if (tooltype == "axe")
                            {
                               // Console.WriteLine(toolharvest);
                                players[id].Wood += toolharvest;
                            }
                            else
                            {
                                players[id].Wood += 1;
                            }
                        }
                        else if (Object.Type == "rock")
                        {
                            if (tooltype == "pickaxe")
                            {
                               // Console.WriteLine(toolharvest);
                                players[id].Stone += toolharvest;
                            }
                            else
                            {
                                players[id].Stone += 1;
                            }
                            
                        }
                    }
                });

                imobileEntities.ForEach(imobileEntity =>
                {
                    if (imobileEntity.X + playerSize >= attackboxX && imobileEntity.Y + playerSize >= attackboxY && imobileEntity.X <= attackboxHight &&
                    imobileEntity.Y <= attackboxWidth)
                    {
                        if (imobileEntity.IsAlive)
                        {
                            if (tooltype == "axe")
                            {
                                imobileEntity.Hp -= tooldamage;
                            }
                            else
                            {
                                imobileEntity.Hp -= 5;
                            }

                            if (imobileEntity.Hp <= 0)
                            {
                                imobileEntity.IsAlive = false;
                                Console.WriteLine(imobileEntity.IsAlive);
                            }
                            // Console.WriteLine(imobileEntity.Hp);
                        }
                    }

                });

                mobileEntities.ForEach(mobileEntity =>
                {
                    if (mobileEntity.X + playerSize >= attackboxX && mobileEntity.Y + playerSize >= attackboxY && mobileEntity.X <= attackboxHight &&
                    mobileEntity.Y <= attackboxWidth)
                    {
                        if (mobileEntity.IsAlive)
                        {
                            if (tooltype == "sword")
                            {
                                mobileEntity.Hp -= tooldamage;
                            }
                            else
                            {
                                mobileEntity.Hp -= 5;
                            }

                            // general up
                            if (angle <= 0.8 && angle >= -0.8)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = -1;
                                mobileEntity.Angle = angle;
                            }

                            // general right
                            if (angle > 0.8 && angle <= 2.2)
                            {
                                mobileEntity.DirectionX = 1;
                                mobileEntity.DirectionY = 0;
                                mobileEntity.Angle = angle;
                            }

                            // general down
                            if (angle > 2.2 && angle <= 3.8)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 1;
                                mobileEntity.Angle = angle;
                            }

                            // general left
                            if (angle > 3.8 || angle < -0.8)
                            {
                                mobileEntity.DirectionX = -1;
                                mobileEntity.DirectionY = 0;
                                mobileEntity.Angle = angle;
                            }

                            attackedThingsId.Add(mobileEntity.Id);

                            if (mobileEntity.Type == "cow")
                            {
                                mobileEntity.AgrivatedBy = type;
                                mobileEntity.AgrivatedById = id;
                            }

                            if (mobileEntity.Hp <= 0)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 0;

                                addAnimals++;

                                mobileEntity.IsAlive = false;
                                Console.WriteLine(mobileEntity.IsAlive);

                                players[id].Points += 10;
                            }
                        }
                    }
                   
                });
            }
            else
            {
                mobileEntities.ForEach(mobileEntity =>
                {
                    if (mobileEntity.Type == type)
                    {
                    }
                    else
                    {
                        if (mobileEntity.X + playerSize >= attackboxX && mobileEntity.Y + playerSize >= attackboxY && mobileEntity.X <= attackboxHight &&
                        mobileEntity.Y <= attackboxWidth)
                        {
                            if (mobileEntity.IsAlive)
                            {

                                mobileEntity.Hp -= 10;
                                attackedThingsId.Add(mobileEntity.Id);

                                if (mobileEntity.Type == "cow")
                                {
                                    mobileEntity.AgrivatedBy = type;
                                    mobileEntity.AgrivatedById = id;
                                }

                                if (mobileEntity.Hp <= 0)
                                {
                                    addAnimals++;

                                    mobileEntity.IsAlive = false;

                                    //mobileEntity[id] = 6;
                                    Console.WriteLine(mobileEntity.IsAlive);
                                }
                            }
                        }
                    }
                });
            }

        }
    }
}
