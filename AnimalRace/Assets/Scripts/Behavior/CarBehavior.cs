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

    private Vector3 startingPosition;

    [SerializeField]
    private float rotationSpeed;
    private List<AbnormalStatus> abnormalStatuses = new List<AbnormalStatus>();

    public PowerUpsHolderObject PowerUps { get { return powerUps; } }
    public string HorizontalMovement;
    public string VerticalMovement;
    [SerializeField]
    private string FIRE_AXIS;

    // Use this for initialization
    void Start()
    {
        startingPosition = transform.position;
        if (maxSpeed == 0) maxSpeed = 20;
        if (aceleration == 0) aceleration = 2;
        if (reverseMaxSpeed == 0) reverseMaxSpeed = -10;
        if (reverseMaxSpeed >0) reverseMaxSpeed *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        if (currentSpeed != 0)
            TurnSideways();
        ReduceAbnormalStatusTime();
        FireSpecialPower();
        RespawnAfterEndlessFall();
    }

    private void RespawnAfterEndlessFall()
    {
        if(transform.position.y < 30)
        {
            transform.rotation = new Quaternion(0,0,0,0);
            transform.position = startingPosition;
        }
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


    void OnCollisionEnter(Collision collisioned)
    {

        if (collisioned.gameObject.tag == "Car")
        {
            collisioned.rigidbody.freezeRotation = true;
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
        reverseMaxSpeed *= multiplier;
    }

    private void MoveForward()
    {
        if (Input.GetAxis(VerticalMovement) > 0)
        {
            UpdateCameraPosition();
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

    internal void ChangeSpeed(float acceleration)
    {
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += acceleration;
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

    internal void RemoveAbnormalStatus(AbnormalStatus status)
    {
        abnormalStatuses.Remove(status);
    }

    internal void AddAbnormalStatus(AbnormalStatus status)
    {
        abnormalStatuses.Add(status);
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
            //TODO
            //RunPowerUpBoxCollisionAnimation();
            //RunPowerupBoxCollisionSound();
            if (CanHaveNewPowerUp())
            {
                //TODO
                //RunRandomPowerUpAnimation();
                //RunRandomPowerUpSound();
                myPowerUp = SpecialPowerBuilder.CreateRandomPower(this);
            }
            RemoveObject(otherObject);
        }
    }
    
    //TODO
    //private void RunPowerUpBoxCollisionAnimation()
    //{
    //    return;
    //}

    //TODO
    //private void RunPowerupBoxCollisionSound()
    //{
    //    return;
    //}

    private bool CanHaveNewPowerUp()
    {
        return myPowerUp == null;
    }

    private bool IsPowerUpBox(Collider otherObject)
    {
        return ConstantsHelper.POWERUP_BOX.Equals(otherObject.tag);
    }
    //TODO
    //private void RunRandomPowerUpAnimation()
    //{
    //    return;
    //}
    //private void RunRandomPowerUpSound()
    //{
    //    return;
    //}

    private void RemoveObject(Collider otherObject)
    {
        Destroy(otherObject.gameObject);
    }
    #endregion
}
