using System.Collections;
using System.Collections.Generic;
using ExactFramework.Component.Examples;
using UnityEngine;

[RequireComponent(typeof(MyRFIDReader))]
public class RFIDReaderBehaviour : MonoBehaviour
{
    MyRFIDReader myTwin;
    RFID rfidReader;

    // Start is called before the first frame update
    void Start()
    {
        myTwin = GetComponent<MyRFIDReader>();
        rfidReader = myTwin.GetDeviceComponent<RFID>();

        myTwin.AddEventListener("rfid.read", OnRFIDChipRead);
    }

    void OnRFIDChipRead(){
        string text = System.Text.Encoding.Default.GetString(rfidReader.GetLastReadID());
        Debug.Log(text);
    }
}
