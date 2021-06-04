using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class RandomTiles : MonoBehaviour
{

    // use these tiles
    public TileBase[] tiles;
    public float probability = 0.2f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (tiles == null) return;
        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        int x, y;
        for(x = tilemap.cellBounds.min.x; x< tilemap.cellBounds.max.x;x++){
            for(y= tilemap.cellBounds.min.y; y< tilemap.cellBounds.max.y;y++){
                Vector3Int pos = new Vector3Int(x, y, 0);
                if (Random.value > probability) {
                    continue;
                }
                tilemap.SetTile(pos, tiles[Random.Range(0, tiles.Length)]);
                // tilemap.SetTileFlags(pos, TileFlags.None);
                // tilemap.SetColor(pos, new Color(r, 0.6f, b));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
