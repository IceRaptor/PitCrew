using BattleTech;
using BattleTech.UI;
using Harmony;
using PitCrew.Helper;
using System;

namespace PitCrew.Patches
{



    [HarmonyPatch(typeof(WorkOrderEntry), "GetCost")]
    public static class WorkOrderEntry_GetCost
    {
        public static void Postfix(WorkOrderEntry __instance)
        {
            Mod.Log.Trace?.Write("WOE:GC entered.");

            // Prevent recursion across all layers of the WO graph
            if (__instance.Parent != null) { return; }

            string mechSimGameUID = null;
            int armorPointsToModify = 0;
            if (__instance.Type == WorkOrderType.MechLabModifyArmor)
            {
                Traverse costT = Traverse.Create(__instance).Field("Cost");
                armorPointsToModify += costT.GetValue<int>();
                Mod.Log.Debug?.Write($"Adding self cost: {costT.GetValue<int>()}");
                mechSimGameUID = (__instance as WorkOrderEntry_ModifyMechArmor).MechID;
            }
            foreach (WorkOrderEntry woe in __instance.SubEntries)
            {
                if (woe.Type == WorkOrderType.MechLabModifyArmor)
                {
                    Traverse costT = Traverse.Create(woe).Field("Cost");
                    armorPointsToModify += costT.GetValue<int>();
                    Mod.Log.Debug?.Write($"Adding child cost: {costT.GetValue<int>()}");
                    mechSimGameUID = (__instance as WorkOrderEntry_ModifyMechArmor).MechID;
                }
            }
            Mod.Log.Debug?.Write($"Total armor points to change is: {armorPointsToModify}");

            SimGameState sgs = UnityGameInstance.BattleTechGame.Simulation;
            foreach (MechDef mechDef in sgs.ActiveMechs.Values)
            {
                if (mechDef.GUID == mechSimGameUID)
                {
                    Mod.Log.Debug?.Write($"Calculating ArmorRepair for mechDef: {mechDef.Name} variant: {mechDef.Chassis.VariantName} " +
                        $"for totalArmor:{armorPointsToModify}");

                    RepairCalculator.ArmorRepair(mechDef, armorPointsToModify, out double techPoints, out double cBills);
                    Mod.Log.Debug?.Write($"  CBcost: {cBills}  TPcost: {techPoints}");



                }
            }

        }
    }

    [HarmonyPatch(typeof(WorkOrderEntry), "GetRemainingCost")]
    public static class WorkOrderEntry_GetRemainingCost
    {
        public static void Postfix(WorkOrderEntry __instance)
        {
            Mod.Log.Trace?.Write("WOE:GRC entered.");
        }
    }

    [HarmonyPatch(typeof(WorkOrderEntry), "GetCostPaid")]
    public static class WorkOrderEntry_GetCostPaid
    {
        public static void Postfix(WorkOrderEntry __instance)
        {
            Mod.Log.Trace?.Write("WOE:GCP entered.");
        }
    }

    [HarmonyPatch(typeof(WorkOrderEntry), "IsCostPaid")]
    public static class WorkOrderEntry_IsCostPaid
    {
        public static void Postfix(WorkOrderEntry __instance)
        {
            Mod.Log.Trace?.Write("WOE:ICP entered.");
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "MaximizeArmor")]
    public static class MechLabLocationWidget_MaximizeArmor
    {
        public static void Postfix(MechLabLocationWidget __instance)
        {
            Mod.Log.Trace?.Write("MLLW:MaximizeArmor entered.");
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "ModifyArmor")]
    public static class MechLabLocationWidget_ModifyArmor
    {
        public static void Postfix(MechLabLocationWidget __instance)
        {
            Mod.Log.Trace?.Write("MLLW:ModifyArmor entered.");
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "SetArmor")]
    public static class MechLabLocationWidget_SetArmor
    {
        public static void Postfix(MechLabLocationWidget __instance)
        {
            Mod.Log.Trace?.Write("MLLW:SetArmor entered.");
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "StripArmor")]
    public static class MechLabLocationWidget_StripArmor
    {
        public static bool Prefix(MechLabLocationWidget __instance, MechLabPanel ___mechLab)
        {
            Mod.Log.Trace?.Write("MLLW:StripArmor:PRE entered.");

            if (__instance.Sim != null)
            {
                int delta = -1 * (int)Math.Ceiling(__instance.currentArmor + __instance.currentRearArmor);

                __instance.currentArmor = 0f;
                __instance.currentRearArmor = 0f;

                Traverse refreshArmorT = Traverse.Create(__instance).Method("RefreshArmor");
                refreshArmorT.GetValue();

                WorkOrderEntry_ModifyMechArmor subEntry = __instance.Sim.CreateMechArmorModifyWorkOrder(___mechLab.activeMechDef.GUID, __instance.loadout.Location, delta, 0, 0);
                ___mechLab.baseWorkOrder.AddSubEntry(subEntry);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(WorkOrderEntry_ModifyMechArmor))]
    [HarmonyPatch(new Type[] { typeof(string), typeof(string), typeof(string), typeof(int),
        typeof(ChassisLocations), typeof(int), typeof(int), typeof(int), typeof(string)})]
    [HarmonyPatch(MethodType.Constructor)]
    public static class WorkOrderEntry_ModifyMechArmor_Patch
    {
        public static void Prefix(ref int cbillCost, ref int techCost, int desiredFrontArmor, int desiredRearArmor)
        {
            Mod.Log.Trace?.Write("WOE:MMA entered.");

            Mod.Log.Info?.Write($"Creating new MechArmorWO with cost: {techCost} and cbillCost: {cbillCost}");
        }
    }
}
