using Spectrum.Library.States;

namespace Spectrum.States
{
    public class Exit : State
    {
        public override void Initialize()
        {
            Application.Instance.Exit();
        }
    }
}
