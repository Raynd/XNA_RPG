using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame2
{
    class OptionsScreen : GameScreen
    {
        String message;

        InputAction menuAccept;
        InputAction menuCancel;
        Texture2D gradientTexture;
        int aiFlag;
        Game1 game;

        public OptionsScreen(Game1 game)
        {
            const string usageText = "Play Against AI?\n"
                                        + "A: Yes B: No";
            this.message = message + usageText;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

            menuAccept = new InputAction(
                new Buttons[] { Buttons.A },
                new Keys[] { Keys.A },
                true);
            menuCancel = new InputAction(
                new Buttons[] { Buttons.B },
                new Keys[] { Keys.B },
                true);

            this.game = game;
        }


        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                ContentManager content = ScreenManager.Game.Content;
                gradientTexture = content.Load<Texture2D>("Popup_Background");
            }
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            PlayerIndex playerIndex;

            if (menuAccept.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                aiFlag = 1;
                game.SetAiFlag(aiFlag);

                OpponentAI ai = new OpponentAI(game);
                game.Components.Add(ai);
                ai.LoadCursor(game);

                game.Services.AddService(typeof(OpponentAI), ai);
                PlayerManager tempRef = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
                tempRef.LoadAI(game);

                ExitScreen();
            }
            if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                aiFlag = 0;
                game.SetAiFlag(aiFlag);
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Darken down any other screens that were drawn beneath the popup.
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            // Center the message text in the viewport.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();

            // Draw the background rectangle.
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, Color.Black);

            spriteBatch.End();
        }
    }
}
