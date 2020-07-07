using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trailer : MonoBehaviour
{


    // References & Dependencies
    [SerializeField]
    private Transform handleTrans = null;
    private List<Wheel> wheels = new List<Wheel>();
    private Transform wheelsCenterTrans = null;


    // Movement
    public Vector3 Velocity { get; private set; } = Vector3.zero;







    private void Start()
    {
        wheels = GetComponentsInChildren<Wheel>().ToList();

        // The center point between the wheels
        Vector3 wheelsCenterPos = Vector3.zero; 

        // Calculates the average position of each wheel
        foreach (Wheel wheel in wheels)
        {
            wheelsCenterPos += wheel.transform.position;
        }

        wheelsCenterPos /= wheels.Count;

        // Creates a gameobject at that point - this is where rotations will be calculated from
        wheelsCenterTrans = new GameObject("Wheels Center").transform;
        wheelsCenterTrans.position = wheelsCenterPos;
        wheelsCenterTrans.parent = transform;
        wheelsCenterTrans.localRotation = Quaternion.identity;
    }








    private void Update()
    {
        if (!handleTrans)
            return;



        // MOVE THE TRAILER INTO POSITION
        // this works by applying a velocity to the current position
        // when within a distance range (slowDistance) a number is returned between 0 and 1 that is used to Lerp the speed
        // this is then applied to the velocity to to slow the object down the closer it gets to the target

        float slowDistance = 1f;
        float speedLerp = (transform.position - handleTrans.position).magnitude / slowDistance; 
        speedLerp = Mathf.Clamp01(speedLerp);
        float speed = Mathf.Lerp(0f, 20f, speedLerp);

        Vector3 moveDirection = (handleTrans.position - transform.position).normalized;
        Velocity = moveDirection * speed;
        transform.position += Velocity * Time.deltaTime;




        // ROTATE THE TRAILER
        // rotates the trailer towards the handle
        // the direction vector used to create the rotation is from the center of the wheels instead of the trailers pivot
        // this allows for the trailer to be pushed backwards and rotate more like how it might in real life

        if (!HasReachedDestination())
        {
            // rotate the trailer, using the direction from the center of the wheels to the handle
            Vector3 handleDirection = (handleTrans.position - wheelsCenterTrans.position).normalized;
            Quaternion rot = Quaternion.FromToRotation(transform.forward, handleDirection);
            rot = Quaternion.Euler(transform.eulerAngles.x, rot.eulerAngles.y, transform.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * rot, Time.deltaTime * 20f);
        }




        // UPDATE THE WHEELS

        foreach(Wheel wheel in wheels)
        {
            wheel.Rotate(transform, Velocity, 0.5f);
        }
    }








    private bool HasReachedDestination()
    {
        if ((handleTrans.position - transform.position).sqrMagnitude <= 0.001)
            return true;

        return false;
    }

}
