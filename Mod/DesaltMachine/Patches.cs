using HarmonyLib;
using UnityEngine;

namespace DesaltMachine
{
	public class Patches
	{
		[HarmonyPatch(typeof (DesalinatorConfig))]
		[HarmonyPatch("ConfigureBuildingTemplate")]
		public class SaltWaterPatch
		{
			public static void Postfix(GameObject go)
			{
				ElementConverter[] components = go.GetComponents<ElementConverter>();
				Debug.Log((object) "Salt converters num:");
				Debug.Log((object) components.Length);

				// SaltWater -> Water + Salt
				ElementConverter saltWaterConverter = components[0];
				saltWaterConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("SaltWater"), 20f)
				};
				saltWaterConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(18.6f, SimHashes.Water, 273.15f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(1.4f, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				// Brine -> Water + Salt
				ElementConverter brineConverter = components[1];
				brineConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("Brine"), 20f)
				};
				brineConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(14f, SimHashes.Water, 273.15f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(6f, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				// MurkyBrine -> DirtyWater + Salt
				ElementConverter murkyBrineConverter = components[2];
				murkyBrineConverter.consumedElements = new ElementConverter.ConsumedElement[1]
				{
					new ElementConverter.ConsumedElement(new Tag("MurkyBrine"), 20f)
				};
				murkyBrineConverter.outputElements = new ElementConverter.OutputElement[2]
				{
					new ElementConverter.OutputElement(14f, SimHashes.DirtyWater, 0.0f, storeOutput: true, diseaseWeight: 0.75f),
					new ElementConverter.OutputElement(6f, SimHashes.Salt, 0.0f, storeOutput: true, diseaseWeight: 0.25f)
				};

				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.consumptionRate = 20f;
				conduitConsumer.capacityKG = 200f;
			}
		}
	}

}
