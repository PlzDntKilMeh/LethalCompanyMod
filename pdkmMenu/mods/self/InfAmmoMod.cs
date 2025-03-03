using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdkmMenu
{
    public class InfAmmoMod
    {
        public static void UpdateMod()
        {
            if (!Plugin.SelfSettings.InfAmmo.Value) return;
            if (GameNetworkManager.Instance.localPlayerController == null) return;
            if (GameNetworkManager.Instance.localPlayerController.ItemSlots == null) return;

            GrabbableObject[] slots = GameNetworkManager.Instance.localPlayerController.ItemSlots;
            foreach (GrabbableObject item in slots)
            {
                if (item != null)
                {
                    if (item.GetComponent<ShotgunItem>() != null)
                    {
                        item.GetComponent<ShotgunItem>().shellsLoaded = 2;
                    }
                }
            }
        }

    }
}