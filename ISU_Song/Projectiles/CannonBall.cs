//Name: Joon Song
//File Name: CannonBall.cs
//Project Name: ISU_Song
//Creation Date: Janurary 13, 2018
//Modified Date: Janurary 18, 2018
//Description: Class to handle cannonball projectile object (Child Class)

using Microsoft.Xna.Framework;

namespace ISU_Song.Projectiles
{
    class CannonBall : Projectile
    {
        //Constructor for cannon ball class
        public CannonBall(Vector2 startingLoc, Enemies.Enemy targetEnemy) : base(startingLoc, targetEnemy)
        {
            //Setting up image and rectangle for cannonball projectile  
            img = ProjectileResources.cannonBallImg;
            rect = new Rectangle((int)(startingLoc.X - 7), (int)(startingLoc.Y - 7), 15, 15);

            //Setting up and playing cannonball soundeffect
            sndInstance = ProjectileResources.cannonSnd.CreateInstance();
            sndInstance.Volume = Driver.Main.actualVolume[1];
            sndInstance.Play();
        }
    }
}
