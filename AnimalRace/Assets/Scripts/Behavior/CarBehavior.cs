using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    //[SerializeField]
    private PowerUpsHolderObject powerUps;
    //[SerializeField]
    private Transform missileLauncher;
    private SpecialPower myPowerUp;
    private float currentSpeed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float aceleration;
    [SerializeField]
    private float reverseMaxSpeed;
    [SerializeField]
    private Vector3 impulseForce;

    [SerializeField]
    private float rotationSpeed;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();

    public PowerUpsHolderObject PowerUps { get { return powerUps; } }
    public Transform MissileLauncher { get { return missileLauncher; } }
    public int Position { get; private set; }

    // Use this for initialization
    void Start()
    {
        Position = 1;
        currentSpeed = 0;
        maxSpeed = 20;
        aceleration = 2;
        reverseMaxSpeed = -10;
        impulseForce.Set(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        if (currentSpeed != 0)
            TurnSideways();
        //Jump();
        ReduceAbnormalStatusTime();
        FireSpecialPower();
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Car")
        {
            Vector3 collForce = transform.forward * currentSpeed / 2;
            impulseForce.Set(collForce.x, 0, collForce.z);
            col.rigidbody.velocity = Vector3.zero;
            col.rigidbody.angularVelocity = Vector3.zero;
            col.rigidbody.AddForce(impulseForce*2, ForceMode.Impulse);
            this.gameObject.GetComponent<Rigidbody>().AddForce(-impulseForce*2, ForceMode.Impulse); //sacar cuando se agregen mas autos
        }


    }

    #region relacionado al movimiento

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
        if (Input.GetAxis("Vertical") > 0)
        {
            ChangeSpeed(aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") == 0 && currentSpeed > 0)
        {
            ChangeSpeed(-aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            ChangeSpeed(-aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") == 0 && currentSpeed < 0)
        {
            ChangeSpeed(+aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
    }

    internal void ChangeSpeed(float aceleration)
    {
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += aceleration;
        }
        if (currentSpeed > maxSpeed)
            currentSpeed = maxSpeed;
        if (currentSpeed < reverseMaxSpeed)
            currentSpeed = reverseMaxSpeed;
    }
    #endregion

    #region PowerUps
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


    private void ReduceAbnormalStatusTime()
    {
        for (var i = 0; i < abnormalStatuses.Count; i++)
        {
            AbnormalStatus status = abnormalStatuses[i];
            status.ReduceTime(Time.deltaTime);
            if (status.Duration <= 0)
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
    #endregion
}
