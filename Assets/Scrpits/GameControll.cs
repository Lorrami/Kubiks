using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameControll : MonoBehaviour
{
    public float ChangeSpeed = 0.5f;
    public Transform CubeCheck;
    public GameObject cubeCreate, allCubes, Effect;
    public GameObject[] CanvasStart;
    public Transform mainCamera;
    public Color[] colors;
    public GameObject Inst;

    private CubeLoc Cubik = new CubeLoc(0, 1, 0);
    private Rigidbody rb;
    private bool Lose;
    private bool firstStep = false;
    private float YCameraMove, SpeedCam = 2f;
    private int CountMax;
    private Color NeedColor;

    private List<Vector3> BannedPositions = new List<Vector3>
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
    };

    private Coroutine Running;

    private void Start()
    {
        NeedColor = Camera.main.backgroundColor;

        mainCamera = Camera.main.transform;
        YCameraMove = 4.31f + Cubik.y - 1f;
        rb = allCubes.GetComponent<Rigidbody>();
        Running  = StartCoroutine(ShowCube());
    }

    private void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && CubeCheck != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
            if (Input.GetTouch(0).position.x == Inst.transform.position.x && Input.GetTouch(0).position.y == Inst.transform.position.y)
                return;
#endif
            if (!firstStep)
            {
                firstStep = true;
                foreach (GameObject obj in CanvasStart)
                    Destroy(obj);
            }

            GameObject newCube = Instantiate(cubeCreate, CubeCheck.position, Quaternion.identity) as GameObject;
            newCube.transform.SetParent(allCubes.transform);
            Cubik.SetVector3(CubeCheck.position);
            BannedPositions.Add(Cubik.GetVector3());

            Instantiate(Effect, CubeCheck.position, Quaternion.identity);
            
            rb.isKinematic = true;
            rb.isKinematic = false;

            Position();
            CameraMove();
        }

        if(!Lose && rb.velocity.magnitude > 0.1f)
        {
            Destroy(CubeCheck.gameObject);
            Lose = true;
            StopCoroutine(Running);
        }

        mainCamera.localPosition = Vector3.MoveTowards(mainCamera.localPosition, 
            new Vector3(mainCamera.localPosition.x, YCameraMove, mainCamera.localPosition.z),
            SpeedCam * Time.deltaTime);

        if (Camera.main.backgroundColor != NeedColor)
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, NeedColor, Time.deltaTime / 1.5f);
    }

    IEnumerator ShowCube()
    {
        while(true)
        {
            Position();
            yield return new WaitForSeconds(ChangeSpeed);
        }
    }
    private void Position()
    {
        List<Vector3> positions = new List<Vector3>();
        if (IsAble(new Vector3(Cubik.x + 1, Cubik.y, Cubik.z)) && Cubik.x + 1 != CubeCheck.position.x)
        {
            positions.Add(new Vector3(Cubik.x + 1, Cubik.y, Cubik.z));
        }
        if (IsAble(new Vector3(Cubik.x - 1, Cubik.y, Cubik.z)) && Cubik.x - 1 != CubeCheck.position.x)
        {
            positions.Add(new Vector3(Cubik.x - 1, Cubik.y, Cubik.z));
        }
        if (IsAble(new Vector3(Cubik.x, Cubik.y + 1, Cubik.z)) && Cubik.y + 1 != CubeCheck.position.y)
        {
            positions.Add(new Vector3(Cubik.x, Cubik.y + 1, Cubik.z));
        }
        if (IsAble(new Vector3(Cubik.x, Cubik.y - 1, Cubik.z)) && Cubik.y - 1 != CubeCheck.position.y)
        {
            positions.Add(new Vector3(Cubik.x, Cubik.y - 1, Cubik.z));
        }
        if (IsAble(new Vector3(Cubik.x, Cubik.y, Cubik.z + 1)) && Cubik.z + 1 != CubeCheck.position.z)
        {
            positions.Add(new Vector3(Cubik.x, Cubik.y, Cubik.z + 1));
        }
        if (IsAble(new Vector3(Cubik.x, Cubik.y, Cubik.z - 1)) && Cubik.z - 1 != CubeCheck.position.z)
        {
            positions.Add(new Vector3(Cubik.x, Cubik.y, Cubik.z - 1));
        }
        if (positions.Count > 1)
            CubeCheck.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else
        if (positions.Count == 0)
            Lose = true;
        else
            CubeCheck.position = positions[0];
    }

    private bool IsAble(Vector3 newpos)
    {
        if (newpos.y == 0)
            return false;
        foreach(Vector3 pos in BannedPositions)
        {
            if (pos.x == newpos.x && pos.y == newpos.y && pos.z == newpos.z)
                return false;
        }
        return true;
    }

    private void CameraMove()
    {
        int MaxX = 0, MaxY = 0, MaxZ = 0, Max;
        foreach (Vector3 pos in BannedPositions)
        {
            if (Math.Abs(Convert.ToInt32(pos.x)) > MaxX)
                MaxX = Convert.ToInt32(pos.x);
            if (Math.Abs(Convert.ToInt32(pos.y)) > MaxY)
                MaxY = Convert.ToInt32(pos.y);
            if (Math.Abs(Convert.ToInt32(pos.z)) > MaxZ)
                MaxZ = Convert.ToInt32(pos.z);
        }
        YCameraMove = 4.817f + Cubik.y - 1f;
        Max = MaxX > MaxZ ? MaxX : MaxZ;
        if (Max % 3 == 0 && CountMax != Max)
        {
            mainCamera.localPosition += new Vector3(0, 0, -2.5f);
            CountMax = Max;
        }
        if (MaxY >= 7)
            NeedColor = colors[2];
        else
        if (MaxY >= 5)
            NeedColor = colors[1];
        else
        if (MaxY >= 2)
            NeedColor = colors[0];

    }

    struct CubeLoc
    {
        public int x, y, z;
        public CubeLoc(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }

        public void SetVector3(Vector3 pos)
        {
            x = Convert.ToInt32(pos.x);
            y = Convert.ToInt32(pos.y);
            z = Convert.ToInt32(pos.z);
        }
    }
}
