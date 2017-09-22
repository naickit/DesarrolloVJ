using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour {
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float rotationSpeed = 100;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveForward();
        TurnSideways();
        //Jump();
        //FireSpecialPower();
    }

    private void FireSpecialPower()
    {
        throw new NotImplementedException();
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
        transform.position += transform.forward * speed * Time.deltaTime * Input.GetAxis("Vertical");
    }
}
