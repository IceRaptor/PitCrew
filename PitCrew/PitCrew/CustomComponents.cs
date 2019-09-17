using CustomComponents;
using System;

namespace PitCrew {

    [CustomComponent("PitCrew")]
    public class PitCrewCC : SimpleCustomComponent {
        // Monthly cost modifiers
        public int FixedCBCost = 0; // 0 means use default cost
        public float CBCostMulti = 1f;
        public float ArmorCBCostMulti = 0.0f;
        public float IntStructCBCostMulti = 0.0f;

        // Tech point modifiers
        public float TechPointMod = 0f;

        public int MonthlyCost() {
            int rawCost = this.Def?.Description?.Cost ?? 0;
            return FixedCBCost != 0 ? FixedCBCost : (int)Math.Ceiling(rawCost * CBCostMulti);
        }

        public override string ToString() {
            return $"PitCrewCC = FixedCBCost:{FixedCBCost} CBCostMulti:{CBCostMulti} " +
                $"armorCBCost:{ArmorCBCostMulti} structCBCost:{IntStructCBCostMulti} " +
                $"techPointMod: {TechPointMod}";
        }
    }

    [CustomComponent("PC_Armor")]
    public class PCArmor : SimpleCustomComponent {
        public float CBMulti = 1f;
        public float TPMulti = 1f;
    }

    [CustomComponent("PC_Structure")]
    public class PCInternalStructure : SimpleCustomComponent {
        public float CBMulti = 1f;
        public float TPMulti = 1f;
    }

    [CustomComponent("PC_Maint")]
    public class PCChassis : SimpleCustomChassis {
        public float CBMulti = 1f;
        public float TPMulti = 1f;
    }

    // how to do ferro-lamm armor, since in theory it should be armor * value? Same for structure?
    // They are comps, so comp values? 
    // mechs/chassisdefs driven tags can supply chassis bonii
}
