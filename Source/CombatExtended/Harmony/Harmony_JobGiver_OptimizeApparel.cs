﻿using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace CombatExtended.HarmonyCE
{
    /* Overrides the CompReloadable reload job if the pawn has suitable ammo in their inventory.
     * If no inventory ammo is available, base method is allowed to execute (reload from stockpiles).
     */
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "ApparelScoreGain")]
    internal static class Harmony_JobGiver_OptimizeApparel_ApparelScoreGain
    {
        internal static bool Prefix(Pawn pawn, Apparel ap, ref float __result)
        {
            if (ap is ShieldBelt && pawn.GetLoadout().Slots.FirstOrDefault(s => s.count >= 1 && (s.thingDef.IsWeaponUsingProjectiles)) != null)
            {
                __result = -1000f;
                return false;
            }

            if (ap is Apparel_Shield)
            {
                foreach (LoadoutSlot potentialshield in pawn.GetLoadout().Slots)
                {
                    if (potentialshield.count < 1)
                        continue;
                    foreach (ThingCategoryDef ApparelItem in potentialshield.thingDef.thingCategories)
                    {
                        // we have a shield in the inventory
                        if (ApparelItem.defName == "Shields")
                        {
                            __result = -1000f;
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}