using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spectrum.Library.Graphics;
using Spectrum.Library.States;
using Spectrum.Components;

namespace Spectrum
{
    public class Application : Microsoft.Xna.Framework.Game
    {
        private const double SPECIAL_INPUT_DELAY = 500; //time between acknowledgement of special inputs in miliseconds

        private static Application sInstance;

        public static Application Instance
        {
            get
            {
                if (sInstance == null) 
                {
                    sInstance = new Application();
                }
                return sInstance;
            }
        }

        public Application()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE
            sInstance = this;
#endif
        }

        protected override void Initialize()
        {
            IsMouseVisible = false;

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            StateMachine = new StateMachine();
            Drawables = new DisplayList();

            StateMachine.Initialize(new States.Start());
        }

        protected override void Update(GameTime gameTime)
        {
            InputController.Instance.Update();
            UpdateSpecialInputs(gameTime);
            StateMachine.Update(gameTime);
        }

        private void UpdateSpecialInputs(GameTime gameTime)
        {
            if (InputController.Instance.HasCalledFullscreenToggle())
                GraphicsDeviceManager.ToggleFullScreen();
        }

        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            Drawables.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        public GraphicsDeviceManager GraphicsDeviceManager;
        private SpriteBatch SpriteBatch;

        public StateMachine StateMachine;
        public DisplayList Drawables;
    }
}
