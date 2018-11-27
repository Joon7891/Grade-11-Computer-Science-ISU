//Name: Joon Song
//File Name: ShopItem.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for shop item object

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.GameData
{
    public class ShopItem
    {
        //Variables for shop item price and name        
        public string name;
        private int priceGold;
        private int priceWood;
        private int priceSteel;

        //Variables for shop item image, rectangle, and colour
        public Texture2D img;
        public Rectangle rect;
        private Color imgColor = Color.White;

        //Variables for shop item properties
        public string upgradeType;
        public int damage;
        public int radius;

        //Variables for tower information drawing
        public bool showInformation;
        public static Texture2D informationBoxImg;
        private Color informationBoxColor = Color.White;
        private Rectangle informationBoxRect;
        private bool frameStatus = true;
        private Vector2[] informationLoc = new Vector2[10];
        private string[] informationText = new string[10];
        private Color[] informationColor = { Color.Black, Color.Red, Color.Green, Color.Green, Color.Green, Color.Black, Color.Black };

        //SpriteFont variables for fonts
        public static SpriteFont[] resourceDataFont = new SpriteFont[2];

        //SoundEffect variables and instances for shop item sounds
        public static SoundEffect transactionSnd;
        public static SoundEffectInstance transactionSndInstance;
        public static SoundEffect errorSnd;
        public static SoundEffectInstance errorSndInstance;

        //Constructor for shop item obejct
        public ShopItem(int upgradeNo)
        {
            //Setting image and rectangle
            img = Towers.Tower.upgradeButtonImg;
            rect = Towers.Tower.upgradeButtonRect[upgradeNo];
        }

        //Overloaded constructor for shop item object, used for resource towers
        public ShopItem(string name, int priceGold, int priceWood, int priceSteel, Texture2D img, Rectangle rect)
        {
            //Setting name and price of item            
            this.name = name;
            this.priceGold = priceGold;
            this.priceWood = priceWood;
            this.priceSteel = priceSteel;

            //Setting image, rectangle, and radius
            this.img = img;
            this.rect = rect;
            radius = 0;

            //Setting up shop item information visuals
            InformationSetup();
            informationText[5] = "Yield: " + Towers.ResourceTower.DEFAULT_YIELD;
            informationText[6] = "";
        }

        //Overloaded constructor for shop item object, used for upgrades
        public ShopItem(string name, int priceGold, int priceWood, int priceSteel, string upgradeType, int upgradeValue, Texture2D img, Rectangle rect)
        {
            //Setting name and price of item
            this.name = name;
            this.priceGold = priceGold;
            this.priceWood = priceWood;
            this.priceSteel = priceSteel;

            //Setting up image, rectangle, and frame status
            this.img = img;
            this.rect = rect;
            frameStatus = false;

            //Setting up shop item information visuals
            InformationSetup();
            this.upgradeType = upgradeType;
            informationText[5] = upgradeType + ": +" + upgradeValue;
            informationText[6] = "";
        }

        //Overloaded constructor for shop item object, used for defensive towers
        public ShopItem(string name, int priceGold, int priceWood, int priceSteel, int damage, int range, Texture2D img, Rectangle rect)
        {
            //Setting name and price of item
            this.name = name;
            this.priceGold = priceGold;
            this.priceWood = priceWood;
            this.priceSteel = priceSteel;

            //Setting up image, rectangle, damage, and radius
            this.img = img;
            this.rect = rect;
            this.damage = damage;
            radius = range;

            //Setting up shop item information visuals
            InformationSetup();
            informationText[5] = "Damage: " + damage;
            informationText[6] = "Range: " + range;
        }

        //Pre: None
        //Post: Shop item information visual is setup
        //Description: Subprogram to setup shop item information visuals
        private void InformationSetup()
        {
            //Setting up rectangles, vectors, and text for shop item information
            informationBoxRect = new Rectangle(rect.X - 130, rect.Y - 60, 110, 140);
            informationLoc[0] = new Vector2(informationBoxRect.X + 5, informationBoxRect.Y);
            informationText[0] = "" + name;
            informationLoc[1] = new Vector2(informationBoxRect.X + 12, informationBoxRect.Y + 22);
            informationText[1] = "Cost";
            informationLoc[2] = new Vector2(informationBoxRect.X + 20, informationBoxRect.Y + 40);
            informationText[2] = "Gold: " + priceGold;
            informationLoc[3] = new Vector2(informationBoxRect.X + 20, informationBoxRect.Y + 58);
            informationText[3] = "Wood: " + priceWood;
            informationLoc[4] = new Vector2(informationBoxRect.X + 20, informationBoxRect.Y + 76);
            informationText[4] = "Steel: " + priceSteel;
            informationLoc[5] = new Vector2(informationBoxRect.X + 12, informationBoxRect.Y + 98);
            informationLoc[6] = new Vector2(informationBoxRect.X + 12, informationBoxRect.Y + 116);
        }

        //Pre: None
        //Post: Various shop item updates are made
        //Description: Subprogram to hold update logic for shop item
        public void Update()
        {
            //If cursor is over item, set bool to show item description
            if (Driver.CollisionDetection.PointToRectangle(rect, GUI.Cursor.circle.center))
            {
                showInformation = true;
            }
            else
            {
                showInformation = false;
            }
        }

        //Pre: None
        //Post: Various shop item graphics are drawn
        //Description: Subprogram to hold draw logic for shop item
        public void Draw()
        {
            //Drawing shop item and its corresponding frame (if requested)
            if (frameStatus)
            {
                Driver.Main.spriteBatch.Draw(GUI.Shop.frameImg, rect, Color.White);
            }
            Driver.Main.spriteBatch.Draw(img, rect, imgColor);
        }

        //Pre: None
        //Post: Returns true or false, depending if user has enough resources to purchase shop item
        //Description: Subprogram to check if user is eligible to purchase shop item
        public bool PurchaseEligibility()
        {
            //If user has enough gold, wood, and steel, return true
            if (Player.gold >= priceGold &&
                Player.wood >= priceWood &&
                Player.steel >= priceSteel)
            {
                return true;
            }

            //Otherwise, give visual and audio indications of ineligibility, and return false
            errorSndInstance.Play();
            Driver.GameLogic.gameStatusStr = "Get More Currency!";
            return false;
        }

        //Pre: None
        //Post: Updates user balance after completing a transaction
        //Description: Subprogram to update user balance after user purchases an item
        public void CompleteTransaction()
        {
            //Updating user balance with respect to the shop item price
            Player.gold -= priceGold;
            Player.wood -= priceWood;
            Player.steel -= priceSteel;

            //Playing cash transaciton soundeffect
            transactionSndInstance.Play();
        }

        //Pre: None
        //Post: Information about the tower is outputted to the user
        //Description: Subprgoram to output tower information to the uer
        public void DrawInformation()
        {
            //Drawing information box and content
            Driver.Main.spriteBatch.Draw(informationBoxImg, informationBoxRect, informationBoxColor);
            Driver.Main.spriteBatch.DrawString(resourceDataFont[0], informationText[0], informationLoc[0], informationColor[0]);
            for (int i = 1; i < 7; i++)
            {
                Driver.Main.spriteBatch.DrawString(resourceDataFont[1], informationText[i], informationLoc[i], informationColor[i]);
            }
        }

        //Pre: None
        //Post: Data is cleared for the shop item
        //Description: Subprogram to clear data for the shop item
        public void Clear()
        {
            //Clearing textual data
            for (int i = 0; i < 7; i++)
            {
                informationText[i] = "";
            }

            //Setting transparencies for shop item button and description
            imgColor = Color.White * 0.75f;
            informationBoxColor = Color.White * 0.0f;
        }
    }
}
