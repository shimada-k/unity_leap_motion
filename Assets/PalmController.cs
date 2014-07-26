using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class PalmController : MonoBehaviour {

	Controller controller = new Controller();

    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;
	public GameObject PalmObject;

	// Use this for initialization
	void Start () {
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;
	}

	// Update is called once per frame
	void Update () {

		Frame frame = controller.Frame();
		HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (PalmObject.tag, hands);

        if (hand == null) {
            return;
        }
		
		InteractionBox interactionBox = frame.InteractionBox;

		var unityPalm = PalmObject;
		utils.setVisible(unityPalm, true);

		unityPalm.transform.position = utils.getScaledUnityPosition(hand.PalmPosition, interactionBox);
	}
}
