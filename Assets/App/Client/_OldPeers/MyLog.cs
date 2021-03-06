﻿/**
 * Author:    Vinh Vu Thanh
 * This class is a part of Unity IoC project that can be downloaded free at 
 * https://github.com/game-libgdx-unity/UnityEngine.IoC or http://u3d.as/1rQJ
 * (c) Copyright by MrThanhVinh168@gmail.com
 **/
using UnityEngine;
using System.Collections;

public class MyLog : SingletonBehaviour<MyLog>
{
    string myLog;
    Queue myLogQueue = new Queue();

    [SerializeField] public int maxLength = 3000;
    [SerializeField] public bool logSystem = true;
    [SerializeField] public LogType logType = LogType.Error;

    void OnEnable()
    {
        if (logSystem) Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        if (logSystem) Application.logMessageReceived -= HandleLog;
    }

    public void HandleLog(string logString, string stackTrace = null, LogType type = LogType.Log)
    {
        //filter logs
        if (type != LogType.Exception && type > logType)
        {
            return;
        }

        //print logs to screen
        myLog = logString;
        string newString = "\n [" + type + "] : " + myLog;
        myLogQueue.Enqueue(newString);
        if (type == LogType.Exception)
        {
            newString = "\n" + stackTrace;
            myLogQueue.Enqueue(newString);
        }

        myLog = string.Empty;
        foreach (string mylog in myLogQueue)
        {
            myLog += mylog;
            if (myLog.Length > maxLength)
            {
                Clear();
            }
        }
    }

    void OnGUI()
    {
        if (GUILayout.Button("Clear", GUILayout.Width(300)))
        {
            Clear();
            
#if UNITY_EDITOR
            Debug.ClearDeveloperConsole();
#endif
            
        }

        GUILayout.Label(myLog);
    }

    private void Clear()
    {
        myLog = "";
        myLogQueue.Clear();
    }
}