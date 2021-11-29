using UnityEngine;
using UnityEngine.EventSystems;

public class MovementJoystick
{
    private Vector2 _joystickVec;
    private Vector2 _joystickTouchPos;

    private float _joystickRadius;
    private float _multiplierByDistanceFromCenter;


    public void Initialize()
    {
        _joystickRadius = 110f;
    }


    public void PointerDown()
    {
        //_joystickDot.transform.position = Input.mousePosition;
        //_joystickCircle.transform.position = Input.mousePosition;

        //_joystickOriginalPos = Input.mousePosition;
        _joystickTouchPos = Input.mousePosition;
    }


    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;
        _joystickVec = (dragPos - _joystickTouchPos).normalized;

        float joystickDist = Vector2.Distance(dragPos, _joystickTouchPos);

        if (joystickDist < _joystickRadius)
        {
            _multiplierByDistanceFromCenter = joystickDist;
            //_joystickDot.transform.position = _joystickTouchPos + _joystickVec * joystickDist;
        }
        else
        {
            _multiplierByDistanceFromCenter = _joystickRadius;
            //_joystickDot.transform.position = _joystickTouchPos + _joystickVec * _joystickRadius;
        }
    }


    public void PointerUp()
    {
        _joystickVec = Vector2.zero;
        //_joystickDot.transform.position = _joystickOriginalPos;
        //_joystickCircle.transform.position = _joystickOriginalPos;
    }


    public Vector2 GetJoystickDirection
    {
        get { return _joystickVec; }
    }


    public float MultiplierByDistanceFromCenter
    {
        get { return _multiplierByDistanceFromCenter; }
    }
}
