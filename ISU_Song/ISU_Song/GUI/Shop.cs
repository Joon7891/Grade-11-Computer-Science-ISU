//Name: Joon Song
//File Name: Shop.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class to hold update and draw information about shop

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

namespace ISU_Song.GUI
{
    public class Shop
    {
        //Texture2D and Rectangle variables for shop background image and rectangle        
        private static Texture2D backImg;
        private static Rectangle backRect;

        //Variables required for player resource output
        private static Texture2D[] resourceImg = new Texture2D[4];
        private static Rectangle[] resourceRect = new Rectangle[4];
        private static Vector2[] resourcesMsgLoc = new Vector2[4];
        public static Texture2D frameImg;
        private static Rectangle shopDividerRect = new Rectangle(650, 168, 150, 3);

        //Array of shop items
        public static GameData.ShopItem[] shopItems = new GameData.ShopItem[8];
        
        //Variables required in new item addition logic
        private static bool isItemInHand = false;
        private static GameData.ShopItem newItem;
        private static string itemName;
        private static Rectangle itemRect = new Rectangle(0, 0, 50, 50);
        private static Texture2D itemImg;
        private static Circle itemRange = new Circle(new Vector2(), 0, Color.White);

        //SpriteFont variables for fonts
        private static SpriteFont resourcesFont;

        //Pre: 'Content', the content manager required to load content
        //Post: Various shop components are loaded and setup
        //Description: Subprogram to load and setup various shop components 
        public static void Load(ContentManager Content)
        {
            //Importing and setting up shop background image and rectangle
            backImg = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/shopBackground");
            backRect = new Rectangle(650, 0, 150, 550);

            //Importing and setting up resource images, rectangles, and message locations
            resourceImg[0] = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/heartResource");
            resourceImg[1] = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/goldResource");
            resourceImg[2] = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/woodResource");
            resourceImg[3] = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/steelResource");
            frameImg = Content.Load<Texture2D>("Images/Backgrounds/ShopComponents/woodenFrame");
            for (int i = 0; i < 4; i++)
            {
                resourceRect[i] = new Rectangle(655, 5 + 40 * i, 35, 35);
                resourcesMsgLoc[i] = new Vector2(700, 3 + 40 * i);
            }

            //Setting up shop items
            GameData.ShopItem.informationBoxImg = Content.Load<Texture2D>("Images/Sprites/informationBoxImg");
            shopItems[0] = new GameData.ShopItem("Cannon", Towers.Cannon.GOLD_PRICE, Towers.Cannon.WOOD_PRICE, Towers.Cannon.STEEL_PRICE, Towers.Cannon.DEFAULT_DAMAGE, Towers.Cannon.DEFAULT_RANGE, Towers.Cannon.cannonImg, new Rectangle(666, 180, 50, 50));
            shopItems[1] = new GameData.ShopItem("Archer", Towers.Archer.GOLD_PRICE, Towers.Archer.WOOD_PRICE, Towers.Archer.STEEL_PRICE, Towers.Archer.DEFAULT_DAMAGE, Towers.Archer.DEFAULT_RANGE, Towers.Archer.archerImg, new Rectangle(734, 180, 50, 50));
            shopItems[2] = new GameData.ShopItem("Bomber", Towers.Bomber.GOLD_PRICE, Towers.Bomber.WOOD_PRICE, Towers.Bomber.STEEL_PRICE, Towers.Bomber.DEFAULT_DAMAGE, Towers.Bomber.DEFAULT_RANGE, Towers.Bomber.bomberImg, new Rectangle(666, 240, 50, 50));
            shopItems[3] = new GameData.ShopItem("Roaster", Towers.Roaster.GOLD_PRICE, Towers.Roaster.WOOD_PRICE, Towers.Roaster.STEEL_PRICE, Towers.Roaster.DEFAULT_DAMAGE, Towers.Roaster.DEFAULT_RANGE, Towers.Roaster.roasterImg, new Rectangle(734, 240, 50, 50));
            shopItems[4] = new GameData.ShopItem("Steel Mine", Towers.SteelMine.GOLD_PRICE, Towers.SteelMine.WOOD_PRICE, Towers.SteelMine.STEEL_PRICE, Towers.SteelMine.steelMineImg, new Rectangle(666, 300, 50, 50));
            shopItems[5] = new GameData.ShopItem("Forestry", Towers.Forestry.GOLD_PRICE, Towers.Forestry.WOOD_PRICE, Towers.Forestry.STEEL_PRICE, Towers.Forestry.forestryImg, new Rectangle(734, 300, 50, 50));
            shopItems[6] = new GameData.ShopItem("Arrow Drop", Items.ArrowDrop.GOLD_PRICE, Items.ArrowDrop.WOOD_PRICE, Items.ArrowDrop.STEEL_PRICE, Items.ArrowDrop.DAMAGE, Items.ArrowDrop.RANGE, Items.ArrowDrop.arrowImg, new Rectangle(666, 360, 50, 50));
            shopItems[7] = new GameData.ShopItem("Giant Bomb", Items.GiantBomb.GOLD_PRICE, Items.GiantBomb.WOOD_PRICE, Items.GiantBomb.STEEL_PRICE, Items.GiantBomb.DAMAGE, Items.GiantBomb.RANGE, Items.GiantBomb.giantBombImg, new Rectangle(734, 360, 50, 50));

            //Importing fonts
            resourcesFont = Content.Load<SpriteFont>("Fonts/ResourceFont");
            for(int i = 0; i < 2; i++)
            {
                GameData.ShopItem.resourceDataFont[i] = Content.Load<SpriteFont>("Fonts/ResourceDataFont" + (i + 1));
            }

            //Importing soundeffects and setting up instances
            GameData.ShopItem.transactionSnd = Content.Load<SoundEffect>("Audio/SoundEffects/cashSound");
            GameData.ShopItem.transactionSndInstance = GameData.ShopItem.transactionSnd.CreateInstance();
            GameData.ShopItem.errorSnd = Content.Load<SoundEffect>("Audio/SoundEffects/errorSoundEffect");
            GameData.ShopItem.errorSndInstance = GameData.ShopItem.errorSnd.CreateInstance();
        }

        //Pre: None
        //Post: Various shop updates are made
        //Description: Subprogram to hold update logic for shop
        public static void Update()
        {
            //If a tower is selected, call tower editing logic            
            if(GameLevel.towerDictionary.ContainsKey(Cursor.selectedGridLoc))
            {
                GameLevel.towerDictionary[Cursor.selectedGridLoc].UpgradeLogic();
            }
            else
            {
                //Calling new item addition logic subprogram                
                NewItemAdditionLogic();

                //Updating each item
                for (int i = 0; i < 8; i++)
                {
                    shopItems[i].Update();
                }
            }

            //Updating shop item rectangle and radius circle
            if (Cursor.circle.center.X < 650)
            {
                itemRect.X = Cursor.highlightingRect.X;
                itemRect.Y = Cursor.highlightingRect.Y;
            }
            else
            {
                itemRect.X = (int)(Cursor.circle.center.X - itemRect.Width / 2.0);
                itemRect.Y = (int)(Cursor.circle.center.Y - itemRect.Height / 2.0);
            }
            itemRange.center.X = itemRect.X + 25;
            itemRange.center.Y = itemRect.Y + 25;
            itemRange.Update();
        }

        //Pre: None
        //Post: Various shop visuals are drawn
        //Description: Subprogram to hold draw logic for shop
        public static void Draw()
        {
            //Drawing shop background
            Driver.Main.spriteBatch.Draw(backImg, backRect, Color.White);

            //Drawing player resource amounts and GUI
            for (int i = 0; i < 4; i++)
            {
                Driver.Main.spriteBatch.Draw(frameImg, resourceRect[i], Color.White);
                Driver.Main.spriteBatch.Draw(resourceImg[i], resourceRect[i], Color.White);
            }
            Driver.Main.spriteBatch.DrawString(resourcesFont, "" + GameData.Player.lives, resourcesMsgLoc[0], Color.Red);
            Driver.Main.spriteBatch.DrawString(resourcesFont, "" + GameData.Player.gold, resourcesMsgLoc[1], Color.Black);
            Driver.Main.spriteBatch.DrawString(resourcesFont, "" + GameData.Player.wood, resourcesMsgLoc[2], Color.Black);
            Driver.Main.spriteBatch.DrawString(resourcesFont, "" + GameData.Player.steel, resourcesMsgLoc[3], Color.Black);
            Driver.Main.spriteBatch.Draw(Driver.Main.whiteRectImg, shopDividerRect, Color.SaddleBrown);

            //If a tower is seleted, draw its details
            if (GameLevel.towerDictionary.ContainsKey(Cursor.selectedGridLoc))
            {
                GameLevel.towerDictionary[Cursor.selectedGridLoc].DrawDetails();
            }
            else
            {
                //Calling subprograms to draw various shop items
                for (int i = 0; i < 8; i++)
                {
                    shopItems[i].Draw();
                }
            }

            //Drawing item descriptions, if intended and there are no shop items in hand
            for (int i = 0; i < 8; i++)
            {
                if (shopItems[i].showInformation && !isItemInHand)
                {
                    shopItems[i].DrawInformation();
                }
            }

            //Drawing shop item with range in hand, if appropriate
            if (isItemInHand)
            {
                Driver.Main.spriteBatch.Draw(itemImg, itemRect, Color.White);
                itemRange.Draw();
            }

        }

        //Pre: None
        //Post: Various shop updates are made regarding new item addition
        //Description: Subprogram to hold update logic for new item addition
        private static void NewItemAdditionLogic()
        {
            //If user clicks on a tile with a shop item            
            if (Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.A, Driver.Main.oldGamePadState.Buttons.A) &&
                Cursor.gridLoc.X < 13 && Cursor.gridLoc.Y < 11 && isItemInHand)
            {
                //Logic to ensure placement is apprropriate                
                if (((newItem.name == "Arrow Drop" || newItem.name == "Giant Bomb") && GameLevel.mapTiles[(int)Cursor.gridLoc.Y, (int)Cursor.gridLoc.X] == 1) ||
                    (newItem.name != "Arrow Drop" && newItem.name != "Giant Bomb" && !GameLevel.towerDictionary.ContainsKey(Cursor.gridLoc) && GameLevel.mapTiles[(int)Cursor.gridLoc.Y, (int)Cursor.gridLoc.X] == 0))
                {
                    //Completing transaction, and moving item from hand to game level    
                    newItem.CompleteTransaction();
                    GameLevel.AddItem(itemName);
                    isItemInHand = false;
                }
            }

            //If user clicks 'A' on a shop item
            if (Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.A, Driver.Main.oldGamePadState.Buttons.A))
            {
                for (int i = 0; i < 8; i++)
                {
                    if (Driver.CollisionDetection.PointToRectangle(shopItems[i].rect, Cursor.circle.center))
                    {
                        //If user is elidigible for purchase                        
                        if (shopItems[i].PurchaseEligibility())
                        {
                            //Set data for current shop item in hand                            
                            itemName = shopItems[i].name;
                            itemImg = shopItems[i].img;
                            newItem = shopItems[i];
                            itemRange = new Circle(new Vector2(), newItem.radius, Color.White * 0.25f);
                            isItemInHand = true;
                        }
                    }
                }
            }
            else if(Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.B, Driver.Main.oldGamePadState.Buttons.B))
            {
                //If user clicks 'B', clear item in hand
                isItemInHand = false;
            }
        }
    }
}
