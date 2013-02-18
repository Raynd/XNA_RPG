using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame2
{
    class MercenaryCamp : Character
    {
        SpriteBatch spriteBatch;
        ContentManager content;
        BattleMap map;
        GameStateManager gameStateManager;
        Texture2D camp;

        public MercenaryCamp(Game1 game, int xLoc, int yLoc)
            : base(game, xLoc, yLoc)
        {
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            map.GetSquare(yLoc, xLoc).setCurrentChar(this);

            Initialize();
            LoadContent();

            position.X = xLoc * 60;
            position.Y = yLoc * 60 + 20;
            Alive = true;
            HasAttacked = false;
            HasMoved = false;
            CharType = "camp";
            HealthPoints = 100;
        }

        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            content = GetContentManager();
            content.RootDirectory = "Content\\MercenaryCamp";
            spriteFont = content.Load<SpriteFont>(@"..\\Arial");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            camp = content.Load<Texture2D>("camp");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameStateManager.State == GameState.playing)
            {
                if (Alive == true)
                {
                    spriteBatch.Draw(camp, position, Color.White);
                }
            }

            spriteBatch.End();
        }
    }
}
