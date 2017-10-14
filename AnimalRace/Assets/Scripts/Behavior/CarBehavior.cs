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
    public int Position { get; private set; }
    public string HorizontalMovement;
    public string VerticalMovement;
    [SerializeField]
    private string FIRE_AXIS;

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
        UpdateCameraPosition();
        MoveForward();
        if (currentSpeed != 0)
            TurnSideways();
        ReduceAbnormalStatusTime();
        FireSpecialPower();

    }

    private void UpdateCameraPosition()
    {
        var maxCameraDistance = 70;
        var minCameraDistance = 60;
        var differenceCamera = maxCameraDistance - minCameraDistance;
        var percentage = currentSpeed / maxSpeed;
        float fieldOfView = minCameraDistance + differenceCamera * percentage;
        GetComponentInChildren<Camera>().fieldOfView = fieldOfView;
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "Car")
        {
            col.rigidbody.freezeRotation = true;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Car")
        {
            col.rigidbody.freezeRotation = false;
        }
    }

    #region relacionado al movimiento

    private void Jump()
    {
        throw new NotImplementedException();
    }

    private void TurnSideways()
    {
        transform.Rotate(transform.up * rotationSpeed * Time.deltaTime * Input.GetAxis(HorizontalMovement));
    }

    public void ChangeMaxSpeed(float multiplier)
    {
        maxSpeed *= multiplier;
    }

    private void MoveForward()
    {
        if (Input.GetAxis(VerticalMovement) > 0)
        {
            ChangeSpeed(aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        if (Input.GetAxis(VerticalMovement) == 0 && currentSpeed > 0)
        {
            ChangeSpeed(-aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }

        if (Input.GetAxis(VerticalMovement) < 0)
        {
            ChangeSpeed(-aceleration);
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        if (Input.GetAxis(VerticalMovement) == 0 && currentSpeed < 0)
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
        if (Input.GetAxisRaw(FIRE_AXIS) != 0)
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
                myPowerUp = ConstantsHelper.ALL_POWERUPS[1];//SpecialPowerBuilder.CreateRandomPower(this);
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
