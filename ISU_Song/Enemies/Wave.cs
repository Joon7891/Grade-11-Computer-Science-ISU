//Name: Joon Song
//File Name: Wave.cs
//Project Name: ISU_Song
//Creation Date: December 26, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for wave object

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Enemies
{
    class Wave
    {
        //List of Enemies and Tombstones        
        public List<Enemy> enemyList = new List<Enemy>();
        public List<GUI.Tombstone> tombstoneList = new List<GUI.Tombstone>();

        //Constructor for wave class
        public Wave(int knightNo, int archerNo, int banditNo, int skeletonNo, int goblinNo, int princeNo, int wizardNo, int fairyNo, int witchNo, int dragonNo)
        {
            //Adding dragons            
            for (int i = 0; i < dragonNo; i++)
            {
                enemyList.Add(new Dragon(-20 * i));
            }

            //Adding witches
            for (int i = 0; i < witchNo; i++)
            {
                enemyList.Add(new Witch(-60 * i));
            }

            //Adding fairies
            for (int i = 0; i < fairyNo; i++)
            {
                enemyList.Add(new Fairy(-50 * i));
            }

            //Adding wizards
            for (int i = 0; i < wizardNo; i++)
            {
                enemyList.Add(new Wizard(-50 * i));
            }

            //Adding princes
            for (int i = 0; i < princeNo; i++)
            {
                enemyList.Add(new Prince(-50 * i));
            }

            //Adding goblins
            for (int i = 0; i < goblinNo; i++)
            {
                enemyList.Add(new Goblin(-75 * i));
            }

            //Adding bandits
            for (int i = 0; i < banditNo; i++)
            {
                enemyList.Add(new Bandit(-75 * i));
            }

            //Adding archers
            for (int i = 0; i < archerNo; i++)
            {
                enemyList.Add(new Archer(-70 * i));
            }

            //Adding skeletons
            for (int i = 0; i < skeletonNo; i++)
            {
                enemyList.Add(new Skeleton(-80 * i));
            }

            //Adding knights
            for (int i = 0; i < knightNo; i++)
            {
                enemyList.Add(new Knight(-50 * i));
            }
        }

        //Pre: None
        //Post: Various wave components are updated
        //Description: Subprogram to hold update logic of enemy wave
        public void Update()
        {
            //Calling each enemy            
            for(int i = 0; i < enemyList.Count; i++)
            {
                //Updating each enemy
                enemyList[i].Update();
                
                //Removing enemy if they have no health or went off screen
                if (enemyList[i].currentHealth <= 0 ||
                    enemyList[i].relativePos > 2025)
                {
                    //Determining type of death                    
                    if(enemyList[i].relativePos > 2025)
                    {
                        //If enemy went off screen, update player health                        
                        GameData.Player.lives -= enemyList[i].weight;
                    }
                    else if (enemyList[i].currentHealth <= 0)
                    {
                        //If enemy died, add tombstone and reward user                        
                        tombstoneList.Add(new GUI.Tombstone(enemyList[i].rect));
                        enemyList[i].AddLoot();
                    }

                    //Removing enemy
                    enemyList.RemoveAt(i);
                    i--;
                }
            }

            //Calling each tombstone
            for (int i = 0; i < tombstoneList.Count; i++)
            {
                //Updating each tombstone                
                tombstoneList[i].Update();

                //Removing tombstones if they are no longer visible
                if (tombstoneList[i].transparency <= 0.0f)
                {
                    tombstoneList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Pre: None
        //Post: Wave of enemies are drawn
        //Description: Subprogram to hold draw logic of enemy wave
        public void Draw()
        {
            //Drawing tombstones
            foreach (GUI.Tombstone newTombstone in tombstoneList)
            {
                newTombstone.Draw();
            }
            
            //Drawing enemies
            foreach(Enemy newEnemy in enemyList)
            {
                newEnemy.Draw();
            }

            //Drawing enemy health
            foreach(Enemy newEnemy in enemyList)
            {
                newEnemy.DrawHealth();
            }
        }
    }
}
