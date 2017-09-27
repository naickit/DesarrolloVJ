using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    [SerializeField]
    private PowerUpsHolderObject powerUps;
    [SerializeField]
    private Transform missileLauncher;
    private SpecialPower myPowerUp;
    [SerializeField]
    private float initialSpeed;
    private float currentSpeed;

    [SerializeField]
    private float rotationSpeed;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();

    public PowerUpsHolderObject PowerUps { get { return powerUps; } }
    public Transform MissileLauncher { get { return missileLauncher; } }
    public int Position { get; private set; }

    internal Vector3 GetLauncherPosition()
    {
        return missileLauncher.position;
    }
    internal Quaternion GetLauncherRotation()
    {
        return missileLauncher.transform.rotation;
    }
    internal Transform GetPowerUp(int powerUp)
    {
        return PowerUps.powerUps[powerUp];
    }

    internal void RemoveAbnormalStatus(Turbo turbo)
    {
        abnormalStatuses.Remove(turbo);
    }

    internal void AddAbnormalStatus(Turbo turbo)
    {
        abnormalStatuses.Add(turbo);
    }

    internal void ChangeSpeed(float multiplier)
    {
        //deberia cappear esto usando initial speed o dejo que sea posible stackear turbos?
        currentSpeed *= multiplier;
    }

    // Use this for initialization
    void Start()
    {
        Position = 1;
        currentSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        TurnSideways();
        //Jump();
        ReduceAbnormalStatusTime();
        FireSpecialPower();
    }

    private void ReduceAbnormalStatusTime()
    {
        for (var i=0; i<abnormalStatuses.Count; i++)
        {
            AbnormalStatus status = abnormalStatuses[i];
            status.ReduceTime(Time.deltaTime);
            if(status.Duration <= 0)
            {
                status.Deactivate(this);
            }
        }
    }

    private void FireSpecialPower()
    {
        if (Input.GetAxisRaw(ConstantsHelper.POWERUP_AXIS) != 0)
        {
            if (myPowerUp != null)
            {
                myPowerUp.Activate(this);
                myPowerUp = null;
            }
        }
    }

    private void Jump()
    {
        throw new NotImplementedException();
    }

    private void TurnSideways()
    {
        transform.Rotate(transform.up * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
    }

    private void MoveForward()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime * Input.GetAxis("Vertical");
    }

    void OnTriggerEnter(Collider otherObject)
    {
        if (IsPowerUpBox(otherObject))
        {
            RunPowerUpBoxCollisionAnimation();
            RunPowerupBoxCollisionSound();
            if (CanHaveNewPowerUp())
            {
                RunRandomPowerUpAnimation();
                RunRandomPowerUpSound();
                myPowerUp = SpecialPowerBuilder.CreateRandomPower(this);
            }
            RemoveObject(otherObject);
        }
    }

    private void RunPowerUpBoxCollisionAnimation()
    {
        return;
    }

    private void RunPowerupBoxCollisionSound()
    {
        return;
    }

    private bool CanHaveNewPowerUp()
    {
        return myPowerUp == null;
    }

    private bool IsPowerUpBox(Collider otherObject)
    {
        return ConstantsHelper.POWERUP_BOX.Equals(otherObject.tag);
    }

    private void RunRandomPowerUpAnimation()
    {
        return;
    }

    private void RunRandomPowerUpSound()
    {
        return;
    }

    private void RemoveObject(Collider otherObject)
    {
        Destroy(otherObject.gameObject);
    }
}
