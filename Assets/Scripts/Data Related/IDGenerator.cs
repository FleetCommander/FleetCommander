using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDGenerator: ScriptableObject {
    [SerializeField]
    private static int id;

    public static int GetNextId() {
        
        return id++;
    }
}
