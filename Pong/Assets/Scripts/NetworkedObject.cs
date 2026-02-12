using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NetworkedObject : MonoBehaviour
{
    // Called when the object is created or spawned
    public abstract void Initialize();

    // Returns a unique ID for networking purposes
    public abstract int GetNetworkId();
}
