using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    //Create an integer named randomInt with a random range between 1 and 10 (Inclusive)
    public int riseSpeed = 0;
    private int randomInt;

    private void Start() {
        randomInt = Random.Range(1, 10);
    }

    private void Update() {
        this.transform.Translate(0, riseSpeed * Time.deltaTime, 0);
    }

    public void GetCollected() {
        if(randomInt > 6) {                                                         //If Speed is greater than 6 (7, 8, 9, or 10)
            this.GetComponent<MeshRenderer>().material.color = Color.green;         //turn material green
            this.GetComponent<Rigidbody>().isKinematic = true;                      //move up by changing the riseSpeed to 5
            riseSpeed = 5;
            Destroy(this.gameObject, 5f);                                           //destroy after a few seconds
        } else {                                                                    //else turn red
            this.GetComponent<MeshRenderer>().material.color = Color.red;
            Destroy(this.gameObject, 1f);                                           //destroy self after 1 second
        }
    }
}
