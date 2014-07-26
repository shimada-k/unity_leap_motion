using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class WristController : MonoBehaviour {
	
	Controller controller = new Controller();
    private Frame basisFrame;
    private Hand basisHand;

    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;
	public GameObject WristObject;
	
	// Use this for initialization
	void Start () {
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;

        HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (WristObject.tag, hands);

        basisFrame = frame;
        basisHand = hand;
	}
	
	// Update is called once per frame
	void Update () {
		
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag(WristObject.tag, hands);
	
        if (hand == null) {
            return;
        }

		InteractionBox interactionBox = frame.InteractionBox;
		
		var unityWrist = WristObject;
		utils.setVisible(unityWrist, true);

        Matrix basis;

        if (basisHand != null) {
            basis = basisHand.Basis;
        }
        else {
            basis = hand.Basis;
        }

        float angleX = hand.RotationAngle(basisFrame, basis.xBasis);
        float angleY = hand.RotationAngle(basisFrame, basis.yBasis);
        float angleZ = hand.RotationAngle(basisFrame, basis.zBasis);

        // 回転する
        unityWrist.transform.Rotate(new Vector3(
                                        LMUtils.rot2Dir(angleX),
                                        LMUtils.rot2Dir(angleY),
                                        -1 * (LMUtils.rot2Dir(angleZ))));
		unityWrist.transform.position = utils.getScaledUnityPosition(hand.WristPosition, interactionBox);
        // 次回のために保存
        basisFrame = frame;
        basisHand = hand;
	}
}
