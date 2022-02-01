using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine.UI;

public class python : MonoBehaviour
{
    public GameObject mcamera;
    public SerialHandler serialHandler;
    public Text text2;
    static UdpClient udp;
    IPEndPoint remoteEP = null;
    int i = 0;
    int kai = 0;
    float sg = 0;
    float mg = 0;
    // Start is called before the first frame update
    void Start()
    {
        int LOCA_LPORT = 50007;
        //serialHandler.OnDataReceived += OnDataReceived;
        udp = new UdpClient(LOCA_LPORT);
        udp.Client.ReceiveTimeout = 2000;

    }

    // Update is called once per frame
    void Update()
    {
        ////IPEndPoint remoteEP = null;
        byte[] data = udp.Receive(ref remoteEP);
        string text = Encoding.UTF8.GetString(data);
        text2.text = text;
        //Debug.Log(text);
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            serialHandler.Write("0");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            serialHandler.Write("1");

        }
        */

        if (text == "right")
        {
            serialHandler.Write("1");
            kai = 1;
        }
        if (text == "left")
        {
            serialHandler.Write("2");
            kai = 2;
        }
        if (text == "up")
        {
            serialHandler.Write("4");
            kai = 4;

        }
        if (text == "down")
        {
            serialHandler.Write("3");
            kai = 3;
        }
        if (text == "stop")
        {
            serialHandler.Write("5");
            kai = 5;
        }

        if (kai==4 &&  sg <= 20.0)//up
        {
            ///Debug.Log(mcamera.transform.rotation.x);
            mcamera.transform.Rotate((float)-9.375 * Time.deltaTime, 0, 0);
            sg += (float)9.375 * Time.deltaTime;
        }
        if (kai== 3 && sg >= -20.0)//down
        {
            mcamera.transform.Rotate((float)9.375 * Time.deltaTime, 0, 0);
            sg -= (float)9.375 * Time.deltaTime;
        }
        if (kai==1 && mg <= 80.0)//right
        {
            mcamera.transform.Rotate(0, (float)9.375 * Time.deltaTime, 0, Space.World);
            mg += (float)9.375 * Time.deltaTime;
        }
        if (kai== 2 && mg >= -80.0)//left
        {
        
            mcamera.transform.Rotate(0, (float)-9.375 * Time.deltaTime, 0, Space.World);
            mg -= (float)9.375 * Time.deltaTime;
        }

    }
}
