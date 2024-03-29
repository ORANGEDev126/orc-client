using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public FixedJoystick joystick;

    private Orc.Direction currDir = Orc.Direction.NoneDir;
    public const double THRESHOLD = 0.3;

    // Start is called before the first frame update
    void Start()
    {
        joystick.handleInputCallback = HandleInput;
        joystick.handleInputUpCallback = HandleUp;
    }

    Orc.Direction GetDirection()
    {
        var dir = Orc.Direction.NoneDir;

        if (joystick.Horizontal > THRESHOLD)
        {
            if (joystick.Vertical > THRESHOLD)
            {
                dir = Orc.Direction.NorthEast;
            }
            else if (joystick.Vertical < -THRESHOLD)
            {
                dir = Orc.Direction.EastSouth;
            }
            else
            {
                dir = Orc.Direction.East;
            }
        }
        else if (joystick.Horizontal < -THRESHOLD)
        {
            if (joystick.Vertical > THRESHOLD)
            {
                dir = Orc.Direction.WestNorth;
            }
            else if (joystick.Vertical < -THRESHOLD)
            {
                dir = Orc.Direction.SouthWest;
            }
            else
            {
                dir = Orc.Direction.West;
            }
        }
        else
        {
            if (joystick.Vertical > THRESHOLD)
            {
                dir = Orc.Direction.North;
            }
            else if (joystick.Vertical < -THRESHOLD)
            {
                dir = Orc.Direction.South;
            }
        }
        return dir;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void HandleInput(float magnitude, Vector2 normal)
    {
        if(PlayGround.globalPlayGround.CanMyCharacterMove() == false)
        {
            return;
        }
        var dir = GetDirection();
        if (dir != currDir)
        {
            currDir = dir;
            Orc.MoveJogReqMessage message = new Orc.MoveJogReqMessage();
            message.Dir = currDir;

            Network.Get().SendProtoMessage(Orc.Request.MoveJogReq, message);
        }
    }

    private void HandleUp(Vector2 deltaPosition)
    {
        Orc.MoveJogReqMessage message = new Orc.MoveJogReqMessage();
        message.Dir = Orc.Direction.NoneDir;
        Network.Get().SendProtoMessage(Orc.Request.MoveJogReq, message);
    }
}
