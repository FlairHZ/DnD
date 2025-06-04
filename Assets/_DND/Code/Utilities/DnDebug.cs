using System;
using System.Collections.Generic;
using System.Text;
using DnDConfig;

namespace DnD.Utilities
{
    internal class DnDebug
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