//Name: Joon Song
//File Name: Cannon.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for cannon tower object (Child Class to Tower Class)

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
    public class Cannon : DefenseTower
    {
        //Texture2D variable to hold image of cannon tower
        public static Texture2D cannonImg;

        //Constants to hold price of tower
        public const int GOLD_PRICE = 0;
        public const int WOOD_PRICE = 200;
        public const int STEEL_PRICE = 200;

        //Various tower type specific constants
        public const int DEFAULT_RANGE = 125;
        public const int DEFAULT_DAMAGE = 80;
        public const float SHOOT_TIME = 18;

        //Variables required for rotation of cannon tower
        private float rotation;
        private Vector2 direction;
        private Vector2 origin;
        private Rectangle sourceRect;

        //Constructor for Cannon object
        public Cannon(int x, int y) : base(x, y)
        {
            //Setting image and type of cannon
            img = cannonImg;
            towerType = "Cannon";

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

            //Setting up animation components
            sourceRect = new Rectangle(rect.X + 25, rect.Y + 25, 50, 50);
            origin = new Vector2(50, 50);
            rotation = (float)(Math.PI / 2.0);

            //Setting up shop item variables for possible upgrades
            upgradeItems[0] = new GameData.ShopItem("Damage +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Damage", (int)Math.Round(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            upgradeItems[1] = new GameData.ShopItem("Range +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Range", (int)Math.Round(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
        }

        //Overloaded constructor for defense tower class
        public Cannon(int x, int y, int propLevel1, int propLevel2, int total) : base(x, y, propLevel1, propLevel2, total)
        {
            //Setting image and type of archer
            img = cannonImg;
            towerType = "Cannon";

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

            //Setting up animation components
            sourceRect = new Rectangle(rect.X + 25, rect.Y + 25, 50, 50);
            origin = new Vector2(50, 50);
            rotation = (float)(Math.PI / 2.0);

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
        //Post: Various cannon defense tower updates are made
        //Description: Subprogram to hold update logic for cannon defense tower
        public override void Update()
        {
            //Empty int variable, required for out parameter
            int emptyInt;

            //Calling base update subprogram
            base.Update();


            //If there exists a enemy within range
            if (Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList).Count > 0 &&
                Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex].relativePos > 0)
            {
                //If it's time to shoot, add cannon ball projectile and reset time
                if (currentShootFrame > shootFrameMax)
                {
                    //Calculating rotation of cannon tower
                    direction = center - Enemies.Enemy.GetCenterLocation(Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex].relativePos +
                        Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex].speed * Projectiles.Projectile.TRAVEL_TIME * 60 / Driver.GameLogic.currentSpeed, out emptyInt);
                    rotation = (float)Math.Atan2(direction.Y, direction.X);

                    projectileList.Add(new Projectiles.CannonBall(center,
                        Driver.CollisionDetection.QuadTree(currentRangeCircle, Driver.GameLogic.currentWave.enemyList)[enemyClosestIndex]));

                    currentShootFrame = 0;
                }
            }
        }

        //Pre: None
        //Post: Cannon defense tower is drawn
        //Description: Subprogram to hold draw logic for cannon
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

            //Drawing cannon tower with rotation
            Driver.Main.spriteBatch.Draw(img, sourceRect, null, Color.White, (float)(rotation + Math.PI * 1.5f), origin, SpriteEffects.None, 0);
        }
    }
}
