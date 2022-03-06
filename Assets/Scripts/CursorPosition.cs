using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorPosition : MonoBehaviour
{

    public RectTransform imageRect;
    public float smoothSpeed = 0.125f;
    public float handPosOffset = 0.1f;
    private float width;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 handPos = GetHandPos();
        if (StaticPosition.isHandAvailable)
        {
            Vector3 desiredPos = new Vector3((handPos.x * (1 + 2 * handPosOffset) - handPosOffset) * width, height - (handPos.y * (1 + 2 * handPosOffset) - handPosOffset) * height, 0);
            Vector3 currentPos = new Vector3(imageRect.rect.position.x, imageRect.rect.position.y, 0);
            
            Vector3 smoothedPos = Vector3.Lerp(imageRect.transform.position, desiredPos, smoothSpeed * Time.deltaTime);
            imageRect.SetPositionAndRotation(smoothedPos, Quaternion.identity);
        }
        else
        {

        }
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
}
