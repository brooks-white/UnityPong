using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        return Input.GetAxis("RightPaddle");
    }
}
