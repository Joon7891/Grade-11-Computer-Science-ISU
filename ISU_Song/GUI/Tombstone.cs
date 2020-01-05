//Name: Joon Song
//File Name: Tombstone.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for tombstone object

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISU_Song.GUI
{
    class Tombstone
    {
        //Texture2D and Rectangle variables for tombstone image and rectangle        
        public static Texture2D img;
        private Rectangle rect;

        //Variables to handle fading of tombstone
        public float transparency = 1.0f;
        private const float TRANSPARENCY_RATE = 0.2f;

        //Constructor for tombstone class, sets rectangle
        public Tombstone(Rectangle rect)
        {
            this.rect = rect;
        }

        //Pre: None
        //Post: Tombstone is updated
        //Description: Subprogram to hold update logic of tombstone object
        public void Update()
        {
            //Updating transparency
            transparency -= (float)(TRANSPARENCY_RATE * Driver.Main.globalDeltaTime * Driver.GameLogic.currentSpeed);
        }

        //Pre: None
        //Post: Tombstone is drawn
        //Description: Subprogram to hold draw logic of tombstone object
        public void Draw()
        {
            //Drawing tombstone
            Driver.Main.spriteBatch.Draw(img, rect, Color.White * transparency);
        }
    }
}
