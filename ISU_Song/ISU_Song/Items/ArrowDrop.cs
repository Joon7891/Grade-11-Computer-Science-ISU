//Name: Joon Song
//File Name: ArrowDrop.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for arrow drop item object (Child Class to Item Class)

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

namespace ISU_Song.Items
{
    public class ArrowDrop : Item
    {
        //Texture2D variable to hold images of arrow drop        
        public static Texture2D arrowImg;
        public static Texture2D animationImg;

        //Constants for item price
        public const int GOLD_PRICE = 150;
        public const int WOOD_PRICE = 0;
        public const int STEEL_PRICE = 0;

        //Constants for item range and damage
        public const int RANGE = 100;
        public const int DAMAGE = 200;

        //Constructor for arrow drop object
        public ArrowDrop(int x, int y) : base(x, y)
        {
            //Setting up item image, rectangle, and circle
            img = animationImg;
            rect = new Rectangle((int)((x - 1.5) * 50), (int)((y - 1.5) * 50), 200, 200);
            rangeCircle = new GUI.Circle(new Vector2(x * 50 + 25, y * 50 + 25), RANGE, Color.White);

            //Setting damange of item
            damageValue = DAMAGE;

            //Setting up and playing bomb soundeffect
            sndInstance = Projectiles.ProjectileResources.archerSnd.CreateInstance();
            sndInstance.Volume = Driver.Main.actualVolume[1];
            sndInstance.Play();
        }

        //Pre: None
        //Post: Various arrow drop components are updated
        //Description: Subprogram to hold update logic for arrow drop
        public override void Update()
        {
            //Calling base update subprogram
            base.Update();

            //Transparency logic
            if (transparency > 0)
            {
                transparency -= 0.004f;
            }
            else
            {
                //Setting item use status to true if item is no longer transparent
                isItemUsed = true;
            }
        }

    }
}
