//Name: Joon Song
//File Name: Archer.cs
//Project Name: ISU_Song
//Creation Date: December 26, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for archer tower object (Child Class to Tower Class)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Towers
{
    public class Archer : DefenseTower
    {
        //Texture2D variable to hold image of archer tower
        public static Texture2D archerImg;

        //Constants for tower price
        public const int GOLD_PRICE = 0;
        public const int WOOD_PRICE = 150;
        public const int STEEL_PRICE = 300;

        //Various tower type specific constants
        public const int DEFAULT_RANGE = 150;
        public const int DEFAULT_DAMAGE = 50;
        public const float SHOOT_TIME = 12;

        //Constructor for Archer object
        public Archer(int x, int y) : base(x, y)
        {
            //Setting image and type of archer
            img = archerImg;
            towerType = "Archer";

            //Setting tower price
            priceGold = GOLD_PRICE;
            priceWood = WOOD_PRICE;
            priceSteel = STEEL_PRICE;

            //Setting damage, range, and shoot time of tower
            propertyValue[0] = DEFAULT_DAMAGE;
            propertyValue[1] = DEFAULT_RANGE;
            shootFrameMax = SHOOT_TIME;

            //Setting up defense tower range circles
            currentRangeCircle = new GUI.Circle(center, propertyValue[1], Color.White * 0.25f);
            futureRangeCircle = new GUI.Circle(center, (int)Math.Round(propertyValue[1] * 1.25), Color.Green * 0.25f);

            //Setting up shop item variables for possible upgrades
            upgradeItems[0] = new GameData.ShopItem("Damage +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Damage", (int)Math.Round(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            upgradeItems[1] = new GameData.ShopItem("Range +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Range", (int)Math.Round(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
        }

        //Overloaded constructor for defense tower class
        public Archer(int x, int y, int propLevel1, int propLevel2, int total) : base(x, y, propLevel1, propLevel2, total)
        {
            //Setting image and type of archer
            img = archerImg;
            towerType = "Archer";

            //Setting tower price
            priceGold = GOLD_PRICE;
            priceWood = WOOD_PRICE;
            priceSteel = STEEL_PRICE;

            //Setting damage, range, shoot time, and kills
            propertyValue[0] = (int)(DEFAULT_DAMAGE * Math.Pow(1.25, propLevel1));
            propertyValue[1] = (int)(DEFAULT_RANGE * Math.Pow(1.25, propLevel2));
            shootFrameMax = SHOOT_TIME;
            totalKills = total;

            //Setting up defense tower range circles
            currentRangeCircle = new GUI.Circle(center, propertyValue[1], Color.White * 0.25f);
            futureRangeCircle = new GUI.Circle(center, (int)Math.Round(propertyValue[1] * 1.25), Color.Green * 0.25f);

            //Selection to either update 1st shop item or clear shop item, depending on level
            if (propertyLevel[0] < 4)
            {
                upgradeItems[0] = new GameData.ShopItem("Damage +", propertyLevel[0] * (priceGold) / 2, propertyLevel[0] * (priceWood) / 2, propertyLevel[0] * (priceSteel) / 2, "Damage", (int)(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            }
            else
            {
                upgradeItems[0] = new GameData.ShopItem(0);
                upgradeItems[0].Clear();
            }

            //Selection to either update 2nd shop item or clear shop item, depending on level
            if (propertyLevel[1] < 4)
            {
                upgradeItems[1] = new GameData.ShopItem("Range +", propertyLevel[1] * (priceGold) / 2, propertyLevel[1] * (priceWood) / 2, propertyLevel[1] * (priceSteel) / 2, "Range", (int)(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
            }
            else
            {
                upgradeItems[1] = new GameData.ShopItem(1);
                upgradeItems[1].Clear();
            }
        }

        //Pre: None
        //Post: Various archer defense tower updates are made
        //Description: Subprogram to hold update logic for archer defense tower
        public override void Update()
        {
            //Calling base update subprogram
            base.Update();

            //If there exists a enemy within range and its time to shoot
            if (Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList).Count > 0 &&
                Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex].relativePos > 0 &&
               currentShootFrame > shootFrameMax)
            {
                //Adding arrow projectile
                projectileList.Add(new Projectiles.Arrow(center,
                    Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex]));

                //Resetting time
                currentShootFrame = 0;
            }
        }
    }
}
