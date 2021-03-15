using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Trial : MonoBehaviour
{
    private Touch touch;
    private float speedModifier;
    private static int localPort;
    private string IP;
    public int port;
    IPEndPoint remoteEndPoint;
    UdpClient client;
    private Vector3 lastposition;
    private Vector3 newposition;
    public GameObject Cube;


    void Start()
    {
        speedModifier = 0.001f;
        lastposition = transform.position = new Vector3(0, 0, 4.15f);
        init();

    }

    private static void Main()
    {
        Trial sendObj = new Trial();
        sendObj.init();
        sendObj.Update();

        //sendObj.sendEndless(" endless infos \n");

    }

    public void init()
    {
        IP = "127.0.0.1";
        port = 8051;

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                
                newposition = transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speedModifier, transform.position.y + touch.deltaPosition.y * speedModifier, transform.position.z + touch.deltaPosition.y * speedModifier);
               
                if (lastposition != newposition)
                {
                    string pos = newposition.ToString();
                    string name = "m";
                    pos = pos + name;
                    byte[] data = Encoding.UTF8.GetBytes(pos);
                    client.Send(data, data.Length, remoteEndPoint);
                     
                }
            }
        }
    }
}
