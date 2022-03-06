using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWebCam : MonoBehaviour
{
    private GameObject sceneDirector;
    // Start is called before the first frame update
    void Start()
    {
        sceneDirector = GameObject.Find("SceneDirector");

        

        sceneDirector.GetComponent<SceneDirector>().ChangeWebCamDevice(WebCamTexture.devices[GameManager.WebCamIndex]);
        Debug.Log(WebCamTexture.devices[GameManager.WebCamIndex].name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
