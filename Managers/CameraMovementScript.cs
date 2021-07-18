using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform cameraTransform; //Camera tranform
    private Transform rig_Transform; //Camera Rig tranform
    private bool useFixedUpdate = false; //use FixedUpdate() or Update()
    public Texture2D cursor01;

    #region Movement

    public float keyboardMovementSpeed = 20f; //speed with keyboard movement
    public float screenEdgeMovementSpeed = 10f; //spee with screen edge movement
    public float followingSpeed = 5f; //speed when following a target
    public float rotationSpeed = 10f;
    public float panningSpeed = 10f;
    public float mouseRotationSpeed = 20f;

    #endregion

    #region Height

    public bool autoHeight = true;
    public LayerMask groundMask = -1; //layermask of ground or other objects that affect height

    public float maxHeight = 10f; //maximal height
    public float minHeight = 15f; //minimnal height
    public float heightDampening = 5f;
    public float keyboardZoomingSensitivity = 2f;
    public float scrollWheelZoomingSensitivity = 25f;

    private float zoomPos = 0; //value in range (0, 1) used as t in Matf.Lerp

    #endregion

    #region MapLimits

    public bool limitMap = true;
    public float limitX = 50f; //x limit of map
    public float limitY = 50f; //z limit of map

    #endregion

    #region Targeting

    public Transform targetFollow; //target to follow
    public Vector3 targetOffset;
    private Quaternion newRot;

    public bool FollowingTarget
    {
        get
        {
            return targetFollow != null;
        }
    }

    #endregion

    #region Input

    public bool useScreenEdgeInput = true;
    public float screenEdgeBorder = 25f;

    public bool useKeyboardInput = true;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    public bool usePanning = true;
    public KeyCode panningKey = KeyCode.Mouse2;

    public bool useKeyboardZooming = true;
    public KeyCode zoomInKey = KeyCode.E;
    public KeyCode zoomOutKey = KeyCode.Q;

    public bool useScrollwheelZooming = true;
    public string zoomingAxis = "Mouse ScrollWheel";

    public bool useKeyboardRotation = true;
    public KeyCode rotateRightKey = KeyCode.X;
    public KeyCode rotateLeftKey = KeyCode.Z;

    public bool useMouseRotation = true;
    public KeyCode mouseRotationKey = KeyCode.Mouse1;

    private Vector2 KeyboardInput
    {
        get { return useKeyboardInput ? new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)) : Vector2.zero; }
    }

    private Vector2 MouseInput
    {
        get { return Input.mousePosition; }
    }

    private float ScrollWheel
    {
        get { return Input.GetAxis(zoomingAxis); }
    }

    private Vector2 MouseAxis
    {
        get { return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); }
    }

    private int ZoomDirection
    {
        get
        {
            bool zoomIn = Input.GetKey(zoomInKey);
            bool zoomOut = Input.GetKey(zoomOutKey);
            if (zoomIn && zoomOut)
                return 0;
            else if (!zoomIn && zoomOut)
                return 1;
            else if (zoomIn && !zoomOut)
                return -1;
            else
                return 0;
        }
    }

    private int RotationDirection
    {
        get
        {
            bool rotateRight = Input.GetKey(rotateRightKey);
            bool rotateLeft = Input.GetKey(rotateLeftKey);
            if (rotateLeft && rotateRight)
                return 0;
            else if (rotateLeft && !rotateRight)
                return -1;
            else if (!rotateLeft && rotateRight)
                return 1;
            else
                return 0;
        }
    }

    #endregion

    #region Unity_Methods

    private void Start()
    {
        rig_Transform = transform;
        newRot = rig_Transform.rotation;
    }

    private void Update()
    {
        if (!useFixedUpdate)
            CameraUpdate();
    }

    private void FixedUpdate()
    {

        if (useFixedUpdate)
            CameraUpdate();
    }

    #endregion

    #region RTSCamera_Methods

    private void CameraUpdate()
    {
        if (FollowingTarget)
            FollowTarget();
        else
            Move();

        HeightCalculation();
        Rotation();
        LimitPosition();
    }

    private void Move()
    {
        if (useKeyboardInput)
        {
            Vector3 desiredMove = new Vector3(KeyboardInput.x, 0, KeyboardInput.y);

            desiredMove *= keyboardMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = rig_Transform.InverseTransformDirection(desiredMove);

            rig_Transform.Translate(desiredMove, Space.Self);
        }

        if (useScreenEdgeInput)
        {
            Vector3 desiredMove = new Vector3();

            Rect leftRect = new Rect(0, 0, screenEdgeBorder, Screen.height);
            Rect rightRect = new Rect(Screen.width - screenEdgeBorder, 0, screenEdgeBorder, Screen.height);
            Rect upRect = new Rect(0, Screen.height - screenEdgeBorder, Screen.width, screenEdgeBorder);
            Rect downRect = new Rect(0, 0, Screen.width, screenEdgeBorder);

            desiredMove.x = leftRect.Contains(MouseInput) ? -1 : rightRect.Contains(MouseInput) ? 1 : 0;
            desiredMove.z = upRect.Contains(MouseInput) ? 1 : downRect.Contains(MouseInput) ? -1 : 0;

            desiredMove *= screenEdgeMovementSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = rig_Transform.InverseTransformDirection(desiredMove);

            rig_Transform.Translate(desiredMove, Space.Self);

            Cursor.SetCursor(cursor01, Vector2.zero, CursorMode.ForceSoftware);
        }

        if (usePanning && Input.GetKey(panningKey) && MouseAxis != Vector2.zero)
        {
            Vector3 desiredMove = new Vector3(-MouseAxis.x, 0, -MouseAxis.y);

            desiredMove *= panningSpeed;
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = rig_Transform.InverseTransformDirection(desiredMove);

            rig_Transform.Translate(desiredMove, Space.Self);
        }
    }

    private void HeightCalculation()
    {
        float distanceToGround = DistanceToGround();
        if (useScrollwheelZooming)
            zoomPos += ScrollWheel * Time.deltaTime * scrollWheelZoomingSensitivity;
        if (useKeyboardZooming)
            zoomPos += ZoomDirection * Time.deltaTime * keyboardZoomingSensitivity;

        zoomPos = Mathf.Clamp01(zoomPos);

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPos);
        float difference = 0;

        if (distanceToGround != targetHeight)
            difference = targetHeight - distanceToGround;

        rig_Transform.position = Vector3.Lerp(rig_Transform.position,
            new Vector3(rig_Transform.position.x, targetHeight + difference, rig_Transform.position.z), Time.deltaTime * heightDampening);
    }

    private void Rotation()
    {
        if (useKeyboardRotation)
            rig_Transform.Rotate(Vector3.up, RotationDirection * Time.deltaTime * rotationSpeed, Space.World);

        if (useMouseRotation && Input.GetKey(mouseRotationKey))
            rig_Transform.Rotate(Vector3.up, -MouseAxis.x * Time.deltaTime * mouseRotationSpeed * 10.0f, Space.World);

    }

    private void FollowTarget()
    {
        Vector3 targetPos = new Vector3(targetFollow.position.x, rig_Transform.position.y, targetFollow.position.z) + targetOffset;
        rig_Transform.position = Vector3.MoveTowards(rig_Transform.position, targetPos, Time.deltaTime * followingSpeed);
    }

    private void LimitPosition()
    {
        if (!limitMap)
            return;

        rig_Transform.position = new Vector3(Mathf.Clamp(rig_Transform.position.x, -limitX, limitX),
            rig_Transform.position.y,
            Mathf.Clamp(rig_Transform.position.z, -limitY, limitY));
    }

    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        targetFollow = target;
    }


    public void ResetTarget()
    {
        targetFollow = null;
    }

    private float DistanceToGround()
    {
        Ray ray = new Ray(rig_Transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, groundMask.value))
            return (hit.point - rig_Transform.position).magnitude;

        return 0f;
    }

    #endregion
}

