using System;
using System.Reflection;
using SPTushonka.Reflection.Patching;
using EFT.HealthSystem;
using HarmonyLib;
using EFT;
using EFT.Ballistics;

namespace Deminvincibility.Patches
{
    internal class ChangeHealth : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ActiveHealthController), nameof(ActiveHealthController.ChangeHealth));
        }

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance, EBodyPart bodyPart, ref float value, DamageInfo damageInfo)
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

                // Only clamp health loss. Heals (positive value) are always left alone.
                if (!DeminvicibilityPlugin.Keep1Health.Value || value >= 0f)
                {
                    return true;
                }

                var currentHealth = __instance.GetBodyPartHealth(bodyPart, false).Current;

                // Head and Chest are always protected; other limbs are only protected
                // when dropping to 0 HP is disallowed - same rule as ApplyDamage
                var isProtectedPart = bodyPart == EBodyPart.Head
                    || bodyPart == EBodyPart.Chest
                    || !DeminvicibilityPlugin.Allow0HpLimbs.Value;

                if (isProtectedPart && currentHealth + value < 1f)
                {
                    // Clamp the delta so the limb lands exactly on 1 HP instead of blacking out.
                    // Math.Min keeps this from ever healing: a limb already below 1 HP stays put.
                    value = Math.Min(0f, 1f - currentHealth);
                }

                // Let the original method run with the clamped delta
                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return true;
        }
    }
}
