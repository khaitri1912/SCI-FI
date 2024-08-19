using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MovementBaseStates
{
    public override void EnterState(PlayerController movement)
    {
        movement.animator.SetBool("isWalking", true);
    }

    public override void UpdateState(PlayerController movement)
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            ExitState(movement, movement.Run);
        }
        else if(movement.move.magnitude < 0.1f)
        {
            ExitState(movement, movement.Idle);
        }

        if (movement.VerticalInput > 0 || movement.HorizontalInput > 0)
        {
            movement.currentSpeed = movement.playerSpeed;
            //Debug.Log("Di bo: " + movement.currentSpeed);
        }
    }

    public void ExitState(PlayerController movement, MovementBaseStates state)
    {
        movement.animator.SetBool("isWalking", false);
        movement.SwitchState(state);
    }
}
