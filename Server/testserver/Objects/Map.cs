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

        public List<MobileEntity> mobileEntities { get; set; } = new List<MobileEntity>();

        public int objectNumber = -1;
        public int entityNumber = -1;

        public int tiks { get; set; } = 0;

        public int mapstartX = 0;
        public int mapstartY = 0;
        public int mapendX = 4000;
        public int mapendY = 2400;

        public int playerReach = 50;
        public int playerSize = 50;

        public int wolfSenseDistanse = 100;

        public int addAnimals = 0;

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
        public void CreateEntity(int x, int y, double Hp, string type)
        {
            entityNumber++;
            MobileEntity mobileEntity = new MobileEntity() { X = x, Y = y, Hp = Hp, Id = entityNumber, Type = type };
            mobileEntities.Add(mobileEntity);

            // Console.WriteLine(mobileEntity.Type);
        }
        public void CreateTool(int playerid, string type, int tier)
        {
            players.ForEach(player =>
            {
                if (player.Id == playerid)
                {
                    int id = -1;
                    player.playertools.ForEach(tool => {
                        id++;
                    });
                    id++;

                    Tool newtool = new Tool() {Id = id, Type = type, Tier = tier};

                    player.playertools.Add(newtool);

                    //  Console.WriteLine(newtool.Type);
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
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 60, "rabit");
                }
                else if (randm > 35 && randm <= 70)
                {
                    Console.WriteLine("pig " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "pig");
                }
                else if (randm > 70 && randm <= 90)
                {
                    Console.WriteLine("cow " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 120, "cow");
                }
                else if (randm > 90)
                {
                    Console.WriteLine("wolf " + randm);
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "wolf");
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
                        if (tiks % 100 == 0)
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

                        if (mobileEntity.X + 50 > mapendX)
                        {
                            mobileEntity.DirectionX = -1;
                        }
                        if (mobileEntity.X < mapstartX)
                        {
                            mobileEntity.DirectionX = 1;
                        }
                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;


                        if (mobileEntity.Y + 50 > mapendY)
                        {
                            mobileEntity.DirectionY = -1;
                        }
                        if (mobileEntity.Y < mapstartY)
                        {
                            mobileEntity.DirectionY = 1;
                        }
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
                }
                else if (mobileEntity.Type == "pig")
                {
                    if (mobileEntity.IsAlive)
                    {
                        if (tiks % 500 == 0)
                        {
                            mobileEntity.DirectionX = 0;
                            mobileEntity.DirectionY = 0;

                        }
                        if (tiks % 1000 == 0)
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

                        if (mobileEntity.X + 50 > mapendX)
                        {
                            mobileEntity.DirectionX = -1;
                        }
                        if (mobileEntity.X < mapstartX)
                        {
                            mobileEntity.DirectionX = 1;
                        }
                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;


                        if (mobileEntity.Y + 50 > mapendY)
                        {
                            mobileEntity.DirectionY = -1;
                        }
                        if (mobileEntity.Y < mapstartY)
                        {
                            mobileEntity.DirectionY = 1;
                        }
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
                }
                else if (mobileEntity.Type == "cow")
                {
                    if (mobileEntity.IsAlive)
                    {
                        
                        if (mobileEntity.AgrivatedById < 0)
                        {
                            if (tiks % 500 == 0)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 0;

                            }
                            if (tiks % 1000 == 0)
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

                            if (mobileEntity.X + 50 > mapendX)
                            {
                                mobileEntity.DirectionX = -1;
                            }
                            if (mobileEntity.X < mapstartX)
                            {
                                mobileEntity.DirectionX = 1;
                            }
                            

                            if (mobileEntity.Y + 50 > mapendY)
                            {
                                mobileEntity.DirectionY = -1;
                            }
                            if (mobileEntity.Y < mapstartY)
                            {
                                mobileEntity.DirectionY = 1;
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
                                Attack(mobileEntity.X, mobileEntity.Y, mobileEntity.Type, mobileEntity.Id, 0);
                            }
                        }

                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
                }
                else if (mobileEntity.Type == "wolf")
                {
                    if (mobileEntity.IsAlive)
                    {
                       // Console.WriteLine(mobileEntity.AgrivatedById +" " + mobileEntity.Id);
                        if (mobileEntity.AgrivatedById < 0)
                        {
                            if (tiks % 500 == 0)
                            {
                                mobileEntity.DirectionX = 0;
                                mobileEntity.DirectionY = 0;

                            }
                            if (tiks % 1000 == 0)
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

                            if (mobileEntity.X + 50 > mapendX)
                            {
                                mobileEntity.DirectionX = -1;
                            }
                            if (mobileEntity.X < mapstartX)
                            {
                                mobileEntity.DirectionX = 1;
                            }

                            if (mobileEntity.Y + 50 > mapendY)
                            {
                                mobileEntity.DirectionY = -1;
                            }
                            if (mobileEntity.Y < mapstartY)
                            {
                                mobileEntity.DirectionY = 1;
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
                                Attack(mobileEntity.X, mobileEntity.Y, mobileEntity.Type, mobileEntity.Id, 0);
                            }
                        }

                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
                }

            });


            mobileEntities.ForEach(mobileEntity =>
            {
                mobileEntity.EntityTiks++;
            });


            if (addAnimals > 0)
            {
               // Console.WriteLine(addAnimals);

                CreateEntityByNumber(addAnimals);

                addAnimals = 0;
            }

            tiks++;
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

        public void Attack(double x, double y, string type, int id, double angle)
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
                            player.Hp -= 10;
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
                            players[id].Wood += 10;
                        }
                        else if (Object.Type == "rock")
                        {
                            players[id].Stone += 10;
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
