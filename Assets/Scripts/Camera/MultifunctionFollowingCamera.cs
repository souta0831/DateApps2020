using UnityEngine;

public class MultifunctionFollowingCamera : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public bool enableInput = true;
    [SerializeField] public bool simulateFixedUpdate = false;
    [SerializeField] public bool enableDollyZoom = true;
    [SerializeField] public bool enableWallDetection = true;
    [SerializeField] public bool enableFixedPoint = false;
    [SerializeField] public float inputSpeed = 4.0f;
    [SerializeField] public Vector3 freeLookRotation;
    [SerializeField] public float height;
    [SerializeField] public float distance = 8.0f;
    [SerializeField] public Vector3 rotation;
    [SerializeField] [Range(0.01f, 100.0f)] public float positionDamping = 16.0f;
    [SerializeField] [Range(0.01f, 100.0f)] public float rotationDamping = 16.0f;
    [SerializeField] [Range(0.1f, 0.99f)] public float dolly = 0.34f;
    [SerializeField] public float noise = 0.0f;
    [SerializeField] public float noiseZ = 0.0f;
    [SerializeField] public float noiseSpeed = 1.0f;
    [SerializeField] public Vector3 vibration = Vector3.zero;
    [SerializeField] public float wallDetectionDistance = 0.3f;

    private Camera cam;
    private float targetDistance;
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 targetFree;
    private float targetHeight;
    private float targetDolly;

    void Start()
    {
        cam = GetComponent<Camera>();

        targetDistance = distance;
        targetRotation = rotation;
        targetFree = freeLookRotation;
        targetHeight = height;
        targetDolly = dolly;

        var dollyDist = targetDistance;
        if (enableDollyZoom)
        {
            var dollyFoV = GetDollyFoV(Mathf.Pow(1.0f / targetDolly, 2.0f), targetDistance);
            dollyDist = GetDollyDistance(dollyFoV, targetDistance);
            cam.fieldOfView = dollyFoV;
        }
        if (target == null) return;
        var pos = target.position + Vector3.up * targetHeight;
        var offset = Vector3.zero;
        offset.x += Mathf.Sin(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        targetPosition = pos + offset;
    }

    void Update()
    {
        if (!simulateFixedUpdate) Simulate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (simulateFixedUpdate) Simulate(Time.fixedDeltaTime);
    }

    private void Simulate(float deltaTime)
    {
        if (enableInput)
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                dolly += Input.GetAxis("Mouse ScrollWheel") * 0.2f;
                dolly = Mathf.Clamp(dolly, 0.1f, 0.99f);
            }
            else
            {
                distance *= 1.0f - Input.GetAxis("Mouse ScrollWheel");
                distance = Mathf.Clamp(distance, 0.01f, 1000.0f);
            }

            if (Input.GetMouseButton(0))
            {
                rotation.x -= Input.GetAxis("Mouse Y") * inputSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -89.9f, 89.9f);
                rotation.y -= Input.GetAxis("Mouse X") * inputSpeed;
            }
            if (Input.GetMouseButton(1))
            {
                freeLookRotation.x -= Input.GetAxis("Mouse Y") * inputSpeed * 0.2f;
                freeLookRotation.y += Input.GetAxis("Mouse X") * inputSpeed * 0.2f;
            }
            if (Input.GetMouseButton(2))
            {
                height += Input.GetAxis("Mouse Y") * inputSpeed * 0.04f;
            }
        }

        var posDampRate = Mathf.Clamp01(deltaTime * 100.0f / positionDamping);
        var rotDampRate = Mathf.Clamp01(deltaTime * 100.0f / rotationDamping);

        targetDistance = Mathf.Lerp(targetDistance, distance, posDampRate);
        targetRotation = Vector3.Lerp(targetRotation, rotation, rotDampRate);
        targetFree = Vector3.Lerp(targetFree, freeLookRotation, rotDampRate);
        targetHeight = Mathf.Lerp(targetHeight, height, posDampRate);
        targetDolly = Mathf.Lerp(targetDolly, dolly, posDampRate);

        if (Mathf.Abs(targetDolly - dolly) > 0.005f)
        {
            targetDistance = distance;
        }

        var dollyDist = targetDistance;
        if (enableDollyZoom)
        {
            var dollyFoV = GetDollyFoV(Mathf.Pow(1.0f / targetDolly, 2.0f), targetDistance);
            dollyDist = GetDollyDistance(dollyFoV, targetDistance);
            cam.fieldOfView = dollyFoV;
        }

        if (target == null) return;

        var pos = target.position + Vector3.up * targetHeight;

        if (enableWallDetection)
        {
            RaycastHit hit;
            var dir = (targetPosition - pos).normalized;
            if (Physics.SphereCast(pos, wallDetectionDistance, dir, out hit, dollyDist))
            {
                dollyDist = hit.distance;
            }
        }

        var offset = Vector3.zero;
        offset.x += Mathf.Sin(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(targetRotation.x * Mathf.Deg2Rad) * dollyDist;

        if (Mathf.Abs(targetDolly - dolly) > 0.005f)
        {
            targetPosition = offset + pos;
        }
        else
        {
            targetPosition = Vector3.Lerp(targetPosition, offset + pos, posDampRate);
        }

        if (!enableFixedPoint) cam.transform.position = targetPosition;
        cam.transform.LookAt(pos, Quaternion.Euler(0.0f, 0.0f, targetRotation.z) * Vector3.up);
        cam.transform.Rotate(targetFree);

        if (noise > 0.0f || noiseZ > 0.0f)
        {
            var rotNoise = Vector3.zero;
            rotNoise.x = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.0f) - 0.5f) * noise;
            rotNoise.y = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.4f) - 0.5f) * noise;
            rotNoise.z = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.8f) - 0.5f) * noiseZ;
            cam.transform.Rotate(rotNoise);
        }

        if (vibration.sqrMagnitude > 0.0f)
        {
            cam.transform.Rotate(new Vector3(Random.Range(-1.0f, 1.0f) * vibration.x, Random.Range(-1.0f, 1.0f) * vibration.y, Random.Range(-1.0f, 1.0f) * vibration.z));
        }
    }

    private float GetDollyDistance(float fov, float distance)
    {
        return distance / (2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));
    }

    private float GetFrustomHeight(float distance, float fov)
    {
        return 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
    }

    private float GetDollyFoV(float dolly, float distance)
    {
        return 2.0f * Mathf.Atan(distance * 0.5f / dolly) * Mathf.Rad2Deg;
    }
}