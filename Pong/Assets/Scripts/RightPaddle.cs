using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPaddle : PaddleController
{
     protected override string GetInputAxis()
    {
        return "RightPaddle";
    }
}
