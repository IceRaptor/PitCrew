﻿using Harmony;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Reflection;
using us.frostraptor.modUtils.logging;

namespace PitCrew {

    public static class Mod {

        public const string HarmonyPackage = "us.frostraptor.PitCrew";
        public const string LogName = "pit_crew";

        public static IntraModLogger Log;
        public static string ModDir;
        public static ModConfig Config;

        public static readonly Random Random = new Random();

        public static void Init(string modDirectory, string settingsJSON) {
            ModDir = modDirectory;

            Exception settingsE = null;
            try {
                Mod.Config = JsonConvert.DeserializeObject<ModConfig>(settingsJSON);
            } catch (Exception e) {
                settingsE = e;
                Mod.Config = new ModConfig();
            }

            Log = new IntraModLogger(modDirectory, LogName, Mod.Config.Debug, Mod.Config.Trace);

            Assembly asm = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
            Log.Info($"Assembly version: {fvi.ProductVersion}");

            Log.Debug($"ModDir is:{modDirectory}");
            Log.Debug($"mod.json settings are:({settingsJSON})");
            Mod.Config.LogConfig();

            if (settingsE != null) {
                Log.Info($"ERROR reading settings file! Error was: {settingsE}");
            } else {
                Log.Info($"INFO: No errors reading settings file.");
            }

            // Initialize custom components
            CustomComponents.Registry.RegisterSimpleCustomComponents(Assembly.GetExecutingAssembly());

            // Initialize harmony
            var harmony = HarmonyInstance.Create(HarmonyPackage);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }
}
