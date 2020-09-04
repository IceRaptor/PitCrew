using BattleTech;
using BattleTech.UI;
using BattleTech.UI.TMProWrapper;
using BattleTech.UI.Tooltips;
using Harmony;
using HBS.Extensions;
using SVGImporter;
using TMPro;
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

    [HarmonyPatch(typeof(SGBarracksRosterSlot), "Init")]
    static class SGBarracksRosterSlot_Init
    {
        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot)
        {
            Mod.Log.Debug?.Write($"Initing with pilot: {___pilot?.Name}");
        }
    }

    [HarmonyPatch(typeof(SGBarracksRosterSlot), "InitNoDrag")]
    static class SGBarracksRosterSlot_InitNoDrag
    {
        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot)
        {
            Mod.Log.Debug?.Write($"InitNoDrag with pilot: {___pilot?.Name}");
        }
    }


    [HarmonyPatch(typeof(SGBarracksRosterSlot), "InitNoInteract")]
    static class SGBarracksRosterSlot_InitNoInteract
    {
        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot)
        {
            Mod.Log.Debug?.Write($"InitNoInteract with pilot: {___pilot?.Name}");
        }
    }

    [HarmonyPatch(typeof(SGBarracksRosterSlot), "Refresh")]
    static class SGBarracksRosterSlot_Refresh
    {
        static void Prefix(Pilot ___pilot)
        {
            Mod.Log.Debug?.Write($"PRE Calling refresh for pilot: {___pilot.Name}");
        }

        static void Postfix(SGBarracksRosterSlot __instance, Pilot ___pilot, 
            GameObject ___AbilitiesObject, LocalizableText ___callsign, Image ___portrait,
            SVGImage ___roninIcon, SVGImage ___veteranIcon, 
            LocalizableText ___expertise, HBSTooltip ___ExpertiseTooltip)
        {
            if (___pilot == null) return;
            Mod.Log.Debug?.Write($"POST Calling refresh for pilot: {___pilot.Name}");

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
                layoutTitleImg.color = Mod.Config.Crew.MechTechCrewColor;

                ___portrait.gameObject.SetActive(false);
                ___AbilitiesObject.SetActive(false);
                ___roninIcon.gameObject.SetActive(false);
                ___veteranIcon.gameObject.SetActive(false);

                SVGAsset icon = ModState.SimGameState.DataManager.GetObjectOfType<SVGAsset>(Mod.Config.Icons.MechTechPortait, BattleTechResourceType.SVGAsset);
                if (icon == null) Mod.Log.Warn?.Write($"ERROR READING ICON: {Mod.Config.Icons.MechTechButton}");

                GameObject profileOverrideGO = GetOrCreateProfileOverride(___portrait, icon);
                profileOverrideGO.SetActive(true);

                GameObject layoutStatsOrHiring = ___portrait.transform.parent.parent.gameObject.FindFirstChildNamed(ModConsts.GO_HBS_Profile_Layout_Stats);
                if (layoutStatsOrHiring == null) Mod.Log.Warn?.Write("FAILED TO FIND layout_Stats-Or-HiringCost, will NRE!");

                GameObject mwStats = ___portrait.transform.parent.parent.gameObject.FindFirstChildNamed(ModConsts.GO_Profile_Override);
                if (mwStats == null) Mod.Log.Warn?.Write("FAILED TO FIND mw_stats, will NRE!");
                mwStats.SetActive(false);

                GameObject contentFitter = new GameObject();
                contentFitter.transform.parent = layoutStatsOrHiring.transform;
                contentFitter.AddComponent<ContentSizeFitter>();
                contentFitter.transform.SetAsFirstSibling();

                GameObject textBlock1 = new GameObject();
                textBlock1.transform.parent = contentFitter.transform;
                LocalizableText lt1 = textBlock1.AddComponent<LocalizableText>();
                lt1.SetText("Size: XXX");
                lt1.fontSize = 24f;
                lt1.alignment = TextAlignmentOptions.Left;
                textBlock1.SetActive(true);
                
                Mod.Log.Debug?.Write("ADDED TEXTBLOCK1");

                GameObject textBlock2 = new GameObject();
                textBlock2.transform.parent = contentFitter.transform;
                LocalizableText lt2 = textBlock2.AddComponent<LocalizableText>();
                lt2.SetText("Skill: XXX");
                lt2.fontSize = 24f;
                lt2.alignment = TextAlignmentOptions.Left;
                textBlock2.SetActive(true);
                Mod.Log.Debug?.Write("ADDED TEXTBLOCK2");

            }
            else if (isMedTech)
            {
                layoutTitleImg.color = Mod.Config.Crew.MedTechCrewColor;

                ___portrait.gameObject.SetActive(false);
                ___AbilitiesObject.SetActive(false);
                ___roninIcon.gameObject.SetActive(false);
                ___veteranIcon.gameObject.SetActive(false);

                SVGAsset icon = ModState.SimGameState.DataManager.GetObjectOfType<SVGAsset>(Mod.Config.Icons.MedTechButton, BattleTechResourceType.SVGAsset);
                if (icon == null) Mod.Log.Warn?.Write($"ERROR READING ICON: {Mod.Config.Icons.MedTechButton}");

                GameObject profileOverrideGO = GetOrCreateProfileOverride(___portrait, icon);
                profileOverrideGO.SetActive(true);
            }
            else if (isVehicle)
            {
                layoutTitleImg.color = Mod.Config.Crew.VehicleCrewColor;
                ___callsign.SetText("VCREW: " + ___pilot.Callsign);

                // VCrew can't be ronin
                ___roninIcon.gameObject.SetActive(false);
            }

            Mod.Log.Debug?.Write($"LayoutTitleImg color set to: {layoutTitleImg.color}");
        }

        private static GameObject GetOrCreateProfileOverride(Image portrait, SVGAsset icon)
        {
            GameObject profileOverrideGO = portrait.transform.parent.gameObject.FindFirstChildNamed(ModConsts.GO_Profile_Override);
            if (profileOverrideGO == null)
            {
                Mod.Log.Debug?.Write(" -- CREATING PORTRAIT OVERRIDE NODE");
                profileOverrideGO = new GameObject();
                profileOverrideGO.name = ModConsts.GO_Profile_Override;
                profileOverrideGO.transform.parent = portrait.transform.parent;
                profileOverrideGO.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

                Vector3 newPos = portrait.transform.position;
                newPos.x -= 30f;
                newPos.y += 30f;
                profileOverrideGO.transform.position = newPos;

                SVGImage image = profileOverrideGO.GetOrAddComponent<SVGImage>();
                if (image == null) Mod.Log.Warn?.Write(" -- FAILED TO LOAD IMAGE - WILL NRE!");
                image.color = Color.white;
                image.vectorGraphics = icon;
                image.enabled = true;
            }

            return profileOverrideGO;
        }
    }
}
