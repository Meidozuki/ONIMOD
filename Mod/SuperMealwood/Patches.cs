using System;
using HarmonyLib;
using UnityEngine;

namespace SuperMealwood
{
    public class Patches
    {

        [HarmonyPatch(typeof(Crop.CropVal))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(string), typeof(float), typeof(int), typeof(bool) })]
        public class MealwoodTimePatch
        {
            const string BasicPlant = "BasicPlantFood";

            public static void Postfix(ref Crop.CropVal __instance)
            {
                if (__instance.cropId == BasicPlant)
                {
                    Debug.Log("Modifying Mealwood...");
                    Debug.Log(STRINGS.CREATURES.SPECIES.BASICSINGLEHARVESTPLANT.NAME);
                    __instance.cropDuration = 600f;
                    PrintCropInfo(__instance);
                }
            }

            private static void PrintCropInfo(Crop.CropVal crop)
            {
                Debug.Log("crop id: " + crop.cropId + ", duration: " + crop.cropDuration);
            }
        }

        [HarmonyPatch(typeof(BasicSingleHarvestPlantConfig))]
        [HarmonyPatch("CreatePrefab")]
        public class MealwoodPatch
        {
            public static void Postfix(ref GameObject __result)
            {
                ref var go = ref __result;
                
                var pressureVulnerable = go.AddOrGet<PressureVulnerable>();
                pressureVulnerable.Configure(new SimHashes[3]
                {
                    SimHashes.Oxygen,
                    SimHashes.ContaminatedOxygen,
                    SimHashes.CarbonDioxide
                });

                float temperature_lethal_low = 218.15f;
                float temperature_warning_low = 273.15f;
                float temperature_warning_high = 313.15f;
                float temperature_lethal_high = 398.15f;
                go.AddOrGet<TemperatureVulnerable>().
                    Configure(temperature_warning_low,temperature_lethal_low,temperature_warning_high,temperature_lethal_high);

            }

        }
    }
}
