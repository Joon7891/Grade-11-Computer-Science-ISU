//Name: Joon Song
//File Name: DefenseTower.cs
//Project Name: ISU_Song
//Creation Date: December 23, 2017
//Modified Date: Janurary 21, 2018
//Description: Class for defense tower object (Sub-Parent Class)

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ISU_Song.Towers
{
    public abstract class DefenseTower : Tower
    {
        //Circles for current and future range
        protected GUI.Circle currentRangeCircle;
        protected GUI.Circle futureRangeCircle;

        //Variables to hold shoot time and kills
        public double currentShootFrame;
        public float shootFrameMax;
        public int totalKills = 0;

        //List of projectiles and closest enemy index
        protected List<Projectiles.Projectile> projectileList = new List<Projectiles.Projectile>();
        protected int enemyClosestIndex = 0;

        //Constructor for defense tower class
        public DefenseTower(int x, int y) : base(x, y)
        {
        }

        //Overloaded constructor for defense tower class, used for IO
        public DefenseTower(int x, int y, int propLevel1, int propLevel2, int total) : base (x, y, propLevel1, propLevel2, total)
        {
        }

        //Pre: None
        //Post: Various defense tower updates are made
        //Description: Subprogram to hold update logic for defense tower
        public override void Update()
        {
            //Calling base upgrade
            base.Update();
            
            //Index and posistion of enemy closest to tower
            double enemyClosestPos = -50;
            enemyClosestIndex = 0;

            //Updating shoot time
            currentShootFrame++;

            //Determining enemy that is closest to tower
            for (int i = 0; i < Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList).Count; i++)
            {
                if (Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[i].relativePos > enemyClosestPos)
                {
                    enemyClosestPos = Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[i].relativePos;
                    enemyClosestIndex = i;
                }
            }
            
            //Looping through each of the tower's projectiles
            for(int i = 0; i < projectileList.Count; i++)
            {
                //Updating each projectile
                projectileList[i].Update();

                //If projectile is no longer active
                if(!projectileList[i].active)
                {
                    //Inflecting damage
                    projectileList[i].targetEnemy.currentHealth -= propertyValue[0];

                    //If enemy is dead and projectile can kill, add to defense tower kill count
                    if (projectileList[i].targetEnemy.currentHealth <= 0 && projectileList[i].canKill)
                    {
                        totalKills++;
                    }

                    //Removing projectile
                    projectileList.RemoveAt(i);
                    i--;
                }
            }
        }

        //Pre: None
        //Post: Various defense tower components are drawn
        //Description: Subprogram to hold draw logic for tower defense
        public override void Draw()
        {
            //Drawing future range if tower is selected, cursor is hovering over upgrade button and range level is less than 4
            if (gridLoc == GUI.Cursor.selectedGridLoc && propertyLevel[1] < 4 &&
                Driver.CollisionDetection.PointToRectangle(upgradeItems[1].rect, GUI.Cursor.circle.center))
            {
                DrawFutureRange();
            }
            
            //Drawing each projectile
            foreach (Projectiles.Projectile newProjectile in projectileList)
            {
                newProjectile.Draw();
            }

            //Calling base draw subprogram
            base.Draw();
        }

        //Pre: None
        //Post: Defense tower details are drawn
        //Description: Subprogram to hold draw logic for tower details
        public override void DrawDetails()
        {
            //Calling base version of subprogram
            base.DrawDetails();

            //Drawing defense tower specific information
            Driver.Main.spriteBatch.DrawString(infoText, "Damage:" , infoLoc[1], Color.Red);
            Driver.Main.spriteBatch.DrawString(infoText, "Level: " + propertyLevel[0], infoLoc[2], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Value: " + propertyValue[0], infoLoc[3], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Range", infoLoc[4], Color.Blue);
            Driver.Main.spriteBatch.DrawString(infoText, "Level: " + propertyLevel[1], infoLoc[5], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Value: " + propertyValue[1], infoLoc[6], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Fire Rate: " + Math.Round(6000/shootFrameMax) / 100.0, infoLoc[7], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Total Kills: " + totalKills, infoLoc[8], Color.Red);
        }

        //Pre: None
        //Post: Draws the range of the defense tower
        //Description: Subprogram to draw the range of the defense tower
        public override void DrawCurrentRange()
        {
            //Drawing current range circle
            currentRangeCircle.Draw();
        }

        //Pre: None
        //Post: Draws the future range of the defense tower
        //Description: Subprogram to draw the future range of the defense tower
        public void DrawFutureRange()
        {
            //Drawing future range circle
            futureRangeCircle.Draw();
        }

        //Pre: None
        //Post: Upgrades the damage of the defense tower
        //Description: Subprogram to upgrade the damage of the defense tower
        public override void Upgrade1()
        {
            //Upgrading damage
            propertyValue[0] = (int)Math.Round(propertyValue[0] * 1.25);
            propertyLevel[0]++;
           
        }

        //Pre: None
        //Post: Upgrades the range of the defense tower
        //Description: Subprogram to upgrade the range if the defense tower
        public override void Upgrade2()
        {
            //Upgrading range
            propertyValue[1] = (int)Math.Round(propertyValue[1] * 1.25);
            currentRangeCircle.SetRadius(propertyValue[1]);
            futureRangeCircle.SetRadius((int)Math.Round(propertyValue[1] * 1.25));
            propertyLevel[1]++;
        }

        //Pre: None
        //Post: Defensive tower's information is written as a string
        //Description: Subprogram to condense the defense tower's information into a string
        public override string GetInformation()
        {
            //Initializing and setting return string
            string returnString;
            returnString = towerType + "," + GetCoordinates() + "," + propertyLevel[0] + "," + propertyLevel[1] + "," + totalKills;

            //Returning tower information as a string
            return returnString;
        }
    }
}
