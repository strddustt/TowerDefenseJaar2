using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASingleActionCall : MonoBehaviour
{
    public static event Action skipped;

    public void Skip()
    {
        skipped?.Invoke();
    }
}
