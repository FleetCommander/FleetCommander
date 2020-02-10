using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dontdestroyoonload : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

}
