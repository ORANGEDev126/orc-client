using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJoystickManager : MonoBehaviour
{
    public Joystick joystick;
    public Network network;

    public const double DRAW_THRESHOLD = 0.5;
    public const double SHOOT_THREADHOLD = 0.3;
    private int? angle;

    void Update()
    {
        var currVec = new Vector2(joystick.Horizontal, joystick.Vertical);
        if(Vector2.Distance(currVec, Vector2.zero) > DRAW_THRESHOLD)
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

        if(Vector2.Distance(currVec, Vector2.zero) < SHOOT_THREADHOLD &&
            angle.HasValue)
        {
            var message = new Orc.ShootProjectileReqMessage();
            message.Angle = angle.Value;
            network.SendProtoMessage(Orc.Protocol.ShootProjectileReq, message);
            angle = null;
        }
    }
}
