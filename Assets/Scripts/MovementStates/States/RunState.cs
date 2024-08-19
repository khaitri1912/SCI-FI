using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MovementBaseStates
{
    public override void EnterState(PlayerController movement)
    {
        movement.animator.SetBool("isRunning", true);
    }

    public override void UpdateState(PlayerController movement)
    {
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            ExitState(movement, movement.Walk);
        }
        else if(movement.move.magnitude < 0.1f)
        {
            ExitState(movement, movement.Idle);
        }

        if (movement.VerticalInput > 0 || movement.HorizontalInput > 0)
        {
            movement.currentSpeed = movement.runSpeed;
            //Debug.Log("Chay:" + movement.currentSpeed);
        }
    }

    public void ExitState(PlayerController movement, MovementBaseStates state)
    {
        movement.animator.SetBool("isRunning", false);
        movement.SwitchState(state);
    }
}
