/* Copyright: Copyright 2014 Beijing Noitom Technology Ltd. All Rights reserved.
* Pending Patents: PCT/CN2014/085659 PCT/CN2014/071006
* 
* Licensed under the Neuron SDK License Beta Version (the “License");
* You may only use the Neuron SDK when in compliance with the License,
* which is provided at the time of installation or download, or which
* otherwise accompanies this software in the form of either an electronic or a hard copy.
* 
* Unless required by applicable law or agreed to in writing, the Neuron SDK
* distributed under the License is provided on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing conditions and
* limitations under the License.
*/

using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectAlice
{
    #region Enum types

    // AliceDataReader输出数据类型
    public enum AliceDataType
    {
        ADT_None = 0,
        ADT_Bvh,                // BVH data
        ADT_RigidBody,          // device data in model coordinate.
        ADT_DeviceData,         // send/recv device data, like button value, wii event and so on.
        ADT_DeviceCmd,          // send/recv device command, such as PC command, hi5 vibrate cmd and so on.
        ADT_Calc,               // calculation data of Hi5.
        ADT_Hi5,                // client to server, Hi5 glove BVH data.
        ADT_UserData,           // user data, forwarding by server.
        ADT_QR,                 // QR data.
        ADT_QRRigidBodyRaw,     // QR raw rigid-body data.
        ADT_QRRigidBody,        // QR rigid-body data.
        ADT_CAR,                // SDK to client, Bumper car cmd for start/stop.
        ADT_MotiveRawPack,      // Raw data received from motive of optitrack.
        ADT_JoyStick,           // Joystick 10 values: 2 for stick, 8 for buttons.
        ADT_TrackingStatus,     // Tracking status data, reference data type: TrackingStatus.
        ADT_RigidBody_Local,    // rigidbody data in local coordinate.
        ADT_RsExtDevice,        // SDK to client, RsExt-Fan control to agent.
        ADT_OrgOptiRigidbodies, // Original optical rigidbody data
        ADT_OrgOptiMarkers,     // Original optical markers data
        ADT_OrgIMUData,         // Original IMU data
        ADT_OPENVR,             // OPENVR
        ADT_DataHeartBeat,      // Alice Data 通道的心跳（用于表征server、agent的在线的心跳是通过p2p收发的，不可表征数据通道是否有效，新增数据通道可识别通道是否有效)
        ADT_TrackerData,        // Tracker data, send to sik.
        ADT_RigidBody_Base,     // PWR data in base coordinate
        ADT_AppData,            // App data include render rate and equip rate /app sdk->agent 
        ADT_AppCmd,             // Agent cmd to app /stop cmd
        ADT_Glove_Sensor,       // Glove sensor data
        ADT_Agent_Delays,       // Server to agent lantency
        ADT_AgentRecvFrameRate, // Agent Recv server frame rate
        ADT_CloudHeartBeat,     // Alice Cloud Gateway 心跳（用于表征server、gateway的通路是否有效，同时用于时延测试)
        ADT_SikBvhData,         // SIK avatar BVH data
        ADT_MAX,
    };


    // AliceDataReader输入数据类型
    public enum AliceInputType // ADT_DeviceCmd | ADT_DeviceData 
    {
        AIT_None = 0,       // Unknown type
        AIT_Wii,            // [DeviceCmdData] WII
        AIT_GameSir,        // [DeviceCmdData] GameSir
        AIT_Keyboard,       // [DeviceCmdData] Keyboard
        AIT_HI5Glove,       // [DeviceCmdData] Hi5 Glove
        AIT_BumperCar,      // [DeviceCmdData] Bumper Car
        AIT_JoyStick,       // [JoystickData] JoyStick
        AIT_RSExt,          // [DeviceCmdData] RSExt
        AIT_ContentStat,    // [DeviceCmdData] 内容状态
    };

    #endregion

    #region DataType
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TimeStamp
    {
        public double timeStamp;  // ms, from 1970, time stamp when data gathered
        public double latency;    // ms, calculation times
    }

    // Frame head
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct FrameHead
    {
        public int type_dev;   // ModuleType
        public int type_data;  // FrameDataType
        public int type_frame; // AliceDataType：DataType.h/AliceDataType
        public static int size = Marshal.SizeOf<FrameHead>();
    };

    // RealtimeDataHeader
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RTDHeader
    {
        public AliceDataType dataType;      // 'AliceDataType' data Type[RigidBody Data, BVH, DeviceCMD...]
        public uint frameIndex;             // frame index
        public double timecode;             // time code (second)
        public uint payloadLen;             // Length of payload
        public uint reserved1;              // reserved1
        public uint reserved2;              // reserved2
        public static int size = Marshal.SizeOf<RTDHeader>();
    };

    // Tracking data about position and rotation
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TrackingData
    {
        public int id;        // ID
        public int flag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public float[] data;         // pos(x,y,z), quat(x,y,z,w), scale(x,y,z)
        public static int size = Marshal.SizeOf<TrackingData>();
    };

    // SIK avatar data, BVH format
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SIKBVHData
    {
        public int avatarID;           // avatar ID
        public int frameIndex;         // frame index
        public double timeStamp;       // time stamp, unit: second
        public int bWithDisp;          // wether displacement in bvh
        public float scale;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public float[] rootData;       // root node rotation(x,y,z,w) and position(x,y,z)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 406)]   // 58 X 7
        public float[] data;           // rot(x,y,z,w), pos(x,y,z)
        public static int size = Marshal.SizeOf<SIKBVHData>();
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GloveSensorData
    {
        public int gloveID;
        public byte gloveType;    // 0 left, 1 right
        public byte flag;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qPalm;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qThumb;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qMiddle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qRing;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] qPinky;
    };

    #endregion

    #region Callbacks for data output
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ConfigDataCallback(IntPtr customObject, IntPtr sockRef, UIntPtr byteData, int byteLen);

    [UnmanagedFunctionPointer( CallingConvention.StdCall )]
	public delegate void RealtimeDataCallback( IntPtr customObject, IntPtr sockRef, IntPtr header, IntPtr data, int len);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void AppLicenseCallback(IntPtr customObject, IntPtr sockRef, IntPtr data, int size);
    #endregion //Callbacks for data output

    // API exportor
    public class AliceDataReader
    {
        #region Importor definition
#if UNITY_IPHONE && !UNITY_EDITOR
		private const string ReaderImportor = "__Internal";
#elif _WINDOWS
		private const string ReaderImportor = "AliceDataReader.dll";
#else
        private const string ReaderImportor = "AliceDataReader";
#endif
        #endregion //Importor definition

        #region Functions API
        // connect to server
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern int BRConnectRemote(string IP, int sendPort, int recvPort);

        // disconnect to server
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRDisconnect();

        // Callback for the config from server.
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRRegisterConfigDataCallback(IntPtr customedObj, ConfigDataCallback handle);

        // Register receiving and parsed frame bvh data callback
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRRegisterRealtimeDataCallback(IntPtr customedObj, RealtimeDataCallback handle);

        // Push data for device cmd.
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRSendDeviceCmdData(IntPtr data, int size);

        // Send user data to server.
        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRSendUserData(IntPtr ptr, int size);

        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRRegisterAppLicenseCallback(IntPtr customedObj, AppLicenseCallback handle);

        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRCheckAppLicense(IntPtr customedObj, int size);

        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BRSendAppData(IntPtr customedObj, int size);

        [DllImport(ReaderImportor, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void BREnableLog(bool enable);
        #endregion //Functions API

        #region
        public static void GetTrackingData<T>(IntPtr dataPtr, int len, out T[] data)
        {
            // 数据前面有12个子节的东西，要跳过去：签名+包大小+Tracker数量
            const int offset = 12;

            int size = Marshal.SizeOf<T>();
            int cnt = len / size;

            // 实例化对象
            data = new T[cnt];

            // 将数据拷贝到托管区
            byte[] buff = new byte[len];
            Marshal.Copy(dataPtr, buff, 0, len);

            // 分解数组
            IntPtr ptrObj = Marshal.AllocHGlobal(size);
            for (int i = 0; i < cnt; i++)
            {
                Marshal.Copy(buff, offset + i * size, ptrObj, size);
                data[i] = Marshal.PtrToStructure<T>(ptrObj);
            }

            // 释放内存
            Marshal.FreeHGlobal(ptrObj);
        }

        public static int ReadAvatarID(IntPtr dataPtr)
        {
            return Marshal.PtrToStructure<int>(dataPtr);
        }

        public static SIKBVHData ReadAvatarData(IntPtr dataPtr, int len)
        {
            return Marshal.PtrToStructure<SIKBVHData>(dataPtr);
        }

        public static Vector3 FlipLRHandedPosition(float x, float y, float z)
        {
            return new Vector3(-x, y, z);
        }
        public static Quaternion FlipLRHandedRotation(float x, float y, float z, float w)
        {
            return new Quaternion(-x, y, z, -w);
        }
        public static Vector3 FlipBVHHipPosition(float x, float y, float z)
        {
            return new Vector3(-x, -y, z) / 100f;
        }
        public static Quaternion FlipBVHHipRotation(float x, float y, float z, float w)
        {
            Quaternion q = new Quaternion(x, y, z, w);
            return new Quaternion(-q.x, q.z, q.y, q.w);
        }
        public static Vector3 FlipBVHPosition(float x, float y, float z)
        {
            return new Vector3(-x, -y, z) / 100f;
        }
        public static Quaternion FlipBVHRotation(float x, float y, float z, float w)
        {
            Quaternion q = new Quaternion(x, y, z, w);
            return new Quaternion(-q.x, -q.y, q.z, q.w);
        }

        #endregion
    }
}
