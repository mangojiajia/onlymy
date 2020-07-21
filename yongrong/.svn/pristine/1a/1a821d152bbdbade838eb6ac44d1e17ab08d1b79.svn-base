using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.Os
{
    public static class Osys
    {
        public static bool IsWin
        {
            get
            {
                System.OperatingSystem osInfo = System.Environment.OSVersion;
                System.PlatformID platformID = osInfo.Platform;

                if (platformID.ToString().Contains("Win"))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
