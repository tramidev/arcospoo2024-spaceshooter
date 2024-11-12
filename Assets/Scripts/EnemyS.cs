using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyS : Enemy
{
    public int horizontalMovementSpeed;
    private int direction = -1;
    private float maxXPos;
    private float minXPos;

    public void Awake()
    {
        float height = Camera.main.orthographicSize;
        float cameraWidth = height * Camera.main.aspect;
        maxXPos = cameraWidth / 2;
        minXPos = -maxXPos;
    }

    protected override void Update()
    {
        base.Update();
        MoveHorizontalPingPong();
    }

    private void MoveHorizontalPingPong()
    {
        float currentYPos = transform.position.x;
        Vector3 positionDelta = Vector3.zero;
        if (direction == -1)
        {
            if (currentYPos <= minXPos)
                direction = 1;
            else
                positionDelta = Vector3.left * Time.deltaTime;
        }
        else
        {
            if (currentYPos >= maxXPos)
                direction = -1;
            else
                positionDelta = -(Vector3.left * Time.deltaTime);
        }

        transform.position += positionDelta*horizontalMovementSpeed;
    }
}
