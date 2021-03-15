using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class TrialZoom : MonoBehaviour
{
    float initialFingerDistance;
    Vector3 initialScale;

    private static int localPort;
    private string IP;
    public int port;
    IPEndPoint remoteEndPoint;
    UdpClient client;
    public GameObject Cube;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        init();

    }

    private static void Main()
    {
        TrialZoom sendObj = new TrialZoom();
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
        if (Input.touches.Length == 2)
            {
                Touch t1 = Input.touches[0];
                Touch t2 = Input.touches[1];

                if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
                {
                    initialFingerDistance = Vector2.Distance(t1.position, t2.position);
                    initialScale = Cube.transform.localScale;
                }
                else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
                {
                    var currentFingerDistance = Vector2.Distance(t1.position, t2.position);
                    var scaleFactor = currentFingerDistance / initialFingerDistance;
                    Cube.transform.localScale = initialScale * scaleFactor;
                }
                    string pos = Cube.transform.localScale.ToString();
                    string name = "z";
                    pos = pos + name;
                    byte[] data = Encoding.UTF8.GetBytes(pos);
                    client.Send(data, data.Length, remoteEndPoint);
                    
        }
    }
}
