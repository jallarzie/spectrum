using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Spectrum.Components;

namespace Spectrum.States
{
    public class Tutorial : Library.States.State
    {
        public Tutorial(Library.States.State previousState)
        {
            mPreviousState = previousState;
            mSprite = new Library.Graphics.Sprite("tutorial");
            Application.Instance.Drawables.Add(mSprite);
        }

        public override void Initialize()
        {
            mIgnoreKeys = new Dictionary<Keys, bool>();
            mIgnoreButtons = new Dictionary<Buttons, bool>();

            KeyboardState keyboardState = Keyboard.GetState();
            foreach (Keys key in new List<Keys> { Keys.Space, Keys.Enter, Keys.Up, Keys.Down, Keys.W, Keys.S })
                mIgnoreKeys[key] = keyboardState.IsKeyDown(key);

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            foreach (Buttons button in new List<Buttons> { Buttons.A, Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.DPadDown, Buttons.LeftThumbstickDown })
                mIgnoreButtons[button] = gamePadState.IsButtonDown(button);
        }

        public override bool Transition()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (this.IsKeyDown(Keys.Enter) || this.IsKeyDown(Keys.Space) || this.IsKeyDown(Keys.Back) || this.IsButtonDown(Buttons.A))
                return Application.Instance.StateMachine.ChangeState( mPreviousState );

            return false;
        }

        public override void Destroy()
        {
            Application.Instance.Drawables.Remove(mSprite);
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateIgnoredKeys();
        }

        private void UpdateIgnoredKeys()
        {
            Dictionary<Keys, bool> newIgnoredKeys = new Dictionary<Keys, bool>();
            KeyboardState keyboardState = Keyboard.GetState();
            foreach (KeyValuePair<Keys, bool> status in mIgnoreKeys)
                newIgnoredKeys[status.Key] = mIgnoreKeys[status.Key] && keyboardState.IsKeyDown(status.Key);
            mIgnoreKeys = newIgnoredKeys;

            Dictionary<Buttons, bool> newIgnoredButtons = new Dictionary<Buttons, bool>();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            foreach (KeyValuePair<Buttons, bool> status in mIgnoreButtons)
                newIgnoredButtons[status.Key] = mIgnoreButtons[status.Key] && gamePadState.IsButtonDown(status.Key);
            mIgnoreButtons = newIgnoredButtons;
        }

        private bool IsKeyDown(Keys key)
        {
            if (mIgnoreKeys.ContainsKey(key) && mIgnoreKeys[key])
                return false;

            KeyboardState keyboardState = Keyboard.GetState();
            return keyboardState.IsKeyDown(key);
        }

        private bool IsButtonDown(Buttons button)
        {
            if (mIgnoreButtons.ContainsKey(button) && mIgnoreButtons[button])
                return false;

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            return gamePadState.IsButtonDown(button);
        }

        private Library.States.State mPreviousState;
        private Library.Graphics.Sprite mSprite;
        private Dictionary<Keys, bool> mIgnoreKeys;
        private Dictionary<Buttons, bool> mIgnoreButtons;
    }
}
