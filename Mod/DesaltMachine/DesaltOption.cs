using Newtonsoft.Json;
using PeterHan.PLib.Options;

namespace DesaltMachine
{
    [JsonObject(MemberSerialization.OptIn)]
    [ModInfo("https://github.com/Meidozuki/ONIMOD/tree/main/Mod")]
    public class DesaltOption : SingletonOptions<DesaltOption>
    {
        [Option("Output Multiplier", "Multiplier applied to base output rates of the Desalinator. Need restart if changed.")]
        [Limit(1, 10)]
        [JsonProperty]
        public int OutputMultiplier { get; set; }
        
        // [Option("Max Salt Storage")]
        // [Limit(945, 10000)]
        // [JsonProperty]
        // public float MaxSalt { get; set; }

        public DesaltOption()
        {
            // Default keeps behavior identical to the original mod (base rates * 4).
            OutputMultiplier = 4;
            
            // MaxSalt = 2000f;
        }
    }
}
