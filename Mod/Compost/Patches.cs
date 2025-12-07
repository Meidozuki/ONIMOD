using HarmonyLib;
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
                // Reduce the heat produced
                __result.ExhaustKilowattsWhenActive = 0f;
                __result.SelfHeatKilowattsWhenActive = 0.1f;
            }
            
            [HarmonyPatch(nameof(CompostConfig.ConfigureBuildingTemplate))]
            public static void Postfix(GameObject go)
            {
                // Double the capacity
                go.AddOrGet<ManualDeliveryKG>().capacity = 600f;
                // Half the work time
                go.AddOrGet<CompostWorkable>().workTime = 10f;
                // Text Description
                go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
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
                Debug.Log("FSM Compost.composting Actions Length "+ actions.Count.ToString());
                actions.RemoveAt(actions.Count - 1);

                self.inert.ScheduleGoTo((smi) => 10.1f, self.composting);
            }

        }

        [HarmonyPatch(typeof(Compost), MethodType.Constructor)]
        public class DebugPatch
        {
            public static void Postfix(ref Compost __instance)
            {
                Debug.Log("after constructor");
                // __instance.flipInterval = 60f;
            }
        }

    }
}
