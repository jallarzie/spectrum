using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spectrum.Library.Graphics;
using Spectrum.Library.States;

namespace Spectrum
{
    class Application : Microsoft.Xna.Framework.Game
    {
        private const double SPECIAL_INPUT_DELAY = 500; //time between acknowledgement of special inputs in miliseconds

        private static Application sInstance;

        public static Application Instance
        {
            get
            {
                return sInstance;
            }
        }

        #if WINDOWS || XBOX
            static void Main(string[] args)
            {
                if (sInstance != null)
                    return;

                using (sInstance = new Application())
                {
                    sInstance.Run();
                }
            }
        #endif

        public Application()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this);
            GraphicsDeviceManager.PreferredBackBufferWidth = 1280;
            GraphicsDeviceManager.PreferredBackBufferHeight = 720;
            
            Content.RootDirectory = "Content";
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
            UpdateSpecialInputs(gameTime);
            StateMachine.Update(gameTime);
        }

        private void UpdateSpecialInputs(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            specialInputTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (specialInputTime <= 0)
            {
                if ((keyboardState.IsKeyDown(Keys.LeftAlt) || keyboardState.IsKeyDown(Keys.RightAlt)) && keyboardState.IsKeyDown(Keys.Enter))
                {
                    GraphicsDeviceManager.ToggleFullScreen();
                    specialInputTime = SPECIAL_INPUT_DELAY;
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            Drawables.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        private GraphicsDeviceManager GraphicsDeviceManager;
        private SpriteBatch SpriteBatch;

        private double specialInputTime;

        public StateMachine StateMachine;
        public DisplayList Drawables;
    }
}
