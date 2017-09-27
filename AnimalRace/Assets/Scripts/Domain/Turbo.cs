
using System;

internal class Turbo : SpecialPower, AbnormalStatus
{
    private const float TURBO_MULTIPLIER = 2;
    private const double TURBO_DURATION = 20;
    private double remainingDuration;

    public double Duration
    {
        get
        {
            return remainingDuration;
        }
    }

    internal override void Activate(CarBehavior car)
    {
        remainingDuration = TURBO_DURATION;
        car.AddAbnormalStatus(this);
        car.ChangeSpeed(TURBO_MULTIPLIER);
    }

    void AbnormalStatus.Deactivate(CarBehavior car)
    {
        car.RemoveAbnormalStatus(this);
        car.ChangeSpeed(1/TURBO_MULTIPLIER);
    }

    void AbnormalStatus.ReduceTime(float deltaTime)
    {
        remainingDuration -= deltaTime;
    }
}