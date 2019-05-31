using BattleTech;
using BattleTech.UI;
using Harmony;

namespace PitCrew {

    [HarmonyPatch(typeof(SimGameState), "UpdateMechLabWorkQueue", MethodType.Normal)]
    public static class SimGameState_UpdateMechLabWorkQueue {

        private static void Postfix(SimGameState __instance, bool passDay) {
            Mod.Log.Debug("SGS:UMLWQ entered.");

        }
    }   
}
