using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    
    public GameObject status;
    private Text ourStatus;
    
    public GameObject menBut;
    private Image ourMenBut;

    private float countdown;

    private float skipTimer;
    private int failedUfoCount;
    
    private bool stop;
    private bool failed;

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
        
        status = GameObject.Find("Status");
        ourStatus = status.GetComponent<Text>();
        
        menBut = GameObject.Find("MenuBut");
        ourMenBut = menBut.GetComponent<Image>();

        ourTimer.text = "Time left: -";

        ourStatus.text = " ";

        ourScore.text = "Score: " + passedUfoCount + "/" + Get80Percent(ufoCount);

        stop = false;
        failed = false;
        ourMenBut.enabled = false;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(3)) {
            int endscore = DataContainer.GetInstance().endScore;
            if (endscore > 113) {
                ourStatus.text = "GRATULATION KOMMANDANT!!!";
                ourStatus.fontSize = 80;
            } else ourStatus.text = "Vielen Dank für die Teilnahme.";
            
            ourScore.text = "Endscore: " + endscore.ToString("0" ) + "/114";
            ourTimer.text = "Benötigte Zeit: " + DataContainer.GetInstance().endTime.ToString("0") + " Sekunden";
            

        }
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
            if(!stop) countdown -= Time.deltaTime;
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

            stop = true;
            ourMenBut.enabled = true;
            ourStatus.text = "Mission fehlgeschlagen! Drücke         um fortzufahren.";
            failed = true;

            if (OVRInput.GetDown(OVRInput.Button.Start)) {
                DataContainer.GetInstance().endScore += passedUfoCount;
                
                GoToNextScene();
            }
        }

        if (Get80Percent(ufoCount) <= passedUfoCount && !failed && SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(3)) {
            stop = true;
            ourMenBut.enabled = true;
            ourStatus.text = "Mission abgeschlossen! Drücke         um fortzufahren.";
            if (OVRInput.GetDown(OVRInput.Button.Start)) {
                
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0)) {
                    DataContainer.GetInstance().endScore += passedUfoCount;
                    DataContainer.GetInstance().endTime -= (int) countdown;
                }
                GoToNextScene();
            }
        }
        else if (passedUfoCount + failedUfoCount == (int) Mathf.Ceil(ufoCount) && SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(3)) {
            stop = true;
            ourMenBut.enabled = true;
            ourStatus.text = "Mission fehlgeschlagen! Drücke         um fortzufahren.";
            if (OVRInput.GetDown(OVRInput.Button.Start)) {
                
                if (SceneManager.GetActiveScene() != SceneManager.GetSceneByBuildIndex(0)) {
                    
                    DataContainer.GetInstance().endScore += passedUfoCount;
                    DataContainer.GetInstance().endTime -= (int) countdown;
                }
                
                GoToNextScene();
            }
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
        if(!stop) passedUfoCount++;
        ourScore.text = "Score: " + passedUfoCount + "/" + Get80Percent(ufoCount);
    }

    public void AddFailedUfos() {
        if(!stop) failedUfoCount++;
    }

    public void Print() {
        Debug.Log(passedUfoCount);
    }
}