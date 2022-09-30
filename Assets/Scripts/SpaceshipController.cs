using System;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public GameObject firstPlanet;
    public GameObject secondPlanet;
    private bool _isReturning;
    private bool _instantiated;
    [SerializeField] private float speed = 6f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void CustomStart()
    {
        // Set the spaceship position to be at the starter planet
        Vector3 spawnPosition = firstPlanet.transform.position;
        spawnPosition.x += 1.5f;
        gameObject.transform.position = spawnPosition;
        _isReturning = false;
        _instantiated = true; // Is there a better way to do this?

        // Go to second, back to first, repeat
    }

    // Update is called once per frame
    void Update()
    {
        // Logical branching every frame which is not needed after the first 0.01 seconds is bad
        // and this should be done better
        if (_instantiated)
        {
            GameObject target = _isReturning ? firstPlanet : secondPlanet;
            Vector3 targetPosition = target.transform.position;
            Vector3 ourPosition = transform.position;
            
            // Move towards the target
            Vector3 moveDir = (targetPosition - ourPosition).normalized;
            transform.position += moveDir * (speed * Time.deltaTime);

            // If we touch the target, re-set _isReturning
            Vector3 difference = ourPosition - targetPosition;
            if (difference.magnitude < 1)
            {
                _isReturning = !_isReturning;
            }
            
            // Change rotation
            double angle = Math.Atan2(difference.y, difference.x);
            transform.Rotate(Vector3.back, (float)angle);
        }
    }
}
