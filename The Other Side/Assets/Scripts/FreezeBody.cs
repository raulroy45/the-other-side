using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBody : MonoBehaviour
{

    public PlayerController pcScript;
    public bool activeWhenInWall = true;
    
    private List<Rigidbody2D> wallRigidBodies;
    private bool currentBobState;

    // Start is called before the first frame update
    void Start()
    {
        // get a hold of all rb2d in this game obejct
        // everything in wall starts inactive
        wallRigidBodies = new List<Rigidbody2D>();
        // loop through children, find all rb2d
        foreach (Transform child in transform) {
            foreach (Rigidbody2D b in child.gameObject.GetComponentsInChildren<Rigidbody2D>()) {
                wallRigidBodies.Add(b);
            }
        }
        if (pcScript != null) {
            currentBobState = pcScript.isWallMerged;
        }

        // first sync
        foreach (Rigidbody2D b in wallRigidBodies) {
            // concise version of Update logic
            if ((currentBobState && activeWhenInWall) || 
                (!currentBobState && !activeWhenInWall)) {  // in wall
                b.bodyType = RigidbodyType2D.Dynamic;
            } else {
                b.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wallRigidBodies == null || pcScript == null) return;
        if (pcScript.isWallMerged != currentBobState) {
            // need to change
            currentBobState = pcScript.isWallMerged; 
            foreach (Rigidbody2D b in wallRigidBodies) {
                if (currentBobState) {  // in wall
                    if (activeWhenInWall) {
                        b.bodyType = RigidbodyType2D.Dynamic;
                    } else {
                        b.bodyType = RigidbodyType2D.Static;
                    }
                } else {
                    // out of wall
                    if (activeWhenInWall) {
                        b.bodyType = RigidbodyType2D.Static;
                    } else {
                        b.bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }
        }
    }
}
