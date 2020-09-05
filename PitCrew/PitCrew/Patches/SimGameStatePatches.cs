using BattleTech;
using BattleTech.Data;
using Harmony;
using SVGImporter;
using System;

namespace PitCrew.Patches
{
    [HarmonyPatch(typeof(SimGameState), "_OnInit")]
    static class SimGameState__OnInit
    {
        static void Postfix(SimGameState __instance, GameInstance game, SimGameDifficulty difficulty)
        {
            if (Mod.Log.IsTrace) Mod.Log.Trace?.Write("SGS:_OI entered.");

            DataManager dm = UnityGameInstance.BattleTechGame.DataManager;
            LoadRequest loadRequest = dm.CreateLoadRequest();

            // Need to load each unique icon
            Mod.Log.Info?.Write("-- Loading HUD icons");
            
            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.MechWarriorButton, null);
            
            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.MechTechButton, null);
            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.MechTechPortait, null);

            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.MedTechButton, null);
            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.MedTechPortrait, null);

            loadRequest.AddLoadRequest<SVGAsset>(BattleTechResourceType.SVGAsset, Mod.Config.Icons.GroupPortrait, null);

            loadRequest.ProcessRequests();
            Mod.Log.Info?.Write("--  Done!");

            ModState.SimGameState = __instance;
        }
    }

    [HarmonyPatch(typeof(SimGameState), "OnDayPassed")]
    static class SimGameState_OnDayPassed
    {
        static void Postfix(SimGameState __instance, int timeLapse)
        {
            Mod.Log.Debug?.Write($"OnDayPassed called with timeLapse: {timeLapse}");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "UpdateMechLabWorkQueue")]
    static class SimGameState_UpdateMechLabWorkQueue
    {
        static void Postfix(SimGameState __instance, bool passDay)
        {
            Mod.Log.Info?.Write($"Paying mechTechSkill: {__instance.MechTechSkill} against queue of size: {__instance.MechLabQueue.Count}");
        }
    }

    [HarmonyPatch(typeof(SimGameState), "CreateMechArmorModifyWorkOrder")]
    public static class SimGameState_CreateMechArmorModifyWorkOrder
    {
        public static void Postfix(SimGameState __instance, string mechSimGameUID, ChassisLocations location,
            int armorDiff, int frontArmor, int rearArmor, ref WorkOrderEntry_ModifyMechArmor __result)
        {
            Mod.Log.Trace?.Write($"SGS::CMAMWO entered");

            var techCost = __instance.Constants.MechLab.ArmorInstallTechPoints * armorDiff;
            var cbillCost = __instance.Constants.MechLab.ArmorInstallCost * armorDiff;
            Mod.Log.Info?.Write($"Creating MechArmor work order with " +
                $"techCost: {techCost} from armorInstallTechPoints: {__instance.Constants.MechLab.ArmorInstallTechPoints} x armorDiff: {armorDiff} and " +
                $"cbillsCost: {cbillCost} from armorInstallTechPoints: {__instance.Constants.MechLab.ArmorInstallCost} x armorDiff: {armorDiff}"
                );

            int armorPoints = Math.Abs(armorDiff);
            __result = new WorkOrderEntry_ModifyMechArmor(__result.ID, __result.Description, mechSimGameUID,
                  techCost, __result.Location, __result.DesiredFrontArmor, __result.DesiredRearArmor,
                  cbillCost, string.Empty);
        }
    }
}
