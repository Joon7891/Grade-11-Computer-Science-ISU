//Name: Joon Song
//File Name: SteelMine.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for steel mine tower object (Child Class to Tower Class)

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
    public class SteelMine : ResourceTower
    {
        //Texture2D variable to hold image of steel mine tower
        public static Texture2D steelMineImg;

        //Constants for tower price
        public const int GOLD_PRICE = 0;
        public const int WOOD_PRICE = 1000;
        public const int STEEL_PRICE = 200;

        //Constructor for Steel Mine object
        public SteelMine(int x, int y) : base(x, y)
        {
            //Setting image and type of steel mine
            img = steelMineImg;
            towerType = "Steel Mine";

            //Setting tower price
            priceGold = GOLD_PRICE;
            priceWood = WOOD_PRICE;
            priceSteel = STEEL_PRICE;

            //Setting yield amount and speed amount
            propertyValue[0] = DEFAULT_YIELD;
            propertyValue[1] = DEFAULT_SPEED;
            yieldRate = propertyValue[0] * propertyValue[1];

            //Setting up shop item variables for possible upgrades
            upgradeItems[0] = new GameData.ShopItem("Amount +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Amount", (int)Math.Round(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            upgradeItems[1] = new GameData.ShopItem("Speed +", GOLD_PRICE / 2, WOOD_PRICE / 2, STEEL_PRICE / 2, "Speed", (int)Math.Round(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
        }

        //Overloaded constructor for defense tower class
        public SteelMine(int x, int y, int propLevel1, int propLevel2, int total) : base(x, y, propLevel1, propLevel2, total)
        {
            //Setting image and type of archer
            img = steelMineImg;
            towerType = "Steel Mine";

            //Setting tower price
            priceGold = GOLD_PRICE;
            priceWood = WOOD_PRICE;
            priceSteel = STEEL_PRICE;

            //Setting damage, range, shoot time, and kills
            propertyValue[0] = (int)(DEFAULT_YIELD * Math.Pow(1.25, propLevel1));
            propertyValue[1] = (int)(DEFAULT_SPEED * Math.Pow(1.25, propLevel2));
            yieldRate = propertyValue[0] * propertyValue[1];
            totalYield = total;

            //Selection to either update 1st shop item or clear shop item, depending on level
            if (propertyLevel[0] < 4)
            {
                upgradeItems[0] = new GameData.ShopItem("Amount +", propertyLevel[0] * (priceGold) / 2, propertyLevel[0] * (priceWood) / 2, propertyLevel[0] * (priceSteel) / 2, "Amount", (int)(propertyValue[0] * 0.25), upgradeButtonImg, upgradeButtonRect[0]);
            }
            else
            {
                upgradeItems[0] = new GameData.ShopItem(0);
                upgradeItems[0].Clear();
            }

            //Selection to either update 2nd shop item or clear shop item, depending on level
            if (propertyLevel[1] < 4)
            {
                upgradeItems[1] = new GameData.ShopItem("Speed +", propertyLevel[1] * (priceGold) / 2, propertyLevel[1] * (priceWood) / 2, propertyLevel[1] * (priceSteel) / 2, "Speed", (int)(propertyValue[1] * 0.25), upgradeButtonImg, upgradeButtonRect[1]);
            }
            else
            {
                upgradeItems[1] = new GameData.ShopItem(1);
                upgradeItems[1].Clear();
            }
        }

        //Pre: None
        //Post: Adds steel to player wallet
        //Description: Subprogram to add produced steel to player wallet
        public override void AddResources()
        {
            //Adding steel to user wallet and updating total production
            GameData.Player.steel += yieldRate;
            totalYield += yieldRate;
        }
    }
}
