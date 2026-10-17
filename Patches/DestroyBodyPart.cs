using System;
using System.Reflection;
using SPTushonka.Reflection.Patching;
using EFT;
using EFT.HealthSystem;
using HarmonyLib;

namespace Deminvincibility.Patches
{
    internal class DestroyBodyPartPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ActiveHealthController), nameof(ActiveHealthController.DestroyBodyPart));
        }

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance, EBodyPart bodyPart, EDamageType damageType)
        {
            try
            {
                // IL2CPP interop exposes the controller's Player field as a property
                var player = __instance.Player;

                // Target is not our player - don't do anything
                if (player == null || !player.IsYourPlayer && player.IsAI)
                {
                    return true;
                }

                // If Keep1Health is disabled, don't do anything
                if (!DeminvicibilityPlugin.Keep1Health.Value)
                {
                    return true;
                }

                // If AllowBlacking is disabled, we prevent the original method from running regardless
                if (!DeminvicibilityPlugin.AllowBlacking.Value)
                {
                    return false;  // skip original method
                } 

                // If limb blacking is enabled, but Head & Thorax are protected (and being currently hit), we'll skip the original method here
                if (!DeminvicibilityPlugin.AllowBlackingHeadAndThorax.Value && (bodyPart == EBodyPart.Head || bodyPart == EBodyPart.Chest))
                {
                    return false; // skip original method
                }

                // In all other cases, we let the original method run
                return true; // run original method
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return true;
        }

    }
}
