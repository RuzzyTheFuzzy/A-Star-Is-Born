using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Randomizer : MonoBehaviour
{
    public GameObject wall;
    [Range( 0, 1f )] public float wallWeight;
    public Vector2Int range;
    private GameObject parent;

    [ContextMenu( "Randomize" )]
    public void Randomize( )
    {
        DestroyImmediate(parent);
        parent = new GameObject( "Walls" );
        for ( int x = -range.x; x < range.x + 1; x++ )
        {
            for ( int y = -range.y; y < range.y + 1; y++ )
            {
                if ( Random.Range( 0f, 1f ) <= wallWeight )
                {
                    Instantiate( wall, new Vector3( x, y ), Quaternion.identity, parent.transform );
                }
            }
        }
    }
}