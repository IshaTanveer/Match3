using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class Fruit : MonoBehaviour
{
    private Vector2 firstTouchPosition; 
    private Vector2 finalTouchPosition;
    // 45 to 135 = up, 135 degree to 135 = left, 45 to -45 = right, -45 to -135 = down
    public float swipeAngle = 0;
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }
    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
    }
    void Start()
    {
    }
    void Update()
    {
        if(Touch.activeTouches.Count == 0) return;
        Touch touch = Touch.activeTouches[0];
        if(touch.phase == TouchPhase.Began)
        {   
            //convert screen coordinates into world coordinates
            firstTouchPosition = Camera.main.ScreenToWorldPoint(touch.screenPosition); 
        }
        else if(touch.phase == TouchPhase.Ended)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(touch.screenPosition);
            CalculateAngle();
        }
    }
    void CalculateAngle()
        {
            //returns a value in radians, so multiply it with 180 and then divide it with pi
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, 
                    finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            Debug.Log(swipeAngle);       
        }
}
