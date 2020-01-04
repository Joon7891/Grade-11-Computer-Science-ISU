//Name: Joon Song
//File Name: IO.cs
//Project Name: ISU_Song
//Creation Date: Janurary 2, 2018
//Modified Date: Janurary 21, 2018
//Description: Class to handle file input and output

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Driver
{
    class IO
    {
        //IO Variables required for file reading and writing logic
        static StreamWriter outFile;
        static StreamReader inFile;

        //Pre: None
        //Post: Game user data is read and setup
        //Description: Subprogram to handle file reading for game user data
        public static void GetUserData()
        {
            //Temporary locations for various game data
            int[] userResources = new int[4];
            int currentWaveNo = 0;
            int numTowers;
            string[] towerData;

            //Try-Catch logic for file reading
            try
            {
                //Opening file for reading
                inFile = File.OpenText("UserData.txt");

                //Getting user resources
                userResources = inFile.ReadLine().Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                //Getting current wave number
                currentWaveNo = Convert.ToInt32(inFile.ReadLine());

                //Getting number of towers
                numTowers = Convert.ToInt32(inFile.ReadLine());

                //Adding appropriate tower
                for (int i = 0; i < numTowers; i++)
                {
                    //Getting data
                    towerData = inFile.ReadLine().Split(',');

                    //Determing type of tower and adding it
                    if (towerData[0] == "Cannon")
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                            new Towers.Cannon(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                    else if (towerData[0] == "Archer")
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                           new Towers.Archer(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                    else if (towerData[0] == "Bomber")
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                           new Towers.Bomber(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                    else if (towerData[0] == "Roaster")
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                           new Towers.Roaster(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                    else if (towerData[0] == "Forestry")
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                           new Towers.Forestry(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                    else
                    {
                        GUI.GameLevel.towerDictionary.Add(new Vector2(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2])),
                           new Towers.SteelMine(Convert.ToInt32(towerData[1]), Convert.ToInt32(towerData[2]), Convert.ToInt32(towerData[3]), Convert.ToInt32(towerData[4]), Convert.ToInt32(towerData[5])));
                    }
                }

                //Closing file
                inFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file has not been found and closing file
                Console.WriteLine("Error: The file cannot be found");
                inFile.Close();
            }
            catch (EndOfStreamException)
            {
                //Informing user that file was read past its end and closing file
                Console.WriteLine("Error: Attempted to read past end of file");
                inFile.Close();
            }

            //Setting data at temporary location to data at permanent locations
            GameData.Player.lives = userResources[0];
            GameData.Player.gold = userResources[1];
            GameData.Player.wood = userResources[2];
            GameData.Player.steel = userResources[3];
            GameLogic.currentWaveNo = currentWaveNo;
        }

        //Pre: None
        //Post: Game user data is written
        //Description: Subprogram to handle file writing for game user data
        public static void SetUserData()
        {            
            //Try-Catch logic for file writing
            try
            {
                //Opening file for writing
                outFile = File.CreateText("UserData.txt");

                //Writing user resource data
                outFile.WriteLine("{0},{1},{2},{3}", GameData.Player.lives, GameData.Player.gold, GameData.Player.wood, GameData.Player.steel);

                //Writing current wave number
                outFile.WriteLine(GameLogic.currentWaveNo);

                //Writing number of towers 
                outFile.WriteLine(GUI.GameLevel.towerDictionary.Count);

                //Writing tower data
                foreach(KeyValuePair<Vector2, Towers.Tower> newTower in GUI.GameLevel.towerDictionary)
                {
                    outFile.WriteLine(newTower.Value.GetInformation());
                }

                //Closing file
                outFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file cannot be found and closing file
                Console.WriteLine("Error: The file cannot be found");
                outFile.Close();
            }
        }

        //Pre: None
        //Post: Global game data is read and setup
        //Description: Subprogram to handle file reading for global game data
        public static void GetGlobalData()
        {
            //Temporary locations for game data
            string[][] leaderboardData = new string[10][];
            
            //Try-Catch logic for file reading
            try
            {
                //Opening file for reading
                inFile = File.OpenText("GlobalData.txt");

                //Getting total played time
                Main.totalTimePlayed = Convert.ToDouble(inFile.ReadLine());

                //Getting settings data
                Main.actualVolume = (inFile.ReadLine().Split(',').Select(n => (float)Convert.ToDouble(n)).ToArray());

                //Reading and setting up leaderboard data
                for (int i = 0; i < 10; i++)
                {
                    leaderboardData[i] = inFile.ReadLine().Split(',');
                    Main.leaderboardScores[i] = new GameData.LeaderboardScore(leaderboardData[i][0], Convert.ToInt32(leaderboardData[i][1]));
                }

                //Closing file
                inFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file cannot be found and closing file
                Console.WriteLine("Error: The file cannot be found");
                inFile.Close();
            }
            catch (EndOfStreamException)
            {
                //Informing user that file was read past its end and closing file
                Console.WriteLine("Error: Attempted to read past end of file");
                inFile.Close();
            }
        }

        //Pre: None
        //Post: Global game data is written
        //Description: Subprogram to handle file writing for global game data
        public static void SetGlobalData()
        {
            //Try-Catch logic for file writing
            try
            {
                //Opening file for writing
                outFile = File.CreateText("GlobalData.txt");

                //Writing total play time
                outFile.WriteLine("" + Main.totalTimePlayed);

                //Writing settings data
                outFile.WriteLine("{0},{1}", Main.actualVolume[0], Main.actualVolume[1]);

                //Writing leaderboard data
                for (int i = 0; i < 10; i++)
                {
                    outFile.WriteLine("{0},{1}", Main.leaderboardScores[i].name, Main.leaderboardScores[i].score);
                }

                //Closing file
                outFile.Close();
            }
            catch (FileNotFoundException)
            {
                //Informing user that file cannot be found and closing file
                Console.WriteLine("Error: The file cannot be found");
                outFile.Close();
            }
        }
    }
}
