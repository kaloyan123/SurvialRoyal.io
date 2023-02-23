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

        public int tiks { get; set; } = 0;

        public int mapstartX = 0;
        public int mapstartY = 0;
        public int mapendX = 4000;
        public int mapendY = 2400;

        public int playerReach = 50;
        public int playerSize = 50;

        public void CreatePlayer(int x, int y,int health, int id)
        {
            Player player = new Player() { X = x, Y = y, Hp = health, Id = id };
            players.Add(player);
        }

        public void Attack(int x, int y, int id)
        {
            int attackboxX = x-playerReach;
            int attackboxY = y- playerReach;
            int attackboxHight = x + playerSize + playerReach;
            int attackboxWidth = y + playerSize + playerReach;

             List<int> attackedPlayersId = new List<int>();

            //Console.WriteLine("atack");
            
            players.ForEach(player =>
            {
                if (player.Id == id)
                {
                }
                else
                {
                    if (player.X + playerSize >= attackboxX && player.Y + playerSize >= attackboxY && player.X <= attackboxHight && 
                    player.Y<= attackboxWidth)
                    {
                        player.Hp -= 10;
                       // Console.WriteLine(player.Hp);
                        attackedPlayersId.Add(player.Id);
                    }
                }
                /*
                Console.WriteLine(player.Id);

                Console.WriteLine(player.X);
                Console.WriteLine(player.Y);
                Console.WriteLine(player.X + playerSize);
                Console.WriteLine(player.Y + playerSize);
                */
            });

            mobileEntities.ForEach(mobileEntity =>
            {
                if (mobileEntity.X + playerSize >= attackboxX && mobileEntity.Y + playerSize >= attackboxY && mobileEntity.X <= attackboxHight &&
                    mobileEntity.Y <= attackboxWidth)
                {
                    mobileEntity.Hp -= 10;
                    Console.WriteLine(mobileEntity.Hp);
                    attackedPlayersId.Add(mobileEntity.Id);
                }
                /*
                if (mobileEntity.Id == id)
                {
                }
                else
                {
                    
                }
                */
                /*
                Console.WriteLine(player.Id);

                Console.WriteLine(player.X);
                Console.WriteLine(player.Y);
                Console.WriteLine(player.X + playerSize);
                Console.WriteLine(player.Y + playerSize);
                */
            });

            /*
            Console.WriteLine(attackboxX);
            Console.WriteLine(attackboxY);
            Console.WriteLine(attackboxHight);
            Console.WriteLine(attackboxWidth);
            attackedPlayersId.ForEach(attackedPlayerId =>
            {
                Console.WriteLine("attack made on");
                Console.WriteLine(attackedPlayerId);
            });
            */

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

        public void CreateObj(int x, int y, int id, string type)
        {
            StationryObj imobileobject = new StationryObj() { X = x, Y = y, Id = id, Type = type };
            imobileObjs.Add(imobileobject);

            //  Console.WriteLine(imobileobject.Type);
        }

        public void CreateEntity(int x, int y,int Hp, int id, string type)
        {
            MobileEntity mobileEntity = new MobileEntity() { X = x, Y = y, Hp = Hp, Id = id, Type = type, DirectionX = 0, DirectionY=0 };
            mobileEntities.Add(mobileEntity);

            // Console.WriteLine(mobileEntity.Type);
        }

        public void StartOfGame()
        {
            for (int i = 0; i < 15; i++)
            {
                if (rnd.Next(0, 100) <= 50)
                {
                    CreateObj(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), i, "tree");
                }
                else
                {
                    CreateObj(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), i, "rock");
                }
            }

            for (int i = 0; i < 10; i++)
            {
                if (rnd.Next(0, 100) <= 50)
                {
                    Console.WriteLine("rabit ");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 50, i, "rabit");
                }
                else
                {
                    Console.WriteLine("pig");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, i, "pig");
                }
            }

            Console.WriteLine(imobileObjs.Count);
            Console.WriteLine(mobileEntities.Count);
        }

        public void Update()
        {
            if (tiks == 0)
            {

            }

            mobileEntities.ForEach(mobileEntity =>
            {
                if (mobileEntity.Type == "rabit")
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

                        //double neshto = rnd.NextDouble() * ((0.1 - 1)*-1) + 0.1;
                        //Console.WriteLine("work eaven" + neshto);
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
                else if(mobileEntity.Type == "pig")
                {
                    if (tiks % 500 == 0)
                    {
                        mobileEntity.DirectionX = 0;
                        mobileEntity.DirectionY = 0;

                        //  Console.WriteLine("work eaven" + mobileEntity.DirectionY);
                    }
                    if (tiks % 1000 == 0)
                    {
                      //  Console.WriteLine("work pig");

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
                        //double neshto = rnd.NextDouble() * ((0.1 - 1)*-1) + 0.1;
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
                



                // Console.WriteLine(mobileEntity.X);
            });

            var obj = this.mobileEntities.FirstOrDefault(x => x.Id == 0);

            //  Console.WriteLine(obj.X);

            // Console.WriteLine("working");
              //Console.WriteLine(tiks);

            tiks++;
        }
    }
}
