

using System.Collections.Generic;

namespace PitCrew {

    public static class ModStats {

    }

    public class MonthlyCost {
        public float DefaultComponentCostMulti = 0.1f;
    }

    public class ArmorRepair {
        public float PointsPerTP = 6; // How many armor points count as one TP
        public int TonsPerTP = 20; // How many points to add by tonnage
        public int CBillsPerPoint = 60;
    }

    public class StructureRepair {
        public float PointsPerTP = 6;
        public int TonsPerTP = 5;
        public int CBillsPerPoint = 600;
    }

    public class TagModifiers {
        public float ArmorRepairTPMulti = 0f;
        public float ArmorRepairCBMulti = 0f;

        public float StructureRepairTPMulti = 0f;
        public float StructureRepairCBMulti = 0f;
    }

    public class ModConfig {

        public bool Debug = false;
        public bool Trace = false;

        public MonthlyCost MonthlyCost = new MonthlyCost();
        public ArmorRepair ArmorRepair = new ArmorRepair();
        public StructureRepair StructureRepair = new StructureRepair();
        public Dictionary<string, TagModifiers> ChassisTagMods = new Dictionary<string, TagModifiers>();
        public Dictionary<string, TagModifiers> ComponentTagMods = new Dictionary<string, TagModifiers>();

        public void LogConfig() {
            Mod.Log.Info("=== MOD CONFIG BEGIN ===");
            Mod.Log.Info($"  DEBUG: {this.Debug} Trace: {this.Trace}");
            Mod.Log.Info("=== MOD CONFIG END ===");
        }
    }
}
