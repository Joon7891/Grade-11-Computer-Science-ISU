//Name: Joon Song
//File Name: Projectile.cs
//Project Name: ISU_Song
//Creation Date: Janurary 13, 2018
//Modified Date: Janurary 21, 2018
//Description: Class to handle projectile object (Parent Class)

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace ISU_Song.Projectiles
{
    public abstract class Projectile
    {
        //Texture2D and Rectangle variables for projectile image and rectangle        
        protected Texture2D img;
        protected Rectangle rect = new Rectangle();

        //Variables required for projectile flight
        public Enemies.Enemy targetEnemy;
        protected Vector2 startingLoc;
        private Vector2 currentLoc;
        private Vector2 endingLoc;
        private Vector2 velocity;
        private double currentTime = 0;
        public const double TRAVEL_TIME = 0.2;
        public bool active = true;
        public bool canKill = true;

        //SoundEffect instance for projectile sounds
        protected SoundEffectInstance sndInstance;

        //Constructor for projectile class
        public Projectile(Vector2 startingLoc, Enemies.Enemy targetEnemy)
        {            
            //Setting locations vectors and rectangle
            this.startingLoc = startingLoc;
            currentLoc = startingLoc;
            this.targetEnemy = targetEnemy;

            //Calculating future location of target enemy and velocity
            int directionHolder;
            endingLoc = Enemies.Enemy.GetCenterLocation(targetEnemy.relativePos + targetEnemy.speed * TRAVEL_TIME / Driver.GameLogic.currentSpeed * 60, out directionHolder);
            velocity.X = (float)((endingLoc.X - startingLoc.X) * Driver.GameLogic.currentSpeed / TRAVEL_TIME);
            velocity.Y = (float)((endingLoc.Y - startingLoc.Y) * Driver.GameLogic.currentSpeed / TRAVEL_TIME);
        }

        //Pre: None
        //Post: Various projectile updates are made
        //Description: Subprogram to hold update logic of projectile
        public virtual void Update()
        {
            //Updating current time and location of projectile, if current time has not exceeded travel time
            if (currentTime < TRAVEL_TIME / Driver.GameLogic.currentSpeed)
            {
                currentTime += Driver.Main.globalDeltaTime;
                currentLoc.X += (float)(velocity.X * Driver.Main.globalDeltaTime);
                currentLoc.Y += (float)(velocity.Y * Driver.Main.globalDeltaTime);
            }
            else
            {
                //Setting final location of projectile, setting active status of bullet to false
                currentLoc = endingLoc;
                active = false;
            }

            //Calculating rectangle of projectile
            rect.X = (int)(currentLoc.X - rect.Width / 2.0);
            rect.Y = (int)(currentLoc.Y - rect.Height / 2.0);
        }

        //Pre: None
        //Post: Projectile is drawn
        //Description: Subprogram to hold draw logic of projectile
        public virtual void Draw()
        {
            //Drawing projectile
            Driver.Main.spriteBatch.Draw(img, rect, Color.White);
        }
    }
}
