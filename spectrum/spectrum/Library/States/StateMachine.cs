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
            mState = initialState;
            this.Initialize();
        }

        public State Take()
        {
            State state = mState;
            mState = null;
            return state;
        }

        public bool SetState(State state)
        {
            mState = state;
            this.Initialize();
            return true;
        }

        public bool ChangeState(State state)
        {
            this.Destroy();
            this.SetState(state);
            return true;
        }

        public void Update(GameTime gameTime)
        {
            mTotalTime = gameTime.TotalGameTime;

            if (!this.Transition())
            {
                mState.Update(gameTime);
            }
        }

        private void Initialize()
        {
            mState.Initialize();

            this.Update(new GameTime(mTotalTime, new TimeSpan(0)));
        }

        private void Destroy()
        {
            mState.Destroy();
        }

        private bool Transition()
        {
            return mState.Transition();
        }

        private TimeSpan mTotalTime;
        private State mState;
    }
}
