using System;
// using System.Collections;
using System.Collections.Generic;
using ArduinoBluetoothAPI;
using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.Events;
// using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // public static GameManager instance;

    // Use this for initialization
    BluetoothHelper bluetoothHelper;
    string deviceName;
    string received_message;
    private string tmp;

    public bool isWet = false;

    // void Awake()
    // {
    //     if (instance)
    //     {
    //         Destroy(gameObject);
    //         return;
    //     }
    //     instance = this;
    //     DontDestroyOnLoad(gameObject);
    // }
    void Start()
    {

        // Debug.Log("BT_Debug :: void Start");

        deviceName = "AR"; //bluetooth should be turned ON;
        try
        {
            BluetoothHelper.BLE = false;
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnMessageReceived; //read the data
            bluetoothHelper.OnScanEnded += OnScanEnded;

            bluetoothHelper.setTerminatorBasedStream("\n");

            if (!bluetoothHelper.ScanNearbyDevices())
            {
                //scan didnt start (on windows desktop (not UWP))
                //try to connect
                bluetoothHelper.Connect();//this will work only for bluetooth classic.
                //scanning is mandatory before connecting for BLE.

            }

            // Debug.Log($"BT_Debug void start work");
        }
        catch (Exception ex)
        {
            // Debug.Log($"BT_Debug void start Not work {ex.Message}");
            write(ex.Message);
        }
    }

    //    void Update()
    //        {
    //            Debug.Log("WTF!!!!!!!");
    //        }


    private void write(string msg)
    {
        tmp += ">" + msg + "\n";
    }

    public void OnMessageReceived()
    {
        //   Debug.Log("It's received_message!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        received_message = bluetoothHelper.Read();
        // Debug.Log($"BT_Debug : {received_message}");
        //    ? write(received_message);

        if (received_message.Contains("ON"))
        {
            isWet = true;
            Debug.Log("true");
        }
        else if (received_message.Contains("OFF"))
        {
            isWet = false;
            Debug.Log("false");
        }
        else
        {
            Debug.Log("Error");

        }
        Debug.Log($"GameMGRIsWet? : {isWet}");

    }

    void OnConnected()
    {
        try
        {
            bluetoothHelper.StartListening();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            write(ex.Message);

        }
    }

    void OnScanEnded(LinkedList<BluetoothDevice> devices)
    {

        if (bluetoothHelper.isDevicePaired()) //we did found our device (with BLE) or we already paired the device (for Bluetooth Classic)
            bluetoothHelper.Connect();
        else
            bluetoothHelper.ScanNearbyDevices(); //we didn't
    }

    void OnConnectionFailed()
    {
        write("Connection Failed");
        // Debug.Log("Connection Failed");
    }

    /* 
        // Call this function to emulate message receiving from bluetooth while debugging on your PC.
        void OnGUI()
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 40;

            tmp = GUI.TextField(new Rect(Screen.width / 10, Screen.height /3 , Screen.width*0.8f , Screen.height / 3), tmp, myButtonStyle);

            if (bluetoothHelper != null)
                bluetoothHelper.DrawGUI();
            else
                return;

            if (!bluetoothHelper.isConnected())
                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Connect", myButtonStyle))
                {
                    if (bluetoothHelper.isDevicePaired())
                        bluetoothHelper.Connect(); // tries to connect
                    else
                        write("Cannot connect, device is not found, for bluetooth classic, pair the device\n\tFor BLE scan for nearby devices");
                }  

            if (bluetoothHelper.isConnected())
            {
                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 10, Screen.height - 2 * Screen.height / 10, Screen.width / 5, Screen.height / 10), "Disconnect", myButtonStyle))
                {
                    bluetoothHelper.Disconnect();
                    write("Disconnected");
                }

                if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 4, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Turn On", myButtonStyle))
                {
                    bluetoothHelper.SendData("1");
                    write("Sending 1");
                }

                if (GUI.Button(new Rect(Screen.width / 2 + Screen.width / 10, Screen.height / 10, Screen.width / 5, Screen.height / 10), "Turn Off" , myButtonStyle))
                {
                    bluetoothHelper.SendData("2");
                    write("Sending 2");
                }

            }

        }
     */
    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }
}