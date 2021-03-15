using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Trial : MonoBehaviour
{
    Thread receiveThread;
    UdpClient client;
    public int port;
    public GameObject Player, Zoom, Rotate, Move;
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = "";
    private float[] transformPosition = new float[3];
    private Vector3 zoomlocation;
    private Vector3 movelocation;
    private Quaternion rotatelocation;
    public char task;

    private static void Main()
    {
        Trial receiveObj = new Trial();
        receiveObj.init();
    }

    public void Start()
    {
        init();
        Zoom.SetActive(false);
        Rotate.SetActive(false);
        Move.SetActive(false);

    }

    private void init()
    {
        port = 8051;
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void OnApplicationQuit()
    {
        client.Close();
        receiveThread.Abort();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {


            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = client.Receive(ref anyIP);
            string result = Encoding.UTF8.GetString(data);

            if (result.EndsWith("z"))
            {
                
                result = result.Substring(0, result.Length - 1);

                if (result.StartsWith("(") && result.EndsWith(")"))
                {
                    result = result.Substring(1, result.Length - 2);
                }

                string[] pos = result.Split(',');
                zoomlocation = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                lastReceivedUDPPacket = result;
                allReceivedUDPPackets = allReceivedUDPPackets + result;
                task = 'z';

            }

            else if (result.EndsWith("r"))
            {
                
                result = result.Substring(0, result.Length - 1);

                if (result.StartsWith("(") && result.EndsWith(")"))
                {
                    result = result.Substring(1, result.Length - 2);
                }

                string[] pos = result.Split(',');
                rotatelocation = new Quaternion(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[2]));
                lastReceivedUDPPacket = result;
                allReceivedUDPPackets = allReceivedUDPPackets + result;
                task = 'r';


            }

            else if (result.EndsWith("m"))
            {
               
                result = result.Substring(0, result.Length - 1);

                if (result.StartsWith("(") && result.EndsWith(")"))
                {
                    result = result.Substring(1, result.Length - 2);
                }
                string[] pos = result.Split(',');
                movelocation = new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
                lastReceivedUDPPacket = result;
                allReceivedUDPPackets = allReceivedUDPPackets + result;
                task = 'm';


            }
        }
    }

    private void Update()
    {

        if (task == 'z')
        {
            Zoom.SetActive(true);
            Rotate.SetActive(false);
            Move.SetActive(false);
            Player.transform.localScale = zoomlocation;
        }
        else if (task == 'r')
        {
            Rotate.SetActive(true);
            Zoom.SetActive(false);
            Move.SetActive(false);
            Player.transform.rotation = rotatelocation;
        }
        else if (task == 'm')
        {
            Move.SetActive(true);
            Rotate.SetActive(false);
            Zoom.SetActive(false);
            Player.transform.position = movelocation;
        }
    }

}