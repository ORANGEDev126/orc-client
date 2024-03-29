using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJoystickManager : MonoBehaviour
{
    public FixedJoystick joystick;

    public const double DRAW_THRESHOLD = 0.5;
    public const double SHOOT_THREADHOLD = 0.3;
    private int? angle;

    private void Start()
    {
        joystick.handleInputCallback = HandleInput;
        joystick.handleInputUpCallback = HandleUp;
    }
    void Update()
    {
    }
    private void HandleInput(float magnitude, Vector2 normal)
    {
        var currVec = normal;
        if (magnitude > DRAW_THRESHOLD)
        {
            var signedAngle = Vector2.SignedAngle(currVec, Vector2.right);
            if (signedAngle < 0)
            {
                angle = (int)-signedAngle;
            }
            else
            {
                angle = (int)(360 - signedAngle);
            }

            return;
        }
    }

    private void HandleUp(Vector2 deltaPosition)
    {
        if (angle.HasValue)
        {
            if (deltaPosition.magnitude > SHOOT_THREADHOLD)
            {
                var message = new Orc.ShootProjectileReqMessage();
                message.Angle = angle.Value;
                Network.Get().SendProtoMessage(Orc.Request.ShootProjectileReq, message);
            }
        }
        else
        {
            var message = new Orc.AttackReqMessage();
            Network.Get().SendProtoMessage(Orc.Request.AttackReq, message);
        }
        angle = null;
    }
}
