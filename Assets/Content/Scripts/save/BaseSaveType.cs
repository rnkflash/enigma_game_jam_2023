using System;
using Newtonsoft.Json;

namespace SaveSystem {
    [Serializable, JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(Position), "Position")]
    [JsonSubtypes.KnownSubType(typeof(RigidBody), "RigidBody")]
    public abstract class BaseSaveType
    {
        public abstract string type { get; }
    }
}
