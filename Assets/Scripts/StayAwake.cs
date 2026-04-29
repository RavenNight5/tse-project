using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayAwake : MonoBehaviour
{
        void Start()
        {
            // Prevent screen from sleeping
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        void OnApplicationQuit()
        {
            // Restore the default timeout when the application quits
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
 
}
