using BattleTech;
using BattleTech.UI;
using Harmony;
using TMPro;

namespace PitCrew.Patches
{
    [HarmonyPatch(typeof(TaskManagementElement), "UpdateTaskInfo")]
    public static class TaskManagementElement_UpdateTaskInfo
    {
        public static void Postfix(TaskManagementElement __instance, TextMeshProUGUI ___titleText, TextMeshProUGUI ___subTitleText, TextMeshProUGUI ___daysText)
        {
            Mod.Log.Debug("TME:UTI entered.");



        }
    }

    [HarmonyPatch(typeof(TaskManagementElement), "UpdateItem")]
    public static class TaskManagementElement_UpdateItem
    {
        public static void Postfix(TaskManagementElement __instance, int cumulativeDays, TextMeshProUGUI ___titleText, TextMeshProUGUI ___subTitleText, TextMeshProUGUI ___daysText)
        {
            Mod.Log.Debug("TME:UI entered.");

            // Assume that cumulative days has been manipulated as 1d * 600m = 600
            if (__instance.Entry.Type == WorkOrderType.MechLabModifyArmor && !__instance.Entry.IsCostPaid())
            {
                SimGameState sgs = ModState.GetSimGameState();
                float costInMins = __instance.Entry.GetRemainingCost() / sgs.MechTechSkill * 100;
                float daysLeft = costInMins / 600;
                Mod.Log.Debug($" costInMins:{costInMins} / daysLeft:{daysLeft}");
                ___daysText.SetText($"{costInMins} min / {daysLeft}d");
            }


        }
    }

    public static class TaskTimelineWidget_RefreshEntries
    {
        public static void Postfix(TaskTimelineWidget __instance)
        {
            Mod.Log.Debug("TTW:RE entered.");

        }
    }
}
