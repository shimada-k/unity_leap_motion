using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;


public class FingerController : MonoBehaviour {
	
	Controller controller = new Controller();
	
    private LMUtils utils = new LMUtils();
	private InteractionBox interactionBox;
	public GameObject[] FingerObjects;
	
	void Start()
	{
		Frame frame = controller.Frame();
		interactionBox = frame.InteractionBox;
	}
	
	void Update()
	{
        Frame frame = controller.Frame ();
        HandList hands = frame.Hands;
        GameObject own = FingerObjects[0].transform.parent.gameObject;
        Hand hand = utils.getHandByTag(own.tag, hands);

        if (hand == null) {
            return;
        }

		InteractionBox interactionBox = frame.InteractionBox;

        int i = 0;
        foreach(Finger f in hand.Fingers){

			var unityFinger = FingerObjects[i];
            utils.setVisible( unityFinger, true);
			if ( f.IsValid ) {

				Finger.FingerType fingerType = f.Type();
				unityFinger.renderer.material.color = Color.white;

                unityFinger.transform.position = utils.getScaledUnityPosition(f.TipPosition, interactionBox);
			}
            i++;
		}
	}
}