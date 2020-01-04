//Name: Joon Song
//File Name: Bomber.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for bomber tower object (Child Class to Tower Class)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Towers
{
    public class Bomber : DefenseTower
    {
        //Texture2D variable to hold image of bomber tower
        public static Texture2D bomberImg;

        //Constants to hold price of tower
        public const int GOLD_PRICE = 0;
        public const int WOOD_PRICE = 50;
        public const int STEEL_PRICE = 500;

        //Various tower type specific constants
        public const int DEFAULT_RANGE = 100;
        public const int DEFAULT_DAMAGE = 200;
        public const int SHOOT_TIME = 60;

        //Constructor for Bomber object
        public Bomber(int x, int y) : base(x, y)
        {
            //Setting image and type of bomber
            img = bomberImg;
            towerType = "Bomber";

            //Setting tower price
            priceGold = GOLD_PRICE;
            priceWood = WOOD_PRICE;
            priceSteel = STEEL_PRICE;

            //Setting damage, range, and shoot time of tower
            propertyValue[0] = DEFAULT_DAMAGE;
            propertyValue[1] = DEFAULT_RANGE;
            shootFrameMax = SHOOT_TIME;

            //Setting up defense tower range circles
            currentRangeCircle = new GUI.Circle(center, (int)propertyValue[1], Color.White * 0.25f);
            futureRangeCircle = new GUI.Circle(center, (int)Math.Round(propertyValue[1] * 1.25), Color.Green * 0.25f);

            //Setting up shop item variables for possible upgrades
            upgradeItems[0] = new GameData.ShopItem("Damage +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Damage", (int)Math.Round(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            upgradeItems[1] = new GameData.ShopItem("Range +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Range", (int)Math.Round(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
        }


        //Overloaded constructor for defense tower class
        public Bomber(int x, int y, int propLevel1, int propLevel2, int total) : base(x, y, propLevel1, propLevel2, total)
        {
            //Setting image and type of archer
            img = bomberImg;
            towerType = "Bomber";

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
        //Post: Various bomber defense tower updates are made
        //Description: Subprogram to hold update logic for bomber defense tower
        public override void Update()
        {
            //Calling base update subprogram
            base.Update();

            //If there exists a enemy within range and its time to shoot
            if (Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList).Count > 0 &&
                Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex].relativePos > 0 &&
               currentShootFrame > shootFrameMax)
            {
                //Adding bomb projectile
                projectileList.Add(new Projectiles.Bomb(center,
                    Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex]));

                //Resetting time
                currentShootFrame = 0;
            }
        }
    }
}
