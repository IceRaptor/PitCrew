using BattleTech;
using Harmony;
using PitCrew.Helper;
using System.Collections.Generic;

namespace PitCrew.Patches
{
    [HarmonyPatch(typeof(StarSystem), "GeneratePilots")]
    static class StarSystem_GeneratePilots
    {
        // TODO: Manipulate # of pilots by planet tags
        // TODO: Manipulate # of ronin by planet tags
        static void Postfix(StarSystem __instance)
        {
            Mod.Log.Info?.Write($"Generating pilots for system: {__instance.Name}");

            // Generate new pilots
            // TODO: randomize, make upper/lower bounds
            // TODO: tie to planet tags
            int vCrew = Mod.Config.Crew.VehiclesToGenerate;
            for (int i = 0; i < vCrew; i++)
            {
                PilotDef pDef = PilotHelper.GenerateVehicleCrew(3);
                pDef.PilotTags.Add(ModConsts.Tag_CrewType_Vehicle);
                __instance.AvailablePilots.Add(pDef);
            }
            int mechCrew = Mod.Config.Crew.MechtechsToGenerate;
            for (int i = 0; i < mechCrew; i++)
            {
                PilotDef pDef = PilotHelper.GenerateTechs(3, true);
                __instance.AvailablePilots.Add(pDef);
            }

            int medCrew = Mod.Config.Crew.MedtechsToGenerate;
            for (int i = 0; i < medCrew; i++)
            {
                PilotDef pDef = PilotHelper.GenerateTechs(3, false);
                __instance.AvailablePilots.Add(pDef);
            }
        }
    }
}
