using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using Spectrum.Components;

namespace Spectrum.States
{
    public class Menu : Library.States.State
    {
        private class Action
        {
            public Action(string text, ActionTarget target, Library.Graphics.String label)
            {
                Text = text;
                Target = target;
                Label = label;
            }

            public string Text;
            public ActionTarget Target;
            public Library.Graphics.String Label;
        }

        public const int PADDING = 20;
        public const int MARGIN_HEADER = 40;
        public const int MARGIN_ACTION = 20;

        public const float SPEED_DISPLAY = 1f;

        public const float REPEAT_DELAY = .35f;

        public delegate Library.States.State ActionTarget();

        public Menu(Library.States.State previousState, string headerText, bool immediate)
        {
            mPreviousState = previousState;

            mHeaderText = headerText;

            mActions = new List<Action>();

            mOffsetX = 240f + (immediate ? 0 : 480f);

            mSelection = 0;

            mCurrentKeyCode = Keys.None;
        }

        public void AddAction(string name, ActionTarget target)
        {
            mActions.Add(new Action(name, target, null));
        }

        public override void Initialize()
        {
            mBackground = new Library.Graphics.Flat(new Vector2(Application.Instance.GraphicsDevice.Viewport.Width, Application.Instance.GraphicsDevice.Viewport.Height), Color.Black, null);
            mBackground.Layer = Layers.Menu;

            mIgnoreKeys = new Dictionary<Keys, bool>();
            mIgnoreButtons = new Dictionary<Buttons, bool>();

            KeyboardState keyboardState = Keyboard.GetState();
            foreach (Keys key in new List<Keys> { Keys.Space, Keys.Enter, Keys.Up, Keys.Down })
                mIgnoreKeys[key] = keyboardState.IsKeyDown(key);

            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            foreach (Buttons button in new List<Buttons> { Buttons.A, Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.DPadDown, Buttons.LeftThumbstickDown })
                mIgnoreButtons[button] = gamePadState.IsButtonDown(button);

            mBackground.Opacity = .75f;
            Application.Instance.Drawables.Add(mBackground);

            mHeader = new Library.Graphics.String("fonts/menus/headers", mHeaderText);
            mHeader.Layer = Layers.MenuText;
            Application.Instance.Drawables.Add(mHeader);

            foreach (Action action in mActions)
            {
                action.Label = new Library.Graphics.String("fonts/menus/actions", action.Text);
                action.Label.Layer = Layers.MenuText;
                Application.Instance.Drawables.Add(action.Label);
            }

            this.ApplyLayout();

            mSelection = -1;
            if (mActions.Count > 0)
            {
                this.Select(0);
            }
        }

        public override bool Transition()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (mOffsetX == Application.Instance.GraphicsDevice.Viewport.Width / 2 
                && (this.IsKeyDown(Keys.Enter) || this.IsKeyDown(Keys.Space) || this.IsButtonDown(Buttons.A))
                && mSelection != -1)
                return Application.Instance.StateMachine.ChangeState(mActions[mSelection].Target());

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            this.UpdateIgnoredKeys();
            this.UpdateAnimation(gameTime);
            this.UpdateSelection(gameTime);
        }

        public override void Destroy()
        {
            Application.Instance.Drawables.Remove(mBackground);
            Application.Instance.Drawables.Remove(mHeader);

            foreach (Action action in mActions)
                Application.Instance.Drawables.Remove(action.Label);

            if (mPreviousState != null)
            {
                mPreviousState.Destroy();
            }
        }

        public Library.States.State ReleasePreviousState()
        {
            Library.States.State state = mPreviousState;
            mPreviousState = null;
            return state;
        }

        private float ComputeHeight()
        {
            float header = mHeader.Height;

            if (mActions.Count == 0)
                return header;

            float actions = MARGIN_HEADER - MARGIN_ACTION;
            foreach (Action action in mActions)
                actions += action.Label.Height + MARGIN_ACTION;

            return header + actions;
        }

        private void ApplyLayout()
        {
            float height = this.ComputeHeight();

            mBackground.Height = (int)height + PADDING * 2;
            mBackground.Origin = new Vector2(mBackground.Width / 2, mBackground.Height / 2);
            mBackground.Position = new Vector2(Application.Instance.GraphicsDevice.Viewport.Width / 2, Application.Instance.GraphicsDevice.Viewport.Height / 2);

            float offsetY = mHeader.Height / 2;
            mHeader.Origin = new Vector2(mHeader.Width / 2, mHeader.Height / 2);
            mHeader.Position = new Vector2(Application.Instance.GraphicsDevice.Viewport.Width / 2, Application.Instance.GraphicsDevice.Viewport.Height / 2 - height / 2 + offsetY);

            offsetY += mHeader.Height + MARGIN_HEADER;

            foreach (Action action in mActions)
            {
                action.Label.Origin = new Vector2(action.Label.Width / 2, action.Label.Height / 2);
                action.Label.Position = new Vector2(Application.Instance.GraphicsDevice.Viewport.Width / 2, Application.Instance.GraphicsDevice.Viewport.Height / 2 - height / 2 + offsetY);
                offsetY += action.Label.Height + MARGIN_ACTION;
            }
        }

        private void Select(int id)
        {
            if (id < 0 || id >= mActions.Count)
                return;
            if (mSelection == id)
                return;

            this.Unselect();

            mActions[id].Label.Text = "< " + mActions[id].Text + " >";
            mActions[id].Label.Origin = new Vector2(mActions[id].Label.Width / 2, mActions[id].Label.Height / 2);
            mSelection = id;
        }

        private void Unselect()
        {
            if (mSelection == -1)
                return;

            mActions[mSelection].Label.Text = mActions[mSelection].Text;
            mActions[mSelection].Label.Origin = new Vector2(mActions[mSelection].Label.Width / 2, mActions[mSelection].Label.Height / 2);
            mSelection = -1;
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
                newIgnoredButtons[status.Key] = mIgnoreButtons[status.Key] && gamePadState.IsButtonUp(status.Key);
            mIgnoreButtons = newIgnoredButtons;
        }

        private void UpdateAnimation(GameTime gameTime)
        {
            mOffsetX -= (Application.Instance.GraphicsDevice.Viewport.Width / 2 + Application.Instance.GraphicsDevice.Viewport.Width) / SPEED_DISPLAY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (mOffsetX < Application.Instance.GraphicsDevice.Viewport.Width / 2)
                mOffsetX = Application.Instance.GraphicsDevice.Viewport.Width / 2;

            mBackground.Position = new Vector2(mOffsetX, mBackground.Position.Y);
            mHeader.Position = new Vector2(mOffsetX, mHeader.Position.Y);
            foreach (Action action in mActions)
                action.Label.Position = new Vector2(mOffsetX, action.Label.Position.Y);
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

        public void UpdateSelection(GameTime gameTime)
        {
            if (mOffsetX != Application.Instance.GraphicsDevice.Viewport.Width / 2)
                return;

            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (this.IsKeyDown(Keys.Down) ^ this.IsKeyDown(Keys.Up) ||
                this.IsButtonDown(Buttons.DPadDown) ^ this.IsButtonDown(Buttons.LeftThumbstickUp) ||
                this.IsButtonDown(Buttons.LeftThumbstickDown) ^ this.IsButtonDown(Buttons.DPadUp))
            {
                Keys key = this.IsKeyDown(Keys.Down) || this.IsButtonDown(Buttons.DPadDown) || this.IsButtonDown(Buttons.LeftThumbstickDown) ? Keys.Down : Keys.Up;
                if (mCurrentKeyCode == Keys.None || gameTime.TotalGameTime.TotalSeconds - mCurrentKeyTime >= REPEAT_DELAY)
                {
                    mCurrentKeyCode = key;
                    mCurrentKeyTime = gameTime.TotalGameTime.TotalSeconds;

                    SoundPlayer.PlayMenuItemSelectionChangeSound();

                    switch (key)
                    {
                        case Keys.Up:
                            this.Select(mSelection - 1);
                            break;

                        case Keys.Down:
                            this.Select(mSelection + 1);
                            break;
                    }
                }
            }
            else
            {
                mCurrentKeyCode = Keys.None;
            }
        }

        private Library.States.State mPreviousState;
        private Library.Graphics.Flat mBackground;
        private string mHeaderText;
        private Library.Graphics.String mHeader;
        private List<Action> mActions;
        private float mOffsetX;
        private Keys mCurrentKeyCode;
        private double mCurrentKeyTime;
        private int mSelection;
        private Dictionary<Keys, bool> mIgnoreKeys;
        private Dictionary<Buttons, bool> mIgnoreButtons;
    }
}
