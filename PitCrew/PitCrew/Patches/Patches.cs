using BattleTech;
using Harmony;

namespace PitCrew {

    [HarmonyPatch(typeof(SimGameState), "UpdateMechLabWorkQueue", MethodType.Normal)]
    public static class SimGameState_UpdateMechLabWorkQueue {

        private static void Postfix(SimGameState __instance, bool passDay) {
            Mod.Log.Debug("SGS:UMLWQ entered.");

        }
    }   


    // SimGameState::OnBreadcrumbArrival

    // SGS::OnDayPassed
    // SGS::UpdateMechLabWorkQueue
    // SGS::UpdateInjuries
    // 

    // Stats - MechTechSkill / MedTechSkil on SGS.companyStats

    // SimGameState.CurSystem.Def.Description.Name

    // this.TravelState -> SGTS.IN_SYSTEM 

    // SGS::AddSpecialEvent (forced event)

    // SGS:CreateComponentRepairWorkOrder

    // SGS:PurchasedArgoUpgrades::Get -or SGS::ShipUpgrades::Get

    // SGS:GetDailyHealValue -> DailyHealValue * MedTechSkill * MedTechSkillMod

}
