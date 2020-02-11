using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton Container, damit dieser nur einmal existiert
public sealed class DataContainer{

    private static DataContainer dataContainer = new DataContainer();

    public int tutorialTime;

    public int id;
    public string type;
    public int level1Inputs = 0;
    public int level1Mistakes = 0;
    public int level1Time;
    public bool level1SkippedLevel = false;
    
    public int level2Inputs = 0;
    public int level2Mistakes = 0;
    public int level2Time;
    public bool level2SkippedLevel = false;

    public static DataContainer GetInstance() {
        return dataContainer;
    }
}
