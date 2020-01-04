//Name: Joon Song
//File Name: ItemResources.cs
//Project Name: ISU_Song
//Creation Date: Janurary 7, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for item object resources

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Items
{
    class ItemResources
    {
        //Array of Texture2D variables for explosion animation images
        public static Texture2D[] explosionImg = new Texture2D[74];
        
        //Pre: 'Content', the content manager required to load content
        //Post: Various item components are loaded and setup
        //Description: Subprogram to load and setup various item components 
        public static void Load(ContentManager Content)
        {
            //Importing explosion img
            for(int i = 0; i < 74; i++)
            {
                explosionImg[i] = Content.Load<Texture2D>("Images/Sprites/Items/Explosion/explosionImg" + (i + 1));
            }

            //Importing arrow animation image
            ArrowDrop.animationImg = Content.Load<Texture2D>("Images/Sprites/Items/arrowDropLevel");
        }
    }
}
