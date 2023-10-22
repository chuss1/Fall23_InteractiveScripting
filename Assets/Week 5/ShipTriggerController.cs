using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTriggerController : MonoBehaviour {
    //HOMEWORK: Week 5 

    private int totalObjectsCollected = 0;

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Cube") || other.gameObject.CompareTag("Capsule") || other.gameObject.CompareTag("Sphere") || other.gameObject.CompareTag("Cylinder")) {
            //Destroy(other.gameObject);
            other.GetComponent<ObjectController>().GetCollected();
            totalObjectsCollected += 1;
            Debug.Log("We have collected " + totalObjectsCollected + " cubes");
        }
    }
}
