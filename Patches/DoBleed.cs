using SPTushonka.Reflection.Patching;
using EFT.HealthSystem;
using HarmonyLib;
using System;
using System.Reflection;
using EFT;

namespace Deminvincibility.Patches
{
    internal class DoBleed : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            // SPT 5.0 also has a DoBleed(bool isHeavy, EBodyPart bodyPart) overload, so pick the body part only one
            return AccessTools.Method(typeof(ActiveHealthController), nameof(ActiveHealthController.DoBleed), new[] { typeof(EBodyPart) });
        }

        [PatchPrefix]
        private static bool Prefix(ActiveHealthController __instance, EBodyPart bodyPart)
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

                // Skip the bleed effect while both 1 HP mode and side effect removal are on
                return !(DeminvicibilityPlugin.Keep1Health.Value && DeminvicibilityPlugin.MedicineBool.Value);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return true;
        }
    }
}
