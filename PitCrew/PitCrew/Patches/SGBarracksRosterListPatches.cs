using BattleTech;
using BattleTech.UI;
using Harmony;
using SVGImporter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PitCrew.Patches
{

    [HarmonyPatch(typeof(SGBarracksRosterList), "AddPilot")]
    static class SGBarracksRosterList_AddPilot
    {
        static void Prefix(SGBarracksRosterList __instance, Pilot pilot)
        {
            Mod.Log.Debug?.Write($"Adding pilot {pilot.Callsign} to roster list.");
        }

    }

    [HarmonyPatch(typeof(SGBarracksRosterList), "ApplySort")]
    static class SGBarracksRosterList_ApplySort
    {
        static bool Prefix(SGBarracksRosterList __instance, List<SGBarracksRosterSlot> inventory)
        {
            // TODO: Apply a logical sort here
            return true;
        }

    }

    [HarmonyPatch(typeof(SGBarracksRosterList), "Awake")]
    static class SGBarracksRosterList_Awake
    {
        static void Postfix(SGBarracksRosterList __instance)
        {
            //Mod.Log.Trace?.Write("SGBRL:A entered.");

            //GameObject sgbrlGO = __instance.gameObject;
            //// Parent should be a HorizontalLayoutGroup
            //HorizontalLayoutGroup hlg = sgbrlGO.GetComponentInParent<HorizontalLayoutGroup>();
            //if (hlg == null) return; // nothing to do... yet

            //if (hlg != null) Mod.Log.Info?.Write("Found HLG!");
            //hlg.spacing = 20f;

            //GameObject containerGO = new GameObject("pc-buttons");
            //containerGO.transform.parent = hlg.gameObject.transform;
            //containerGO.transform.SetAsFirstSibling();

            //Mod.Log.Info?.Write("Adding VLG to HLG");
            //VerticalLayoutGroup vlg = containerGO.AddComponent<VerticalLayoutGroup>();
            //vlg.spacing = 24f;
            //vlg.gameObject.SetActive(true);

            //Mod.Log.Info?.Write("Creating radioSet");
            //GameObject radioSetGO = new GameObject("pc-radio-set");
            //radioSetGO.transform.parent = vlg.gameObject.transform;

            //HBSRadioSet radioSet = radioSetGO.AddComponent<HBSRadioSet>();
            //Traverse radioSetT = Traverse.Create(radioSet).Field("radioButtons");
            //radioSetT.SetValue(new List<HBSButton>());
            //if (radioSet == null) Mod.Log.Warn?.Write("RadioSet was null!");

            //// Add the buttons
            //Mod.Log.Info?.Write("Creating MechW Button");
            //GameObject mechWarriorGO = CreateIcon(vlg.gameObject, Mod.Config.Icons.MechWarriorButton, "pc-mech-warrior-icon");

            //Mod.Log.Info?.Write("Adding MechW Toggle");
            //HBSToggle mechWarriorButton = mechWarriorGO.AddComponent<HBSToggle>();
            //mechWarriorButton.SetParent(radioSet);

            //Mod.Log.Info?.Write("Creating MechT Button");
            //GameObject mechTechGO = CreateIcon(vlg.gameObject, Mod.Config.Icons.MechTechButton, "pc-mech-tech-icon");
            
            //Mod.Log.Info?.Write("Adding MechT Toggle");
            //HBSToggle mechTechButton = mechTechGO.AddComponent<HBSToggle>();
            //mechTechButton.SetParent(radioSet);

            //Mod.Log.Info?.Write("Creating MedT Button");
            //GameObject medTechGO = CreateIcon(vlg.gameObject, Mod.Config.Icons.MedTechButton, "pc-med-tech-icon");
            
            //Mod.Log.Info?.Write("Adding MedT Toggle");
            //HBSToggle medTechButton = medTechGO.AddComponent<HBSToggle>();
            //medTechButton.SetParent(radioSet);


        }

        static GameObject CreateIcon(GameObject parent, string iconId, string objectId)
        {
            Mod.Log.Debug?.Write($"Creating icon for iconId: {iconId}");
            try
            {
                SimGameState sgs = ModState.SimGameState;
                SVGAsset icon = sgs.DataManager.GetObjectOfType<SVGAsset>(iconId, BattleTechResourceType.SVGAsset);
                if (icon == null) Mod.Log.Warn?.Write($"Icon: {iconId} was not loaded! Check the manifest load");

                GameObject imageGO = new GameObject();
                imageGO.name = objectId;
                imageGO.transform.parent = parent.transform;
                imageGO.transform.localScale = new Vector3(0.5f, 1.5f, 1f);

                SVGImage image = imageGO.AddComponent<SVGImage>();
                if (image == null) Mod.Log.Warn?.Write("Failed to create image for icon, load will fail!");

                image.vectorGraphics = icon;
                image.color = Color.white;
                image.enabled = true;
                
                LayoutElement le = imageGO.AddComponent<LayoutElement>();
                le.preferredHeight = 24f;
                le.preferredWidth = 24f;

                RectTransform rectTransform = imageGO.GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(50f, 0f);

                return imageGO;
            }
            catch (Exception e)
            {
                Mod.Log.Error?.Write(e, $"Failed to create icon image: {iconId}");
                return null;
            }
        }
    }
}
