using Mediapipe;
using UnityEngine;
class PoseTrackingValue {
  public readonly NormalizedLandmarkList PoseLandmarkList;
  public readonly Detection PoseDetection;

  public PoseTrackingValue(NormalizedLandmarkList landmarkList, Detection detection) {
    PoseLandmarkList = landmarkList;
    PoseDetection = detection;
        if (landmarkList.Landmark.Count > 20)
        {
            //Debug.Log("Right X: " + landmarkList.Landmark[15].X + " Y: " + landmarkList.Landmark[15].Y);
            //Debug.Log("Left X: " + landmarkList.Landmark[16].X + " Y: " + landmarkList.Landmark[16].Y);
            StaticPosition.RightHandPos = new Vector2(landmarkList.Landmark[19].X, landmarkList.Landmark[19].Y);
            StaticPosition.LeftHandPos = new Vector2(landmarkList.Landmark[20].X, landmarkList.Landmark[20].Y);

        }
    }

  public PoseTrackingValue(NormalizedLandmarkList landmarkList) : this(landmarkList, new Detection()) {}

  public PoseTrackingValue() : this(new NormalizedLandmarkList()) {}
}
