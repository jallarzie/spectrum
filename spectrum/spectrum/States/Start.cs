using System;
using System.Collections.Generic;
using Spectrum.Components;

namespace Spectrum.States
{
    public class Start : States.Menu
    {
        public Start()
            : base(new States.Game(), "SPECTRUM", true)
        {
            this.AddAction("new game", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return this.ReleasePreviousState();
            });

            #if WINDOWS
                this.AddAction("switch fullscreen", delegate () {
                    SoundPlayer.PlayMenuItemSelectionClickedSound();
                    SoundPlayer.IncreaseMainGameSoundVolume();

                    Application.Instance.GraphicsDeviceManager.ToggleFullScreen();

                    return null;
                });
            #endif

            this.AddAction("exit", delegate()
            {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Exit();
            });
        }
    }
}
