using System;
using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class BoneController : MonoBehaviour {
	
	Controller controller = new Controller();

	private int fingerIndex;
    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;
	public GameObject[] JointObjects;
	public GameObject parentFinger;

	void Start()
	{
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;

		//GameObject Fingers = GameObject.Find ("Fingers");

		//if (Fingers.GetComponent<FingerController> ().FingerObjects != null) {
			//FingerObjects = Fingers.GetComponent<FingerController> ().FingerObjects;
		//	Debug.Log(FingerObjects.Length);
		//}
		fingerIndex = int.Parse(parentFinger.name.Substring(6));
	}
	
	void Update()
	{
		Frame frame = controller.Frame();

        HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (parentFinger.transform.parent.tag, hands);

        if (hand == null) {
            return;
        }

		var leapFinger = hand.Fingers[fingerIndex];

		Bone bone;

		foreach (Bone.BoneType boneType in (Bone.BoneType[]) Enum.GetValues(typeof(Bone.BoneType)))
		{
			Finger.FingerType fingerType = leapFinger.Type();
			if(fingerType == Finger.FingerType.TYPE_THUMB &&
			   boneType == Bone.BoneType.TYPE_METACARPAL){
				utils.setVisible(JointObjects[(int)boneType], false);
				continue;
			}

			bone = leapFinger.Bone(boneType);
			utils.setVisible(JointObjects[(int)boneType], true);

			doLineRender((int)boneType, bone);

            JointObjects[(int)boneType].transform.position = utils.getScaledUnityPosition(bone.PrevJoint, interactionBox);
		}
	}

	void doLineRender(int jointIndex, Bone bone)
	{
		var lineRenderer =JointObjects[jointIndex].GetComponent<LineRenderer>();
		
		lineRenderer.enabled = true;
		lineRenderer.SetVertexCount(2);

        var prev_position = utils.getScaledUnityPosition(bone.PrevJoint, interactionBox);
        var next_position = utils.getScaledUnityPosition(bone.NextJoint, interactionBox);

		lineRenderer.SetPosition(0, prev_position);
		lineRenderer.SetPosition(1, next_position);
	}


	Vector3 getScaledUnityPosition(Vector position)
	{
		Vector normalizedPosition = interactionBox.NormalizePoint(position);
		normalizedPosition.x *= 10;
		normalizedPosition.y *= 10;
		normalizedPosition.z *= 10;
		
		normalizedPosition.z = -normalizedPosition.z;
		
		return new UnityEngine.Vector3(normalizedPosition.x,
		                               normalizedPosition.y,
		                               normalizedPosition.z);
	}
}