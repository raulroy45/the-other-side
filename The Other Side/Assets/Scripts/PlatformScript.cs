using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PlatformScript : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D boxCollider;
    private GameObject player;
    public bool isWallMerged;
    private int wallMergesLeft;
    void Awake() {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = COMMON.FindMyBob();
        boxCollider = GetComponent<BoxCollider2D>();
        isWallMerged = false;
        wallMergesLeft = PlayerController.wallMergesLimit;
    }
    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().wallMergeEnabled) {
            if (Input.GetKeyDown(COMMON.WALL_MERGE_KEY) && wallMergesLeft != 0) {
                wallMergesLeft--;
                isWallMerged = !isWallMerged;
                if (isWallMerged) {
                    boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y + 0.28f);
                } else {
                    boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - 0.28f);
                }
            }
        }
    }

    void OnApplicationQuit() {
        if (isWallMerged) {
            boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - 0.28f);
        }
    }
}
