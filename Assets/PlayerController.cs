using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public LineRenderer line;
    public List<Vector3> currentPath;
    public float moveTime;

    private Vector3 nextPoint;
    private float timer;
    private Vector3 prevPoint;
    private Camera currentCamera;

    private void Start( )
    {
        currentCamera = Camera.main;
        var position = player.transform.position;
        nextPoint = position;
        prevPoint = position;
    }

    private void Update( )
    {
        var position = player.transform.position;

        if ( Input.GetMouseButtonDown( 0 ) )
        {
            currentPath = AStar.FindPath( 10000, Vector2Int.RoundToInt( player.transform.position ),
                Vector2Int.RoundToInt( currentCamera.ScreenToWorldPoint( Input.mousePosition ) ) );
            LineUpdate();
            timer = 0;
            prevPoint = position;
        }


        if ( currentPath.Count > 1 )
        {
            nextPoint = currentPath[1];
        }

        if ( position != nextPoint )
        {
            timer += Time.deltaTime;
            if ( timer < moveTime )
            {
                position.x = Mathf.Lerp( prevPoint.x, nextPoint.x, ( timer / moveTime ) );
                position.y = Mathf.Lerp( prevPoint.y, nextPoint.y, ( timer / moveTime ) );
            }
            else
            {
                position = nextPoint;

                if ( currentPath.Count > 1 )
                {
                    currentPath.Remove( currentPath[0] );
                    prevPoint = currentPath[0];
                }

                timer = 0;
            }

            if ( currentPath.Count > 1 )
            {
                currentPath[0] = position;
            }

            player.transform.position = position;

            LineUpdate();
        }
    }

    private void LineUpdate( )
    {
        line.positionCount = currentPath.Count;
        line.SetPositions( currentPath.ToArray() );
    }
}