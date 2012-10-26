using Microsoft.Xna.Framework;
using System;

namespace Spectrum.Library.States
{
    public class StateMachine
    {
        public StateMachine()
        {
        }

        public void Initialize(State initialState)
        {
            State = initialState;
            this.Initialize();
        }

        public State Take()
        {
            State state = State;
            State = null;
            return state;
        }

        public void Update(GameTime gameTime)
        {
            State.Update(gameTime);
            if (this.Transition( )) {
                GameTime newGameTime = new GameTime(gameTime.TotalGameTime, new TimeSpan(0));
                this.Update(gameTime);
            }
        }

        private void Initialize()
        {
            State.Initialize( );
            this.Transition( );
        }

        private bool Transition()
        {
            State newState = State.Transition();

            if (newState == null)
                return false;

            State = newState;
            this.Initialize();

            return true;
        }

        private void Destroy()
        {
            State.Destroy();
        }

        private State State;
    }
}
