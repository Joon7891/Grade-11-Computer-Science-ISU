//Name: Joon Song
//File Name: GameLogic.cs
//Project Name: ISU_Song
//Creation Date: Janurary 2, 2018
//Modified Date: Janurary 21, 2018
//Description: Class to handle major game logic

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

namespace ISU_Song.Driver
{
    class GameLogic
    {
        //Current wave and wave object
        public static int currentWaveNo;
        public static Enemies.Wave currentWave;

        //Variables and constants to hold current game state button and logic
        private static Texture2D[] gameStateButtonImg = new Texture2D[3];
        private static Rectangle gameStateButtonRect;
        public static int currentGameState = 0;
        public const int WAVE_ENDED = 0;
        private const int SPEED_1X = 1;
        private const int SPEED_2X = 2;
        public const int GAME_ENDED = 3;
        public const int GAME_PENDING = 4;
        public static double currentSpeed = 1;

        //Vector2 and SpriteFont variables for drawing text
        private static Vector2[] numEnemiesLoc = {new Vector2(670, 454), new Vector2(665, 472), new Vector2(708, 490)};
        private static Vector2 gameStatusLoc = new Vector2(658, 435);
        public static string gameStatusStr = "";
        private static SpriteFont gameLogicFont;
        private static SpriteFont gameStatusFont;


        //Pre: 'Content', the content manager required to load content
        //Post: Various game logic components are loaded and set up
        //Description: Subprogram to load and set up various game logic components
        public static void Load(ContentManager Content)
        {
            //Importing game screen button images and setting up rectangles
            for (int i = 0; i < 3; i++)
            {
                gameStateButtonImg[i] = Content.Load<Texture2D>("Images/Sprites/Buttons/gameButton" + (i + 1));
            }
            gameStateButtonRect = new Rectangle(660, 512, 130, 35);
            
            //Importing fonts
            gameLogicFont = Content.Load<SpriteFont>("Fonts/GameLogicFont");
            gameStatusFont = Content.Load<SpriteFont>("Fonts/GameStatusFont");

            //Setting a default blank wave
            currentWave = new Enemies.Wave(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        //Pre: None
        //Post: Various game components are reset
        //Description: Subprogram to hold reset logic for various game components
        public static void Reset()
        {
            //Resetting current wave and wave number
            currentWaveNo = 1;
            currentWave = new Enemies.Wave(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            //Resetting game state and speed to not running
            currentGameState = WAVE_ENDED;
            currentSpeed = 1;
            Main.Instance.SetFrameRate(60.0f);
        }

        //Pre: None
        //Post: Various game components are updated
        //Description: Subprogram to hold game update logic
        public static void Update()
        {
            //If media player is not already playing, play game background song
            if (MediaPlayer.State != MediaState.Playing)
            {
                MediaPlayer.Play(Main.gameScreenBackMsc[0]);
            }

            //If game state button is pressed
            if (CollisionDetection.PointToRectangle(gameStateButtonRect, GUI.Cursor.circle.center) && Main.NewButtonPress(Main.newGamePadState.Buttons.A, Main.oldGamePadState.Buttons.A))
            {
                //Changing game state depending on current game state
                if (currentGameState == WAVE_ENDED)
                {
                    currentGameState = SPEED_1X;
                    currentSpeed = 1;

                    //Adding wave
                    AddWave();

                    //Calling subprogram to set frame rate
                    Main.Instance.SetFrameRate(60.0f);
                }
                else if (currentGameState == SPEED_1X)
                {
                    currentGameState = SPEED_2X;
                    currentSpeed = 2;

                    //Calling subprogram to set frame rate
                    Main.Instance.SetFrameRate(120.0f);
                }
                else
                {
                    currentGameState = SPEED_1X;
                    currentSpeed = 1;

                    //Calling subprogram to set frame rate
                    Main.Instance.SetFrameRate(60.0f);
                }
            }

            //If there are no more enemies and wave/game has not ended
            if (!(currentWave.enemyList.Count > 0) && currentGameState != WAVE_ENDED && currentGameState != GAME_ENDED)
            {
                //Iterating through each tower
                foreach (KeyValuePair<Vector2, Towers.Tower> newTower in GUI.GameLevel.towerDictionary)
                {
                    //If the tower is a resource tower, add resources
                    if (newTower.Value is Towers.ResourceTower)
                    {
                        (newTower.Value as Towers.ResourceTower).AddResources();
                    }
                }

                //Saving game
                IO.SetUserData();
                IO.SetGlobalData();

                //Ending wave, increasing wave number and generating new wave
                currentGameState = WAVE_ENDED;
                currentWaveNo++;
            }

            //Only updating wave of wave and game has not ended
            if (currentGameState != WAVE_ENDED && currentGameState != GAME_ENDED)
            {
                currentWave.Update();
            }

            //If user ran out of health, reset game state, lives, cursor, and media player
            if (GameData.Player.lives <= 0 && (currentGameState == SPEED_1X || currentGameState == SPEED_2X))
            {
                currentGameState = GAME_ENDED;
                GameData.Player.lives = 0;
                GUI.Cursor.gridRect = false;
                MediaPlayer.Stop();
            }

            //Updating each tower defense
            foreach (KeyValuePair<Vector2, Towers.Tower> newTower in GUI.GameLevel.towerDictionary)
            {
                newTower.Value.Update();
            }

            //For loop to call each item in item list
            for (int i = 0; i < GUI.GameLevel.itemList.Count; i++)
            {
                //Updating item
                GUI.GameLevel.itemList[i].Update();

                //Removing item if it has been used
                if (GUI.GameLevel.itemList[i].isItemUsed)
                {
                    GUI.GameLevel.itemList.RemoveAt(i);
                    i--;
                }
            }

            //If a tower is selected
            if (GUI.GameLevel.towerDictionary.ContainsKey(GUI.Cursor.selectedGridLoc))
            {
                //If remove button is pressed, remove tower
                if (Main.NewButtonPress(Main.newGamePadState.Buttons.A, Main.oldGamePadState.Buttons.A) && 
                    CollisionDetection.PointToRectangle(Towers.Tower.removeButtonRect, GUI.Cursor.circle.center))
                {
                    GUI.GameLevel.towerDictionary.Remove(GUI.Cursor.selectedGridLoc);
                }
            }
        }

        //Pre: None
        //Post: Various game components are drawn
        //Description: Subprogram to hold game draw logic
        public static void Draw()
        {
            //Drawing game control button
            Main.spriteBatch.Draw(gameStateButtonImg[currentGameState % 3], gameStateButtonRect, Color.White);

            //Drawing text indicating game status
            Main.spriteBatch.DrawString(gameStatusFont, gameStatusStr, gameStatusLoc, Color.Red);

            //Drawing text indicating current wave
            Main.spriteBatch.DrawString(gameLogicFont, "Wave No: " + currentWaveNo, numEnemiesLoc[0], Color.Black);

            //Drawing text indicating number of enemies remaining, if game is running
            if (currentGameState != WAVE_ENDED)
            {
                Main.spriteBatch.DrawString(gameLogicFont, "Enemies Left:", numEnemiesLoc[1], Color.Red);
                Main.spriteBatch.DrawString(gameLogicFont, "" + currentWave.enemyList.Count, numEnemiesLoc[2], Color.Black);
            }
        }

        //Pre: None
        //Post: Updates username
        //Description: Subprogram to update username
        public static void UsernameUpdate()
        {
            //If length of username is less than 10, call subrogram to add letters/number to end of username
            if (GameData.Player.name.Length < 10)
            {
                GameData.Player.name += AddKey(Keys.A, "A");
                GameData.Player.name += AddKey(Keys.B, "B");
                GameData.Player.name += AddKey(Keys.C, "C");
                GameData.Player.name += AddKey(Keys.D, "D");
                GameData.Player.name += AddKey(Keys.E, "E");
                GameData.Player.name += AddKey(Keys.F, "F");
                GameData.Player.name += AddKey(Keys.G, "G");
                GameData.Player.name += AddKey(Keys.H, "H");
                GameData.Player.name += AddKey(Keys.I, "I");
                GameData.Player.name += AddKey(Keys.J, "J");
                GameData.Player.name += AddKey(Keys.K, "K");
                GameData.Player.name += AddKey(Keys.L, "L");
                GameData.Player.name += AddKey(Keys.M, "M");
                GameData.Player.name += AddKey(Keys.N, "N");
                GameData.Player.name += AddKey(Keys.O, "O");
                GameData.Player.name += AddKey(Keys.P, "P");
                GameData.Player.name += AddKey(Keys.Q, "Q");
                GameData.Player.name += AddKey(Keys.R, "R");
                GameData.Player.name += AddKey(Keys.S, "S");
                GameData.Player.name += AddKey(Keys.T, "T");
                GameData.Player.name += AddKey(Keys.U, "U");
                GameData.Player.name += AddKey(Keys.V, "V");
                GameData.Player.name += AddKey(Keys.W, "W");
                GameData.Player.name += AddKey(Keys.X, "X");
                GameData.Player.name += AddKey(Keys.Y, "Y");
                GameData.Player.name += AddKey(Keys.Z, "Z");
                GameData.Player.name += AddKey(Keys.D0, "0");
                GameData.Player.name += AddKey(Keys.D1, "1");
                GameData.Player.name += AddKey(Keys.D2, "2");
                GameData.Player.name += AddKey(Keys.D3, "3");
                GameData.Player.name += AddKey(Keys.D4, "4");
                GameData.Player.name += AddKey(Keys.D5, "5");
                GameData.Player.name += AddKey(Keys.D6, "6");
                GameData.Player.name += AddKey(Keys.D7, "7");
                GameData.Player.name += AddKey(Keys.D8, "8");
                GameData.Player.name += AddKey(Keys.D9, "9");
            }

            //If enter or 'A' is pressed, add score, if backspace is pressed, delete last character
            if (NewKeyStroke(Keys.Enter) || Main.NewButtonPress(Main.newGamePadState.Buttons.A, Main.oldGamePadState.Buttons.A))
            {
                //Adding score and changing game state
                AddScore();
                currentGameState = GAME_PENDING;
            }
            else if (NewKeyStroke(Keys.Back) && GameData.Player.name.Length > 0)
            {
                GameData.Player.name = GameData.Player.name.Substring(0, GameData.Player.name.Length - 1);
            }
        }

        //Pre: None
        //Post: A new leaderboard score is added
        //Description: Subprogram to add a new leaderboard score
        private static void AddScore()
        {
            //Figuring out what posistion the new score is in and making adjustments as necessary
            for (int posNo = 0; posNo < Main.leaderboardScores.Length; posNo++)
            {
                if (currentWaveNo >= Main.leaderboardScores[posNo].score)
                {
                    for (int adjustNo = Main.leaderboardScores.Length - 1; adjustNo >= posNo + 1; adjustNo--)
                    {
                        Main.leaderboardScores[adjustNo].score = Main.leaderboardScores[adjustNo - 1].score;
                        Main.leaderboardScores[adjustNo].name = Main.leaderboardScores[adjustNo - 1].name;
                    }

                    Main.leaderboardScores[posNo].score = currentWaveNo;
                    Main.leaderboardScores[posNo].name = GameData.Player.name;

                    break;
                }
            }
        }

        //Pre: 'newKey', the key to test if its a new keystroke, and 'letter', the letter to be returned if its a new keystroke
        //Post: Returns the letter if keystroke is a new one
        //Description: Subprogram to return a certain letter if keystroke is a new one
        private static string AddKey(Keys newKey, string letter)
        {
            //If keystroke is a new one, return letter
            if (NewKeyStroke(newKey))
            {
                return letter;
            }
            
            //Otherwise, return a blank string
            return "";
        }

        //Pre: 'newKey', the key to test if its a newkeystroke
        //Post: Returns true or false, depending on if the keystroke is a new one
        //Description: Subprogram to determine if keystorke is a new one
        private static bool NewKeyStroke(Keys newKey)
        {
            //If keystroke is a new one, return true
            if (Main.newKeyboardState.IsKeyDown(newKey) && Main.oldKeyboardState.IsKeyUp(newKey))
            {
                return true;
            }
            
            //Otherwise, return false
            return false;
        }

        //Pre: None
        //Post: A new wave is added
        //Description: Subprogram to add a new wave
        private static void AddWave()
        {
            //Int variables to hold number of each enemy for the new wave
            int newKnightNo;
            int newArcherNo;
            int newBanditNo;
            int newSkeletonNo;
            int newGoblinNo;
            int newPrinceNo;
            int newWizardNo;
            int newFairyNo;
            int newWitchNo;
            int newDragonNo;

            //Getting number of each enemy as a function of wave number
            newKnightNo = (int)Math.Ceiling(15 * Math.Log10(currentWaveNo) + 10);
            newArcherNo = (int)Math.Ceiling(18 * Math.Log10(currentWaveNo) + 5);
            newBanditNo = (int)Math.Ceiling(12 * Math.Log10(currentWaveNo) - 5);
            newSkeletonNo = (int)Math.Ceiling(4 * Math.Sqrt(currentWaveNo * 0.8) + 5);
            newGoblinNo = (int)Math.Ceiling(10 * Math.Sin(0.5 * (currentWaveNo - 5)) + 5 + 0.05 * currentWaveNo);
            newPrinceNo = (int)Math.Ceiling(0.8 * currentWaveNo - 11);
            newWizardNo = (int)Math.Ceiling(0.5 * currentWaveNo - 7);
            newFairyNo = (int)Math.Ceiling(8 * Math.Log10(2 * currentWaveNo) - 11);
            newWitchNo = (int)Math.Ceiling(0.6 * currentWaveNo - 11);
            newDragonNo = (int)(0.15 * currentWaveNo - 2);

            //Resetting wave
            currentWave = new Enemies.Wave(newKnightNo, newArcherNo, newBanditNo, newSkeletonNo, newGoblinNo, newPrinceNo, newWizardNo, newFairyNo, newWitchNo, newDragonNo);
        }
    }
}
