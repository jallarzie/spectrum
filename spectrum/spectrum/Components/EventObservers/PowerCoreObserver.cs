using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum.Components.EventObservers
{
    /// <summary>
    /// Observer Interface that responds to Power Core Events.
    /// </summary>
    public interface PowerCoreObserver
    {
        /// <summary>
        /// Executes When the PowerCore reaches zero
        /// </summary>
        void OnPowerCoreHealthReachedZero();

        /// <summary>
        /// Executes when the PowerCore's health is reduced.
        /// </summary>
        /// <param name="Damage">The Amount of HP lost by the PowerCore</param>
        void OnPowerCoreHealthReduced(int Damage);
    }
}
