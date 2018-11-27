//Name: Joon Song
//File Name: Circle.cs
//Project Name: ISU_Song
//Creation Date: Janurary 1, 2018
//Modified Date: Janurary 18, 2018
//Description: Class to hold update and draw information about circle objects

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

namespace ISU_Song.GUI
{
    public class Circle
    {
        //Variabels to hold various properties of circle        
        private static Texture2D img;
        private Rectangle rect;
        public Vector2 center = new Vector2();
        public int radius;
        private Color colour;

        //Constructor for circle object
        public Circle(Vector2 center, int radius, Color colour)
        {
            //Initializing center, radius, adn colour of circle
            this.center = center;
            this.radius = radius;
            this.colour = colour;

            //Setting up circle image rectangle
            rect = new Rectangle((int)(center.X - radius), (int)(center.Y - radius), radius * 2, radius * 2);
        }

        //Pre: 'Content', the content manager required to load content
        //Post: Circle image is loaded
        //Description: Subprogram to load and setup circle image
        public static void Load(ContentManager Content)
        {
            //Loading circle image
            img = Content.Load<Texture2D>("Images/Sprites/whiteCircle");
        }

        //Pre: None
        //Post: Various circle updates are made
        //Description: Subprogram to hold update logic of circle
        public void Update()
        {
            //Updating rectangle location with respect to circle center            
            rect.X = (int)center.X - radius;
            rect.Y = (int)center.Y - radius;
        }

        //Pre: None
        //Post: Circle radiuS is updated 
        //Description: Subprogram to set circle radiusS
        public void SetRadius(int newRadius)
        {
            //Updating radius and rectangle
            radius = newRadius;
            rect = new Rectangle((int)(center.X - newRadius), (int)(center.Y - newRadius), newRadius * 2, newRadius * 2);
        }

        //Pre: None
        //Post: Circle visual is drawn
        //Description: Subprogram to hold draw logic of circle 
        public void Draw()
        {
            //Drawing circle
            Driver.Main.spriteBatch.Draw(img, rect, colour);
        }

        //Pre: None
        //Post: Returns the left 'X' coordinate of the circle
        //Description: Subprogram to return left 'X' coordinate of circle
        public int Left()
        {
            return (int)center.X - radius;
        }

        //Pre: None
        //Post: Returns the right 'X' coordiante of the circle
        //Description: Subprogram to return right 'X' coordinate of circle
        public int Right()
        {
            return (int)center.X + radius;
        }

        //Pre: None
        //Post: Returns the top 'Y' coordinate of the circle
        //Description: Subprogram to return top 'Y' coordinate of circle
        public int Top()
        {
            return (int)center.Y - radius;
        }

        //Pre: None
        //Post: Returns the bottom 'Y' coordinate of the circle
        //Description: Subprogram to return bottom 'Y' coordinate of circle
        public int Bottom()
        {
            return (int)center.Y + radius;
        }
    }
}
