//Name: Joon Song
//File Name: Main.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 21, 2018
//Description: Tower defense game program (Created using Visual Studio Community 2017)

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Driver
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        //Instance of parent class, used to set and get data
        public static Main Instance { get; private set; }

        //Instances of graphics objects
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;

        //Variables to hold new and old game pad state and keyboard state
        public static GamePadState newGamePadState;
        public static GamePadState oldGamePadState;
        public static KeyboardState newKeyboardState;
        public static KeyboardState oldKeyboardState;

        //Instance of random number generator
        public static Random rng = new Random();

        //Time variables
        public static double globalDeltaTime;
        public static double totalTimePlayed;
        private static string totalTimePlayerStr;

        //Variables to hold main menu selection, screen mode and misc. components
        int selectOption;
        int screenMode;
        const int MAIN_MENU = 0;
        const int GAME_SCREEN = 1;
        const int LEADERBOARD = 2;
        const int SETTINGS = 3;
        public static bool isGamePaused = false;
        public static bool tutorialShown = false;

        //Float variables to hold both current and actual sound effect and music levels
        public static float[] currentVolume = new float[2];
        public static float[] actualVolume = new float[2];

        //Texture2D variable for white rectangles
        public static Texture2D whiteRectImg;

        //Texture2D and Rectangle variables to hold images, dimensions and location of main menu buttons
        Texture2D[] mainMenuButtonsImg = new Texture2D[3];
        Rectangle[] mainMenuButtonsRect = new Rectangle[3];

        //Texture2D and Rectangle variables to hold images, dimensions and location of xbox buttons
        Texture2D backButtonImg;
        Rectangle backButtonRect;
        Texture2D xboxAButtonImg;
        Rectangle xboxAButtonRect;
        Texture2D xboxBButtonImg;
        Rectangle xboxBButtonRect;
        Texture2D xboxXButtonImg;
        Rectangle xboxXButtonRect;
        Texture2D xboxYButtonImg;
        Rectangle xboxYButtonRect;
        Texture2D[] xboxRearButtonImg = new Texture2D[4];
        Rectangle[] xboxRearButtonRect = new Rectangle[4];

        //Texture2D and Rectangle variables to hold images, dimensions and location of options buttons
        Texture2D exitButtonImg;
        Rectangle exitButtonRect;
        Texture2D backMenuButtonImg;
        Texture2D saveQuitButtonImg;
        Texture2D resetGameButtonImg;
        Texture2D settingsButtonImg;
        Texture2D reviveButtonImg;
        Rectangle[] buttonRect = new Rectangle[4];
        Rectangle[] optionsRect = new Rectangle[2];
        Vector2 optionsHeaderLoc;

        //Texture2D and rectangle variables to hold image and rectangle for background images
        Texture2D mainMenuBackImg;
        Texture2D tutorialBackImg;
        Texture2D leaderboardBackImg;
        Texture2D settingsBackImg;
        Rectangle backRect = new Rectangle(0, 0, 800, 550);
        Texture2D[] fogImg = new Texture2D[2];
        Rectangle[] fogRect = new Rectangle[2];

        //Variables for leaderboard screen
        public static GameData.LeaderboardScore[] leaderboardScores = new GameData.LeaderboardScore[10];
        Texture2D leaderboardLogoImg;
        Rectangle[] leaderboardLogoRect = { new Rectangle(170, 8, 50, 50), new Rectangle(580, 8, 50, 50)};
        Vector2[,] leaderboardTextLoc = new Vector2[3, 10];

        //Variables required for various settings screen graphics
        Rectangle[] musicSliderRect = { new Rectangle(85, 100, 5, 50), new Rectangle(85, 122, 560, 5), new Rectangle(645, 100, 5, 50) };
        Rectangle[] soundEffectSliderRect = { new Rectangle(85, 200, 5, 50), new Rectangle(85, 222, 560, 5), new Rectangle(645, 200, 5, 50) };
        Rectangle[] sliderTabRect = { new Rectangle(645, 110, 5, 30), new Rectangle(645, 210, 5, 30)};
        Vector2[] settingsTextLoc = { new Vector2(318, 15), new Vector2(30, 70), new Vector2(30, 170), new Vector2(30, 275) };
        Vector2[] volumeLeveLoc = { new Vector2(715, 105), new Vector2(715, 205) };
        string[] settingsText = { "Settings", "Music:", "Sound Effect:", "Apply Changes: "};
        Vector2 totalPlayTimeLoc = new Vector2(25, 500);
        Texture2D playGameButtonImg;
        Rectangle playGameButtonRect;

        //Song and bool variables to hold various background music and song logic
        Song mainMenuBackMsc;
        public static Song[] gameScreenBackMsc = new Song[2];
        Song leaderboardScreenBackMsc;
        Song settingsScreenBackMsc;

        //SoundEffect varibales and their instances to hold soundeffects
        SoundEffect buttonClickSnd;
        static SoundEffectInstance buttonClickSndInstance;

        //SpriteFont variables to hold fonts
        SpriteFont mainMenuFont;
        SpriteFont optionsFont;
        SpriteFont[] gameEndedFont = new SpriteFont[2];
        SpriteFont[] leaderboardFont = new SpriteFont[2];
        SpriteFont[] settingsFont = new SpriteFont[2];

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Instance = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //Setting up window size
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 550;

            //Applying changes and initializing the game
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            //Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Setting default screen as main menu and main menu select option as 1
            screenMode = MAIN_MENU;
            selectOption = 1;

            //Calling subprograms to load data for various classes
            GUI.GameLevel.Load(Content);
            Towers.Tower.Load(Content);
            Projectiles.ProjectileResources.Load(Content);
            Items.Item.Load(Content);
            GUI.Shop.Load(Content);
            Enemies.EnemyResources.Load(Content);
            Items.ItemResources.Load(Content);
            GUI.Circle.Load(Content);
            GameLogic.Load(Content);

            //File Input
            IO.GetUserData();
            IO.GetGlobalData();

            //Importing image for white rectangle
            whiteRectImg = Content.Load<Texture2D>("Images/Sprites/whiteRect");

            //Importing button images and setting up button rectangles
            backButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/backButton");
            backButtonRect = new Rectangle(10, 10, 40, 40);
            xboxAButtonImg = Content.Load<Texture2D>("Images/Sprites/XBoxButtons/xboxAButton");
            xboxAButtonRect = new Rectangle(510, 265, 40, 40);
            xboxBButtonImg = Content.Load<Texture2D>("Images/Sprites/XboxButtons/xboxBButton");
            xboxBButtonRect = new Rectangle(60, 10, 40, 40);
            xboxXButtonImg = Content.Load<Texture2D>("Images/Sprites/XboxButtons/xboxXButton");
            xboxXButtonRect = new Rectangle(590, 500, 40, 40);
            xboxYButtonImg = Content.Load<Texture2D>("Images/Sprites/XboxButtons/xboxYButton");
            xboxYButtonRect = new Rectangle(260, 270, 40, 40);
            for (int i = 0; i < 4; i++)
            {
                xboxRearButtonImg[i] = Content.Load<Texture2D>("Images/Sprites/XboxButtons/xboxRearButton" + (i + 1));
                xboxRearButtonRect[i] = new Rectangle(40 + 615 * (i % 2), 103 + (int)(100 * Math.Floor(i / 2.0)), 40, 40);
            }
            for (int i = 0; i < 3; i++)
            {
                mainMenuButtonsImg[i] = Content.Load<Texture2D>("Images/Sprites/Buttons/mainMenuButton" + (i + 1));
                mainMenuButtonsRect[i] = new Rectangle(300, 260 + 65 * i, 200, 50);
            }

            //Importing images and setting up rectangles for options menu
            optionsRect[0] = new Rectangle(265, 165, 270, 250);
            optionsRect[1] = new Rectangle(275, 175, 250, 230);
            exitButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/exitButton");
            exitButtonRect = new Rectangle(490, 180, 30, 30);
            backMenuButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/mainMenuButton");
            saveQuitButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/saveQuitButton");
            resetGameButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/resetGameButton");
            settingsButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/settingsButton");
            reviveButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/reviveButton");
            playGameButtonImg = Content.Load<Texture2D>("Images/Sprites/Buttons/playGameButton");
            playGameButtonRect = new Rectangle(640, 500, 150, 40);
            for (int i = 0; i < 4; i++)
            {
                buttonRect[i] = new Rectangle(315, 225 + 45 * i, 170, 40);
            }
            optionsHeaderLoc = new Vector2(330, 172);

            //Importing background images
            mainMenuBackImg = Content.Load<Texture2D>("Images/Backgrounds/mainMenuBackground");
            tutorialBackImg = Content.Load<Texture2D>("Images/Backgrounds/howToPlay");
            leaderboardBackImg = Content.Load<Texture2D>("Images/Backgrounds/leaderboardBack");
            settingsBackImg = Content.Load<Texture2D>("Images/Backgrounds/castleBackground");

            //Importing fog image and setting up rectangle
            for (int i = 0; i < 2; i++)
            {
                fogImg[i] = Content.Load<Texture2D>("Images/Backgrounds/fog" + (i +1));
                fogRect[i] = new Rectangle(800 * i, 0, 800, 550);
            }

            //Importing and setting up various leaderboard components
            leaderboardLogoImg = Content.Load<Texture2D>("Images/Backgrounds/leagueImg");
            for (int i = 0; i < 10; i++)
            {
                leaderboardTextLoc[0, i] = new Vector2(80, 140 + 38 * i);
                leaderboardTextLoc[1, i] = new Vector2(335, 140 + 38 * i);
                leaderboardTextLoc[2, i] = new Vector2(682, 140 + 38 * i);
            }

            //Importing songs
            mainMenuBackMsc = Content.Load<Song>("Audio/Music/Night at the Tavern");
            for (int i = 0; i < 2; i++)
            {
                gameScreenBackMsc[i] = Content.Load<Song>("Audio/Music/gameMusic" + (i + 1));
            }
            leaderboardScreenBackMsc = Content.Load<Song>("Audio/Music/leaderboardBackMsc");
            settingsScreenBackMsc = Content.Load<Song>("Audio/Music/settingsBackMsc");

            //Importing soundeffects and setting up their instances
            buttonClickSnd = Content.Load<SoundEffect>("Audio/SoundEffects/buttonClick");
            buttonClickSndInstance = buttonClickSnd.CreateInstance();

            //Importing fonts
            mainMenuFont = Content.Load<SpriteFont>("Fonts/MainMenuFont");
            optionsFont = Content.Load<SpriteFont>("Fonts/OptionsFont");
            for (int i = 0; i < 2; i++)
            {
                settingsFont[i] = Content.Load<SpriteFont>("Fonts/SettingsFont" + (i + 1));
                leaderboardFont[i] = Content.Load<SpriteFont>("Fonts/LeaderboardFont" + (i + 1));
                gameEndedFont[i] = Content.Load<SpriteFont>("Fonts/GameEndedFont" + (i + 1));
            }

            //Setting volume levels
            AudioUpdate();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Updating global delta time and total play time
            globalDeltaTime = gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
            totalTimePlayed += globalDeltaTime;
            TotalTimePlayedString();

            //Updating new and old game pad state and keyboard state
            oldGamePadState = newGamePadState;
            newGamePadState = GamePad.GetState(PlayerIndex.One);
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();

            //Calling appropriate update subprogram for each screen mode (Ex. Main menu)
            switch (screenMode)
            {
                case MAIN_MENU:
                    {
                        MainMenuUpdate();

                        break;
                    }
                case GAME_SCREEN:
                    {
                        GameScreenUpdate();

                        break;
                    }
                case LEADERBOARD:
                    {
                        LeaderboardUpdate();

                        break;
                    }
                case SETTINGS:
                    {
                        SettingsUpdate();

                        break;
                    }
            }

            //Updating gameTime
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Setting default background as black
            GraphicsDevice.Clear(Color.Black);

            //Beginning spriteBatch, allowing the drawing of images past this point
            spriteBatch.Begin();

            //Calling appropriate draw subprogram for each screen mode (Ex. Main menu)
            switch (screenMode)
            {
                case MAIN_MENU:
                    {
                        MainMenuDraw();

                        break;
                    }
                case GAME_SCREEN:
                    {
                        GameScreenDraw();

                        break;
                    }
                case LEADERBOARD:
                    {
                        LeaderboardDraw();

                        break;
                    }
                case SETTINGS:
                    {
                        SettingsDraw();

                        break;
                    }
            }

            //Ending spriteBatch, preventing the drawing of images past this point
            spriteBatch.End();

            //Drawing gameTime
            base.Draw(gameTime);
        }

        //Pre: None
        //Post: Various main menu screen updates are made
        //Description: Subprogram to hold update logic for main menu screen
        private void MainMenuUpdate()
        {            
            //If the D-Pad/left thumbstick is moved, change selection option. If 'A' is pressed, go to the appropriate screen
            if (GameLogic.NewKeyStroke(Keys.Down) ||
                NewButtonPress(newGamePadState.DPad.Down, oldGamePadState.DPad.Down) || 
                NewThumbstickMovement(newGamePadState.ThumbSticks.Left, oldGamePadState.ThumbSticks.Left) == "DOWN")
            {                
                selectOption = (selectOption % 3) + 1;
                xboxAButtonRect.Y = 200  + selectOption * 65;
            }
            else if (GameLogic.NewKeyStroke(Keys.Up) ||
                NewButtonPress(newGamePadState.DPad.Up, oldGamePadState.DPad.Up) || 
                NewThumbstickMovement(newGamePadState.ThumbSticks.Left, oldGamePadState.ThumbSticks.Left) == "UP")
            {
                selectOption = (selectOption + 1) % 3 + 1;
                xboxAButtonRect.Y = 200 + selectOption * 65;
            }
            else if (GameLogic.NewKeyStroke(Keys.Enter) || NewButtonPress(newGamePadState.Buttons.A, oldGamePadState.Buttons.A))
            {
                //Changing screen mode based off of selection option
                screenMode = selectOption;

                //If selected screen is settings screen, update actual volume
                if (selectOption == SETTINGS)
                {
                    SetActualVolume();
                }

                //Stopping main menu background music
                MediaPlayer.Stop();
            }

            //Updating fog location
            for (int i = 0; i < 2; i++)
            {
                //Updating fog location
                if (fogRect[i].X <= -800)
                {
                    fogRect[i].X = fogRect[(i + 1) % 2].X + 800;
                }
                fogRect[i].X -= 2;
            }

            //Playing main menu background music if media player is not already playing and screenMode is menu screen
            if (MediaPlayer.State != MediaState.Playing && screenMode == MAIN_MENU)
            {
                MediaPlayer.Play(mainMenuBackMsc);               
            }
        }

        //Pre: None
        //Post: Various main menu graphics are drawn
        //Description: Subprogram to hold draw logic for main menu screen
        private void MainMenuDraw()
        {
            //Drawing background and fog
            spriteBatch.Draw(mainMenuBackImg, backRect, Color.White);
            for(int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(fogImg[i], fogRect[i], Color.White * 0.75f);
            }

            //Drawing title
            spriteBatch.DrawString(mainMenuFont, "Save The Princess", new Vector2(130, 80), Color.Black * 0.85f);

            //Drawing each button, selection exists to make current button more opaque
            for (int i = 0; i < 3; i++)
            {
                if (i == selectOption - 1)
                {
                    spriteBatch.Draw(mainMenuButtonsImg[i], mainMenuButtonsRect[i], Color.White);
                }
                else
                {
                    spriteBatch.Draw(mainMenuButtonsImg[i], mainMenuButtonsRect[i], Color.White * 0.70f);
                }                
            }

            //Drawing 'A' button; informs the user that they need to press the 'A' button
            spriteBatch.Draw(xboxAButtonImg, xboxAButtonRect, Color.White);
        }

        //Pre: None
        //Post: Various game screen updates are made
        //Description: Subprogram to hold update logic for game screen
        private void GameScreenUpdate()
        {
            //Selection to call right update logic
            if (!tutorialShown)
            {
                //If tutorial has not been seen yet, wait for user to press 'Y' to resume game
                if (GameLogic.NewKeyStroke(Keys.Enter) || NewButtonPress(newGamePadState.Buttons.Y, oldGamePadState.Buttons.Y))
                {
                    tutorialShown = true;
                }
            }
            else if (isGamePaused)
            {
                //If game is paused, call options update subprogram
                OptionsUpdate();
            }
            else if (GameLogic.currentGameState == GameLogic.GAME_ENDED || GameLogic.currentGameState == GameLogic.GAME_PENDING)
            {
                //Playing end game music, if media player is not already running
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(gameScreenBackMsc[1]);
                }
                
                //If game ended, call game ended update subprogram
                GameEndedUpdate();
            }
            else
            {
                //Calling subprograms to update game logic and shop
                GameLogic.Update();
                GUI.Shop.Update();
            }

            //Calling subprogram to handle cursor logic
            GUI.Cursor.Update();

            //If start is pressed, change game state from paused to unpaused or vise versa
            if (NewButtonPress(newGamePadState.Buttons.Start, oldGamePadState.Buttons.Start))
            {
                isGamePaused = !isGamePaused;
                GUI.Cursor.gridRect = !isGamePaused;
            }
        }

        //Pre: None
        //Post: Various game screen graphics are drawn
        //Description: Subprogram to hold draw logic for game screen
        private void GameScreenDraw()
        {
            //Drawing game level background
            GUI.GameLevel.Draw();

            //Drawing shop
            GUI.Shop.Draw();

            //Drawing game logic components
            GameLogic.Draw();

            //If game is paused, or ended, call appropriate draw subprogram
            if (isGamePaused)
            {
                OptionsDraw();
            }
            else if (GameLogic.currentGameState == GameLogic.GAME_ENDED || GameLogic.currentGameState == GameLogic.GAME_PENDING)
            {
                GameEndedDraw();
            }

            //Drawing cursor
            GUI.Cursor.Draw();

            //If user has not seen tutorial, draw tutorial
            if (!tutorialShown)
            {
                spriteBatch.Draw(tutorialBackImg, backRect, Color.White);
            }
        }

        //Pre: None
        //Post: Various options menu updates are made
        //Description: Subprogram to hold update logic for options menu
        private void OptionsUpdate()
        {
            //If exit button or 'B' button is [ressed, unpause game
            if ((NewButtonPress(newGamePadState.Buttons.A, oldGamePadState.Buttons.A) &&
                CollisionDetection.PointToRectangle(exitButtonRect, GUI.Cursor.circle.center)) ||
                NewButtonPress(newGamePadState.Buttons.B, oldGamePadState.Buttons.B))
            {
                isGamePaused = false;
            }

            //If a certain option button is pressed, proceed with appropriate logic
            if (NewButtonPress(newGamePadState.Buttons.A, oldGamePadState.Buttons.A))
            {
                if (CollisionDetection.PointToRectangle(buttonRect[0], GUI.Cursor.circle.center))
                {
                    //Changing screen to main menu and stopping media player
                    screenMode = MAIN_MENU;
                    MediaPlayer.Stop();
                }
                else if (CollisionDetection.PointToRectangle(buttonRect[1], GUI.Cursor.circle.center))
                {
                    //Saving game and exiting
                    IO.SetUserData();
                    IO.SetGlobalData();
                    Exit();
                }
                else if (CollisionDetection.PointToRectangle(buttonRect[2], GUI.Cursor.circle.center))
                {
                    //Resetting game
                    ResetGame();
                }
                else if (CollisionDetection.PointToRectangle(buttonRect[3], GUI.Cursor.circle.center))
                {
                    //Going to settings screen and stopping media player
                    MediaPlayer.Stop();
                    screenMode = SETTINGS;
                }
            }
        }

        //Pre: None
        //Post: Various options menu graphics are drawn
        //Description: Subprogram to hold draw logic for options menu
        private void OptionsDraw()
        {
            //Drawing options box and exit button
            spriteBatch.Draw(whiteRectImg, optionsRect[0], Color.White * 0.4f);
            spriteBatch.Draw(whiteRectImg, optionsRect[1], Color.Black * 0.4f);
            spriteBatch.DrawString(optionsFont, "Options", optionsHeaderLoc, Color.Black);
            spriteBatch.Draw(exitButtonImg, exitButtonRect, Color.White);

            //Drawing options buttons
            spriteBatch.Draw(backMenuButtonImg, buttonRect[0], Color.White);
            spriteBatch.Draw(saveQuitButtonImg, buttonRect[1], Color.White);
            spriteBatch.Draw(resetGameButtonImg, buttonRect[2], Color.White);
            spriteBatch.Draw(settingsButtonImg, buttonRect[3], Color.White);
        }

        //Pre: None
        //Post: Various game components are updated 
        //Description: Subprogram to hold update logic for when game ends
        private void GameEndedUpdate()
        {
            //Selection for game screen
            if (GameLogic.currentGameState == GameLogic.GAME_ENDED)
            {
                //If 'Y' is pressed and user can revive
                if (NewButtonPress(newGamePadState.Buttons.Y, oldGamePadState.Buttons.Y))
                {
                    if (!GameData.Player.reviveUsed)
                    {
                        //If user is elidigible for purchase, complete purchase, reset wave and make misc. updates
                        if (GameData.Player.gold >= 10000)
                        {
                            GameData.Player.gold -= 10000;
                            GameLogic.currentWave = new Enemies.Wave(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                            GameLogic.currentGameState = GameLogic.WAVE_ENDED;
                            GameData.Player.lives = 100;
                            GameData.Player.reviveUsed = true;

                            //Stopping background music
                            MediaPlayer.Stop();

                            //Saving game
                            IO.SetUserData();
                            IO.SetGlobalData();
                        }
                    }
                    else
                    {
                        //Playing error sound effect and informing user
                        GameData.ShopItem.errorSndInstance.Play();
                        GameLogic.gameStatusStr = "Already Used";
                    }
                }

                //Calling subprogram to update username
                GameLogic.UsernameUpdate();
            }
            else if (GameLogic.currentGameState == GameLogic.GAME_PENDING)
            {
                //If a certain option button is pressed, proceed with appropriate logic
                if (NewButtonPress(newGamePadState.Buttons.A, oldGamePadState.Buttons.A))
                {
                    if (CollisionDetection.PointToRectangle(buttonRect[0], GUI.Cursor.circle.center))
                    {
                        //Saving game, resetting game, and changing to main menu screen
                        ResetGame();
                        IO.SetUserData();
                        IO.SetGlobalData();
                        screenMode = MAIN_MENU;

                        //Stopping background music
                        MediaPlayer.Stop();
                    }
                    else if (CollisionDetection.PointToRectangle(buttonRect[1], GUI.Cursor.circle.center))
                    {
                        //Saving game, resetting game, and exiting
                        ResetGame();
                        IO.SetUserData();
                        IO.SetGlobalData();
                        Exit();
                    }
                    else if (CollisionDetection.PointToRectangle(buttonRect[2], GUI.Cursor.circle.center))
                    {
                        //Saving game, and resetting game
                        ResetGame();
                        IO.SetUserData();
                        IO.SetGlobalData();

                        //Stopping background music
                        MediaPlayer.Stop();
                    }
                    else if (CollisionDetection.PointToRectangle(buttonRect[3], GUI.Cursor.circle.center))
                    {
                        //Saving game, resetting game, and going to settings screen
                        ResetGame();
                        IO.SetUserData();
                        IO.SetGlobalData();
                        screenMode = SETTINGS;

                        //Stopping background music
                        MediaPlayer.Stop();
                    }
                }
            }
        }

        //Pre: None
        //Post: Various game components are drawn
        //Description: Subprogram to hold draw logic for when game ends
        private void GameEndedDraw()
        {
            //Drawing options box and buttons
            spriteBatch.Draw(whiteRectImg, optionsRect[0], Color.White * 0.4f);
            spriteBatch.Draw(whiteRectImg, optionsRect[1], Color.Black * 0.4f);
            spriteBatch.DrawString(gameEndedFont[0], "Princess Was Captured", new Vector2(280, 177), Color.Red);
            if (GameLogic.currentGameState == GameLogic.GAME_ENDED)
            {
                spriteBatch.DrawString(gameEndedFont[1], "Username:", new Vector2(290, 215), Color.Black);
                spriteBatch.DrawString(gameEndedFont[1], "" + GameData.Player.name, new Vector2(320, 240), Color.Red);
                spriteBatch.DrawString(gameEndedFont[1], "Press Enter/'A' When Done", new Vector2(290, 270), Color.Black);
                spriteBatch.Draw(reviveButtonImg, new Rectangle(315, 300, 170, 40), Color.White * (1.0f - 0.5f * Convert.ToInt32(GameData.Player.reviveUsed)));
                spriteBatch.DrawString(gameEndedFont[1], "Cost: -10,000 Gold", new Vector2(320, 345), Color.Yellow);
                spriteBatch.DrawString(gameEndedFont[1], "        +100 Lives", new Vector2(325, 365), Color.Red);
            }
            else
            {
                //Drawing options buttons
                spriteBatch.Draw(backMenuButtonImg, buttonRect[0], Color.White);
                spriteBatch.Draw(saveQuitButtonImg, buttonRect[1], Color.White);
                spriteBatch.Draw(resetGameButtonImg, buttonRect[2], Color.White);
                spriteBatch.Draw(settingsButtonImg, buttonRect[3], Color.White);
            }
        }

        //Pre: None
        //Post: Game is reset
        //Description: Subprogram to reset game
        private void ResetGame()
        {
            //Removing all towers
            GUI.GameLevel.towerDictionary.Clear();

            //Resetting user data and cursor
            GameData.Player.ResetPlayer();
            GUI.Cursor.gridRect = true;

            //Calling subprogram to reset game logic data
            GameLogic.Reset();

            //Resetting output string
            GameLogic.gameStatusStr = "";

            //Resuming game and showing tutorial to user
            tutorialShown = false;
            isGamePaused = false;
        }

        //Pre: None
        //Post: Various leaderboard screen updates are made
        //Description: Subprogram to hold update logic for leaderboard screen
        private void LeaderboardUpdate()
        {
            //If 'B' button is pressed, go back to main menu screen
            if (NewButtonPress(newGamePadState.Buttons.B, oldGamePadState.Buttons.B))
            {
                screenMode = MAIN_MENU;

                MediaPlayer.Stop();
            }
            
            //Playing leaderboard music if media player is not already playing and screen is leaderboard screen
            if (MediaPlayer.State != MediaState.Playing && screenMode == LEADERBOARD)
            {
                MediaPlayer.Play(leaderboardScreenBackMsc);
            }
        }

        //Pre: None
        //Post: Various leaderboad screen graphics are drawn
        //Description: Subprogram to hold draw logic for leaderboard screen
        private void LeaderboardDraw()
        {
            Rectangle[] backgroundRect = { new Rectangle(30, 90, 740, 450), new Rectangle(40, 100, 720, 430) };

            //Drawing background
            spriteBatch.Draw(leaderboardBackImg, backRect, Color.White);
            
            //Drawing back and 'B' button
            spriteBatch.Draw(backButtonImg, backButtonRect, Color.DarkGray);
            spriteBatch.Draw(xboxBButtonImg, xboxBButtonRect, Color.White);

            //Drawing score background
            spriteBatch.Draw(whiteRectImg, backgroundRect[0], Color.SaddleBrown * 0.5f);
            spriteBatch.Draw(whiteRectImg, backgroundRect[1], Color.WhiteSmoke * 0.5f);

            //Drawing headers
            spriteBatch.DrawString(leaderboardFont[0], "Leaderboard", new Vector2(233, 3), Color.White);
            spriteBatch.Draw(leaderboardLogoImg, leaderboardLogoRect[0], Color.White);
            spriteBatch.Draw(leaderboardLogoImg, leaderboardLogoRect[1], Color.White);
            spriteBatch.DrawString(leaderboardFont[1], "Place", new Vector2(50, 100), Color.Brown);
            spriteBatch.DrawString(leaderboardFont[1], "Name", new Vector2(370, 100), Color.Brown);
            spriteBatch.DrawString(leaderboardFont[1], "Wave", new Vector2(660, 100), Color.Brown);

            //Drawing score text
            for (int i = 0; i < 10; i++)
            {
                spriteBatch.DrawString(leaderboardFont[1], "" + (i + 1), leaderboardTextLoc[0, i], Color.Black);
                spriteBatch.DrawString(leaderboardFont[1], "" + leaderboardScores[i].name, leaderboardTextLoc[1, i], Color.Black);
                spriteBatch.DrawString(leaderboardFont[1], "" + leaderboardScores[i].score, leaderboardTextLoc[2, i], Color.Black);
            }
        }

        //Pre: None
        //Post: Various settings screen updates are made
        //Description: Subprogram to hold update logic for settings screen
        private void SettingsUpdate()
        {
            //If a certain xbox button is pressed
            if (NewButtonPress(newGamePadState.Buttons.B, oldGamePadState.Buttons.B))
            {
                //If 'A' is pressed, change screen to main menu and stop music
                screenMode = MAIN_MENU;
                MediaPlayer.Stop();
            }
            else if (NewButtonPress(newGamePadState.Buttons.Y, oldGamePadState.Buttons.Y))
            {
                //If 'Y' is pressed, set actual music levels and call subprogram to make appropriate audio updates
                for (int i = 0; i < 2; i++)
                {
                    actualVolume[i] = currentVolume[i];
                }
                AudioUpdate();
            }
            else if (NewButtonPress(newGamePadState.Buttons.X, oldGamePadState.Buttons.X))
            {
                //If 'X' is pressed, change screen to game screen and stop music
                screenMode = GAME_SCREEN;
                MediaPlayer.Stop();
            }

            //If left/right trigger/bumper is pressed, update appropriate volume level and slider location (if possible) 
            if (NewTriggerPress(newGamePadState.Triggers.Left, oldGamePadState.Triggers.Left) && currentVolume[0] >= 0.05)
            {
                currentVolume[0] = (float) (Math.Round(100 * currentVolume[0] - 5)/100.0);
                sliderTabRect[0].X -= 28;
            }
            else if (NewTriggerPress(newGamePadState.Triggers.Right, oldGamePadState.Triggers.Right) && currentVolume[0] <= 0.95)
            {
                currentVolume[0] = (float)(Math.Round(100 * currentVolume[0] + 5) / 100.0);
                sliderTabRect[0].X += 28;
            }
            else if (NewButtonPress(newGamePadState.Buttons.LeftShoulder, oldGamePadState.Buttons.LeftShoulder) && currentVolume[1] >= 0.05)
            {
                currentVolume[1] = (float)(Math.Round(100 * currentVolume[1] - 5) / 100.0);
                sliderTabRect[1].X -= 28;
            }
            else if (NewButtonPress(newGamePadState.Buttons.RightShoulder, oldGamePadState.Buttons.RightShoulder) && currentVolume[1] <= 0.95)
            {
                currentVolume[1] = (float)(Math.Round(100 * currentVolume[1] + 5) / 100.0);
                sliderTabRect[1].X += 28;
            }

            //Playing settings background music if media player is not already playing and screenMode is settings screen
            if (MediaPlayer.State != MediaState.Playing && screenMode == SETTINGS)
            {
                MediaPlayer.Play(settingsScreenBackMsc);
            }
        }

        //Pre: None
        //Post: Various settings screen graphics are drawn
        //Description: Subprogram to hold draw logic for settings screen
        private void SettingsDraw()
        {
            //Drawing background
            spriteBatch.Draw(settingsBackImg, backRect, Color.White);
            
            //Drawing various buttons
            spriteBatch.Draw(backButtonImg, backButtonRect, Color.DarkGray);
            spriteBatch.Draw(xboxBButtonImg, xboxBButtonRect, Color.White);
            for (int i = 0; i < 4; i++)
            {
                spriteBatch.Draw(xboxRearButtonImg[i], xboxRearButtonRect[i], Color.White);
            }
            spriteBatch.Draw(playGameButtonImg, playGameButtonRect, Color.White);
            spriteBatch.Draw(xboxXButtonImg, xboxXButtonRect, Color.White);
            spriteBatch.Draw(xboxYButtonImg, xboxYButtonRect, Color.White);

            //Drawing on screen text
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.DrawString(settingsFont[i], settingsText[i], settingsTextLoc[i], Color.White);
                spriteBatch.DrawString(settingsFont[1], currentVolume[i] * 100 + "%", volumeLeveLoc[i], Color.LightGray);
            }
            spriteBatch.DrawString(settingsFont[1], settingsText[2], settingsTextLoc[2], Color.White);
            spriteBatch.DrawString(settingsFont[1], settingsText[3], settingsTextLoc[3], Color.White);

            //Drawing volume sliders and settings text
            for (int i = 0; i < 3; i++)
            {
                spriteBatch.Draw(whiteRectImg, musicSliderRect[i], Color.LightGray);
                spriteBatch.Draw(whiteRectImg, soundEffectSliderRect[i], Color.LightGray);
            }

            //Drawing volume slider tabs
            for (int i = 0; i < 2; i++)
            {
                spriteBatch.Draw(whiteRectImg, sliderTabRect[i], Color.Red);
            }

            //Drawing text indicating total play time
            spriteBatch.DrawString(settingsFont[1], "Total Play Time: " + totalTimePlayerStr, totalPlayTimeLoc, Color.Red);
        }

        //Pre: None
        //Post: Audio is updated according to user preferances
        //Description: Subprogram to update audio
        private void AudioUpdate()
        {
            //Setting media player volume
            MediaPlayer.Volume = actualVolume[0];

            //Setting soundeffect volume
            buttonClickSndInstance.Volume = actualVolume[1];
            GameData.ShopItem.errorSndInstance.Volume = actualVolume[1];
            GameData.ShopItem.transactionSndInstance.Volume = actualVolume[1];

            //Setting actual volume
            SetActualVolume();

            //Saving changes to file
            IO.SetGlobalData();
        }

        //Pre: None
        //Post: Total time played is changed into a string
        //Description: Subprogam to change total time played into a string
        public void TotalTimePlayedString()
        {
            //Variables for time
            int[] timeVarInt = new int[3];
            string[] timeVarStr = new string[3];

            //Calculating seconds, minutes, hours
            timeVarInt[0] = (int)(totalTimePlayed / 3600.0);
            timeVarInt[1] = (int)((totalTimePlayed - 3600 * Convert.ToInt32(timeVarInt[0])) / 60.0);
            timeVarInt[2] = (int)(totalTimePlayed % 60);

            //Converting to string
            for (int i = 0; i < 3; i++)
            {
                if (timeVarInt[i] < 9)
                {
                    timeVarStr[i] = "0" + timeVarInt[i];
                }
                else
                {
                    timeVarStr[i] = "" + timeVarInt[i];
                }
            }

            //Updating global instance
            totalTimePlayerStr = timeVarStr[0] + ":" + timeVarStr[1] + ":" + timeVarStr[2];
        }

        //Pre: None
        //Post: Slider tab and actual volume level is updated
        //Description: Subprogam to update slider tab and actual volume level
        public void SetActualVolume()
        {
            //Setting tab location and current volume all as a function of actual volume
            for(int i = 0; i < 2; i++)
            {
                sliderTabRect[i].X = (int)(actualVolume[i] * 560 + 85);
                currentVolume[i] = actualVolume[i];
            }
        }

        //Pre: 'newFrameRate', the new frame rate
        //Post: Frame rate is changed
        //Description: Subprogram to change frame rate
        public void SetFrameRate(float newFrameRate)
        {
            //Setting frame rate
            TargetElapsedTime = TimeSpan.FromSeconds(1.0f / newFrameRate);
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = false;
        }


        //Pre: 'testButtonNew' and 'testButtonOld', the button states of the new gamepad and old gamepad
        //Post: Returns a bool; true if button press is a new one, or else false
        //Description: Subprogram to determine if a button press is a new one
        public static bool NewButtonPress(ButtonState testButtonNew, ButtonState testButtonOld)
        {
            //Selection to determine if button press is a new one, return true if it is and play click sound effect
            if (testButtonNew == ButtonState.Pressed && testButtonOld != ButtonState.Pressed)
            {
                buttonClickSndInstance.Play();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Pre: 'testThumbStickNew' and 'testThumbStickOld', the thumbstick posistions of the new gamepad and old gamepad
        //Post: Returns a string that indicates whether theb movement was a new up, a new down or not new
        //Description: Subprogram to determine if thumbstick movement is a new one
        private string NewThumbstickMovement(Vector2 testThumbStickNew, Vector2 testThumbStickOld)
        {
            if (testThumbStickNew.Y > 0.0 && testThumbStickOld.Y <= 0.0)
            {
                return "UP";
            }
            else if (testThumbStickNew.Y < 0.0 && testThumbStickOld.Y >= 0.0)
            {
                return "DOWN";
            }
            else
            {
                return "";
            }
        }

        //Pre: 'testTriggerNew', and 'testTriggerOld', the trigger posistions of the new and old gamepad
        //Post: Returns a bool; true if trigger press is a new one, or else false
        //Description: Subprogram to determine if trigger press is a new one
        private bool NewTriggerPress(float testTriggerNew, float testTriggerOld)
        {
            //Selection to determine if trigger press is a new one, return true if it is and play click sound effect
            if (testTriggerNew > 0 && testTriggerOld <= 0)
            {
                buttonClickSndInstance.Play();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
