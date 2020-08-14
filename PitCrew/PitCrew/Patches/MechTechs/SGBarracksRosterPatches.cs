using BattleTech;
using BattleTech.UI;
using Harmony;
using System.Collections.Generic;

namespace PitCrew.Patches.MechTechs
{

    [HarmonyPatch(typeof(SGBarracksRosterList), "PopulateRosterAsync")]
    public static class SGBarracksRosterList_PopulateRosterAsync
    {
        public static void Postfix(SGBarracksRosterList __instance)
        {
            Mod.Log.Debug("SGBRL:PRA entered.");
        }
    }

    [HarmonyPatch(typeof(SG_HiringHall_Screen), "AddPeople")]
    public static class SG_HiringHall_Screen_AddPeople
    {
        public static void Postfix(SG_HiringHall_Screen __instance)
        {
            Mod.Log.Debug("SG_HH_S:AP entered.");

            int bobNum = 0;
            List<Pilot> mechTechsAsPilots = new List<Pilot>();
            foreach (TechDef mechTech in ModState.GetSimGameState().CurSystem.AvailableMechTechs)
            {
                Mod.Log.Debug($"Found techDef with desc: {mechTech.Description} skill: {mechTech.Skill}");
                PilotDef mechTechPD = new PilotDef();
                HumanDescriptionDef mechTechPDDef = mechTechPD.Description;
                mechTechPD.Description.SetFirstName($"Bob");
                mechTechPD.Description.SetLastName($"{bobNum}");

                Pilot mechTechAsPilot = new Pilot(new PilotDef(), mechTechPD.Description.FullName(), true);
            }
        }
    }
}
