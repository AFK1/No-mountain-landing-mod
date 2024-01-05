using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace no_mountain_land;

public class MyPatcher
{
  public static void DoPatching()
  {
    var harmony = new Harmony("boats.sergwest.no_mountain_land");
    harmony.PatchAll();
  }
}

[StaticConstructorOnStartup]
public static class HelloWorld
{
  static HelloWorld()
  {
    var harmony = new Harmony("boats.sergwest.no_mountain_land");
    harmony.PatchAll();
  }
}


[HarmonyPatch(typeof(DropCellFinder), nameof(DropCellFinder.CanPhysicallyDropInto))]
static class DropCellFinder_CanPhysicallyDropInto_Patch
{
  static bool Prefix(ref bool __result, IntVec3 c, Map map)
  {
    if (map.roofGrid.RoofAt(c) == RoofDefOf.RoofRockThick | map.roofGrid.RoofAt(c) == RoofDefOf.RoofRockThin) {
    __result = false;
      return false;
    }
    Building edifice = c.GetEdifice (map);
    if ((edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock) | (c.Roofed (map) && c.GetRoof (map).isThickRoof)) {
    __result = false;
      return false;
    }
    return true;
  }
}

[HarmonyPatch(typeof(CellFinderLoose), nameof(CellFinderLoose.TryFindSkyfallerCell))]
static class CellFinderLoose_TryFindSkyfallerCell_Patch 
{
  static void Postfix(ref bool __result, Map map, IntVec3 cell)
  {
    if (__result == true) {
      if (cell.Roofed (map)) {
        if (map.roofGrid.RoofAt(cell) == RoofDefOf.RoofRockThick | map.roofGrid.RoofAt(cell) == RoofDefOf.RoofRockThin) {
        __result = false;
          return;
        }
        Building edifice = cell.GetEdifice (map);
        if ((edifice != null && edifice.def.category == ThingCategory.Building && edifice.def.building.isNaturalRock) | (cell.Roofed (map) && cell.GetRoof (map).isThickRoof)) {
          __result = false;
        }
      }
    }
  }
}