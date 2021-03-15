using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Trial : MonoBehaviour
{
    private Touch touch;
    private float Rotatespeed = 0.25f;
    private Quaternion rotationX;
    private Quaternion rotationY;
    private Quaternion newrotationX;
    private Quaternion newrotationY;
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
                rotationX = Quaternion.Euler(-touch.deltaPosition.y * Rotatespeed, 0f, 0f);
                newrotationX = transform.rotation = rotationX * transform.rotation;

                rotationY = Quaternion.Euler(0f, -touch.deltaPosition.x * Rotatespeed, 0f);
                newrotationY = transform.rotation = rotationY * transform.rotation;

                if (newrotationY != rotationY || newrotationX != rotationX)
                {
                    string pos = Cube.transform.rotation.ToString();
                    string name = "r";
                    pos = pos + name;
                    byte[] data = Encoding.UTF8.GetBytes(pos);
                    client.Send(data, data.Length, remoteEndPoint);
                }
            }
        }
    }
}
   