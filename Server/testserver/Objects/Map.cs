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
        public int entityNumber = -1;

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
        public void CreateObj(int x, int y, int id, string type)
        {
            StationryObj imobileobject = new StationryObj() { X = x, Y = y, Id = id, Type = type };
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
                int randm = rnd.Next(0, 100);
                if (randm <= 33)
                {
                    Console.WriteLine("rabit ");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 50, "rabit");
                }
                else if(randm <= 66)
                {
                    Console.WriteLine("pig");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "pig");
                }
                else
                {
                    Console.WriteLine("cow");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "cow");
                }
            }

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

                            //general up-right
                            if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0) 
                            {
                                mobileEntity.Angle = 0.8;
                            }
                            //general down-right
                            if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                            {
                                mobileEntity.Angle = 2.2;
                            }
                            //general down-left
                            if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                            {
                                mobileEntity.Angle = 3.8;
                            }
                            //general up-left
                            if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                            {
                                mobileEntity.Angle = -0.8;
                            }

                            // if speed by Y is low  \/ \/
                            if (mobileEntity.DirectionY<=0.4 && mobileEntity.DirectionY> 0 || mobileEntity.DirectionY >= -0.4 && mobileEntity.DirectionY < 0)
                            {
                                //general up
                                if (mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 0;
                                }
                                //general down
                                if (mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 3;
                                }
                            }
                            // if speed by X is low  \/ \/
                            if (mobileEntity.DirectionX <= 0.4 && mobileEntity.DirectionX > 0 || mobileEntity.DirectionX >= -0.4 && mobileEntity.DirectionX < 0)
                            {
                                //general left
                                if (mobileEntity.DirectionY < 0)
                                {
                                    mobileEntity.Angle = 4;
                                }
                                //general right
                                if (mobileEntity.DirectionY > 0)
                                {
                                    mobileEntity.Angle = 1.6;
                                }
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
                }
                else if (mobileEntity.Type == "pig")
                {
                    if (mobileEntity.IsAlive)
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
                            //general up-right
                            if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX < 0)
                            {
                                mobileEntity.Angle = 0.8;
                                Console.WriteLine("general up-right");

                            }
                            //general down-right
                            if (mobileEntity.DirectionY > 0 && mobileEntity.DirectionX > 0)
                            {
                                mobileEntity.Angle = 2.2;
                                Console.WriteLine("general down-right");
                            }
                            //general down-left
                            if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX > 0)
                            {
                                mobileEntity.Angle = 3.8;
                                Console.WriteLine("general down-left");
                            }
                            //general up-left
                            if (mobileEntity.DirectionY < 0 && mobileEntity.DirectionX < 0)
                            {
                                mobileEntity.Angle = -0.8;
                                Console.WriteLine("general up-right");
                            }

                            // if speed by Y is low  \/ \/
                            if (mobileEntity.DirectionY <= 0.2 && mobileEntity.DirectionY > 0 )
                            {
                                //general up
                                if (mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 0;
                                    Console.WriteLine("general up");
                                }
                                //general down
                                if (mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 3;
                                    Console.WriteLine("general down");
                                }
                            }
                            if (mobileEntity.DirectionY >= -0.2 && mobileEntity.DirectionY < 0)
                            {
                                //general up
                                if (mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 0;
                                    Console.WriteLine("general up");
                                }
                                //general down
                                if (mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 3;
                                    Console.WriteLine("general down");
                                }
                            }

                            // if speed by X is low  \/ \/
                            if (mobileEntity.DirectionX <= 0.2 && mobileEntity.DirectionX > 0)
                            {
                                //general left
                                if (mobileEntity.DirectionY < 0)
                                {
                                    mobileEntity.Angle = 4;
                                    Console.WriteLine("general left");
                                }
                                //general right
                                if (mobileEntity.DirectionY > 0)
                                {
                                    mobileEntity.Angle = 1.6;
                                    Console.WriteLine("general right");
                                }
                            }
                            if (mobileEntity.DirectionX >= -0.2 && mobileEntity.DirectionX < 0)
                            {
                                //general left
                                if (mobileEntity.DirectionX < 0)
                                {
                                    mobileEntity.Angle = 4;
                                    Console.WriteLine("general left");
                                }
                                //general right
                                if (mobileEntity.DirectionX > 0)
                                {
                                    mobileEntity.Angle = 1.6;
                                    Console.WriteLine("general right");
                                }
                            }

                            Console.WriteLine(mobileEntity.DirectionX);
                            Console.WriteLine(mobileEntity.DirectionY);
                             Console.WriteLine(mobileEntity.Angle);

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
                
            });

            tiks++;
        }

        public void Attack(int x, int y, int id, double angle)
        {
            int attackboxX = 0;
            int attackboxY = 0;
            int attackboxHight = 0;
            int attackboxWidth = 0;

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

            List<int> attackedPlayersId = new List<int>();

            players.ForEach(player =>
            {
                if (player.Id == id)
                {
                }
                else
                {
                    if (player.X + playerSize >= attackboxX && player.Y + playerSize >= attackboxY && player.X <= attackboxHight &&
                    player.Y <= attackboxWidth)
                    {
                        player.Hp -= 10;
                        attackedPlayersId.Add(player.Id);

                        if (player.Hp <= 0 && player.IsAlive)
                        {
                            player.IsAlive = false;
                            players[id].Points += 50;
                        }
                    }
                }
            });

            imobileObjs.ForEach(Object =>
            {
                if (Object.Id == id)
                {
                }
                else
                {
                    if (Object.X + playerSize >= attackboxX && Object.Y + playerSize >= attackboxY && Object.X <= attackboxHight &&
                    Object.Y <= attackboxWidth)
                    {
                        if (Object.Type == "tree")
                        { // Object.Type is tree/rock
                            players[id].Wood += 10;
                        }
                        else
                        {
                            players[id].Stone += 10;
                        }
                    }
                }
            });

            int addAnimals = 0;
            mobileEntities.ForEach(mobileEntity =>
            {
                if (mobileEntity.X + playerSize >= attackboxX && mobileEntity.Y + playerSize >= attackboxY && mobileEntity.X <= attackboxHight &&
                    mobileEntity.Y <= attackboxWidth)
                {
                    if (mobileEntity.IsAlive)
                    {

                        mobileEntity.Hp -= 10;
                        attackedPlayersId.Add(mobileEntity.Id);

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
            if (addAnimals > 0)
            {
                for (int i = 0; i < addAnimals; i++)
                {
                    int randm = rnd.Next(0, 100);
                    if (randm <= 33)
                    {
                        Console.WriteLine("rabit ");
                        CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 50, "rabit");
                    }
                    else if (randm <= 66)
                    {
                        Console.WriteLine("pig");
                        CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "pig");
                    }
                    else
                    {
                        Console.WriteLine("cow");
                        CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), 100, "cow");
                    }
                }
            }

        }
    }
}
