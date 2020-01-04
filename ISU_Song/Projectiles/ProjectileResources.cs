//Name: Joon Song
//File Name: ProjectileResources.cs
//Project Name: ISU_Song
//Creation Date: Janurary 13, 2018
//Modified Date: Janurary 18, 2018
//Description: Class to hold resources for projectiles

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Projectiles
{
    class ProjectileResources
    {
        //Texture2D variables to hold projectile images    
        public static Texture2D cannonBallImg;
        public static Texture2D arrowImg;
        public static Texture2D bombImg;
        public static Texture2D fireballImg;

        //SoundEffect variables and instances for projectile sounds
        public static SoundEffect cannonSnd;
        public static SoundEffect archerSnd;
        public static SoundEffect bomberSnd;
        public static SoundEffect roasterSnd;

        //Pre: 'Content', the content manager required to load content
        //Post: Various projectile components are loaded and setup
        //Description: Subprogram to hold load logic for projectiles
        public static void Load(ContentManager Content)
        {
            //Importing projectile images            
            cannonBallImg = Content.Load<Texture2D>("Images/Sprites/Projectiles/cannonBall");
            arrowImg = Content.Load<Texture2D>("Images/Sprites/Projectiles/arrow");
            bombImg = Content.Load<Texture2D>("Images/Sprites/Projectiles/bomb");
            fireballImg = Content.Load<Texture2D>("Images/Sprites/Projectiles/fireball");

            //Importing projectile sound effects and setting up instances
            cannonSnd = Content.Load<SoundEffect>("Audio/SoundEffects/cannonSound");
            archerSnd = Content.Load<SoundEffect>("Audio/SoundEffects/arrowSound");
            bomberSnd = Content.Load<SoundEffect>("Audio/SoundEffects/bombSound");
            roasterSnd = Content.Load<SoundEffect>("Audio/SoundEffects/fireballSound");
        }
    }
}
