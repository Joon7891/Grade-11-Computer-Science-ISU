//Name: Joon Song
//File Name: GiantBomb.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for giant bomb item object (Child Class to Item Class)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ISU_Song.Items
{
    public class GiantBomb : Item
    {
        //Variables for giant bomb image and animation logic
        public static Texture2D giantBombImg;
        private int animationFrame = 0;
        private Texture2D[] animationImg;

        //Constants for item price
        public const int GOLD_PRICE = 300;
        public const int WOOD_PRICE = 0;
        public const int STEEL_PRICE = 0;

        //Constants for item range and damage
        public const int RANGE = 60;
        public const int DAMAGE = 400;


        //Constructor for giant bomb object
        public GiantBomb(int x, int y) : base (x, y)
        {
            //Setting up item image, rectangle, and circle
            animationImg = ItemResources.explosionImg;
            rect = new Rectangle(x * 50, y * 50, 50, 50);
            rangeCircle = new GUI.Circle(new Vector2(x * 50 + 25, y * 50 + 25), RANGE, Color.White);

            //Setting value of damage
            damageValue = DAMAGE;

            //Setting up and playing bomb soundeffect
            sndInstance = Projectiles.ProjectileResources.bomberSnd.CreateInstance();
            sndInstance.Volume = Driver.Main.actualVolume[1];
            sndInstance.Play();
        }

        //Pre: None
        //Post: Various giant bomb components are updated
        //Description: Subprogram to hold update logic for giant bomb
        public override void Update()
        {
            //Calling base update subprogram
            base.Update();
           
            //Animation logic
            if (animationFrame < 73)
            {
                img = animationImg[animationFrame];
                animationFrame++;
            }
            else
            {
                //Setting item use status to true if animation is complete
                isItemUsed = true;
            }
        }
    }
}
