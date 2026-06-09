using System.Collections;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Engine")]
    [SerializeField] float motorForce = 2000f;
    [SerializeField] float maxSpeed = 15f;
    [SerializeField] float maxReverseSpeed = 5f;

    [Header("Steering")]
    [SerializeField] float maxSteerAngle = 30f;

    [Header("Brakes")]
    [SerializeField] float brakeForce = 5000f;

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWC;
    public WheelCollider frontRightWC;
    public WheelCollider rearLeftWC;
    public WheelCollider rearRightWC;

    [Header("Wheel Meshes")]
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    [Header("Horn")]
    public AudioSource hornAudio;

    [Header("Turn Signals")]
    public Light[] leftTurnLights;
    public Light[] rightTurnLights;
    [SerializeField] float turnBlinkInterval = 0.3f;

    Rigidbody rb;
    float hInput;
    float vInput;
    bool braking;

    bool isLeftSignalOn;
    bool isRightSignalOn;
    Coroutine leftBlinkCoroutine;
    Coroutine rightBlinkCoroutine;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f;
        rb.centerOfMass = new Vector3(0f, -0.15f, 0f);
    }

    void Start()
    {
        if (frontLeftWC == null) FindWheelColliders();
        if (frontLeftWheel == null) FindWheelMeshes();
        if (hornAudio == null) hornAudio = GetComponent<AudioSource>();
    }

    void FindWheelColliders()
    {
        foreach (Transform child in transform)
        {
            var wc = child.GetComponent<WheelCollider>();
            if (wc == null) continue;
            if (child.name.Contains("FL")) frontLeftWC = wc;
            else if (child.name.Contains("FR")) frontRightWC = wc;
            else if (child.name.Contains("RL")) rearLeftWC = wc;
            else if (child.name.Contains("RR")) rearRightWC = wc;
        }
    }

    void FindWheelMeshes()
    {
        foreach (Transform child in transform)
        {
            if (!child.name.Contains("Tire")) continue;
            if (child.name.Contains("FL")) frontLeftWheel = child;
            else if (child.name.Contains("FR")) frontRightWheel = child;
            else if (child.name.Contains("BL") || child.name.Contains("RL")) rearLeftWheel = child;
            else if (child.name.Contains("BR") || child.name.Contains("RR")) rearRightWheel = child;
        }
    }

    void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");
        braking = Input.GetKey(KeyCode.Space);

        if (Input.GetKeyDown(KeyCode.H)) PlayHorn();
        if (Input.GetKeyDown(KeyCode.Q)) ToggleLeftSignal();
        if (Input.GetKeyDown(KeyCode.E)) ToggleRightSignal();
    }

    void FixedUpdate()
    {
        float speed = rb.linearVelocity.magnitude;

        float motor = 0f;
        if (vInput > 0 && speed < maxSpeed)
            motor = motorForce * vInput;
        else if (vInput < 0 && speed < maxReverseSpeed)
            motor = motorForce * vInput;

        frontLeftWC.motorTorque = motor;
        frontRightWC.motorTorque = motor;

        float steer = hInput * maxSteerAngle;
        frontLeftWC.steerAngle = steer;
        frontRightWC.steerAngle = steer;

        float brake = braking ? brakeForce : 0f;
        frontLeftWC.brakeTorque = brake;
        frontRightWC.brakeTorque = brake;
        rearLeftWC.brakeTorque = brake;
        rearRightWC.brakeTorque = brake;

        UpdateWheel(frontLeftWC, frontLeftWheel);
        UpdateWheel(frontRightWC, frontRightWheel);
        UpdateWheel(rearLeftWC, rearLeftWheel);
        UpdateWheel(rearRightWC, rearRightWheel);
    }

    void UpdateWheel(WheelCollider wc, Transform wheelMesh)
    {
        if (wc == null || wheelMesh == null) return;
        wc.GetWorldPose(out var pos, out var rot);
        wheelMesh.SetPositionAndRotation(pos, rot);
    }

    void PlayHorn()
    {
        if (hornAudio == null) return;
        hornAudio.Stop();
        hornAudio.Play();
    }

    void ToggleLeftSignal()
    {
        if (isLeftSignalOn)
        {
            StopBlinkCoroutine(ref leftBlinkCoroutine);
            SetLightsEnabled(leftTurnLights, false);
            isLeftSignalOn = false;
        }
        else
        {
            if (isRightSignalOn) ToggleRightSignal();
            isLeftSignalOn = true;
            leftBlinkCoroutine = StartCoroutine(BlinkLights(leftTurnLights));
        }
    }

    void ToggleRightSignal()
    {
        if (isRightSignalOn)
        {
            StopBlinkCoroutine(ref rightBlinkCoroutine);
            SetLightsEnabled(rightTurnLights, false);
            isRightSignalOn = false;
        }
        else
        {
            if (isLeftSignalOn) ToggleLeftSignal();
            isRightSignalOn = true;
            rightBlinkCoroutine = StartCoroutine(BlinkLights(rightTurnLights));
        }
    }

    IEnumerator BlinkLights(Light[] lights)
    {
        while (true)
        {
            SetLightsEnabled(lights, true);
            yield return new WaitForSeconds(turnBlinkInterval);
            SetLightsEnabled(lights, false);
            yield return new WaitForSeconds(turnBlinkInterval);
        }
    }

    void SetLightsEnabled(Light[] lights, bool enabled)
    {
        if (lights == null) return;
        foreach (var l in lights)
        {
            if (l != null) l.enabled = enabled;
        }
    }

    void StopBlinkCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
