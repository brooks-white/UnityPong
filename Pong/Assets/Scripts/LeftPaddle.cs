using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPaddle : PaddleController
{
    protected override string GetInputAxis()
    {
        return "LeftPaddle";
    }
}
