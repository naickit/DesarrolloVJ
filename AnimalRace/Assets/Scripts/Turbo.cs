using System;

internal class Turbo : SpecialPower
{
    private const double TURBO_MULTIPLIER = 2;
    private const double TURBO_DURATION = 2;
    internal override void Activate(CarBehavior car)
    {
        //Comentado para no hacer lio con nico que tiene que hacer lo del movimiento
        //car.ChangeSpeed(TURBO_MULTIPLIER, TURBO_DURATION);
    }
}