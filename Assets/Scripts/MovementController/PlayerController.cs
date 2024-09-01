using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }
    [field: SerializeField] public Transform CameraLookAt { get; private set; }
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;

    public InputData inputData = new InputData();

    private const float GROUND_CHECK_RADIUS = 0.001f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        MovementController.instance.AddMoveUnit(MoveUnit);
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.W))
            inputData.MoveUp = true;
        else if(Input.GetKeyDown(KeyCode.A))
            inputData.MoveLeft = true;
        else if(Input.GetKeyDown(KeyCode.D))
            inputData.MoveRight = true;
        #endif

        if (inputData.MoveUp)
            MoveUp();
        else if (inputData.MoveLeft)
            MoveLeft();
        else if (inputData.MoveRight)
            MoveRight();
    }

    public void MoveLeft()
    {
        MoveUnit.PointNumber = Mathf.Clamp(MoveUnit.PointNumber - 1, 0, 2);
        inputData.MoveLeft = false;
    }

    public void MoveRight()
    {
        MoveUnit.PointNumber = Mathf.Clamp(MoveUnit.PointNumber + 1, 0, 2);
        inputData.MoveRight = false;
    }

    public void MoveUp()
    {
        if (MoveUnit.jump == false && isGrounded())
            MoveUnit.jump = true;
        inputData.MoveUp = false;
    }

    public void ProcessInput(InputData i)
    {
        if (inputData.MoveUp == false)
            inputData.MoveUp = i.MoveUp;

        if (inputData.MoveRight == false)
            inputData.MoveRight = i.MoveRight;

        if (inputData.MoveLeft == false)
            inputData.MoveLeft = i.MoveLeft;
    }

    public bool isGrounded()
       => Physics.CheckSphere(groundCheck.position, GROUND_CHECK_RADIUS, groundMask);
    
}

