//Name: Joon Song
//File Name: Arrow.cs
//Project Name: ISU_Song
//Creation Date: Janurary 13, 2018
//Modified Date: Janurary 18, 2018
//Description: Class to handle arrow projectile object (Child Class)

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

namespace ISU_Song.Projectiles
{
    class Arrow : Projectile
    {
        //Variables required for rotation of arrow
        private Vector2 direction = new Vector2();
        private float rotation;
        private Vector2 origin = new Vector2(35, 15);
        private Rectangle sourceRect;

        //Constructor for arrow class
        public Arrow(Vector2 startingLoc, Enemies.Enemy targetEnemy) : base(startingLoc, targetEnemy)
        {
            //Setting up image and rectangle for arrow projectile  
            img = ProjectileResources.arrowImg;
            rect = new Rectangle((int)(startingLoc.X - 17), (int)(startingLoc.Y - 7), 35, 15);

            //Calculating arrow rotation
            sourceRect = new Rectangle(rect.X + 17, rect.Y + 7, 35, 15);
            direction = startingLoc - targetEnemy.centerLoc;
            rotation = (float)Math.Atan2(direction.Y, direction.X);

            //Setting up and playing fireball soundeffect
            sndInstance = ProjectileResources.archerSnd.CreateInstance();
            sndInstance.Volume = Driver.Main.actualVolume[1];
            sndInstance.Play();
        }

        //Pre: None
        //Post: Arrow updates are made
        //Description: Subprogram to hold update logic of arrow
        public override void Update()
        {
            //Calling base update
            base.Update();

            //Updating source rectangle as a function of rectangle
            sourceRect.X = rect.X + 17;
            sourceRect.Y = rect.Y + 7;
        }

        //Pre: None
        //Post: Arrow is drawn
        //Description: Subprogram to hold draw logic of arrow
        public override void Draw()
        {
            //Drawing projectile, with rotation
            Driver.Main.spriteBatch.Draw(img, sourceRect, null, Color.White, (float)(rotation + Math.PI), origin, SpriteEffects.None, 0);
        }
    }
}
