using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace AutoCompost
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("https://github.com/Meidozuki/ONIMOD/tree/main/Mod")]
    public class AutoCompostOption: SingletonOptions<AutoCompostOption>
    {
        [Option("Empty Heat kW", "Minimum heat that Compost produces. Need restart if changed.", "Thermal")]
        [Limit(0, 0.125)]
        [JsonProperty]
        public float ExhaustKilowatts { get; set; }
        
        [Option("Working Heat kW", "Extra heat when Compost is converting. Need restart if changed.", "Thermal")]
        [Limit(0, 1)]
        [JsonProperty]
        public float SelfHeatKilowatts { get; set; }
        
        [Option("Max Storage Mass", null, "Delivery")]
        [Limit(300, 3000)]
        [JsonProperty]
        public float Capacity { get; set; }
        
        [Option("Refill Mass", null, "Delivery")]
        [Limit(60, 300)]
        [JsonProperty]
        public float RefillMass { get; set; }
        
        public AutoCompostOption()
        {
            // Reduce the heat that it produces
            ExhaustKilowatts = 0f;
            SelfHeatKilowatts = 0.1f;
            // Double the capacity
            Capacity = 600f;
            
            // Consistent
            RefillMass = 60f;
        }
    }
}