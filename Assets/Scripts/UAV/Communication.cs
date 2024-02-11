using System;
using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class Communication : UnitySubscriber<MessageTypes.Std.Int32MultiArray>
    {
        private int[] intArray;
        private bool isMessageReceived = false;
        public DronMovement pose;

        protected override void Start()
        {
            base.Start();
        }

        public void Update()
        {
            if (isMessageReceived)
            {
                PrintIntArray();
                isMessageReceived = false;
            }
        }

        protected override void ReceiveMessage(Int32MultiArray message)
        {
            intArray = message.data;
            isMessageReceived = true;
        }

        void PrintIntArray()
        {
            if (intArray != null)
            {
                //Debug.Log("Received Int32MultiArray with " + intArray.Length + " elements:"+intArray[0]+" "+intArray[1]+" " + intArray[2]+" " + intArray[3]);
                pose.PosGetX(intArray[0]);
                pose.PosGetY(intArray[1]);
                pose.PosGetZ(intArray[2]);
                pose.PosGetRot(intArray[3]);
                //print("rotation = "+ intArray[3]);
                
                /* siu
                for (int i = 0; i < intArray.Length; i++)
                {
                    Debug.Log("Element " + i + ": " + intArray[i]);
                }*/
                
            }
            else
            {
                Debug.LogWarning("Int32MultiArray is null or empty.");
            }
        }
    }
}
