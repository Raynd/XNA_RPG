using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace WindowsGame2
{
    class EndingScreen : MenuScreen
    {
        Character character;
        protected SpriteFont spriteFont;
        Game game;
        ScreenManager screenManager;
        GameStateManager gameStateManager;
        InputAction exit;


        public EndingScreen(Game game)
            : base("")
        {
            this.game = game;

            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            MenuEntry exit = new MenuEntry("Exit Game");

            exit.Selected += ExitSelected;

            MenuEntries.Add(exit);
        }




        public void ExitSelected(object sender, PlayerIndexEventArgs e)
        {
            screenManager.Game.Exit();
        }

    }
}
