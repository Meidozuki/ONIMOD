using HarmonyLib;
using Klei.Actions;
using UnityEngine;

namespace AutoCompost
{
    public class Patches
    {
        [HarmonyPatch(typeof(CompostConfig))]
        public class ConfigPatch
        {
            [HarmonyPatch(nameof(CompostConfig.CreateBuildingDef))]
            public static void Postfix(ref BuildingDef __result)
            {
                __result.ExhaustKilowattsWhenActive = 0f;
                __result.SelfHeatKilowattsWhenActive = 0.5f;
            }
            
            [HarmonyPatch(nameof(CompostConfig.ConfigureBuildingTemplate))]
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
                return;
            }
        }
        

        [HarmonyPatch(typeof(Compost.States))]
        [HarmonyPatch("InitializeStates")]
        public class AutoPatch
        {
            
            public static void Postfix(Compost.States __instance)
            {
                ref var self = ref __instance;
                var actions = self.composting.enterActions;
                Debug.Log("Actions Length "+ actions.Count.ToString());
                actions.RemoveAt(actions.Count - 1);
            }

        }

        [HarmonyPatch(typeof(Compost), MethodType.Constructor)]
        public class Patch1
        {
            public static void Postfix(ref Compost __instance)
            {
                Debug.Log("after constructor");
                // __instance.flipInterval = 60f;
            }
        }

    }
}
