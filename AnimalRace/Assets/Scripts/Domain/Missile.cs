using UnityEditor;
using UnityEngine;

internal class Missile : SpecialPower
{
    internal override void Activate(CarBehavior car)
    {
        Transform missile = car.GetPowerUp(PowerUpsHolderObject.MISSILE_POSITION);
        float missileLengthHorizontally = missile.GetComponent<Renderer>().bounds.extents.z;
        Vector3 missilePosition = car.GetLauncherPosition() + (car.transform.forward * missileLengthHorizontally);
        Object.Instantiate(
            missile,
            missilePosition, 
            car.GetLauncherRotation());
    }
}