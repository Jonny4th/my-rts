using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;

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
        moveSpeed = 50;
    }

    private void Update()
    {
        MoveByKeyboard();
    }

    private void MoveByKeyboard()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);

        transform.position += moveSpeed * Time.deltaTime * dir;
    }
}
