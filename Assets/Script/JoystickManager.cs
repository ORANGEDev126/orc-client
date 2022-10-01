using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour
{
    public Joystick joystick;
    public Network network;

    private Orc.Direction currDir = Orc.Direction.NoneDir;
    public const double THRESHOLD = 0.3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dir = Orc.Direction.NoneDir;

        if(joystick.Horizontal > THRESHOLD)
        {
            if(joystick.Vertical > THRESHOLD)
            {
                dir = Orc.Direction.NorthEast;
            }
            else if(joystick.Vertical < -THRESHOLD)
            {
                dir = Orc.Direction.EastSouth;
            }
            else
            {
                dir = Orc.Direction.East;
            }
        }
        else if(joystick.Horizontal < -THRESHOLD)
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

        if (dir != currDir)
        {
            currDir = dir;
            Orc.MoveJogReqMessage message = new Orc.MoveJogReqMessage();
            message.Dir = currDir;

            network.SendProtoMessage(Orc.Request.MoveJogReq, message);
        }
    }
}
