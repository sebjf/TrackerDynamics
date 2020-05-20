using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // By convention, all time values of type long are in microseconds from the Windows Epoch.

    public long gameTime;
    public long physicsTime;

    private long processTime;
    private long realStartTime;

    public bool controlPhysics = false;

    public enum TimeMode
    {
        LocalTime,
        SystemTime,
        RelativeSystemTime
    }

    public TimeMode mode;

    private void Awake()
    {
        if (controlPhysics)
        {
            Physics.autoSimulation = false;
            Physics.autoSyncTransforms = false;
        }
    }

    void Start()
    {
        processTime = 0;
        realStartTime = RealTime;

        UpdateGameTime();

        if (controlPhysics)
        {
            physicsTime = gameTime;
        }
    }

    private void FixedUpdate()
    {
        processTime += fixedDeltaTime;

        if(!controlPhysics)
        {
            physicsTime += fixedDeltaTime;
        }

        UpdateGameTime();

        if (controlPhysics)
        {
            Physics.SyncTransforms();

            // For stability reasons, PhysX cannot update over a period longer than TIme.maximumDeltaTime, but also not smaller than 8 ms (empirical figure).
            // We therefore keep a seperate 'physics clock' in sync with our game time to control the updates.
            while ((physicsTime - gameTime) < fixedDeltaTime)
            {
                var timestep = ToSeconds(gameTime - physicsTime);
                timestep = Mathf.Clamp(timestep, Time.fixedDeltaTime, Time.maximumDeltaTime);
                Physics.Simulate(timestep);
                physicsTime += FromSeconds(timestep);
            }
        }
    }

    private void Update()
    {
        UpdateGameTime();
    }

    private void UpdateGameTime()
    {
        switch (mode)
        {
            case TimeMode.LocalTime:
                gameTime = processTime;
                break;
            case TimeMode.SystemTime:
                gameTime = RealTime;
                break;
            case TimeMode.RelativeSystemTime:
                gameTime = RealTime - realStartTime;
                break;
            default:
                break;
        }
    }

    public long fixedDeltaTime
    {
        get
        {
            return (long)(Time.fixedDeltaTime * 1e6f);
        }
    }

    public static long FromSeconds(float seconds)
    {
        return (long)(seconds * 1e6f);
    }

    public static float ToSeconds(long microseconds)
    {
        return (float)microseconds / 1e6f;
    }

    public static long RealTime
    {
        get
        {
            return HighResolutionDateTime.UtcNow.ToMicroseconds();
        }
    }

}

//https://manski.net/2014/07/high-resolution-clock-in-csharp/

public static class HighResolutionDateTime
{
    public static bool IsAvailable { get; private set; }

    [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
    private static extern void GetSystemTimePreciseAsFileTime(out long filetime);

    public static DateTime UtcNow
    {
        get
        {
            if (!IsAvailable)
            {
                throw new InvalidOperationException("High resolution clock isn't available.");
            }
            long filetime;
            GetSystemTimePreciseAsFileTime(out filetime);
            return DateTime.FromFileTimeUtc(filetime);
        }
    }

    static HighResolutionDateTime()
    {
        try
        {
            long filetime; GetSystemTimePreciseAsFileTime(out filetime);
            IsAvailable = true;
        }
        catch (EntryPointNotFoundException) // Not running Windows 8 or higher. 
        {             
            IsAvailable = false;
        }
    }

    public static double ToSeconds(this DateTime time)
    {
        return time.Ticks / 10e6; //https://docs.microsoft.com/en-us/dotnet/api/system.datetime.ticks?view=netframework-4.8
    }

    public static long ToMicroseconds(this DateTime time)
    {
        return time.Ticks / 10;
    }
}