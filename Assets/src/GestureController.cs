using UnityEngine;
using System.Collections;
using Leap;
using LeapMotionUtils;

public class GestureController : MonoBehaviour {

    Controller controller = new Controller();
    public GameObject gauge;
    private LMUtils utils = new LMUtils();
    private int selectedGestureId;
    private Vector3 trail_sphere_basis = new Vector3((float)6.917379, (float)2.825647, (float)-10.48881);

	// Use this for initialization
	void Start () {
        gameObject.renderer.material.color = Color.clear;
        controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
	}

    void detectSwipeGesture(SwipeGesture gesture){
        Vector swipeDirection = gesture.Direction;
        Debug.Log ("Direction" + swipeDirection);

        // swipeの向きによってtrailRendererの向きを調整する
        if(swipeDirection.x <= 0){
            gameObject.transform.Translate ((float)-1.2, (float)0, (float)0);
        }
        else{
            gameObject.transform.Translate ((float)1.2, (float)0, (float)0);
        }

        // ゲージを減らす
        if(gauge.transform.lossyScale.x >= 0){
            gauge.transform.localScale += new Vector3((float)-0.8, (float)0, (float)0);
        }
    }

	// Update is called once per frame
	void Update () {
        // ジェスチャーを検出してDurationSecondsが最長の一つを採用する
        Frame frame = controller.Frame();
        float max_duration = 0;
        Gesture max_duration_gesture = null;

        // durationの最大値を持つものを探して画面に反映させる
        for(int g = 0; g < frame.Gestures().Count; g++){
            float _duration = frame.Gestures()[g].Duration;
            if (_duration > max_duration) {
                max_duration = _duration;
                max_duration_gesture = frame.Gestures()[g];
            }
        }

        if(max_duration > 0 && max_duration_gesture.State == Gesture.GestureState.STATESTOP){
            // 座標をもとに戻す
            gameObject.GetComponent<TrailRenderer>().enabled = false;
            gameObject.transform.position = trail_sphere_basis;
        }
        else if(max_duration > 0 &&
                (max_duration_gesture.State == Gesture.GestureState.STATESTART ||
                max_duration_gesture.State == Gesture.GestureState.STATEUPDATE) ){

            if(gameObject.GetComponent<TrailRenderer>().enabled == false){
                gameObject.GetComponent<TrailRenderer>().enabled = true;
            }
            detectSwipeGesture (new SwipeGesture(max_duration_gesture));
        }
	}
}
