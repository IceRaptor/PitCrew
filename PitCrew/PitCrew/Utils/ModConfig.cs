

namespace PitCrew {

    public class ModConfig {

        public bool Debug = false;
        public bool Trace = false;

        public void LogConfig() {
            Mod.Log.Info("=== MOD CONFIG BEGIN ===");
            Mod.Log.Info($"  DEBUG: {this.Debug} Trace: {this.Trace}");
            Mod.Log.Info("=== MOD CONFIG END ===");
        }
    }
}
