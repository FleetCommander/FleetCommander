using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minen : MonoBehaviour {
    private float speed = 10.0f;
    private float time = 0f;
    private bool left = true;
    void Update() {
        if(time <= 25)
            Move();
        else
        {
            time = 0;
            left = !left;
        }
    }
    private void Move() {
        if (left == true) {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
            time += Time.deltaTime;
        }
        else {
            transform.Translate(-Vector3.left * speed * Time.deltaTime);
            time += Time.deltaTime;
        }
    }
}