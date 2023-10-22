using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CreateCube : MonoBehaviour {
#region Synopsis
    //Synopsis
    //Complete script for the cube spawner for Week 3 of Interactive Scripting
    //Gives the ability to create cubes in a certain area.


    //Challenge 1: COMPLETE
    // Change the Color of each spawned object
    // Change how many objects are spawned
    // Change the time between spawns
    // Change the size of the spawn area
    // Add physics to the spawned object
    // Reset the spawn logic on a key press

    //Challenge 2: 
    //create an array of names
    //assign a random name to each new cube
    // BONUS (append a new number fore each duplicate name e.g. tom2, betty4)
    //HOMEWORK: Use an array of colors to choose the random color of your cubes.
#endregion

#region Variables
    #region Editable Variables
    //Editable Variables
    [SerializeField] List<GameObject> spawnableObjects; //Which object or objects do you want to spawn?
    [SerializeField] int amountOfObjectsToSpawn; //Total number of objects wanting to spawn
    
    [Range(0.1f, 5f)] //A slider for ease of use to set the time between object spawning
    [SerializeField] float timeBetweenSpawns;

    [Header("Spawn Ranges")] // Here is where you set your spawn range for x and z. y is always 2
    [SerializeField] float spawnRangeX;  
    [SerializeField] float spawnRangeZ;

    [Header("Spawn Scales")] //This is where you set your spawn scale for the objects if you want it randomized
    [SerializeField] float spawnScaleX; 
    [SerializeField] float spawnScaleY; 
    [SerializeField] float spawnScaleZ;
    #endregion

    #region  Optional Choices
    [Header("Optional Choices")]
    [SerializeField] bool useDebug;
    [SerializeField] bool usePhysics; //If you want your objects to have rigidbodies
    [SerializeField] bool useRandomColor; //If you want your objects to spawn with a random color
    [SerializeField] bool useRandomName; //If you want your objects to use name from the array
    [SerializeField] bool correlateNameToShape; //If you want your objects to be named a certain name
    [SerializeField] bool useDefaultScale; //If you want your objects to spawn with their prefabs default scale
    [SerializeField] bool spawnInfinite; //If you don't want to have an end to the spawning
    #endregion

    #region Private Variables
    //Private variables
    int objectsSpawned; //A counter for the amount of objects that have been spawned
    bool canReset; //A condition that will stop the spawning process when turned on
    Vector3 spawnableObjectScale; //A variable for setting the object scale to a random value
    List<GameObject> objectsToDelete = new List<GameObject>(); //A list of spawned objects so we can delete them when we restart
    #endregion

    #region Challenge 2 Variables
    [Header("Challenge 2")]
    [SerializeField] string[] names; // Creating an array of string variables titled "names". You set the names in the inspector.
    [SerializeField] Color[] colors;
    private Dictionary<string, int> spawnedObjectCounts = new Dictionary<string, int>();


    #endregion
#endregion

    private void Start() {
        Debug.Log("Press Shift+0 to enable debug mode.");
        if(useDebug) Debug.Log("The first name in the array is " + names[0]);
        if(useDebug) Debug.Log("Press R to restart");
        if(useDebug) Debug.Log("Press B to collect objects");
        
        if(spawnableObjects.Count == 0) { //We are checking if there are any objects in the spawn list 
            if(useDebug) Debug.LogError("No objects to spawn"); //If there aren't any objects to spawn, throw an error
        } else {
            GetRandomSpawnObject(); //Start the spawning process
        }

    }

    private void Update() {

        if(Input.GetKeyDown(KeyCode.R)) { //This section will check if we can wipe the created objects and restart or not.
            if(canReset || spawnInfinite) { //If it is infinitely spawning, you can restart at any time
                Restart();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            if(Input.GetKeyDown(KeyCode.Alpha0)) {
                useDebug = !useDebug; // Toggles the use debug boolean
                Debug.Log("Debug mode is now " + useDebug);
            }
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            //CollectObjects();
            StartCoroutine(MoveObjects());
        }
    }

    private void GetRandomSpawnObject() { //Pick a random object from the list to spawn
        var objectToSpawn = Random.Range(0, spawnableObjects.Count);
        
        SpawnObject(objectToSpawn); //Start the spawn sequence for that object
    }

    private void SpawnObject(int objectToSpawn) {
        if(!canReset || spawnInfinite) { //This checks to see if we are to the max number of spawnobjects or if infinite spawning is true
            GameObject prefabToSpawn = spawnableObjects[objectToSpawn]; //Using the objectToSpawn from the GetRandomSpawnObject function,
                                                                        //I find what that correlates to in the spawnableObjects
            Vector3 spawnPosition = GetRandomSpawnPosition(); //Set a random spawn position for the object that is spawning

            //CHALLENGE 2 BONUS/HOMEWORK/EXTRA CREDIT
            int randomIndex = Random.Range(0, names.Length); //gets an int from one of the arrays

            string randomName = names[randomIndex]; //set that int that is randomly grabbed from one of the arrays to be the name chosen
            Color randomColor = colors[randomIndex]; //set that int that is randomly grabbed from one of the arrays to be the color chosen

            if(correlateNameToShape && useRandomName) { //If the boolean is checked, each type of object spawned will spawn with the same name
                prefabToSpawn = spawnableObjects[randomIndex];
            }

            string finalName = randomName;
            if (useRandomName) {
                if (spawnedObjectCounts.ContainsKey(randomName)) {
                    finalName = randomName + spawnedObjectCounts[randomName];
                    spawnedObjectCounts[randomName]++;
                } else {
                    spawnedObjectCounts.Add(randomName, 1);
                }
            }

            GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            spawnedObject.name = finalName;


            ColorSpawnedObject(spawnedObject, randomColor); //Set the color of the newly spawned object

            if(!useDefaultScale) { //If the uniformScalingObjects bool is false, it will spawn with a custom scale randomly generated
                GetRandomSpawnScale();
                spawnedObject.transform.localScale = spawnableObjectScale;
            }
            
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if(usePhysics && rb == null) { //If we want physics for the object, we can have the spawner add a rigidbody
                rb = spawnedObject.AddComponent<Rigidbody>();
            }

            objectsToDelete.Add(spawnedObject); //This is a list for for wiping the board when we restart
            if(!spawnInfinite) { //This checks if we are spawning infinetley or if we have a limit
                objectsSpawned++;
                ObjectCounter(); //If we have a limit set, we run a check to see if we have reached the limit
            } else { //If infinite spawning is activated, we skip the check and start the countdown for the next spawn
                StartCoroutine(SpawnTimer());
            }
        }
    }

    private Vector3 GetRandomSpawnPosition() { //This generates a random position between the -spawnrange and +spawn range. Always spawn on Y 2
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        float randomZ = Random.Range(-spawnRangeZ, spawnRangeZ);
        Vector3 randomSpawnPosition = new Vector3(randomX, 2, randomZ);

        if(useDebug) Debug.Log("The Random Spawn Position is " + randomSpawnPosition);
        return randomSpawnPosition; //This is the random position that we give to the object when we spawn it
    }

    private Vector3 GetRandomSpawnScale() { //If we are getting a random scale for the object, this generates the random scale
        float randomX = Random.Range(0, spawnScaleX);
        float randomY = Random.Range(0, spawnScaleY);
        float randomZ = Random.Range(0, spawnScaleZ);
        spawnableObjectScale = new Vector3(randomX,randomY,randomZ);

        if(useDebug) Debug.Log("The Random Spawn Scale is " + spawnableObjectScale);
        return spawnableObjectScale; //This is what we input to set the random scale of the spawned object
    }

    private void ColorSpawnedObject(GameObject spawnedObject, Color randomColor) {
        if(useRandomColor) { //If random color is checked, we set the color to a random color, otherwise it will use the prefabs default
            //Color newColor = Random.ColorHSV(); THIS IS THE OLD VERSION THAT DOESN'T USE THE "colors" ARRAY
            Color newColor = randomColor;
            spawnedObject.GetComponent<MeshRenderer>().material.color = newColor;

            if(useDebug) Debug.Log("Setting color to " + newColor);
        }
    }

    IEnumerator MoveObjects() {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] cylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        GameObject[] capsules = GameObject.FindGameObjectsWithTag("Capsule");
        GameObject[] spheres = GameObject.FindGameObjectsWithTag("Sphere");
        
        for(int cubeCount = 0; cubeCount < cubes.Length; cubeCount++) {
            cubes[cubeCount].transform.position = new Vector3(0,2,0);
            yield return new WaitForSeconds(0.1f);
        }

        for(int cylinderCount = 0; cylinderCount < cylinders.Length; cylinderCount++) {
            cylinders[cylinderCount].transform.position = new Vector3(4,2,0);
            yield return new WaitForSeconds(0.1f);
        }

        for(int capsuleCount = 0; capsuleCount < capsules.Length; capsuleCount++) {
            capsules[capsuleCount].transform.position = new Vector3(-4,2,0);
            yield return new WaitForSeconds(0.1f);
        }

        for(int sphereCount = 0; sphereCount < spheres.Length; sphereCount++) {
            spheres[sphereCount].transform.position = new Vector3(0,2,-4);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /*
    private void CollectObjects() { //Old Method, Turning into a coroutine
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] cylinders = GameObject.FindGameObjectsWithTag("Cylinder");
        GameObject[] capsules = GameObject.FindGameObjectsWithTag("Capsule");
        GameObject[] spheres = GameObject.FindGameObjectsWithTag("Sphere");

        int cubeCount = 0;
        while(cubeCount < cubes.Length) {
            cubes[cubeCount].transform.position = new Vector3(0,2,0);
            cubeCount += 1;
        }

        int cylinderCount = 0;
        while(cylinderCount < cylinders.Length) {
            cylinders[cylinderCount].transform.position = new Vector3(4,2,0);
            cylinderCount += 1;
        }

        int capsuleCount = 0;
        while(capsuleCount < capsules.Length) {
            capsules[capsuleCount].transform.position = new Vector3(-4,2,0);
            capsuleCount += 1;
        }

        int sphereCount = 0;
        while(sphereCount < spheres.Length) {
            spheres[sphereCount].transform.position = new Vector3(0,2,-4);
            sphereCount += 1;
        }
    }
    */

    private void ObjectCounter() { //This will run after an object is spawned to see if we have reached the maximum number of spawnable objects
        if(objectsSpawned >= amountOfObjectsToSpawn) { //If we have reached the maximum, we stop the spawning and send a message saying we reached the max
            canReset = true;
            if(useDebug) Debug.Log("Reached the max amount of spawnable objects: " + amountOfObjectsToSpawn);
        } else { //If we haven't reached the maximum, we start the timer to spawn another one
            StartCoroutine(SpawnTimer());
        }
    }

    IEnumerator SpawnTimer() { //This is a simple timer that will start the spawn process after the time between spawns has been met
        yield return new WaitForSeconds(timeBetweenSpawns);
        GetRandomSpawnObject();
    }

    private void Restart() { //When we want to restart the whole spawning process, we can call this function
        objectsSpawned = 0; //sets the object spawn count to 0
        canReset = false; //allows for spawning to start happening again

        foreach(GameObject gameObject in objectsToDelete) { //This gets all of the gameobjects from the objects to delete list
            if(gameObject != null) {
                Destroy(gameObject); //We want to clear the board when we restart so this will delete all of the spawned objects
            }
        }

        objectsToDelete.Clear(); //This clears the list so its not bloated with non-existing objects
        spawnedObjectCounts.Clear();

        if(!spawnInfinite) { //If we have spawn infinite enabled, then this would cause an additional spawn when not needed
            ObjectCounter(); //This starts the spawning process all over again
        }
    }

    private void OnDrawGizmosSelected() { //This is a simple visual for the editor so you can see how large the spawn range is
        Gizmos.color = Color.green;
        
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(spawnRangeX * 2, 0, spawnRangeZ * 2));
    }
}
