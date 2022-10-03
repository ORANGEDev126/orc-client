using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);

        if(handleInputCallback != null)
        {
            handleInputCallback(magnitude, normalised);
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if(handleInputUpCallback != null)
        {
            handleInputUpCallback(eventData.delta);
        }
    }

    public delegate void HandleInputDelegate(float magnitude, Vector2 normal);
    public HandleInputDelegate handleInputCallback;

    public delegate void HandleInputUpDelegate(Vector2 deltaPosition);
    public HandleInputUpDelegate handleInputUpCallback;
}