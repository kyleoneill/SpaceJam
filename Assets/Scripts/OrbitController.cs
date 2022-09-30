using System;
using UnityEngine;

public class OrbitController : MonoBehaviour
{
    /*
     * To calculate orbital position:
     *  On Start:
     *   1. Calculate and store distance from parent (order matters)
     *   2. Store the angle of that distance
     *  On Update:
     *   1. Get the parents position
     *   2. Add to that position. Add `cos(angle) * stored_distance` to x and `sin(angle) * stored_distance` to y
     *   3. Set current position to our modified position
     *   4. Add `time.Deltatime * sqrt(orbital_speed / stored_distance)` to our angle
     *     a. This models the mean orbital speed equation, v = sqrt(GM / semi-major_axis_length)
     */
    [SerializeField] private GameObject parentObject;
    private float _distanceToParent;
    private double _angleAroundParent;
    [SerializeField] private float orbitSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        // Vector points to the first from the second
        // We want the vector that points to me from the parent
        Vector3 difference = gameObject.transform.position - parentObject.transform.position;
        _distanceToParent = difference.magnitude;
        _angleAroundParent = Math.Atan2(difference.y, difference.x);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = parentObject.transform.position;
        newPosition.x += (float)(Math.Cos(_angleAroundParent) * _distanceToParent);
        newPosition.y += (float)(Math.Sin(_angleAroundParent) * _distanceToParent);

        gameObject.transform.position = newPosition;
        _angleAroundParent += Time.deltaTime * Math.Sqrt(orbitSpeed / _distanceToParent);
    }

    public void SetParent(GameObject parent)
    {
        parentObject = parent;
    }
}
