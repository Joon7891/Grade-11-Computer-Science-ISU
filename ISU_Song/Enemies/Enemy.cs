//Name: Joon Song
//File Name: Enemy.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for enemy object (Parent Class)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Enemies
{
    public abstract class Enemy
    {
        //Texture2D and Rectangle variables for enemy image and rectangle
        protected Texture2D img;
        protected Texture2D[,] animationImg;
        public Rectangle rect;

        //Variables and constants for enemy movement
        public Vector2 centerLoc = new Vector2(275, 575);
        public float speed;
        public double relativePos = 0;
        private int currentDirection;
        private const int UP = 0;
        private const int RIGHT = 1;
        private const int DOWN = 2;
        private const int LEFT = 3;

        //Variables for animation
        protected int numFrames;
        protected int frameNo = 0;
        protected int updateCountMax;
        protected int updateCount;

        //Variables for health and 'weight' information of enemy
        public double totalHealth;
        private Rectangle totalHealthRect;
        public int currentHealth;
        public Rectangle currentHealthRect;
        public int weight;

        //Int variables for enemy loot
        public int lootGold;
        public int lootWood;
        public int lootSteel;

        //Constructor for enemy object
        public Enemy(double relativePos)
        {
            //Setting up relative posistion of enemy
            this.relativePos = relativePos;
        }

        //Pre: None
        //Post: Enemy health bar rectangle is set up
        //Description: Subprogram to set up enemy health bar
        protected void HealthBarSetup()
        {
            //Setting up healh bar rectangles
            totalHealthRect = new Rectangle(0, 0, rect.Width, 5);
            currentHealthRect = new Rectangle(0, 0, rect.Width, 5);
        }

        //Pre: None
        //Post: Various enemy components are updated
        //Description: Subprogram to hold update logic for enemy
        public virtual void Update()
        {
            //Calling subprograms to animation and movement the enemy
            Animate();
            Move();
        }

        //Pre: None
        //Post: Enemy is drawn
        //Description: Subprogram to hold draw logic for enemy
        public virtual void Draw()
        {
            //Drawing enemy, if image is not null
            if(img != null)
            {
                Driver.Main.spriteBatch.Draw(img, rect, Color.White);
            }
        }

        //Pre: None
        //Post: Enemy health bar is drawn
        //Description: Subprogrma to hold draw logic for enemy health bar
        public void DrawHealth()
        {
            //Drawing full health bar
            Driver.Main.spriteBatch.Draw(Driver.Main.whiteRectImg, totalHealthRect, Color.Gray);

            //Drawing current health bar with selection to choose what colour to use
            if(currentHealth >= totalHealth/2.0)
            {
                Driver.Main.spriteBatch.Draw(Driver.Main.whiteRectImg, currentHealthRect, Color.Green);
            }
            else
            {
                Driver.Main.spriteBatch.Draw(Driver.Main.whiteRectImg, currentHealthRect, Color.Red);
            }
        }

        //Pre: None
        //Post: Animation frame of enemy frame is updated
        //Description: Subprogram to control animation logic of enemy
        private void Animate()
        {
            //Frame animation logic
            if(updateCount == updateCountMax)
            {
                frameNo = (frameNo + 1) % numFrames;
                updateCount = 0;
            }
            else
            {
                updateCount++;
            }

            //Selection for image direction
            for (int i = 0; i < 4; i++)
            {
                if (currentDirection == i)
                {
                    img = animationImg[i, frameNo];

                    break;
                }
            }
        }

        //Pre: None
        //Post: Enemies are moved appropriately
        //Description: Subprogram to hold movement logic for enemies
        private void Move()
        {
            //Moving enemy and getting center and direction
            relativePos += speed;
            centerLoc = GetCenterLocation(relativePos, out currentDirection);

            //Updating enemy rectangle
            rect.X = (int)(centerLoc.X - rect.Width / 2.0);
            rect.Y = (int)(centerLoc.Y - rect.Height / 2.0);

            //Updating health rectangle
            totalHealthRect.X = rect.X;
            totalHealthRect.Y = rect.Y - 6;
            currentHealthRect.X = rect.X;
            currentHealthRect.Y = rect.Y - 6;
            currentHealthRect.Width = (int)(totalHealthRect.Width * (currentHealth / totalHealth));
        }

        //Pre: 'newRelativePos', the relative posistion of the enemy to the end, 'newDirction', the direction of movement for the enemy
        //Post: Returns a Vector2, the center coordinate of the enemy
        //Description: Subprogram to calculate coordinates of the enemy
        public static Vector2 GetCenterLocation(double newRelativePos, out int newDirection)
        {
            //Vector2 to hold center coordinate of the enemy
            Vector2 newCenterLoc = new Vector2();

            //Calculating enemy center and direction of movement
            if (newRelativePos < 150)
            {
                newCenterLoc.X = 275;
                newCenterLoc.Y = (float)(575 - newRelativePos);
                newDirection = UP;
            }
            else if (newRelativePos < 350)
            {
                newCenterLoc.X = (float)(425 - newRelativePos);
                newCenterLoc.Y = 425;
                newDirection = LEFT;
            }
            else if (newRelativePos < 500)
            {
                newCenterLoc.X = 75;
                newCenterLoc.Y = (float)(775 - newRelativePos);
                newDirection = UP;
            }
            else if (newRelativePos < 850)
            {
                newCenterLoc.X = (float)(-425 + newRelativePos);
                newCenterLoc.Y = 275;
                newDirection = RIGHT;
            }
            else if (newRelativePos < 1000)
            {
                newCenterLoc.X = 425;
                newCenterLoc.Y = (float)(-575 + newRelativePos);
                newDirection = DOWN;
            }
            else if (newRelativePos < 1150)
            {
                newCenterLoc.X = (float)(-575 + newRelativePos);
                newCenterLoc.Y = 425;
                newDirection = RIGHT;
            }
            else if (newRelativePos < 1450)
            {
                newCenterLoc.X = 575;
                newCenterLoc.Y = (float)(1575 - newRelativePos);
                newDirection = UP;
            }
            else if (newRelativePos < 1900)
            {
                newCenterLoc.X = (float)(2025 - newRelativePos);
                newCenterLoc.Y = 125;
                newDirection = LEFT;
            }
            else
            {
                newCenterLoc.X = 125;
                newCenterLoc.Y = (float)(2025 - newRelativePos);
                newDirection = UP;
            }

            //Returning center location
            return newCenterLoc;
        }

        //Pre: None
        //Post: Enemy loot is added to user wallet
        //Description: Subprogram to add enemy loot
        public void AddLoot()
        {
            //Adding resources to user wallet
            GameData.Player.gold += lootGold;
            GameData.Player.wood += lootWood;
            GameData.Player.steel += lootSteel;
        }
    }
}
