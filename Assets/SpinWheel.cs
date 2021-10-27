using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* TODO: 
    make candy fall into pile
    delete candy after a certain amount of time
	fix edge case if all candies are toggled off
	add models
	make background
*/

public class SpinWheel : MonoBehaviour {
    private float rotationSpeed = 0f;
    private bool isSlowingDown = false;
    private bool isSpinning = false;
    private int finalAngle = 0;
    int candyCode = 0;

    // candy toggles
    private bool mm = true;
    private bool twix = true;
    private bool snickers = true;
    private bool milky = true;

    // candy prefabs
    public GameObject mmPrefab;
    public GameObject twixPrefab;
    public GameObject snickersPrefab;
    public GameObject milkyPrefab;
    public GameObject candySpawner;

    void Start() {

    }

    // randomly chooses candy
    private void generateCandy() {
        candyCode = Random.Range(0,4);

        // if a candy is disabled, cant land on it
        if (mm && candyCode == 0) {
            Debug.Log("mm");
            finalAngle = 45;
            spinWheel();
        }else if (twix && candyCode == 1){
            Debug.Log("twix");
            finalAngle = 135;
            spinWheel();
        }else if (snickers && candyCode == 2){
            Debug.Log("snickers");
            finalAngle = 225;
            spinWheel();
        }else if (milky && candyCode == 3){
            Debug.Log("milky way");
            finalAngle = 295;
            spinWheel();
        } else if (!mm && !twix && !snickers && !milky) {
            // if all candies are turned off, by default, mm will turn back on
            mm = true;
            generateCandy();
        } else {
            // regenerate code if candy is disabled
            generateCandy();
        }
        
    }

    private void spinWheel() {
        // marks wheel as spinning
        isSpinning = true;

        // sets inital wheel rotation speed
        rotationSpeed = speedToDegrees(360) * 2;
        
        // lets wheel freely spin for some time
        StartCoroutine(initialSpin());
    }

    // creates instance of candy prefab depending on candy code
    private void createCandy() {
        // empty game object spawner
        var spawnerPosition = candySpawner.transform.position;
        GameObject candy;

        switch (candyCode) {
            case 0:
                candy = Instantiate(mmPrefab) as GameObject;
                break;
            case 1:
                candy = Instantiate(twixPrefab) as GameObject;
                break;
            case 2:
                candy = Instantiate(snickersPrefab) as GameObject;
                break;
            default:
                candy = Instantiate(milkyPrefab) as GameObject;
                break;
        }
        candy.transform.position = new Vector3(Random.Range(-1.13f, 1.78f) + spawnerPosition.x, spawnerPosition.y, spawnerPosition.z);
    }

    void Update() {
        // checks for when space is pressed
        if (Input.GetKeyDown(KeyCode.Space)) {
            // makes sure wheel is not spinning
            if (!isSpinning) { 
                // resets wheel for next spin
                isSlowingDown = false;
                isSpinning = false;
                rotationSpeed = 0;
                transform.eulerAngles = new Vector3(
                    0,0,0
                );

                // generates candy code
                generateCandy();
            }
        }

        // resets wheel when up arrow is pressed
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            isSlowingDown = false;
            isSpinning = false;
            rotationSpeed = 0;
            transform.eulerAngles = new Vector3(
                0,0,0
            );
        }

        // toggles m&ms
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            // if mm is true, then make it false, else make it true
            mm = mm ? false : true;
        }

        // toggles twix
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            // if twix is true, then make it false, else make it true
            twix = twix ? false : true;
        }

        // toggles snickers
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            // if snickers is true, then make it false, else make it true
            snickers = snickers ? false : true;
        }

        // toggles milky way
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            // if milky way is true, then make it false, else make it true
            milky = milky ? false : true;
        }
        
    }

    // updates at a constant rate
    void FixedUpdate() {
        // checks if the wheel should be slowing down
        if (isSlowingDown) {
            // slows it down so enough to reach specified degree
            // is adding because the initial velocity is rotating counter clockwise
            rotationSpeed += deaccelerationToDegrees(finalAngle);
            if (rotationSpeed >= 0f) {
                // sets final speed to exactly zero to prevent drift
                rotationSpeed = 0f;
                isSlowingDown = false;
                isSpinning = false;
                // after wheel has spun, start spawning in candy objects
                //StartCoroutine(spawnCandy());

                // creates 1 instance of rolled candy
                createCandy();
            }
        }
        
        // rotates the wheel as fast as rotation speed
        transform.Rotate(new Vector3(0f, 0f, rotationSpeed));
    }

    // lets the wheel freely spin for 1 sec
    IEnumerator initialSpin() {
        yield return new WaitForSeconds(1);
        isSlowingDown = true;
    }

    // candy spawning loop
    // IEnumerator spawnCandy() {
    //     // spawns 10 random instances of candy at random intervals
    //     for (int i = 0; i < 10; i++) {
    //         yield return new WaitForSeconds(Random.Range(0f,1f));
    //         createCandy();
    //     }
    // }

    // takes degrees and outputs speed to get to that angle in 1 sec
    private float speedToDegrees(float degrees) {
        return -1 * degrees * 0.019999f;
    }

    // takes degrees and outputs (de)acceleration needed to get to that angle(not accurate)
    // this is so bad lol, smooth brain cant math
    private float deaccelerationToDegrees(float degrees) {
        return (float)(0.000000172 * Mathf.Pow(degrees, 2) - 0.000193 * degrees + 0.143);
    }
}
