using System;
using Newtonsoft.Json;

namespace SaveSystem {
    [Serializable, JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(Position), "Position")]
    [JsonSubtypes.KnownSubType(typeof(RigidBody), "RigidBody")]
    [JsonSubtypes.KnownSubType(typeof(Item), "Item")]
    [JsonSubtypes.KnownSubType(typeof(Door), "Door")]
    public abstract class BaseSaveType
    {
        public abstract string type { get; }
    }
}
