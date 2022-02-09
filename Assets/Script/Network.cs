using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using Google.Protobuf;

public class Network : MonoBehaviour
{
    public const int MAX_READ_BUFFER_SIZE = 65536;
    public const int PACKET_LENGTH_FIELD_SIZE = 4;
    public const int PACKET_PROTOCOL_SIZE = 4;
    public const int HEADER_LENGTH = PACKET_LENGTH_FIELD_SIZE + PACKET_PROTOCOL_SIZE;

    public delegate void PacketHandler(byte[] message);

    private TcpClient tcpClient;
    private NetworkStream stream;
    private int readSize = 0;
    private byte[] readBuf = new byte[MAX_READ_BUFFER_SIZE];

    public string ServerIP = "127.0.0.1";
    public int ServerPort = 1004;
    public Dictionary<Orc.Protocol, PacketHandler> Handler = new Dictionary<Orc.Protocol, PacketHandler>();

    // Start is called before the first frame update
    void Start()
    {
        tcpClient = new TcpClient(ServerIP, ServerPort);
        stream = tcpClient.GetStream();
        Debug.Log("[Network] connect to server");

        Handler[Orc.Protocol.EnterPlayerNoti] = EventHandler.HandlePlayerEnterNoti;
        Handler[Orc.Protocol.MoveObjectNoti] = EventHandler.HandleMoveNoti;
        Handler[Orc.Protocol.WelcomePlayerNoti] = EventHandler.HandleWelcomeNoti;
        Handler[Orc.Protocol.LeaveObjecNoti] = EventHandler.HandleLeaveNoti;
        Handler[Orc.Protocol.EnterProjectileNoti] = EventHandler.HandleProjectileEnterNoti;
        PacketHandler handleProjectileAttackNoti = EventHandler.HandleProjectileAttackNoti;
        Handler[Orc.Protocol.ProjectileAttackNoti] = EventHandler.HandleProjectileAttackNoti;
    }

    // Update is called once per frame
    void Update()
    { 
        while(stream != null && stream.DataAvailable)
        {
            readSize += stream.Read(readBuf, readSize, MAX_READ_BUFFER_SIZE - readSize);

            while (readSize > HEADER_LENGTH)
            {
                int packetLength = BitConverter.ToInt32(readBuf, 0);
                Orc.Protocol protocolID = (Orc.Protocol)BitConverter.ToInt32(readBuf, PACKET_LENGTH_FIELD_SIZE);
                
                if (readSize < packetLength)
                {
                    break;
                }

                byte[] message = new byte[packetLength - HEADER_LENGTH];
                Array.Copy(readBuf, HEADER_LENGTH,
                 message, 0, packetLength - HEADER_LENGTH);

                Array.Copy(readBuf, packetLength, readBuf, 0, readSize - packetLength);
                readSize -= packetLength;

                if (!Handler.ContainsKey(protocolID))
                {
                    Debug.Log("[Network] cannot find protocol id in handler id : " + protocolID.ToString());
                    continue;
                }

                Handler[protocolID](message);
            }
        }
    }

    public void SendProtoMessage(Orc.Protocol id, Google.Protobuf.IMessage message)
    {
        if(stream == null)
        {
            Debug.Log("[Network] stream is null when send message");
            return;
        }

        var packetLength = message.CalculateSize() + HEADER_LENGTH;
        var writeBuf = new byte[packetLength];
        writeBuf[0] = (byte)packetLength;
        writeBuf[1] = (byte)(packetLength >> 8);
        writeBuf[2] = (byte)(packetLength >> 16);
        writeBuf[3] = (byte)(packetLength >> 24);
        writeBuf[4] = (byte)id;
        writeBuf[5] = (byte)((int)id >> 8);
        writeBuf[6] = (byte)((int)id >> 16);
        writeBuf[7] = (byte)((int)id >> 24);

        var serializedMessage = message.ToByteArray();

        Array.Copy(serializedMessage, 0, writeBuf, HEADER_LENGTH, serializedMessage.Length);

        stream.Write(writeBuf, 0, writeBuf.Length);
    }
}
