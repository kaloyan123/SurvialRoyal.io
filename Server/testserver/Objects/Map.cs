﻿using System;
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

        public int mapstartX = -1900;
        public int mapstartY = -1100;
        public int mapendX = 2600;
        public int mapendY = 1600;

        public void CreatePlayer(int x, int y, int id)
        {
            Player player = new Player() { X = x, Y = y, Id = id };
            players.Add(player);
        }

        public void AttackPlayer(int x, int y, int id)
        {
            Player player = new Player() { X = x, Y = y, Id = id };
            players.Add(player);
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

        public void CreateEntity(int x, int y, int id, string type)
        {
            MobileEntity mobileEntity = new MobileEntity() { X = x, Y = y, Id = id, Type = type, DirectionX = 0, DirectionY=0 };
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
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), i, "rabit");
                }
                else
                {
                    Console.WriteLine("pig");
                    CreateEntity(rnd.Next(mapstartX + 100, mapendX - 200), rnd.Next(mapstartY + 100, mapendY - 200), i, "pig");
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

                    if (mobileEntity.X + 50 > mapendX && mobileEntity.X < mapstartX)
                    {
                        //mobileEntity.DirectionX = -1;
                    }
                    else
                    {
                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                    }

                    if (mobileEntity.Y + 50 > mapendY && mobileEntity.Y < mapstartY)
                    {
                        //mobileEntity.DirectionX = -1;
                    }
                    else
                    {
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
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

                    if (mobileEntity.X + 50 > mapendX && mobileEntity.X < mapstartX)
                    {
                        //mobileEntity.DirectionX = -1;
                    }
                    else
                    {
                        mobileEntity.X = mobileEntity.X + mobileEntity.DirectionX;
                    }

                    
                    if (mobileEntity.Y + 50 > mapendY && mobileEntity.Y < mapstartY)
                    {
                        //mobileEntity.DirectionX = -1;
                    }
                    else
                    {
                        mobileEntity.Y = mobileEntity.Y + mobileEntity.DirectionY;
                    }
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
