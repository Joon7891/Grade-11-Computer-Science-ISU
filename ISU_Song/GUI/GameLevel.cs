//Name: Joon Song
//File Name: GameLevel.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class to hold update and draw information for game levels

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ISU_Song.GUI
{
    class GameLevel
    {
        //2D Array to hold map seed (0 = Grass, 1 = Path)        
        public static int[,] mapTiles = new int[,]
        {
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0 },
            { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 }
        };

        //Texture2D variables to hold images for each tile type
        private static Texture2D grassTileImg;
        private static Texture2D pathTileImg;

        //2D Array ot Texture2D and Rectangle variables to hold images and rectangles for tiles
        private static Texture2D[,] tileImg = new Texture2D[11,13];
        private static Rectangle[,] tileRect = new Rectangle[11,13];

        //Dictionary of Towers and List of Items
        public static Dictionary<Vector2, Towers.Tower> towerDictionary = new Dictionary<Vector2, Towers.Tower>();
        public static List<Items.Item> itemList = new List<Items.Item>();

        //Pre: 'Content', the content manager required to load content
        //Post: Various game level components are loaded and set up
        //Description: Subprogram to load and set up various game level components
        public static void Load(ContentManager Content)
        {
            //Loading images of tiles
            grassTileImg = Content.Load<Texture2D>("Images/Sprites/Tiles/grassTile");
            pathTileImg = Content.Load<Texture2D>("Images/Sprites/Tiles/pathTile");

            //Looping through each map tile
            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int j = 0; j < mapTiles.GetLength(1); j++)
                {
                    //Setting tile image and rectangle                
                    if (mapTiles[i, j] == 0)
                    {
                        tileImg[i, j] = grassTileImg;
                    }
                    else if (mapTiles[i, j] == 1)
                    {
                        tileImg[i, j] = pathTileImg;
                    }
                    tileRect[i, j] = new Rectangle(50 * j, 50 * i, 50, 50);
                }
            }

            //Loading tombstone image
            Tombstone.img = Content.Load<Texture2D>("Images/Sprites/tombstone");
        }

        //Pre: None
        //Post: Various game level graphics are drawn
        //Description: Subprogram to hold draw logic for game level
        public static void Draw()
        {
            //Drawing game grid            
            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int j = 0; j < mapTiles.GetLength(1); j++)
                {
                    Driver.Main.spriteBatch.Draw(tileImg[i,j], tileRect[i,j], Color.White);
                }
            }

            //Drawing tower range for selected tower
            if (towerDictionary.ContainsKey(Cursor.selectedGridLoc))
            {
                towerDictionary[Cursor.selectedGridLoc].DrawCurrentRange();
            }

            //Drawing each tower
            foreach (KeyValuePair<Vector2, Towers.Tower> newTower in towerDictionary)
            {
                newTower.Value.Draw();
            }

            //Drawing each item
            foreach (Items.Item newItem in itemList)
            {
                newItem.Draw();
            }

            //Drawing current wave
            Driver.GameLogic.currentWave.Draw();
        }

        //Pre: 'itemType', the type of item
        //Post: Adds a item to the game level map
        //Description: Subprogram to add a item to the game level map
        public static void AddItem(string itemType)
        {
            //Determining type of item and adding an instance of that item            
            switch (itemType)
            {
                case "Cannon":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.Cannon((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Archer":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.Archer((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Bomber":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.Bomber((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Roaster":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.Roaster((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Steel Mine":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.SteelMine((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Forestry":
                    {
                        towerDictionary.Add(Cursor.gridLoc, new Towers.Forestry((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Arrow Drop":
                    {
                        itemList.Add(new Items.ArrowDrop((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
                case "Giant Bomb":
                    {
                        itemList.Add(new Items.GiantBomb((int)Cursor.gridLoc.X, (int)Cursor.gridLoc.Y));

                        break;
                    }
            }
        }
    }
}