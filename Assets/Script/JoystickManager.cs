using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public Joystick joystick;
    public Network network;

    private Orc.Direction currDir = Orc.Direction.NoneDir;
    public const double GUIAD_VALUE = 0.3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Orc.Direction.NoneDir;

        if(joystick.Horizontal > GUIAD_VALUE)
        {
            if(joystick.Vertical > GUIAD_VALUE)
            {
                dir = Orc.Direction.NorthEast;
            }
            else if(joystick.Vertical < -GUIAD_VALUE)
            {
                dir = Orc.Direction.EastSouth;
            }
            else
            {
                dir = Orc.Direction.East;
            }
        }
        else if(joystick.Horizontal < -GUIAD_VALUE)
        {
            if (joystick.Vertical > GUIAD_VALUE)
            {
                dir = Orc.Direction.WestNorth;
            }
            else if (joystick.Vertical < -GUIAD_VALUE)
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
            if (joystick.Vertical > GUIAD_VALUE)
            {
                dir = Orc.Direction.North;
            }
            else if (joystick.Vertical < -GUIAD_VALUE)
            {
                dir = Orc.Direction.South;
            }
            else
            {
                dir = Orc.Direction.NoneDir;
            }
        }

        if (dir != currDir)
        {
            currDir = dir;
            Orc.MoveJogReqMessage message = new Orc.MoveJogReqMessage();
            message.Dir = currDir;

            network.SendProtoMessage(Orc.Protocol.MoveJogReq, message);
        }
    }
}
