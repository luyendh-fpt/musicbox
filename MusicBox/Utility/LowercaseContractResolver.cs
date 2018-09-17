using Newtonsoft.Json.Serialization;

namespace MusicBox.Utility
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string key)
        {
            return key.ToLower();
        }
    }
}