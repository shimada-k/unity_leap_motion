using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class WristController : MonoBehaviour {
	
	Controller controller = new Controller();
    private Frame basisFrame;
    private Hand basisHand;
    private Quaternion basisRotation;

    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;

	// Use this for initialization
	void Start () {
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;

        HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (gameObject.tag, hands);

        utils.setVisible(gameObject, true);
        basisFrame = frame;
        basisHand = hand;
        basisRotation = gameObject.transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		
		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag(gameObject.tag, hands);
	
        // Handが取得できなかったら終了
        if (hand == null) {
            return;
        }

		InteractionBox interactionBox = frame.InteractionBox;

        Matrix basis;

        gameObject.transform.position = utils.getScaledUnityPosition(hand.WristPosition,
                                                                        interactionBox);
        if (basisHand == null) {
            // 回転の基準になるhandが無かったら保存して終了
            basisFrame = frame;
            basisHand = hand;
            return;
        }

        float angleX = hand.RotationAngle(basisFrame, basisHand.Basis.xBasis);
        float angleY = hand.RotationAngle(basisFrame, basisHand.Basis.yBasis);
        float angleZ = hand.RotationAngle(basisFrame, basisHand.Basis.zBasis);

        // 回転する
        gameObject.transform.rotation = basisRotation * Quaternion.Euler(LMUtils.rot2Dir(angleX),
                                                          LMUtils.rot2Dir(angleY),
                                                          -1 * (LMUtils.rot2Dir(angleZ)));
        //gameObject.transform.Rotate(new Vector3(
        //                                LMUtils.rot2Dir(angleX),
        //                                LMUtils.rot2Dir(angleY),
        //                                -1 * (LMUtils.rot2Dir(angleZ))));
      
        //gameObject.transform.rotation = new Vector3(angleX, angleY, angleZ);

        // 次回のために保存
        basisRotation = gameObject.transform.rotation;
        basisFrame = frame;
        basisHand = hand;
	}
}
