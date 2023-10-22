using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour {
    [SerializeField] ShipCamera shipCam;
    private ShipMovement shipMove;
    [SerializeField] private BallWrecker wreckingBall;

    private void Start() {
        if(shipCam == null) shipCam = this.GetComponent<ShipCamera>();
        if(shipMove == null) shipMove = this.GetComponent<ShipMovement>();
        if(wreckingBall == null) wreckingBall = FindObjectOfType<BallWrecker>();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse2)) shipCam.ZoomIn();
        if(Input.GetKeyUp(KeyCode.Mouse2)) shipCam.DefaultZoom();
        if(Input.GetKeyDown(KeyCode.Mouse0)) wreckingBall.Launch();
    }

    private void FixedUpdate() {
        shipMove.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
    }
}
