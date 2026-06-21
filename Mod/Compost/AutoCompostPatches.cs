using HarmonyLib;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using UnityEngine;

namespace AutoCompost
{
    public class AutoCompostPatches: KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(AutoCompostOption));
            
            UnityEngine.Debug.Log("AutoCompost load." + SingletonOptions<AutoCompostOption>.Instance.ToString());
            
        }

        private static AutoCompostOption GetOptions()
        {
            var options = SingletonOptions<AutoCompostOption>.Instance;
            return options;
        }

        [HarmonyPatch(typeof(CompostConfig))]
        public class ConfigPatch
        {
            [HarmonyPatch(nameof(CompostConfig.CreateBuildingDef))]
            public static void Postfix(ref BuildingDef __result)
            {
                __result.ExhaustKilowattsWhenActive = GetOptions().ExhaustKilowatts;
                __result.SelfHeatKilowattsWhenActive = GetOptions().SelfHeatKilowatts;
            }
            
            [HarmonyPatch(nameof(CompostConfig.ConfigureBuildingTemplate))]
            public static void Postfix(GameObject go)
            {
                ManualDeliveryKG manualDelivery =  go.AddOrGet<ManualDeliveryKG>();
                manualDelivery.capacity = GetOptions().Capacity;
                manualDelivery.refillMass = GetOptions().RefillMass;
                
                // Half the work time
                go.AddOrGet<CompostWorkable>().workTime = 10f;
                // Hide Descriptor
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

    }
}
