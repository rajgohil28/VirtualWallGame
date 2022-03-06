using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseGraphSelector : MonoBehaviour
{
    [SerializeField] GameObject poseTrackingGraph = null;
    private GameObject sceneDirector;
    // Start is called before the first frame update
    void Start()
    {
        sceneDirector = GameObject.Find("SceneDirector");
        sceneDirector.GetComponent<SceneDirector>().ChangeGraph(poseTrackingGraph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
