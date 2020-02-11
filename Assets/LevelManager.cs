using System.Collections;
using System.Collections.Generic;
using OVR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {
    private float ufoCount;
    private int passedUfoCount;

    public GameObject score;
    private Text ourScore;

    public GameObject timer;
    private Text ourTimer;

    private float countdown;

    private float skipTimer;
    private int failedUfoCount;

    [SerializeField]
    void Awake() {
        CountUfos("Right Side Ufos");
        CountUfos("Left Side Ufos");
        CountUfos("Mix Rectangle");
        CountUfos("Red Rectangle");
        CountUfos("Blue Rectangle");
        CountUfos("Red Triangle");
        CountUfos("Blue Triangle");
        CountUfos("Red Pyramid");
        CountUfos("Blue Pyramid");
        CountUfos("Red Cube");
        CountUfos("Blue Cube");
        CountUfos("Blue Ufo");
        Debug.Log("The ufos in this scene are " + ufoCount);

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Level 1 Laser" || sceneName == "Level 1 Bubble") {
            countdown = 180;
        }
        else if (sceneName == "Level 2 Laser" || sceneName == "Level 2 Bubble") {
            countdown = 300;
        }
        else countdown = 999; // Platzhalterzahl für kein Timer


        score = GameObject.Find("Score");
        ourScore = score.GetComponent<Text>();

        timer = GameObject.Find("Timer");
        ourTimer = timer.GetComponent<Text>();

        ourTimer.text = "Time left: -";

        ourScore.text = "Score: " + passedUfoCount + "/" + Get80Percent(ufoCount);
    }

    private void CountUfos(string groupName) {
        if (GameObject.Find(groupName)) {
            GameObject group = GameObject.Find(groupName);
            foreach (Transform child in group.transform) {
                ufoCount++;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if (countdown < 999) {
            countdown -= Time.deltaTime;
            ourTimer.text = "Time left: " + countdown.ToString("0") + " seconds";
        }

        if (countdown < 0) {
            FindObjectOfType<SoundManager>().Play("Zeit abgelaufen");
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
                DataContainer.GetInstance().level1SkippedLevel = true;
            }

            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
                DataContainer.GetInstance().level2SkippedLevel = true;
            }

            GoToNextScene();
        }

        if (Get80Percent(ufoCount) <= passedUfoCount) {
            GoToNextScene();
        }
        else if (passedUfoCount + failedUfoCount == (int) Mathf.Ceil(ufoCount)) {
            GoToNextScene();
        }

        if (OVRInput.Get(OVRInput.Button.One) &&
            OVRInput.Get(OVRInput.Button.Two) &&
            OVRInput.Get(OVRInput.Button.Three) &&
            OVRInput.Get(OVRInput.Button.Four) &&
            (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) >= 0.9f) &&
            (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) >= 0.9f)) {
            skipTimer += Time.deltaTime;
            if (skipTimer > 2) {
                skipTimer = 0;
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
                    DataContainer.GetInstance().level1SkippedLevel = true;
                }

                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
                    DataContainer.GetInstance().level2SkippedLevel = true;
                }

                Debug.Log("Next Level");
                GoToNextScene();
            }
        }
    }

    private static void GoToNextScene() {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0)) {
            DataContainer.GetInstance().tutorialTime = (int) Time.time;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {
            DataContainer.GetInstance().level1Time = ((int) Time.time) - DataContainer.GetInstance().tutorialTime;
        }

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(2)) {
            DataContainer.GetInstance().level2Time = ((int) Time.time) - DataContainer.GetInstance().level1Time - DataContainer.GetInstance().tutorialTime;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private float Get80Percent(float count) {
        return Mathf.Ceil((count / 100) * 80);
    }

    public void AddPassedUfo() {
        passedUfoCount++;
        ourScore.text = "Score: " + passedUfoCount + "/" + Get80Percent(ufoCount);
    }

    public void AddFailedUfos() {
        failedUfoCount++;
    }

    public void Print() {
        Debug.Log(passedUfoCount);
    }
}