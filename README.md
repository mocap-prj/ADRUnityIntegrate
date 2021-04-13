# Integrate AliceDataReader to Unity

AliceDataReader is a SDK used to receive tracking data of Noitom AliceTrackingSystem.

This repo try to integrate the AliceDataReader SDK to Unity.

The demo can run on Android and Windows OS.

# ThirdPary
zmq, czmq NDK version, download from github of zmq.

This package is a jar file for java development on android, but you can unzip it and only use the *so files for Unity.

```
czmq-android-armeabi-v7a-4.2.1.jar
```

It included:

|Files           | Version   |
|----------------|-----------|
|libzmq.so       | 4.3.4.0   |
|libczmq.so      | 4.2.1     |
|libsodium.so    | 1.0.18.0  |
|libc++_shared.so| unknown   |

## Will be used adb commands

* Install app

```
adb install ADRUnityIntegrate.apk
```

* Uninstall app

```
adb uninstall com.Noitom.AliceDataReader
```

* Force to stop an app

```
adb shell am force-stop com.Noitom.AliceDataReader
```

* Check log

```
adb logcat
```