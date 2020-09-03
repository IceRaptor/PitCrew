using System.Collections.Generic;

namespace PitCrew
{

    public class ModCrewNames
    {
        public List<string> MechTech = new List<string>();
        public List<string> MedTech = new List<string>();
        public List<string> Vehicle = new List<string>();
    }

    public class CrewCfg
    {
        public int VehiclesToGenerate = 5;
        public int MechtechsToGenerate = 4;
        public int MedtechsToGenerate = 3;
    }

    public class MonthlyCost
    {
        public float DefaultComponentCostMulti = 0.1f;
    }

    public class ArmorRepair
    {
        public float PointsPerTP = 6; // How many armor points count as one TP
        public int TonsPerTP = 20; // How many points to add by tonnage
        public int CBillsPerPoint = 60;
    }

    public class StructureRepair
    {
        public float PointsPerTP = 6;
        public int TonsPerTP = 5;
        public int CBillsPerPoint = 600;
    }

    public class TagModifiers
    {
        public float ArmorRepairTPMulti = 0f;
        public float ArmorRepairCBMulti = 0f;

        public float StructureRepairTPMulti = 0f;
        public float StructureRepairCBMulti = 0f;
    }

    public class Icons
    {
        public string MechWarriorButton = "pc_missile-mech";
        
        public string MechTechButton = "pc_apc";
        public string MedTechPortrait = "pc_auto-repair";
        
        public string MedTechButton = "pc_hospital-cross";
        public string MechTechPortait = "pc_auto-repair";

        public string GroupPortrait = "pc_three-friends";
    }

    public class ModConfig
    {

        public bool Debug = false;
        public bool Trace = false;

        public Icons Icons = new Icons();

        public CrewCfg Crew = new CrewCfg();
        public MonthlyCost MonthlyCost = new MonthlyCost();
        public ArmorRepair ArmorRepair = new ArmorRepair();
        public StructureRepair StructureRepair = new StructureRepair();
        public Dictionary<string, TagModifiers> ChassisTagMods = new Dictionary<string, TagModifiers>();
        public Dictionary<string, TagModifiers> ComponentTagMods = new Dictionary<string, TagModifiers>();

        public void LogConfig()
        {
            Mod.Log.Info?.Write("=== MOD CONFIG BEGIN ===");
            Mod.Log.Info?.Write($"  DEBUG: {this.Debug} Trace: {this.Trace}");
            Mod.Log.Info?.Write("=== MOD CONFIG END ===");
        }
    }
}
