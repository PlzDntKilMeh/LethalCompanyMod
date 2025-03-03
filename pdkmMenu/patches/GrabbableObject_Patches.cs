using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch(typeof(GrabbableObject))]
internal class GrabbableObject_Patches
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    public static void Start_Postfix(GrabbableObject __instance)
    {
        if (__instance == null) return;
        __instance.gameObject.AddComponent<Grabbable_Label>();
    }
}