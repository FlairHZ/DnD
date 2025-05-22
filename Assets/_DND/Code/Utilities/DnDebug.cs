using System;
using System.Collections.Generic;
using System.Text;
using DnDConfig;

namespace FlairsCards.Utilities
{
    internal class DnDDebug
    {
        public static void Log(object message)
        {
            if (Config.isDebugBuild)
            {
                UnityEngine.Debug.Log(message);
            }

        }
    }
}