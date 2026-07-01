using HarmonyLib;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using UnityEngine;

namespace DesaltMachine
{
	public class Patches : KMod.UserMod2
	{
		private const float INPUT_RATE = 5f;
		private const float SALT_WATER_TO_SALT_OUTPUT_RATE = 0.35f;
		private const float SALT_WATER_TO_CLEAN_WATER_OUTPUT_RATE = 4.65f;
		private const float BRINE_TO_SALT_OUTPUT_RATE = 1.5f;
		private const float BRINE_TO_CLEAN_WATER_OUTPUT_RATE = 3.5f;
		private const float MURKYBRINE_TO_SALT_OUTPUT_RATE = 1.5f;
		private const float MURKYBRINE_TO_DIRTY_WATER_OUTPUT_RATE = 3.5f;

		public override void OnLoad(Harmony harmony)
		{
			base.OnLoad(harmony);
			PUtil.InitLibrary(false);
			new POptions().RegisterOptions(this, typeof(DesaltOption));

			Debug.Log("DesaltMachine load. Multiplier=" + GetOptions().OutputMultiplier);
		}

		private static DesaltOption GetOptions()
		{
			return SingletonOptions<DesaltOption>.Instance;
		}

		[HarmonyPatch(typeof(DesalinatorConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SaltWaterPatch
		{
			public static void Postfix(GameObject go)
			{
				// go.AddOrGet<Desalinator>().maxSalt = GetOptions().MaxSalt;
				
				int multiplier = GetOptions().OutputMultiplier;

				ElementConverter[] components = go.GetComponents<ElementConverter>();
				Debug.Log((object)"Salt converters num:");
				Debug.Log((object)components.Length);

				// SaltWater -> Water + Salt
				ElementConverter saltWaterConverter = components[0];
				saltWaterConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("SaltWater"), INPUT_RATE * multiplier)
				};
				saltWaterConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(SALT_WATER_TO_CLEAN_WATER_OUTPUT_RATE * multiplier, SimHashes.Water, 273.15f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(SALT_WATER_TO_SALT_OUTPUT_RATE * multiplier, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				// Brine -> Water + Salt
				ElementConverter brineConverter = components[1];
				brineConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("Brine"), INPUT_RATE * multiplier)
				};
				brineConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(BRINE_TO_CLEAN_WATER_OUTPUT_RATE * multiplier, SimHashes.Water, 273.15f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(BRINE_TO_SALT_OUTPUT_RATE * multiplier, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				// MurkyBrine -> DirtyWater + Salt
				ElementConverter murkyBrineConverter = components[2];
				murkyBrineConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("MurkyBrine"), INPUT_RATE * multiplier)
				};
				murkyBrineConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(MURKYBRINE_TO_DIRTY_WATER_OUTPUT_RATE * multiplier, SimHashes.DirtyWater, 0.0f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(MURKYBRINE_TO_SALT_OUTPUT_RATE * multiplier, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.consumptionRate = 10f * multiplier;
				conduitConsumer.capacityKG = 20f * multiplier;
			}
		}
	}
}
