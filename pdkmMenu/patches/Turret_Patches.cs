using HarmonyLib;

[HarmonyPatch(typeof(Turret))]
internal class Turret_Patches
{
    [HarmonyPatch("Start")]
    [HarmonyPostfix]
    public static void Start_Postfix(Turret __instance)
    {
        if (__instance == null) return;
        __instance.gameObject.AddComponent<Turret_Label>();
    }
}