using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileColorChange : MonoBehaviour
{

    public Tilemap tilemap; 

    private GameObject bob;

    void Start() {
        bob = COMMON.FindMyBob();
    }


    // Update is called once per frame
    void Update()
    {

        // Set the colour.

        float r = COMMON.Map(bob.transform.position.x, -8, 8, 0, 1);
        float b = COMMON.Map(bob.transform.position.y, 1, 6, 0, 1);

        int x, y;
        for(x = tilemap.cellBounds.min.x; x< tilemap.cellBounds.max.x;x++){
            for(y= tilemap.cellBounds.min.y; y< tilemap.cellBounds.max.y;y++){
                Vector3Int pos = new Vector3Int(x, y, 0);
                tilemap.SetTileFlags(pos, TileFlags.None);

                tilemap.SetColor(pos, new Color(r, 0.6f, b));
            }
        }
    }
}
