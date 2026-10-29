
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriftTrail : MonoBehaviour
{
    [Header("Trail References")]
    public TrailRenderer leftTrail;
    public TrailRenderer rightTrail;
    public TrailRenderer topRightTrail;
    public TrailRenderer topLeftTrail;

    [Header("Drift Settings")]
    public float minSpeed = 2f;
    public float minDriftAngle = 15f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        leftTrail.emitting = false;
        rightTrail.emitting = false;
        topLeftTrail.emitting = false;
        topRightTrail.emitting = false;
    }

    void Update()
    {
        Vector3 velocity = rb.linearVelocity;

        float speed = velocity.magnitude;

        // Compare the car's forward direction
        // with its actual movement direction.
        float driftAngle = 0f;

        if (speed > 0.01f)
        {
            driftAngle = Vector3.Angle(
                transform.up,
                velocity
            );
        }

        bool isDrifting =
            speed >= minSpeed &&
            driftAngle >= minDriftAngle &&
            driftAngle <= 90f;

        leftTrail.emitting = isDrifting;
        rightTrail.emitting = isDrifting;
        topLeftTrail.emitting = isDrifting;
        topRightTrail.emitting = isDrifting;
    }
}
