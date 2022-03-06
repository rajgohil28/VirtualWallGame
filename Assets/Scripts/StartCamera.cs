using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{
    WebCamDevice? webCamDevice;
    GameObject webCamScreen;
    Coroutine cameraSetupCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        webCamScreen = GameObject.Find("WebCamScreen");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        StopCamera();
    }

    public void ResetCamera(WebCamDevice? webCamDevice)
    {
        StopCamera();
        cameraSetupCoroutine = StartCoroutine(webCamScreen.GetComponent<WebCamScreenController>().ResetScreen(webCamDevice));
        this.webCamDevice = webCamDevice;
    }

    void StopCamera()
    {
        if (cameraSetupCoroutine != null)
        {
            StopCoroutine(cameraSetupCoroutine);
            cameraSetupCoroutine = null;
        }
    }
}
