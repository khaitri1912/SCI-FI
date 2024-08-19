using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBaseStates
{
    public abstract void EnterState(PlayerController movement);
    public abstract void UpdateState(PlayerController movement);            
}
