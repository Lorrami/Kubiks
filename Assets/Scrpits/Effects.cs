using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Transform Camera;
    private float Shake = 1f, ShakeAmount = 0.04f, Factor = 1.5f;

    private Vector3 StartPos;

    private void Start()
    {
        Camera = GetComponent<Transform>();
        StartPos = Camera.localPosition;
    }

    private void Update()
    {
        if (Shake > 0)
        {
            Camera.localPosition = StartPos + UnityEngine.Random.insideUnitSphere * ShakeAmount;
            Shake -= Time.deltaTime * Factor;
        }
        else
        {
            Shake = 0;
            Camera.localPosition = StartPos;
        }
    }
}
