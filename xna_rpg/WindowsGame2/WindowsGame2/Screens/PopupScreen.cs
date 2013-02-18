using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame2
{
    class PopupScreen : GameScreen
    {

        string message = "";
        Texture2D gradientTexture;

        InputAction menuMove;
        InputAction menuAttack;
        InputAction menuCancel;
        InputAction menuEndTurn;
        InputAction menuBuy;
        Cursor cursor;
        Character selectedChar;
        PlayerManager playerManager;
        GameScreen selectScreen;
        ScreenManager screenManager;
        GameStateManager gameStateManager;
        BattleMap map;

        public PopupScreen(Game game, Character character)
        {
            cursor = (Cursor)game.Services.GetService(typeof(Cursor));
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));

            selectedChar = character;
            selectScreen = new SelectScreen(game, character);
            screenManager.AddScreen(selectScreen, null);

            const string usageText = "A: Move" + "\nX: Attack" + "\nStart: End Turn" + "\nB: Cancel"; 
            this.message = message + usageText;
            if (selectedChar == null)
            {
                this.message = "Start: End Turn" + "\nB: Cancel";
            }
            else if (selectedChar.CharType == "champion")
            {
                this.message += "\nY: Buy Mercenary";
            }
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);

                menuMove = new InputAction(
                    new Buttons[] { Buttons.A },
                    new Keys[] { Keys.A },
                    true);
                menuAttack = new InputAction(
                    new Buttons[] { Buttons.X, },
                    new Keys[] { Keys.X },
                    true);
                menuCancel = new InputAction(
                    new Buttons[] { Buttons.B, },
                    new Keys[] { Keys.B },
                    true);
                menuEndTurn = new InputAction(
                    new Buttons[] { Buttons.Start, },
                    new Keys[] { Keys.Enter },
                    true);
                menuBuy = new InputAction(
                    new Buttons[] { Buttons.Y, },
                    new Keys[] { Keys.Y },
                    true);
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
            if (menuMove.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (selectedChar == null || selectedChar.HasMoved == true)
                {
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
                else
                {
                    cursor.MoveHelper(selectedChar);
                    selectedChar.HasMoved = true;
                    ExitScreen();
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
            }
            else if (menuAttack.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (selectedChar == null || selectedChar.HasAttacked == true)
                {
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
                else
                {
                    cursor.AttackHelper(selectedChar);
                    selectedChar.HasAttacked = true;
                    ExitScreen();
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
            }

            else if (menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                ExitScreen();
                selectScreen.ExitScreen();
                screenManager.RemoveScreen(selectScreen);
            }

            else if (menuEndTurn.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                playerManager.EndTurn();
                ExitScreen();
                selectScreen.ExitScreen();
                screenManager.RemoveScreen(selectScreen);
            }
            else if (menuBuy.Evaluate(input, ControllingPlayer, out playerIndex))
            {
                if (selectedChar == null || selectedChar.CharType != "champion")
                {
                    gameStateManager.State = GameState.playing;
                    ExitScreen();
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
                else
                {
                    selectedChar.BuyMercenary();
                    ExitScreen();
                    selectScreen.ExitScreen();
                    screenManager.RemoveScreen(selectScreen);
                }
            }
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
            textPosition.X = (map.getWidth() * 60 + 5);
            textPosition.Y = 60*4;

            // The background includes a border somewhat larger than the text itself.
            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle(map.getWidth() * 60,
                                                          60*4,
                                                          (int)textSize.X + 10,
                                                          (int)textSize.Y + vPad);

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
