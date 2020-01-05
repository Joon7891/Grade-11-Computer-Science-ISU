//Name: Joon Song
//File Name: Item.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for tower item object (Parent Class)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ISU_Song.Items
{
    public abstract class Item
    {
        //Int variables for item coordinates        
        protected int x;
        protected int y;

        //Variables for item image, rectangle and transparency
        protected Texture2D img;
        protected Rectangle rect;
        protected float transparency = 1.0f;

        //Bool variable for use status of item
        public bool isItemUsed = false;

        //Variables for range and damage of item
        protected GUI.Circle rangeCircle;
        protected int damageValue;
        private bool damageStatus = false;

        //Soundeffect instance for item soundeffects
        protected SoundEffectInstance sndInstance;

        //Constructor for item class
        public Item(int x, int y)
        {
            //Setting values of item coordinates
            this.x = x;
            this.y = y;
        }

        //Pre: 'Content', the content manager required to load content
        //Post: Various item components are loaded and setup
        //Description: Subprogram to load and setup various item components 
        public static void Load(ContentManager Content)
        {
            //Importing item images
            ArrowDrop.arrowImg = Content.Load<Texture2D>("Images/Sprites/Items/arrowDrop");
            GiantBomb.giantBombImg = Content.Load<Texture2D>("Images/Sprites/Items/giantBomb");
        }

        //Pre: None
        //Post: Various item components are updated
        //Description: Subprogram to hold update logic for item
        public virtual void Update()
        {
            //Updating health of every enemy within range of bomb, if not performed yet
            if (!damageStatus)
            {
                for (int i = 0; i < Driver.CollisionDetection.QuadTree(rangeCircle, Driver.GameLogic.currentWave.enemyList).Count; i++)
                {
                    Driver.CollisionDetection.QuadTree(rangeCircle, Driver.GameLogic.currentWave.enemyList)[i].currentHealth -= damageValue;
                }

                //Setting damage status to true
                damageStatus = true;
            }
        }

        //Pre: None
        //Post: Various item components are drawn
        //Description: Subprogram to hold draw logic for item
        public virtual void Draw()
        {
            //Drawing item, if image is not null
            if(img != null)
            {
                Driver.Main.spriteBatch.Draw(img, rect, Color.White * transparency);
            }
        }
    }
}