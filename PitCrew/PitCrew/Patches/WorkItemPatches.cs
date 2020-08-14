using BattleTech;
using BattleTech.UI;
using Harmony;
using TMPro;
using UnityEngine;

namespace PitCrew.Patches
{

    [HarmonyPatch(typeof(SGWorkItem), "UpdateItem")]
    public static class SGWorkItem_UpdateItem
    {
        public static void Postfix(SGWorkItem __instance, int? cost, WorkOrderEntry ___entry, TextMeshProUGUI ___Time)
        {
            Mod.Log.Debug("SGWI:UI entered.");

            if (!___entry.IsCostPaid())
            {

                SimGameState sgs = ModState.GetSimGameState();
                int techPointsPerDay = sgs.MechTechSkill * 100;

                float remainingCost = ___entry.GetRemainingCost() * 100;
                float daysRemaining = remainingCost / techPointsPerDay;
                Mod.Log.Debug($"WOE: {___entry.ID} has cost: {remainingCost} / days: {daysRemaining}");
                if (daysRemaining < 1)
                {
                    ___Time.SetText($"{remainingCost} minutes");
                }
                else
                {
                    ___Time.SetText($"{remainingCost} minutes / {daysRemaining} Days");
                }


            }

        }
    }

}
