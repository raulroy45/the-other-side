using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class PlatformScript : MonoBehaviour
{
    public Tilemap tilemap;
    public BoxCollider2D boxCollider;
    public bool isWallMerged;
    void Awake() {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
    }
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        isWallMerged = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) {
            isWallMerged = !isWallMerged;
            if (isWallMerged) {
                boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y + 0.28f);
            } else {
                boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - 0.28f);
            }
        }
    }

    void OnApplicationQuit() {
        if (isWallMerged) {
            boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - 0.28f);
        }
    }
}
