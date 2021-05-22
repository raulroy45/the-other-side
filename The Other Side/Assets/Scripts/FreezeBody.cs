using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// does not affect nodes that contain "fixed" in name 
public class FreezeBody : MonoBehaviour
{

    public PlayerController pcScript;
    public bool activeWhenInWall = true;
    
    private List<Rigidbody2D> wallRigidBodies;
    private Vector2[] oldVelocities;
    private float[] oldAngularVelocities;
    private bool currentBobState;

    // Start is called before the first frame update
    void Start()
    {
        // get a hold of all rb2d in this game obejct
        // everything in wall starts inactive
        wallRigidBodies = new List<Rigidbody2D>();
        // loop through children, find all rb2d
        foreach (Transform child in transform) {
            if (child.gameObject.name.ToLower().Contains("fixed")) {
                continue;  // skip nodes that contains fixed
            }
            foreach (Rigidbody2D b in child.gameObject.GetComponentsInChildren<Rigidbody2D>()) {
                wallRigidBodies.Add(b);
            }
        }
        if (pcScript != null) {
            currentBobState = pcScript.isWallMerged;
        }

        oldVelocities = new Vector2[wallRigidBodies.Count];
        oldAngularVelocities = new float[wallRigidBodies.Count];
        // first sync
        for (int i = 0; i < wallRigidBodies.Count; i++) {
            Rigidbody2D b = wallRigidBodies[i];
            // concise version of Update logic
            if ((currentBobState && activeWhenInWall) || 
                (!currentBobState && !activeWhenInWall)) {  // in wall
                b.bodyType = RigidbodyType2D.Dynamic;
            } else {
                b.bodyType = RigidbodyType2D.Kinematic;
                // save v and w
                oldVelocities[i] = b.velocity;
                oldAngularVelocities[i] = b.angularVelocity;
                b.velocity = new Vector2(0, 0);
                b.angularVelocity = 0;
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
            for (int i = 0; i < wallRigidBodies.Count; i++) {
                Rigidbody2D b = wallRigidBodies[i];
                if (b == null) continue;
                
                if (currentBobState) {  // in wall
                    if (activeWhenInWall) {
                        b.bodyType = RigidbodyType2D.Dynamic;
                        // restore v and w
                        b.velocity = oldVelocities[i];
                        b.angularVelocity = oldAngularVelocities[i];
                    } else {
                        b.bodyType = RigidbodyType2D.Kinematic;
                        // save v and w
                        oldVelocities[i] = b.velocity;
                        oldAngularVelocities[i] = b.angularVelocity;
                        // bug fix: changed from Static to Kinematic, and need to set v to (0,0)
                        b.velocity = new Vector2(0, 0);
                        b.angularVelocity = 0;
                    }
                } else {
                    // out of wall
                    if (activeWhenInWall) {
                        b.bodyType = RigidbodyType2D.Kinematic;
                        // save v and w
                        oldVelocities[i] = b.velocity;
                        oldAngularVelocities[i] = b.angularVelocity;
                        b.velocity = new Vector2(0, 0);
                        b.angularVelocity = 0;
                    } else {
                        b.bodyType = RigidbodyType2D.Dynamic;
                        // restore v and w
                        b.velocity = oldVelocities[i];
                        b.angularVelocity = oldAngularVelocities[i];
                    }
                }
            }
        }
    }
}
