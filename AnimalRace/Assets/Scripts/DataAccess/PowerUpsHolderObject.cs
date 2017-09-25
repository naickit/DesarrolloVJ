using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUps.asset", menuName = "Power Ups")]
public class PowerUpsHolderObject : ScriptableObject
{
    public Transform[] powerUps;
    private static PowerUpsHolderObject instance;
    internal static readonly int MISSILE_POSITION = 0;

    public static PowerUpsHolderObject GetInstance()
    {
        if(instance == null)
        {
            instance = CreateInstance<PowerUpsHolderObject>();
        }
        return instance;
    }
}
