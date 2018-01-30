using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static Vector3 holdScreenPos3;
    public static Vector2 holdScreenPos2;

    public static Vector3 holdWorldPos;

    public static bool clicked;
    public static Vector3 clickScreenPos;

    public float clickThresh = 0.2f;

#if UNITY_ANDROID || UNITY_IOS
    List<int> touchIDs;
    Hashtable touchLength;
    Hashtable touches;
#endif

    // Use this for initialization
    void Start () {
#if UNITY_ANDROID || UNITY_IOS
        touchIDs = new List<int>();
        touchLength = new Hashtable();
        touches = new Hashtable();
#endif
    }

    // Update is called once per frame
    void Update () {
#if UNITY_EDITOR
        //Debug.Log("Editor");
        holdScreenPos3 = Input.mousePosition;
        holdScreenPos2 = holdScreenPos3;
        {
            var ray = Camera.main.ScreenPointToRay(holdScreenPos3);
            var distance = Destructible2D.D2dHelper.Divide(ray.origin.z, ray.direction.z);

            holdWorldPos = ray.origin - ray.direction * distance;
        }

        clicked = Input.GetMouseButtonDown(0);
        clickScreenPos = holdScreenPos3;
#endif
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        Debug.Log("mobile");
        clicked = false;
        if (Input.touchCount > 0) {
            foreach (Touch t in Input.touches) {
                switch (t.phase) {
                    case TouchPhase.Began: {
                            touchIDs.Add(t.fingerId);
                            touchLength.Add(t.fingerId, 0f);
                            touches.Add(t.fingerId, t);
                            break;
                        }
                    case TouchPhase.Ended: {
                            if ((float)touchLength[t.fingerId] != 0 && (float)touchLength[t.fingerId] < clickThresh) {
                                clicked = true;
                                clickScreenPos = t.position;
                            }

                            touchIDs.Remove(t.fingerId);
                            touchLength.Remove(t.fingerId);
                            touches.Remove(t.fingerId);
                            break;
                        }
                    case TouchPhase.Stationary: {
                            touchLength[t.fingerId] = (float)touchLength[t.fingerId] + Time.deltaTime;
                            touches[t.fingerId] = t;
                            break;
                        }
                    case TouchPhase.Moved: {
                            touchLength[t.fingerId] = (float)touchLength[t.fingerId] + Time.deltaTime;
                            touches[t.fingerId] = t;
                            break;
                        }
                    case TouchPhase.Canceled: {
                            touchIDs.Remove(t.fingerId);
                            touchLength.Remove(t.fingerId);
                            touches.Remove(t.fingerId);
                            break;
                        }
                }
            }
            float longestTime = -1f;
            Touch longestTouch = Input.touches[0];
            foreach (int id in touchIDs) {
                if ((float)touchLength[id] > longestTime) {
                    longestTime = (float)touchLength[id];
                    longestTouch = (Touch)touches[id];
                }
            }
            holdScreenPos2 = longestTouch.position;
            holdScreenPos3 = holdScreenPos2;
            {
                var ray = Camera.main.ScreenPointToRay(holdScreenPos3);
                var distance = Destructible2D.D2dHelper.Divide(ray.origin.z, ray.direction.z);

                holdWorldPos = ray.origin - ray.direction * distance;
            }

        }

#endif
    }
}
