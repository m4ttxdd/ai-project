using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/ReachedThrower")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "ReachedThrower", message: "Agent has reached Thrower", category: "Events", id: "f269071a9c999482b8890fe3dedfd80c")]
public sealed partial class ReachedThrower : EventChannel { }

