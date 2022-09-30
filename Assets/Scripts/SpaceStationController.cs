using UnityEngine;

public class SpaceStationController : MonoBehaviour
{
    private Vector3 from = new Vector3(0f, 15f, 0f);
    private Vector3 to   = new Vector3(0f, 0f, 0f);
    [SerializeField] private float FloatSpeed = 1.0f;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = Mathf.PingPong(Time.time * FloatSpeed * 2.0f, 1.0f);
        transform.eulerAngles = Vector3.Lerp (from, to, t);
    }
}
