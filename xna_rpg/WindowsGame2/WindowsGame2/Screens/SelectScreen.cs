using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame2
{
    class SelectScreen : GameScreen
    {
        PlayerManager playerManager;
        Character selectedChar;
        string message = "";
        Texture2D gradientTexture;
        BattleMap map;

        public SelectScreen(Game game, Character character)
        {
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            selectedChar = character;

            const string usageText = "";
            this.message = message + usageText;
            if (selectedChar == null)
            {
                //this.message = "";
            }
            else
            {
                this.message += "LVL: " + selectedChar.Level + " "
                                + "HP: " + selectedChar.CurrentHealth + "/" + selectedChar.HealthPoints + " "
                                + "STR: " + selectedChar.Strength + " "
                                + "DEX: " + selectedChar.Dexterity + " "
                                + "INT: " + selectedChar.Intelligence + "\n"
                                + "PDEF: " + selectedChar.PDefense + " "
                                + "MDEF: " + selectedChar.MDefense + " "
                                + "EXP: " + selectedChar.Experience;
                if (selectedChar.CharType == "champion") message += " Gold: " + selectedChar.Gold;
            }
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                ContentManager content = ScreenManager.Game.Content;
                gradientTexture = content.Load<Texture2D>("Popup_Background");
            }
        }


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
        }


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            //ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition;
            textPosition.X = 2;
            textPosition.Y = map.getHeight()*60+30;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle(0,
                                                          map.getHeight()*60+30,
                                                          (int)textSize.X +10,
                                                          (int)textSize.Y);

            // Fade the popup alpha during transitions.
            Color color = Color.White;// *TransitionAlpha;

            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, Color.Black);

            spriteBatch.End();
        }
    }
}
