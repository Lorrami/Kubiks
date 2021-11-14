using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float speed = 5f;
    private Transform rotator;

    private void Start()
    {
        rotator = GetComponent<Transform>();
    }
    public void Update()
    {
        rotator.Rotate(0, speed * Time.deltaTime, 0);
    }
}
