using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : PaddleController
{
    protected override float GetMovementInput()
    {
        return Input.GetAxis("LeftPaddle");
    }
}
