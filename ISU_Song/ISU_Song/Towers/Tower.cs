//Name: Joon Song
//File Name: Tower.cs
//Project Name: ISU_Song
//Creation Date: December 23, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for tower object (Parent Class)

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
    public abstract class Tower
    {
        //Variabels for tower image, rectangle, and coordinates        
        protected Rectangle rect;
        protected Texture2D img;
        public Vector2 gridLoc;
        public Vector2 center;

        //Variables for tower price
        protected int priceGold;
        protected int priceWood;
        protected int priceSteel;

        //Int variables for level and values of tower properties (1 = Damage or Yield, 2 = Range or Rate)
        protected int[] propertyLevel = new int[2];
        protected int[] propertyValue = new int[2];

        //Array of shop item variables for upgrades
        public GameData.ShopItem[] upgradeItems = new GameData.ShopItem[2];

        //Variables for tower properties GUI
        public static Texture2D upgradeButtonImg;
        public static Rectangle[] upgradeButtonRect = { new Rectangle(675, 250, 90, 25), new Rectangle(675, 330, 90, 25)};
        protected static Vector2[] infoLoc =
        {
            new Vector2(660, 180),
            new Vector2(660, 200),
            new Vector2(680, 215),
            new Vector2(680, 230),
            new Vector2(660, 280),
            new Vector2(680, 295),
            new Vector2(680, 310),
            new Vector2(660, 365),
            new Vector2(660, 385)
        };
        public static Texture2D removeButtonImg;
        public static Rectangle removeButtonRect;
        protected string towerType = "";

        //SpriteFont variables for fonts
        protected static SpriteFont infoText;

        //Constructor for tower class
        public Tower(int x, int y)
        {
            //Setting tower coordinates, center, and rectangle            
            gridLoc = new Vector2(x, y);
            center = new Vector2(50 * x + 25, 50 * y + 25);
            rect = new Rectangle(50 * x, 50 * y, 50, 50);
        }

        //Overloaded constructor for tower class, used for IO
        public Tower(int x, int y, int propLevel1, int propLevel2, int total)
        {
            //Setting up tower coordinates, center, and rectangle
            gridLoc = new Vector2(x, y);
            center = new Vector2(50 * x + 25, 50 * y + 25);
            rect = new Rectangle(50 * x, 50 * y, 50, 50);

            //Setting property levels
            propertyLevel[0] = propLevel1;
            propertyLevel[1] = propLevel2;
        }

        //Pre: 'Content', the content manager required to load content
        //Post: Various tower components are loaded and setup
        //Description: Subprogram to load and setup various tower components 
        public static void Load(ContentManager Content)
        {
            //Importing tower images
            Cannon.cannonImg = Content.Load<Texture2D>("Images/Sprites/Towers/cannonTower");
            Archer.archerImg = Content.Load<Texture2D>("Images/Sprites/Towers/archerTower");
            Bomber.bomberImg = Content.Load<Texture2D>("Images/Sprites/Towers/bombTower");
            Roaster.roasterImg = Content.Load<Texture2D>("Images/Sprites/Towers/roaster");
            SteelMine.steelMineImg = Content.Load<Texture2D>("Images/Sprites/Towers/steelMine");
            Forestry.forestryImg = Content.Load<Texture2D>("Images/Sprites/Towers/forestry");

            //Importing and setting up button images and rectangles
            removeButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/removeButton");
            removeButtonRect = new Rectangle(660, 405, 130, 28);
            upgradeButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/upgradeButton");
            infoText = Content.Load<SpriteFont>("Fonts/TowerInformationFont");
        }

        //Pre: None
        //Post: Various tower updates are made
        //Description: Subprogram to hold update logic of tower
        public virtual void Update()
        {
            //Updating shop items
            for (int i = 0; i < 2; i++)
            {
                upgradeItems[i].Update();
            }
        }

        //Pre: None
        //Post: Draws the image of the tower
        //Description: Subprogram to draw the tower
        public virtual void Draw()
        {
            //Drawing tower
            Driver.Main.spriteBatch.Draw(img, rect, Color.White);
        }

        //Pre: None
        //Post: Draws details for the tower
        //Description: Subprogram to hold draw logic for tower details
        public virtual void DrawDetails()
        {
            //Drawing text indicating tower type
            Driver.Main.spriteBatch.DrawString(infoText, "" + towerType, infoLoc[0], Color.Black);
            
            //Drawing remove tower details
            Driver.Main.spriteBatch.Draw(removeButtonImg, removeButtonRect, Color.White);

            //Drawing upgrade shop item and information (if appropriate)
            for (int i = 0; i < 2; i++)
            {
                upgradeItems[i].Draw();
                if (upgradeItems[i].showInformation)
                {
                    upgradeItems[i].DrawInformation();
                }
            }
        }

        //Pre: None
        //Post: Various tower upgrade updates are made
        //Description: Subprogram to hold update logic for upgrades
        public virtual void UpgradeLogic()
        {
            //Looping through each upgrade
            for (int i = 0; i < 2; i++)
            {
                //If user clicks on item and is eligible for purchase
                if (Driver.CollisionDetection.PointToRectangle(upgradeItems[i].rect, GUI.Cursor.circle.center) &&
                    Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.A, Driver.Main.oldGamePadState.Buttons.A) &&
                    upgradeItems[i].PurchaseEligibility() && propertyLevel[i] < 4)
                {
                    //Completing transaction and updatting appropriate property
                    upgradeItems[i].CompleteTransaction();
                    if (i == 0)
                    {
                        Upgrade1();
                    }
                    else
                    {
                        Upgrade2();
                    }

                    //Updating or clearing shop item, depending on property level
                    if (propertyLevel[i] < 4)
                    {
                        upgradeItems[i] = new GameData.ShopItem(upgradeItems[i].name, propertyLevel[i] * (priceGold) / 2, propertyLevel[i] * (priceWood) / 2, propertyLevel[i] * (priceSteel) / 2, upgradeItems[i].upgradeType, (int)(propertyValue[i] * 0.25), upgradeItems[i].img, upgradeItems[i].rect);
                    }
                    else
                    {
                        upgradeItems[i].Clear();
                    }
                }
            }
        }

        //Pre: None
        //Post: Upgrades the 1st property of the tower
        //Description: Subprogram to upgrade the 1st property of the tower
        public virtual void Upgrade1()
        {
        }

        //Pre: None
        //Post: Upgrades the 2nd property of the tower
        //Description: Subprogram to upgrade the 2nd property of the tower
        public virtual void Upgrade2()
        {

        }

        //Pre: None
        //Post: Draws the range of the tower
        //Description: Subprogram to draw the range of the defense tower
        public virtual void DrawCurrentRange()
        {
        }

        //Pre: None
        //Post: Tower's information is written as a string
        //Description: Subprogram to condense the tower's information into a string
        public virtual string GetInformation()
        {
            return "";
        }
        
        //Pre: None
        //Post: Tower coordiantes are returned a string
        //Description: Subprogram to return tower coordinates as a string
        protected string GetCoordinates()
        {
            //Returning cordinates
            return (gridLoc.X + "," + gridLoc.Y);
        }
    }
}