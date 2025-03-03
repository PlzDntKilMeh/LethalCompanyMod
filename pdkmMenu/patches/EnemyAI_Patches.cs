using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[HarmonyPatch(typeof(EnemyAI))]
internal class EnemyAI_Patches
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    public static void Start_Postfix(EnemyAI __instance)
    {
        if (__instance == null) return;
        __instance.gameObject.AddComponent<EnemyAI_Label>();
    }
}