using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using Harmony;
using HBS.Extensions;
using SVGImporter;
using UnityEngine;
using UnityEngine.UI;

namespace PitCrew.Patches
{
    [HarmonyPatch(typeof(SGBarracksRosterSlot), "RefreshCostColorAndAvailability")]
    static class SGBarracksRosterSlot_RefreshCostColorAndAvailability
    {
        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot)
        {
            Mod.Log.Debug?.Write($"Updating colors for pilot: {___pilot.Name}");
        }
    }

    [HarmonyPatch(typeof(SGBarracksRosterSlot), "Refresh")]
    static class SGBarracksRosterSlot_Refresh
    {
        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot, 
            GameObject ___AbilitiesObject, LocalizableText ___callsign, Image ___portrait)
        {
            if (___pilot == null) return;
            Mod.Log.Debug?.Write($"Updating roster for pilot: {___pilot.Name}");

            bool isMechTech = false;
            bool isMedTech = false;
            bool isVehicle = false;
            foreach (string tag in ___pilot.pilotDef.PilotTags)
            {
                if (ModConsts.Tag_CrewType_MechTech.Equals(tag, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    isMechTech = true;
                }
                if (ModConsts.Tag_CrewType_MedTech.Equals(tag, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    isMedTech = true;
                }
                if (ModConsts.Tag_CrewType_Vehicle.Equals(tag, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    isVehicle = true;
                }

            }

            if (!isMechTech && !isMedTech && !isVehicle) return; // nothing to do

            GameObject layoutTitleGO = __instance.GameObject.FindFirstChildNamed("layout_title");
            Image layoutTitleImg = layoutTitleGO.GetComponent<Image>();
            if (isMechTech)
            {
                layoutTitleImg.color = Color.green;
                layoutTitleImg.SetAllDirty();

                SVGAsset icon = ModState.SimGameState.DataManager.GetObjectOfType<SVGAsset>(Mod.Config.Icons.MechTechPortait, BattleTechResourceType.SVGAsset);
                if (icon == null) Mod.Log.Warn?.Write("ERROR READING ICON!");

                Mod.Log.Info?.Write($"PORTRAIT GO NAME: {___portrait.gameObject.name}");

                GameObject profileOverrideGO = ___portrait.transform.parent.gameObject.FindFirstChildNamed(ModConsts.GO_Profile_Override);
                if (profileOverrideGO == null)
                {
                    Mod.Log.Debug?.Write("CREATING PORTRAIT OVERRIDE NODE");
                    profileOverrideGO = new GameObject();
                    profileOverrideGO.name = ModConsts.GO_Profile_Override;
                    profileOverrideGO.transform.parent = ___portrait.transform.parent;
                    profileOverrideGO.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    Vector3 newPos = ___portrait.transform.position;
                    newPos.x -= 30f;
                    newPos.y += 30f;
                    profileOverrideGO.transform.position = newPos;

                    SVGImage image = profileOverrideGO.GetOrAddComponent<SVGImage>();
                    if (image == null) Mod.Log.Warn?.Write("FAILED TO LOAD IMAGE - WILL NRE!");
                    image.vectorGraphics = icon;
                    image.color = Color.white;
                    image.enabled = true;
                }

                profileOverrideGO.SetActive(true);
                ___portrait.gameObject.SetActive(false);
                ___AbilitiesObject.SetActive(false);

            }
            else if (isMedTech)
            {
                layoutTitleImg.color = Color.red;
                ___AbilitiesObject.SetActive(false);

                SVGAsset icon = ModState.SimGameState.DataManager.GetObjectOfType<SVGAsset>(Mod.Config.Icons.MedTechButton, BattleTechResourceType.SVGAsset);
                //SVGImage image = ___portrait.gameObject.GetComponent<SVGImage>();
                //if (image == null) ___portrait.gameObject.AddComponent<SVGImage>();
                //image.vectorGraphics = icon;
                //image.color = Color.white;
                //image.enabled = true;

            }
            else if (isVehicle)
            {
                layoutTitleImg.color = Color.blue;
                ___callsign.SetText("VCREW: " + ___pilot.Callsign);
            }
        }
    }
}
