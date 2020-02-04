using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton Container, damit dieser nur einmal existiert
public sealed class DataContainer:ScriptableObject {

    private static DataContainer dataContainer = new DataContainer();

    public int id;
    public string type;
    public int inputs = 0;
    public int mistakes = 0;
    public float time;
    public bool skippedLevel = false;

    public static DataContainer getInstance() {
        return dataContainer;
    }
}
