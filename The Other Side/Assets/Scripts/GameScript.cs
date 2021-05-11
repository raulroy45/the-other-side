using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour {

    public int wallMerges = -1; // infinite wall merges.
    public static int wallMergesLimit;

    void Awake() {
        wallMergesLimit = wallMerges;
    }

}
