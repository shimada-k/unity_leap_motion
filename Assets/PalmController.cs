using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class PalmController : MonoBehaviour {

	Controller controller = new Controller();

    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;

	// Use this for initialization
	void Start () {
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;
	}

	// Update is called once per frame
	void Update () {

		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (gameObject.tag, hands);

        if (hand == null) {
            return;
        }
		
		InteractionBox interactionBox = frame.InteractionBox;

		utils.setVisible(gameObject, true);

		gameObject.transform.position = utils.getScaledUnityPosition(hand.PalmPosition, interactionBox);
	}
}
