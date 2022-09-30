using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private float mainSpeed = 8.0f; // Regular speed
    [SerializeField] private float scrollSpeed = -0.2f;
    private const float ShiftAdd = 5.0f; // Multiplied by how long shift is held
    private const float MaxShift = 100.0f; // Maximum speed when holding shift
    private float _totalRun= 1.0f;
    private float _cameraSize;
    private Camera _mainCamera;
     
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _cameraSize = _mainCamera.orthographicSize;
    }

    void Update () {
        if (Input.GetKeyDown("p"))
        {
            ResetCameraToStarterPlanet();
        }
        UpdateSize();
        UpdatePosition();
    }

    void UpdateSize()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta != 0)
        {
            float newSize = (float)(_cameraSize * Math.Pow(1 + scrollSpeed, delta));
            if (newSize >= 1 && newSize <= 10)
            {
                _mainCamera.orthographicSize = newSize;
                _cameraSize = newSize;
            }
            // Need to fix issue where zooming out at the edge breaks camera nav
        }
    }

    void UpdatePosition()
    {
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0){ // Only move while a direction key is pressed
            if (Input.GetKey (KeyCode.LeftShift)){
                _totalRun += Time.deltaTime;
                p  = p * (_totalRun * ShiftAdd);
                p.x = Mathf.Clamp(p.x, -MaxShift, MaxShift);
                p.y = Mathf.Clamp(p.y, -MaxShift, MaxShift);
                p.z = Mathf.Clamp(p.z, -MaxShift, MaxShift);
            } else {
                _totalRun = Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
                p *= mainSpeed;
            }
         
            p *= Time.deltaTime;
            Vector3 currentPos = transform.position;
            float resultingX = Math.Abs(p.x + currentPos.x);
            float resultingY = Math.Abs(p.y + currentPos.y);
            
            float maxSize = 50 - (_cameraSize * 2);
            if (resultingX > maxSize)
                p.x = 0;
            if (resultingY > maxSize)
                p.y = 0;
            if(p.x != 0 || p.y != 0)
                transform.Translate(p);
        }
    }

    public void ResetCameraToStarterPlanet()
    {
        Vector3 starterPlanetPosition = GameManager.Instance.GetStarterPlanet().transform.position;
        starterPlanetPosition.z = -10;
        ResetPosition(starterPlanetPosition);
    }

    public void ResetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    private Vector3 GetBaseInput() {
        Vector3 pVelocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            pVelocity += new Vector3(0, 1 , 0);
        }
        if (Input.GetKey (KeyCode.S)){
            pVelocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey (KeyCode.A)){
            pVelocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            pVelocity += new Vector3(1, 0, 0);
        }
        return pVelocity;
    }
}
