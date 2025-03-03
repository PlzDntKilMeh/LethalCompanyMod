using HarmonyLib;

[HarmonyPatch(typeof(Landmine))]
internal class Landmine_Patches
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    public static void Start_Postfix(Landmine __instance)
    {
        if (__instance == null) return;
        __instance.gameObject.AddComponent<Landmine_Label>();
    }
}