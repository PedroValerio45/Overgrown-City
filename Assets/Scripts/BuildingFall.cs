using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFall : MonoBehaviour
{
    public float fallDuration = 2f;
    public float fallAngle = 90f;

    private float elapsedTime = 0f;
    private float currentAngle = 0f;
    private bool isFalling = false;

    void Update()
    {
        if (!isFalling)
            return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / fallDuration);

        float targetAngle = fallAngle * (t * t);
        float deltaAngle = targetAngle - currentAngle;

        transform.Rotate(Vector3.right, deltaAngle, Space.Self);
        currentAngle = targetAngle;

        if (t >= 1f)
        {
            isFalling = false;
        }
    }

    public void StartFalling()
    {
        if (!isFalling && currentAngle == 0f)
        {
            isFalling = true;
            elapsedTime = 0f;
            currentAngle = 0f;
        }
    }
}
