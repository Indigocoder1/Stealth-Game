using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovement : MonoBehaviour
{
    private void Start()
    {
        
    }

    /*
    private void HandleJump()
    {
        if (!grounded)
        {
            return;
        }

        if (timeSinceLastJump < minTimeSinceLastJump)
        {
            timeSinceLastJump += Time.deltaTime;
            return;
        }

        if (playerActions.Player.AscendDescend.ReadValue<float>() > 0)
        {
            rb.AddForce(-gravityDirection * jumpForce * 10f, ForceMode.Force);
        }
    }
    */
}
