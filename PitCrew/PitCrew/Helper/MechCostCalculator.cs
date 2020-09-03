using BattleTech;
using CustomComponents;
using System;

namespace PitCrew
{

    public static class MonthlyCostCalcs
    {

        public static int SumMonthlyMechCosts(SimGameState sgs)
        {
            int sumCost = 0;
            foreach (MechDef mechDef in sgs.ActiveMechs.Values)
            {
                sumCost += CalcMechCost(mechDef);
            }
            return sumCost;
        }

        public static int CalcMechCost(MechDef mechDef)
        {

            // Iterate the components in the mech
            int sumComponentCost = 0;
            float armorMulti = 1f;
            float intStructureMulti = 1f;

            foreach (MechComponentRef mcRef in mechDef.Inventory)
            {
                string compName = mcRef.Def.Description.Name;
                Mod.Log.Debug?.Write($"  Evaluating component:({compName})");

                if (mcRef.Is<PitCrewCC>(out PitCrewCC cc))
                {
                    Mod.Log.Debug?.Write(cc.ToString());

                    // Comp cost
                    sumComponentCost += cc.MonthlyCost();
                    Mod.Log.Debug?.Write($"  Comp. cost from CC: {cc.MonthlyCost()}");

                    // Armor / structure mods
                    armorMulti += cc.ArmorCBCostMulti;
                    intStructureMulti += cc.IntStructCBCostMulti;
                }
                else
                {
                    int compRawCost = mcRef.Def?.Description?.Cost ?? 0;
                    int compCost = (int)Math.Ceiling(compRawCost * Mod.Config.MonthlyCost.DefaultComponentCostMulti);
                    Mod.Log.Debug?.Write($"  Comp. cost from description.cost: {compCost}");
                }
            }

            // TODO: Check chassis for tags that impact armor (like ArmorRepair)
            int chassisCost = mechDef?.Chassis?.Description?.Cost ?? 0;

            int armorPoints = SumArmor(mechDef);
            int armorCost = (int)Math.Ceiling(armorPoints * armorMulti);

            int structurePoints = SumIntStructure(mechDef);
            int intStructCost = (int)Math.Ceiling(structurePoints * intStructureMulti);

            int totalCost = chassisCost + sumComponentCost + armorCost + intStructCost;
            Mod.Log.Debug?.Write($" Chassis: {mechDef?.Description?.Name} has totalCost: {totalCost} = " +
                $"chassisCost: {chassisCost} + componentsCost: {sumComponentCost} + " +
                $"armorCost: {armorCost} + structureCost: {intStructCost}");

            return totalCost;
        }

        private static int SumArmor(MechDef mechDef)
        {
            float armor = 0;

            armor += mechDef.Head.CurrentArmor;

            armor += mechDef.CenterTorso.CurrentArmor;
            armor += mechDef.CenterTorso.CurrentRearArmor;

            armor += mechDef.LeftArm.CurrentArmor;
            armor += mechDef.LeftLeg.CurrentArmor;

            armor += mechDef.LeftTorso.CurrentArmor;
            armor += mechDef.LeftTorso.CurrentRearArmor;

            armor += mechDef.RightArm.CurrentArmor;
            armor += mechDef.RightLeg.CurrentArmor;

            armor += mechDef.RightTorso.CurrentArmor;
            armor += mechDef.RightTorso.CurrentRearArmor;

            return (int)Math.Ceiling(armor);
        }

        private static int SumIntStructure(MechDef mechDef)
        {
            float intStructure = 0;

            intStructure += mechDef.Head.CurrentInternalStructure;

            intStructure += mechDef.CenterTorso.CurrentInternalStructure;

            intStructure += mechDef.LeftArm.CurrentInternalStructure;
            intStructure += mechDef.LeftLeg.CurrentInternalStructure;

            intStructure += mechDef.LeftTorso.CurrentInternalStructure;

            intStructure += mechDef.RightArm.CurrentInternalStructure;
            intStructure += mechDef.RightLeg.CurrentInternalStructure;

            intStructure += mechDef.RightTorso.CurrentInternalStructure;

            return (int)Math.Ceiling(intStructure);
        }
    }
}
