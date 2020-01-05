//Name: Joon Song
//File Name: Cursor.cs
//Project Name: ISU_Song
//Creation Date: December 28, 2017
//Modified Date: Janurary 21, 2018
//Description: Class to hold update and draw information about cursor

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ISU_Song.GUI
{
    class Cursor
    {
        //Objects for cursor view methods and constant for cursor speed
        public static Circle circle = new Circle(new Vector2(10, 10), 5, Color.Black);
        public static Rectangle highlightingRect = new Rectangle(0, 0, 50, 50);
        const int CURSOR_SPEED = 7;

        //Vector2 variables to hold various location data
        public static Vector2 gridLoc = new Vector2();
        public static Vector2 selectedGridLoc = new Vector2(-1, -1);

        //Bool variable to hold cursor type on grid
        public static bool gridRect = true;

        //Pre: None
        //Post: Updates the cursor location
        //Description: Subprogram to hold update logic for cursor
        public static void Update()
        {
            //If D-Pad or Left Thumbstick Left is pressed, move cursor left, as long as it can move left            
            if (Driver.Main.newGamePadState.DPad.Left == ButtonState.Pressed && circle.Left() > 0)
            {
                circle.center.X -= (int)(CURSOR_SPEED / (1.5 * Driver.GameLogic.currentSpeed));
            }
            else if (Driver.Main.newGamePadState.ThumbSticks.Left.X < 0 && circle.Left() > 0)
            {
                circle.center.X += (int)(CURSOR_SPEED * Driver.Main.newGamePadState.ThumbSticks.Left.X / Driver.GameLogic.currentSpeed);
            }

            //If D-Pad or Left Thumbstick Right is pressed, move cursor right, as long as it can move right
            if (Driver.Main.newGamePadState.DPad.Right == ButtonState.Pressed && circle.Right() < 800)
            {
                circle.center.X += (int)(CURSOR_SPEED / (1.5 * Driver.GameLogic.currentSpeed));
            }
            else if (Driver.Main.newGamePadState.ThumbSticks.Left.X > 0 && circle.Right() < 800)
            {
                circle.center.X += (int)(CURSOR_SPEED * Driver.Main.newGamePadState.ThumbSticks.Left.X / Driver.GameLogic.currentSpeed);
            }

            //If D-Pad or Left Thumbstick Down is pressed, move cursor down, as long as it can move down
            if (Driver.Main.newGamePadState.DPad.Down == ButtonState.Pressed && circle.Bottom() < 550)
            {
                circle.center.Y += (int)(CURSOR_SPEED / (1.5 * Driver.GameLogic.currentSpeed));
            }
            else if (Driver.Main.newGamePadState.ThumbSticks.Left.Y < 0 && circle.Bottom() < 550)
            {
                circle.center.Y -= (int)(CURSOR_SPEED * Driver.Main.newGamePadState.ThumbSticks.Left.Y / Driver.GameLogic.currentSpeed);
            }

            //If D-Pad or Left Thumbstuck Up is pressed, move cursor up, as long as it can move up
            if (Driver.Main.newGamePadState.DPad.Up == ButtonState.Pressed && circle.Top() > 0)
            {
                circle.center.Y -= (int)(CURSOR_SPEED / (1.5 * Driver.GameLogic.currentSpeed));
            }
            else if (Driver.Main.newGamePadState.ThumbSticks.Left.Y > 0 && circle.Top() > 0)
            {
                circle.center.Y -= (int)(CURSOR_SPEED * Driver.Main.newGamePadState.ThumbSticks.Left.Y / Driver.GameLogic.currentSpeed);
            }

            //If cursor goes off screen, put it back on screen
            if (circle.Left() < 0)
            {
                circle.center.X = circle.radius;
            }
            else if (circle.Right() > 800)
            {
                circle.center.X = 800 - circle.radius;
            }
            else if (circle.Top() < 0)
            {
                circle.center.Y = circle.radius;
            }
            else if (circle.Bottom() > 550)
            {
                circle.center.Y = 550 - circle.radius;
            }

            //Calling subprogram to update cursor circle
            circle.Update();

            //Updating grid location and block highlighting of cursor
            gridLoc.X = (int)(Math.Floor(circle.center.X / 50.0));
            gridLoc.Y = (int)(Math.Floor(circle.center.Y / 50.0));
            highlightingRect.X = (int)(50 * gridLoc.X);
            highlightingRect.Y = (int)(50 * gridLoc.Y);

            //If 'A' is pressed and cursor is on game map, set grid location, if 'B' is pressed, clear selected grid location
            if (Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.A, Driver.Main.oldGamePadState.Buttons.A) &&
                gridLoc.X < 13 && gridLoc.Y < 11)
            {
                selectedGridLoc = gridLoc;
            }
            else if (Driver.Main.NewButtonPress(Driver.Main.newGamePadState.Buttons.B, Driver.Main.oldGamePadState.Buttons.B))
            {
                selectedGridLoc = new Vector2(-1, -1);
            }
        }

        //Pre: None
        //Post: Draws the cursor location
        //Description: Subprogram to hold draw logic of cursor
        public static void Draw()
        {
            //Drawing highlighting if cursor is on map and should be shown otherwise, draw cursor
            if (circle.center.X < 650 && gridRect)
            {
                Driver.Main.spriteBatch.Draw(Driver.Main.whiteRectImg, highlightingRect, Color.Yellow * 0.35f);
            }
            else
            {
                circle.Draw();
            }
        }
    }
}
