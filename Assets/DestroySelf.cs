using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

    void Awake() {
        AudioSource boom = GetComponent<AudioSource>();
        boom.Play();
        Destroy(gameObject, 5f);
    }
}
