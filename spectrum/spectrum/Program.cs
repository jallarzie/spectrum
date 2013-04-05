using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spectrum
{
    public static class Program
    {
#if WINDOWS || XBOX
        public static void Main(string[] args)
        {
            using (Application.Instance)
            {
                Application.Instance.Run();
            }
        }
#endif
    }
}
