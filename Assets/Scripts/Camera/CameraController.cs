using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    public Camera Cam => cam;

    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform cornerMin;
    public Transform CornerMin => cornerMin;

    [SerializeField] private Transform cornerMax;
    public Transform CornerMax => cornerMax;

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float zoomModifier;

    [SerializeField] private float minZoomDist;
    [SerializeField] private float maxZoomDist;

    [SerializeField] private float dist; //between camera base and camera

    [Header("Rotation")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private Quaternion newRotation;

    public static CameraController instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        cam = Camera.main;
    }

    private void Start()
    {
        newRotation = transform.rotation;
    }

    private void Update()
    {
        MoveByKeyboard();
        Zoom();
        Rotate();
    }

    private void MoveByKeyboard()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);

        transform.position += moveSpeed * Time.deltaTime * dir;
        transform.position = Clamp(cornerMin.position, cornerMax.position);
    }

    private Vector3 Clamp(Vector3 lowerLeft, Vector3 topRight)
    {
        var pos = new Vector3(
            Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, lowerLeft.z, topRight.z)
            );

        return pos;
    }

    private void Zoom()
    {
        zoomModifier = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.Z))
            zoomModifier = 0.01f;
        if (Input.GetKey(KeyCode.X))
            zoomModifier = -0.01f;

        dist = Vector3.Distance(transform.position, cam.transform.position);

        if (dist < minZoomDist && zoomModifier > 0f)
            return; //too close
        else if (dist > maxZoomDist && zoomModifier < 0f)
            return; //too far

        cam.transform.position += zoomModifier * zoomSpeed * cam.transform.forward;
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(rotationAmount * Vector3.up);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(-rotationAmount * Vector3.up);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * moveSpeed);
    }

    public void FocusOnPostion(Vector3 pos)
    {
        transform.position = pos;
    }
}
