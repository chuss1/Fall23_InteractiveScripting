using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {
    Rigidbody rb;

    [SerializeField] float speed = 10;
    [SerializeField] float rotationSpeed = 1f;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
    }

    public void Move(float zAxis, float yAxis) {
        rb.AddRelativeForce(0, 0, zAxis * speed);
        rb.AddTorque(0, yAxis * rotationSpeed, 0);
    }
}
