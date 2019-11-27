using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plattformtrigger : MonoBehaviour
{


    void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<MeshRenderer>().material.color == other.gameObject.GetComponent<MeshRenderer>().material.color)
        {
            Debug.Log(gameObject.name + " was triggered by " + other.gameObject.name);
        }
    }

    // Update is called once per frame
    /*void Update()
    {
        // other.gameObject.Getcomponent<MeshRenderer>().material.color()
    }*/
}
