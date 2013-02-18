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

namespace WindowsGame2
{
    public enum GameState { mainMenu, playing, combat, combatOver, buying, gameOver};
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        int victoryFlag = 0;
        private SpriteBatch spriteBatch;
        protected SpriteFont spriteFont;
        private ScreenManager screenManager;
        private Texture2D grass;
        private Texture2D background;
        private Texture2D dirt;
        private Texture2D square;
        private Texture2D buyScreenBackground;
        private Texture2D victoryBackground;
        private BattleMap map;
        private int currentPlayer;
        private PlayerManager playerManager;
        private int aiFlag;
        SoundEffect victorySong;
        SoundEffect bgm;
        SoundEffectInstance bgmInstance;
        Vector2 turnDisplay;
        List<Character> charList = new List<Character>();
        GameStateManager gameStateManager;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            Services.AddService(typeof(List<Character>), charList);
            gameStateManager = new GameStateManager();
            gameStateManager.State = GameState.mainMenu;
            Services.AddService(typeof(GameStateManager), gameStateManager);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            Services.AddService(typeof(ScreenManager), screenManager);

            screenManager.AddScreen(new OptionsScreen(this), null);

            playerManager = new PlayerManager(this);
            Services.AddService(typeof(PlayerManager), playerManager);
            

            AddInitialScreens();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720; 

        }

        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(this), null);
        }
           
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);
            spriteFont = Content.Load<SpriteFont>(@"Arial");

            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            grass = Content.Load<Texture2D>("grass");
            dirt = Content.Load<Texture2D>("dirt");
            square = Content.Load<Texture2D>("empty_square");
            buyScreenBackground = Content.Load<Texture2D>("buyBackground");
            background = Content.Load<Texture2D>("empire");
            victoryBackground = Content.Load<Texture2D>("victory");
            victorySong = Content.Load<SoundEffect>("FF7_victory");
            bgm = Content.Load<SoundEffect>("guiles_theme");
            bgmInstance = bgm.CreateInstance();
            bgmInstance.IsLooped = true;
            bgmInstance.Play();
           
            map = new BattleMap(this, 10,10);
            map.RandomMap();
            Services.AddService(typeof(BattleMap), map);

            MercenaryCamp camp = new MercenaryCamp(this, 6,1);
            MercenaryCamp camp2 = new MercenaryCamp(this, 2, 8);
            Character champ = new ChampionOne(this, 2, 1, 1);
            Character champ2 = new ChampionOne(this, 5, 8, 2);
            Cursor cursor = new Cursor(this);

            Components.Add(camp);
            Components.Add(camp2);
            Components.Add(champ);
            Components.Add(champ2);
            Components.Add(cursor);
            charList.Add(champ);
            charList.Add(champ2);
            charList.Add(camp);
            charList.Add(camp2);

            Services.AddService(typeof(Cursor), cursor);

            /*OpponentAI  ai= new OpponentAI(this);
            Components.Add(ai);
            ai.LoadCursor(this);

            Services.AddService(typeof(OpponentAI), ai);
            PlayerManager tempRef = (PlayerManager)Services.GetService(typeof(PlayerManager));
            tempRef.LoadAI(this);*/

            turnDisplay = new Vector2(map.getWidth() * 60, 50);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);

            PlaySound();
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            
            if (gameStateManager.State == GameState.buying)
            {

                spriteBatch.Draw(buyScreenBackground, new Vector2(0, 0), Color.White);
            }

            if (gameStateManager.State == GameState.gameOver)
            {
                spriteBatch.Draw(victoryBackground, new Vector2(0, 0), Color.White);
                screenManager.AddScreen(new EndingScreen(this), null);
                spriteBatch.DrawString(spriteFont, "This day belongs to player " + playerManager.GetCurrentPlayer() + "!\nAll the spoils belong to the victor!\nPress A to exit.", new Vector2(500, 300), Color.White);
            }

            if (gameStateManager.State == GameState.playing)
            {

                GraphicsDevice.Clear(Color.CornflowerBlue);

                spriteBatch.Draw(background, new Vector2(0,0), Color.White);

                spriteBatch.DrawString(spriteFont, "Current Player: Player " + playerManager.GetCurrentPlayer(), turnDisplay, Color.White);

                Vector2 pos = new Vector2(0, 20);

                for (int i = 0; i < map.getHeight(); i++)
                {
                    for (int j = 0; j < map.getWidth(); j++)
                    {
                        if (map.GetSquare(i, j).getTerrainType() == "dirt")
                        {
                            spriteBatch.Draw(dirt, pos, Color.White);
                        }
                        if (map.GetSquare(i, j).getTerrainType() == "grass")
                        {
                            spriteBatch.Draw(grass, pos, Color.White);
                        }
                        if (map.GetSquare(i, j).isTileSelected())
                        {
                            spriteBatch.Draw(square, pos, new Color(0, 0, 100, 80));
                        }
                        pos.X += 60;
                    }
                    pos.X = 0;
                    pos.Y += 60;
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetAiFlag(int flag)
        {
            aiFlag = flag;
        }
        public int GetAiFlag()
        {
            return aiFlag;
        }

        
        void PlaySound()
        {
            
            if (gameStateManager.State == GameState.gameOver)
            {
                bgmInstance.Stop();
                if (victoryFlag == 0)
                {
                    victorySong.Play();
                    victoryFlag = 1;
                }
            }
        }
    }
}
