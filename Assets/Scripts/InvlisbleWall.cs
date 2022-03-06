using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InvlisbleWall : MonoBehaviour
{
    public Transform targetR;
    public Transform targetL;
    public float targetDistance;
    public float handPosOffset = 0.1f;
    public float smoothSpeed = 0.95f;
    private float width;
    private float height;
    private Vector3 prevPosR;
    private Vector3 prevPosL;

    public Joystick joystick;
    public PhotonView PV;
    public Camera cam;
    // Start is called before the first frame update
    private void Awake()
    {
        width = Screen.width;
        height = Screen.height;
        PV = gameObject.GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PV.IsMine)
            return;
        Destroy(gameObject.GetComponentInChildren<Camera>().gameObject);
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().forceRenderingOff = true;
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().forceRenderingOff = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        MoveWithKeyboard();
        
        //MoveWithTouch();
        Move();
    }

    void Move()
    {
        GetHandPos();
        Vector3 inputVecR = new Vector3();
        Vector3 inputVecL = new Vector3();
        Vector2 handPosR = StaticPosition.RightHandPos;
        Vector2 handPosL = StaticPosition.LeftHandPos;

        if (StaticPosition.isHandAvailable)
        {
            inputVecR = new Vector3((handPosR.x * (1 + 2 * handPosOffset) - handPosOffset) * width, height - (handPosR.y * (1 + 2 * handPosOffset) - handPosOffset) * height, 0);
            inputVecL = new Vector3((handPosL.x * (1 + 2 * handPosOffset) - handPosOffset) * width, height - (handPosL.y * (1 + 2 * handPosOffset) - handPosOffset) * height, 0);
        }
        else
        {
            inputVecR = prevPosR;
            inputVecL = prevPosL;
            //inputVec = Input.mousePosition;
        }

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(inputVecR);
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Invisible")))
        {
            Vector3 desiredPos = hit.point;
            Vector3 smoothedPos = Vector3.Lerp(targetR.position, desiredPos, smoothSpeed * Time.deltaTime);
            targetR.position = smoothedPos;
        }
        ray = cam.ScreenPointToRay(inputVecL);
        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Invisible")))
        {
            Vector3 desiredPos = hit.point;
            Vector3 smoothedPos = Vector3.Lerp(targetL.position, desiredPos, smoothSpeed * Time.deltaTime);
            targetL.position = smoothedPos;
        }


        prevPosR = inputVecR;
        prevPosL = inputVecL;



    }
    Vector2 GetHandPos()
    {
        bool isRightHandAvail = false;
        bool isLeftHandAvail = false;

        if (StaticPosition.RightHandPos.x > 0 && StaticPosition.RightHandPos.x < 1 && StaticPosition.RightHandPos.y > 0 && StaticPosition.RightHandPos.y < 1)
            isRightHandAvail = true;
        else isRightHandAvail = false;

        if (StaticPosition.LeftHandPos.x > 0 && StaticPosition.LeftHandPos.x < 1 && StaticPosition.LeftHandPos.y > 0 && StaticPosition.LeftHandPos.y < 1)
            isLeftHandAvail = true;
        else isLeftHandAvail = false;

        Vector2 handPos = new Vector2();
        if (isLeftHandAvail) handPos = StaticPosition.LeftHandPos;
        if (isRightHandAvail) handPos = StaticPosition.RightHandPos;

        if (isLeftHandAvail && isRightHandAvail) handPos = StaticPosition.RightHandPos;

        StaticPosition.isHandAvailable = isLeftHandAvail || isRightHandAvail;


        return handPos;
    }

    void MoveWithKeyboard()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0f);

        targetR.Translate(direction * 10f * Time.deltaTime);
        targetL.Translate(direction * 10f * Time.deltaTime);
    }

    void MoveWithTouch()
    {
        joystick = GameObject.FindObjectOfType<Joystick>();

        Vector3 direction = new Vector3(joystick.Horizontal, joystick.Vertical, 0f);

        targetR.Translate(direction * 10f * Time.deltaTime);
        targetL.Translate(direction * 10f * Time.deltaTime);

    }
}
