using BattleTech;
using CustomComponents;
using System;
using System.Collections.Generic;

namespace PitCrew.Helper
{

    public class RepairCalculator
    {

        public static void ArmorRepair(MechDef mechDef, int pointsToRepair, out double techPoints, out double cBills)
        {

            double baseCost = Math.Ceiling(pointsToRepair / Mod.Config.ArmorRepair.PointsPerTP);
            Mod.Log.Debug($" baseCost: {baseCost} = {pointsToRepair} / {Mod.Config.ArmorRepair.PointsPerTP}");

            double tonnageCost = Math.Ceiling(mechDef.Chassis.Tonnage / Mod.Config.ArmorRepair.TonsPerTP);
            if (tonnageCost > baseCost) { tonnageCost = baseCost; }
            Mod.Log.Debug($" tonnageCost: {tonnageCost} = {mechDef.Chassis.Tonnage} tons / {Mod.Config.ArmorRepair.TonsPerTP}");

            double repairCost = baseCost + tonnageCost;

            // Check chassis for: 1) customizations that influence armor repair 
            //                    2) tags that influence armor repair if not tags found
            double chassisTPMulti = 1.0;
            double chassisCBMulti = 1.0;
            if (mechDef.Chassis.Is<PCChassis>(out PCChassis pcChassis))
            {
                Mod.Log.Debug($"  unit has PCChassis, overriding values TP: {pcChassis.TPMulti} CB: {pcChassis.CBMulti}");
                chassisTPMulti = pcChassis.TPMulti;
                chassisCBMulti = pcChassis.CBMulti;
            }
            else
            {
                // Look for matching tags
                foreach (KeyValuePair<string, TagModifiers> kvp in Mod.Config.ChassisTagMods)
                {
                    if (mechDef.Chassis.ChassisTags.Contains(kvp.Key))
                    {
                        Mod.Log.Debug($"  unit has chassisTag: {kvp.Key}, adding multipliers TP: x{kvp.Value.ArmorRepairTPMulti} CB: x{kvp.Value.ArmorRepairCBMulti}");
                        chassisTPMulti += kvp.Value.ArmorRepairTPMulti;
                        chassisCBMulti += kvp.Value.ArmorRepairCBMulti;
                    }
                }
            }
            Mod.Log.Debug($"Final chassisMulti: {chassisTPMulti}");

            // Check components for: 1) customizations that influence armor repair 
            //                       2) tags that influence armor repair if not tags found
            double componentsTPMulti = 0.0;
            double componentsCBMulti = 0.0;
            foreach (MechComponentRef mcRef in mechDef.Inventory)
            {
                if (mcRef.Is<PCArmor>(out PCArmor pcArmor))
                {
                    Mod.Log.Debug($"  unit has PCArmor, adding values TP: {pcChassis.TPMulti} CB: {pcChassis.CBMulti}");
                    componentsTPMulti += pcArmor.TPMulti;
                    componentsCBMulti += pcArmor.CBMulti;
                }
                else
                {
                    foreach (KeyValuePair<string, TagModifiers> kvp in Mod.Config.ComponentTagMods)
                    {
                        if (mcRef.Def.ComponentTags.Contains(kvp.Key))
                        {
                            Mod.Log.Debug($"  unit has component with tag: {kvp.Key}, adding multiplierd TP: x{kvp.Value.ArmorRepairTPMulti} CB: x{kvp.Value.ArmorRepairCBMulti}");
                            componentsTPMulti += kvp.Value.ArmorRepairTPMulti;
                            componentsCBMulti += kvp.Value.ArmorRepairCBMulti;
                        }
                    }
                }
            }
            Mod.Log.Debug($"Final componentsMulti: {componentsTPMulti}");

            double finalTPMulti = chassisTPMulti + componentsTPMulti;
            int finalTPCost = (int)Math.Ceiling(repairCost * finalTPMulti);
            techPoints = finalTPCost;
            Mod.Log.Debug($"finalTPCost: {finalTPCost} = repairCost:{repairCost} x finalMulti: {finalTPMulti}");

            double finalCBMulti = chassisCBMulti + componentsCBMulti;
            int finalCBCost = (int)Math.Ceiling(repairCost * finalCBMulti) * Mod.Config.ArmorRepair.CBillsPerPoint;
            cBills = finalCBCost;
            Mod.Log.Debug($"finalCBCost: {finalCBCost} = (repairCost:{repairCost} x finalMulti: {finalTPMulti}) " +
                $"x cBillsPerPoint: {Mod.Config.ArmorRepair.CBillsPerPoint}");
        }

        public static void StructureRepair(MechDef mechDef, out double techPoints, out double cBills)
        {
            techPoints = 0d;
            cBills = 0d;

        }

    }
}
