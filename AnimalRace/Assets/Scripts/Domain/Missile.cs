using UnityEditor;
using UnityEngine;

internal class Missile : SpecialPower
{
    internal override void Activate(CarBehavior car)
    {
        Object.Instantiate(
            car.GetPowerUp(PowerUpsHolderObject.MISSILE_POSITION), 
            car.GetLauncherPosition(), 
            car.GetLauncherRotation());
    }
}