using UnityEngine;
using DG.Tweening;

public class Basket : MonoBehaviour
{
    public ScoreCounter scoreCounter;

    [Header("Steering")]
    public float rotationSmoothTime = 0.15f;

    [Header("Movement")]
    public float acceleration = 12f;
    public float maxSpeed = 8f;
    public float stopDistance = 0.5f;
    public float fullAccelerationDistance = 5f;

    [Header("Drifting")]
    public float directionGrip = 2f;

    [Header("Wall Collision")]
    public float wallSlowdown = 0.8f;

    private Rigidbody rb;
    private float rotationVelocity;
    private Vector3 mouseScreenPosition;

    
    [Header("Wall Impact Animation")]
    public Transform carVisual;

    public float impactDuration = 0.3f;
    public float impactStrength = 0.2f;

    private Tween impactTween;
    private ApplePicker gameManager;
    private bool canControl = true;
    void Start()
    {
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        rb = GetComponent<Rigidbody>();
        gameManager = FindFirstObjectByType<ApplePicker>();
    }
    void Update()
    {
        mouseScreenPosition = Input.mousePosition;
    }
    void FixedUpdate()
    {
        if (!canControl)
            return;
        if (gameManager == null || gameManager.currentState != ApplePicker.GameState.Racing)
        {
            return;
        }
        // Mouse position in world space
        Vector3 mousePos = mouseScreenPosition;
        mousePos.z = -Camera.main.transform.position.z;

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 direction =
            mouseWorld - rb.position;

        // Rotation
        float targetAngle =
            Mathf.Atan2(direction.y, direction.x)
            * Mathf.Rad2Deg - 90f;

        float angle = Mathf.SmoothDampAngle(
            rb.rotation.eulerAngles.z,
            targetAngle,
            ref rotationVelocity,
            rotationSmoothTime,
            Mathf.Infinity,
            Time.fixedDeltaTime
        );

        rb.MoveRotation(
            Quaternion.Euler(0f, 0f, angle)
        );

        // Distance-based acceleration
        float distance = direction.magnitude;

        float throttle = Mathf.InverseLerp(
            stopDistance,
            fullAccelerationDistance,
            distance
        );

        // Calculate forward direction from new rotation
        Vector3 forward =
            Quaternion.Euler(0f, 0f, angle) * Vector3.up;

        // Apply force
        rb.AddForce(
            forward * acceleration * throttle,
            ForceMode.Acceleration
        );
        
        // Limit maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity =
                rb.linearVelocity.normalized * maxSpeed;
        }
        Vector3 currentVelocity = rb.linearVelocity;

        if (currentVelocity.magnitude > 0.1f)
        {
            Vector3 targetVelocity =
                transform.up * currentVelocity.magnitude;

            rb.linearVelocity = Vector3.Lerp(
                currentVelocity,
                targetVelocity,
                1f - Mathf.Exp(-directionGrip * Time.fixedDeltaTime)
            );
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall collision detected!");

            rb.linearVelocity *= wallSlowdown;
            // Stop previous impact animation
            if (impactTween != null && impactTween.IsActive())
            {
                impactTween.Complete();
            }

            // Animate the car's visual scale
            impactTween = carVisual.DOPunchScale(
                new Vector3(
                    -impactStrength,
                    impactStrength,
                    0f
                ),
                impactDuration,
                8,
                0.5f
            );
        }
    }
    public void DisableControls()
    {
        canControl = false;
    }
}