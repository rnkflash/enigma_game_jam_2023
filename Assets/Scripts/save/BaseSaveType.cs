using System;
using Newtonsoft.Json;

namespace SaveSystem {
    [Serializable, JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(Position), "Position")]
    [JsonSubtypes.KnownSubType(typeof(Rotation), "Rotation")]
    public abstract class BaseSaveType
    {
        public abstract string type { get; }
    }
}
