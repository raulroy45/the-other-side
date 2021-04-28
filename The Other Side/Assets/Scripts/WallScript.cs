using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class WallScript : MonoBehaviour
{
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
