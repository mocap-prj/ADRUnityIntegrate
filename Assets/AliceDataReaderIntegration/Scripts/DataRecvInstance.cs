using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using ProjectAlice;

public class DataRecvInstance : MonoBehaviour
{
    private string remoteIP = "127.0.0.1";
    private int sendPort = 3011;
    private int recvPort = 3014;

    private GUIStyle guiStyle = null;
    private ProjectAlice.RealtimeDataCallback fn;

    public GameObject trackerPrefab;
    public GameObject avatarPrefab;

    public Dictionary<int, Tracker> trackDic = new Dictionary<int, Tracker>();
    public Dictionary<int, Avatar> avatarDic = new Dictionary<int, Avatar>();

    private TrackingData[] deviceData = null;
    private TrackingData[] pwrData = null;
    private Dictionary<int, SIKBVHData> sikDataDic = new Dictionary<int, SIKBVHData>();

    // Start is called before the first frame update
    void Start()
    {
        remoteIP = ConfigInI.GetValue("remoteIP");
        Int32.TryParse(ConfigInI.GetValue("sendPort"), out sendPort);
        Int32.TryParse(ConfigInI.GetValue("recvPort"), out recvPort);
        Debug.Log("Read config.ini params: remoteIP=" + remoteIP + ", sendPort=" + sendPort + ", recvPort=" + recvPort);

        Debug.Log("<color=green>Info: </color> Connect to remote: " + remoteIP + ", send port:" + sendPort + ", recv port:" + recvPort);
        AliceDataReader.BRConnectRemote(remoteIP, sendPort, recvPort);

        Debug.Log("<color=green>Info: </color> Register callback handler");
        fn = new RealtimeDataCallback(FrameDataReceivedHandle);
        AliceDataReader.BRRegisterRealtimeDataCallback(IntPtr.Zero, fn);

        //Debug.Log("<color=red>Error: </color>Initialized");
        Debug.Log("<color=green>Info: </color>Initialized");

    }

    // Update is called once per frame
    void Update()
    {
        if (deviceData != null)
        {
            foreach (var item in deviceData)
            {
                Tracker tracker = null;
                if (!trackDic.TryGetValue(item.id, out tracker))
                {
                    var ob = GameObject.Instantiate(this.trackerPrefab);
                    ob.name = item.id.ToString();

                    tracker = ob.GetComponent<Tracker>();
                    tracker.ID = item.id;
                    trackDic.Add(item.id, tracker);
                }

                tracker.trackPos = AliceDataReader.FlipLRHandedPosition(item.data[0], item.data[1], item.data[2]);
                tracker.trackRot = AliceDataReader.FlipLRHandedRotation(item.data[3], item.data[4], item.data[5], item.data[6]);
            }
        }

        if (pwrData != null)
        {
            foreach (var item in pwrData)
            {
                Tracker tracker = null;
                if (!trackDic.TryGetValue(item.id, out tracker))
                {
                    var ob = GameObject.Instantiate(this.trackerPrefab);
                    ob.name = item.id.ToString();

                    tracker = ob.GetComponent<Tracker>();
                    tracker.ID = item.id;
                    trackDic.Add(item.id, tracker);
                }

                tracker.trackPos = AliceDataReader.FlipLRHandedPosition(item.data[0], item.data[1], item.data[2]);
                tracker.trackRot = AliceDataReader.FlipLRHandedRotation(item.data[3], item.data[4], item.data[5], item.data[6]);
            }
        }

        foreach (var item in sikDataDic)
        {
            Avatar avatar = null;
            if (!avatarDic.TryGetValue(item.Key, out avatar))
            {
                var ob = GameObject.Instantiate(this.avatarPrefab);
                ob.name = "Avatar" + item.Key.ToString();

                avatar = ob.GetComponent<Avatar>();
                avatar.ID = item.Key;
                avatarDic.Add(item.Key, avatar);
            }

            avatar.sikBVHData = item.Value;
        }
    }

    private void OnGUI()
    {
        if (guiStyle == null)
        {
            guiStyle = new GUIStyle();
            guiStyle.normal.background = null;
            guiStyle.normal.textColor = new Color(1, 0.5f, 0.5f);
        }
    }

    private void FrameDataReceivedHandle(IntPtr customObject, IntPtr sockRef, IntPtr header, IntPtr data, int len)
    {
        // parsering header
        RTDHeader ver = Marshal.PtrToStructure<RTDHeader>(header);
        //Debug.Log("Received data," + " frameIndex=" + ver.frameIndex + " dataType=" + ver.dataType + " timecode=" + ver.timecode + " payloadLen=" + ver.payloadLen);

        switch (ver.dataType)
        {
            case AliceDataType.ADT_TrackerData:
                break;
            case AliceDataType.ADT_Bvh:
                break;
            case AliceDataType.ADT_RigidBody_Base:  // PWR data in base coordinate
                AliceDataReader.GetTrackingData<TrackingData>(data, len, out pwrData);
                break;
            case AliceDataType.ADT_Glove_Sensor:    // Glove sensor data
                //AliceDataReader.GetTrackingData<GloveSensorData>(data, len, out gloveData);
                break;
            case AliceDataType.ADT_RigidBody:       // device data in model coordinate.
                AliceDataReader.GetTrackingData<TrackingData>(data, len, out deviceData);
                break;
            case AliceDataType.ADT_DeviceData:
                break;
            case AliceDataType.ADT_DeviceCmd:
                break;
            case AliceDataType.ADT_Agent_Delays:
                break;
            case AliceDataType.ADT_AgentRecvFrameRate:
                break;
            case AliceDataType.ADT_CloudHeartBeat:
            case AliceDataType.ADT_DataHeartBeat:
                break;
            case AliceDataType.ADT_SikBvhData:
                {
                    int id = AliceDataReader.ReadAvatarID(data);
                    sikDataDic[id] = AliceDataReader.ReadAvatarData(data, len);
                }
                break;
            default:
                break;
        }

        //Debug.Log("<color=green>Info: </color>Received data, type:" + dtype.ToString());
    }
}
