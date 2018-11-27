//Name: Joon Song
//File Name: Bomb.cs
//Project Name: ISU_Song
//Creation Date: Janurary 13, 2018
//Modified Date: Janurary 18, 2018
//Description: Class to handle bomb projectile object (Child Class)

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
    class Bomb : Projectile
    {
        //Constructor for bomb class
        public Bomb(Vector2 startingLoc, Enemies.Enemy targetEnemy) : base(startingLoc, targetEnemy)
        {
            //Setting up image and rectangle for bomb projectile  
            img = ProjectileResources.bombImg;
            rect = new Rectangle((int)(startingLoc.X - 12), (int)(startingLoc.Y - 12), 25, 25);

            //Setting up and playing bomb soundeffect
            sndInstance = ProjectileResources.bomberSnd.CreateInstance();
            sndInstance.Volume = Driver.Main.actualVolume[1];
            sndInstance.Play();
        }
    }
}
