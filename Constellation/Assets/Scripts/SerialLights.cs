using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialLights : MonoBehaviour
{

    public SerialHandler serialHandler;
    public Text text;

    // Use this for initialization
    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;
        //serialHandler = GetComponent<SerialHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            serialHandler.Write("0");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            serialHandler.Write("1");

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            serialHandler.Write("2");

        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            serialHandler.Write("3");

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            serialHandler.Write("4");

        }

    }

    /*
	 * シリアルを受け取った時の処理
	 */
    void OnDataReceived(string message)
    {
        try
        {
            text.text = message; // シリアルの値をテキストに表示
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}