using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spectrum.Library.Graphics;
using Spectrum.Library.States;

namespace Spectrum
{
    class Application : Microsoft.Xna.Framework.Game
    {
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            StateMachine = new StateMachine();
            Drawables = new DisplayList();

            StateMachine.Initialize(new States.Game());
        }

        protected override void Update(GameTime gameTime)
        {
            StateMachine.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();
            Drawables.Draw(gameTime, SpriteBatch);
            SpriteBatch.End();
        }

        private GraphicsDeviceManager GraphicsDeviceManager;
        private SpriteBatch SpriteBatch;

        public StateMachine StateMachine;
        public DisplayList Drawables;
    }
}
