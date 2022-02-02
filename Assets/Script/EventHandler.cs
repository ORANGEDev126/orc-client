using UnityEngine;
using Google.Protobuf;

public class EventHandler
{
    public static void HandlePlayerEnterNoti(byte[] packet)
    {
        Orc.EnterPlayerNotiMessage message =
            Orc.EnterPlayerNotiMessage.Parser.ParseFrom(packet);

        PlayGround.globalPlayGround.EnterPlayer(message.Player, false);
    }

    public static void HandleProjectileEnterNoti(byte[] packet)
    {
        Orc.EnterProjectileNotiMessage message =
            Orc.EnterProjectileNotiMessage.Parser.ParseFrom(packet);

        PlayGround.globalPlayGround.EnterProjectile(message.Projectile);
    }

    public static void HandleLeaveNoti(byte[] packet)
    {
        Orc.LeavePlayerNotiMessage message =
            Orc.LeavePlayerNotiMessage.Parser.ParseFrom(packet);

        PlayGround.globalPlayGround.Leave(message.Id);
    }

    public static void HandleMoveNoti(byte[] packet)
    {
        Orc.MoveObjectNotiMessage message =
            Orc.MoveObjectNotiMessage.Parser.ParseFrom(packet);

        foreach(Orc.MoveObjectNotiMessage.Types.Object movedObject in message.Objects)
        {
            PlayGround.globalPlayGround.Move(movedObject.Id,
                new Vector3((float)movedObject.X, (float)movedObject.Y, 0),
                movedObject.Dir);
        }
    }

    public static void HandleWelcomeNoti(byte[] packet)
    {
        Orc.WelcomePlayerNotiMessage message =
            Orc.WelcomePlayerNotiMessage.Parser.ParseFrom(packet);

        foreach(Orc.PlayerMessage player in message.Players)
        {
            PlayGround.globalPlayGround.EnterPlayer(player, player.Id == message.MyId);
        }
    }

    public static void HandleProjectileAttackNoti(byte[] packet)
    {
        Orc.ProjectileAttackNotiMessage message =
            Orc.ProjectileAttackNotiMessage.Parser.ParseFrom(packet);
        PlayGround.globalPlayGround.ProjectileAttacked(message.PlayerId,
            message.ProjectileId);
    }
}
