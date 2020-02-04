using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDGenerator {
    [SerializeField]
    private static int id = 0;

    public static int GetNextId() {
        return id++;
    }
}
