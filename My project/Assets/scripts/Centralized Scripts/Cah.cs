using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cah : MonoBehaviour
{
    private static CashText textScript;
    private static int Cash = 0;
    public static int cash { get { return Cash; } private set { Cash = value; } }

    private void Start()
    {
        textScript = GetComponent<CashText>();
        textScript.ChangeText(cash);
        HitManager.onHit += AddCash;
    }

    public static void AddCash(int value)
    {
        cash += value;
        textScript.ChangeText(cash);
    }
}
