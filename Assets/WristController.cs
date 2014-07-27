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
	
	// Use this for initialization
	void Start () {
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;

        HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (gameObject.tag, hands);

        basisFrame = frame;
        basisHand = hand;
	}
	
	// Update is called once per frame
	void Update () {
		
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag(gameObject.tag, hands);
	
        if (hand == null) {
            return;
        }

		InteractionBox interactionBox = frame.InteractionBox;
		
		utils.setVisible(gameObject, true);

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
        gameObject.transform.Rotate(new Vector3(
                                        LMUtils.rot2Dir(angleX),
                                        LMUtils.rot2Dir(angleY),
                                        -1 * (LMUtils.rot2Dir(angleZ))));
		gameObject.transform.position = utils.getScaledUnityPosition(hand.WristPosition,
                                                                        interactionBox);
        // 次回のために保存
        basisFrame = frame;
        basisHand = hand;
	}
}
