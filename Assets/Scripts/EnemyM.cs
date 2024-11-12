using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyM : Enemy
{
    private Square movementSquare;
    public float timeToTeleport = 1;
    private float teleportTimer;
    public void Awake()
    {
        float height = Camera.main.orthographicSize;
        float cameraWidth = height * Camera.main.aspect;
        movementSquare = new Square(
            -cameraWidth,
            height/2,
            cameraWidth,
            -(height/2)
            );
    }
    
    protected override void Update()
    {
        //no usar el metodo base, solo disparar
        //base.Update();
        CountDownToShoot();

        CountDownToRandomPosInSquare();
    }

    private void CountDownToRandomPosInSquare()
    {
        teleportTimer += Time.deltaTime;
        if (teleportTimer>=timeToTeleport)
        {
            teleportTimer = 0;
            Vector2 randomPos = movementSquare.GetRandomPoint();
            transform.position = new Vector3(randomPos.x, randomPos.y);
        }
    }
}

public class Square
{
    private Vector2 topLeftCornerPoint;
    private Vector2 buttomRightCornerPoint;

    public Square(float x1, float y1, float x2, float y2)
    {
        topLeftCornerPoint = new Vector2(x1, y1);
        buttomRightCornerPoint = new Vector2(x2, y2);
    }

    public Vector2 GetRandomPoint()
    {
        float randomX = Random.Range(topLeftCornerPoint.x, buttomRightCornerPoint.x);
        float randomY = Random.Range(topLeftCornerPoint.y, buttomRightCornerPoint.y);
        return new Vector2(randomX, randomY);
    }
}
