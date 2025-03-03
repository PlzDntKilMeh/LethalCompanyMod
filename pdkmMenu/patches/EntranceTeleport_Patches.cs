using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch(typeof(EntranceTeleport))]
internal class EntranceTeleport_Patches
{
    [HarmonyPatch("Awake")]
    [HarmonyPostfix]
    public static void Awake_Postfix(EntranceTeleport __instance)
    {
        if (__instance == null) return;
        __instance.gameObject.AddComponent<Entrace_Label>();
    }
}