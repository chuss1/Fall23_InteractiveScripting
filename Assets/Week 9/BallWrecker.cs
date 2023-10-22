using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallWrecker : MonoBehaviour {
    [SerializeField] private float returnDelay = 1f;
    [SerializeField] private float launchForce = 30f;

    private Rigidbody rb;
    private Transform ballStart;
    private bool readyToLaunch = true;

    private void Start() {
        rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        ballStart = GameObject.Find("BallStart").transform;
    }

    private void Update() {

        if(readyToLaunch) {
            this.transform.position = ballStart.position;
            this.transform.rotation = ballStart.rotation;
        }
    }

    public void Launch() {
        readyToLaunch = false;
        StartCoroutine(Return());
        rb.isKinematic = false;
        rb.AddForce(ballStart.forward * launchForce, ForceMode.Impulse);
    }

    private IEnumerator Return() {
        yield return new WaitForSeconds(returnDelay);
        rb.isKinematic = true;

        float counter = 0;
        float intervalInSeconds = 1;
        
        Vector3 startPosition = this.transform.position;
        Quaternion startRotation = this.transform.rotation;

        //Vector3 endPosition = ballStart.position;

        while(counter < intervalInSeconds) {
            counter += Time.deltaTime;

            this.transform.position = Vector3.Lerp(startPosition, ballStart.position, counter/intervalInSeconds);
            this.transform.rotation = Quaternion.Lerp(startRotation, ballStart.rotation, counter/intervalInSeconds);
            
            yield return new WaitForEndOfFrame();
        }

        readyToLaunch = true;
    }
}
