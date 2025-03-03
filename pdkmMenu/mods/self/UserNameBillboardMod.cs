using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdkmMenu
{
    public class UserNameBillboardMod
    {
        public static void UpdateMod()
        {
            if(!Plugin.SelfSettings.AlwaysShowUserNames.Value) return;
            foreach (PlayerControllerB player in Plugin.GetRealPlayerScripts())
            {
                player.ShowNameBillboard();
            }
        }
    }
}
