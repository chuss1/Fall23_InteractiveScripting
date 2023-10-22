//ensure that the camera is the first child of the game object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamera : MonoBehaviour {
    private float startingFOV = 75;
    [SerializeField]private Camera cam;

    private void Start() {
        cam = this.gameObject.transform.GetChild(0).GetComponent<Camera>();
        startingFOV = cam.fieldOfView;
    }

    public void ZoomIn() {
        cam.fieldOfView = startingFOV - 15;
    }

    public void DefaultZoom() {
        cam.fieldOfView = startingFOV;
    }

}
