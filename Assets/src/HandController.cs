using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class HandController : MonoBehaviour {

    Controller controller = new Controller();
    private LMUtils utils = new LMUtils();
    private Vector3 gauge_basis_position = new Vector3(0, -4, -1);
    public GameObject gauge;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();

        HandList hands = frame.Hands;
        Hand hand = utils.getHandByTag (gameObject.tag, hands);

        //Debug.Log(hand.GetType() + ":" + hand.GrabStrength);

        if(hand != null){
            gauge.transform.localScale += new Vector3(hand.GrabStrength / 5, 0, 0);

            var renderer = gauge.GetComponent<Renderer>();
            float width = renderer.bounds.size.x;

            gauge.transform.position = new Vector3(width / 2, -4, -1);
        }
	}
}
