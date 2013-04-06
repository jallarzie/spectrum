using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#if WINDOWS_PHONE
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace Spectrum.Components
{
    public enum ControlType { Keyboard, GamePad, TouchScreen }

    public class InputController
    {
        private const int MAX_PLAYERS = 4;
        private static InputController instance;

        public static InputController Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputController();

                return instance;
            }
        }
        
        private KeyboardState keyboardState;
        private MouseState mouseState;
        private GamePadState[] gamePadStates;

#if WINDOWS_PHONE
        private TouchCollection TouchLocationState;
        private PhoneThumbsticController LeftAreaPhoneInputListener;
        private PhoneThumbsticController RightAreaPhoneInputListener;       
#endif

        private InputController()
        {
            gamePadStates = new GamePadState[MAX_PLAYERS];
            for (int i = 0; i < MAX_PLAYERS; ++i)
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.Circular);

#if WINDOWS_PHONE
            LeftAreaPhoneInputListener = new PhoneThumbsticController(new Rectangle(
                0,
                0,
                Application.Instance.GraphicsDevice.Viewport.Width / 2,
                Application.Instance.GraphicsDevice.Viewport.Height)
            );
            RightAreaPhoneInputListener = new PhoneThumbsticController(new Rectangle(
                Application.Instance.GraphicsDevice.Viewport.Width / 2,
                0,
                Application.Instance.GraphicsDevice.Viewport.Width / 2,
                Application.Instance.GraphicsDevice.Viewport.Height)
            );
#endif

        }

        public void Update()
        {

#if WINDOWS_PHONE
            TouchLocationState = TouchPanel.GetState();
            RightAreaPhoneInputListener.Update(TouchLocationState);
            LeftAreaPhoneInputListener.Update(TouchLocationState);
#else
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            for (int i = 0; i < MAX_PLAYERS; ++i)
                gamePadStates[i] = GamePad.GetState((PlayerIndex)i, GamePadDeadZone.Circular);
#endif

        }

        public ControlType GetControlType(PlayerIndex player)
        {
            if (player != PlayerIndex.One)
                return ControlType.GamePad;

            if (gamePadStates[(int)player].IsConnected)
                return ControlType.GamePad;
#if WINDOWS_PHONE
            return ControlType.TouchScreen;
#else
            return ControlType.Keyboard;
#endif
        }

        public Vector2 GetShootingDirection(Ship player)
        {
#if WINDOWS_PHONE
            return RightAreaPhoneInputListener.GetMovingDirection();
#else
            Vector2 direction = Vector2.Zero;

            if (GetControlType(player.PlayerIndex) == ControlType.Keyboard)
            {
                int mouseX = (int)MathHelper.Clamp(mouseState.X, 1, Application.Instance.GraphicsDevice.Viewport.Width - 1);
                int mouseY = (int)MathHelper.Clamp(mouseState.Y, 1, Application.Instance.GraphicsDevice.Viewport.Height - 1);

                Mouse.SetPosition(mouseX, mouseY);

                direction = new Vector2(mouseX, mouseY) - player.Position;
            }
            else
            {
                direction = gamePadStates[(int)player.PlayerIndex].ThumbSticks.Right;
                direction.Y *= -1;
            }

            return direction;
#endif



        }

        public Vector2 GetMovingDirection(PlayerIndex player)
        {
           

#if WINDOWS_PHONE
            return LeftAreaPhoneInputListener.GetMovingDirection();
#else
             Vector2 direction = Vector2.Zero;
                        if (GetControlType(player) == ControlType.Keyboard)
            {
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Q))
                    direction.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    direction.X += 1;
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Z))
                    direction.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    direction.Y += 1;
            }
            else
            {
                direction = gamePadStates[(int)player].ThumbSticks.Left;

                if (direction != Vector2.Zero)
                {
                    direction.Y *= -1;
                }
                else
                {
                    if (gamePadStates[(int)player].IsButtonDown(Buttons.DPadLeft))
                        direction.X -= 1;
                    if (gamePadStates[(int)player].IsButtonDown(Buttons.DPadRight))
                        direction.X += 1;
                    if (gamePadStates[(int)player].IsButtonDown(Buttons.DPadUp))
                        direction.Y -= 1;
                    if (gamePadStates[(int)player].IsButtonDown(Buttons.DPadDown))
                        direction.Y += 1;
                }
            }

            return direction;
#endif


        }

        public bool IsCharging(PlayerIndex player)
        {
            if (GetControlType(player) == ControlType.GamePad)
                return gamePadStates[(int)player].Triggers.Right != 0;
            else
                return mouseState.LeftButton == ButtonState.Pressed;
        }

        public bool HasCalledMenu(PlayerIndex player)
        {
            return keyboardState.IsKeyDown(Keys.Escape) || gamePadStates[(int)player].IsButtonDown(Buttons.Start);
        }

        public bool HasCalledFullscreenToggle()
        {
            return (keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)) && keyboardState.IsKeyDown(Keys.Enter);
        }

        public int GetNumberOfAvailableInputs()
        {
            for (int i = 0; i < MAX_PLAYERS; ++i)
            {
                if (!gamePadStates[i].IsConnected)
                    return i;
            }

            return MAX_PLAYERS;
        }

        public void TurnOffVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Three, 0.0f, 0.0f);
            GamePad.SetVibration(PlayerIndex.Four, 0.0f, 0.0f);
        }
    }
}
