using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
using System;

public class UIControlScript : MonoBehaviour
{
    public Button BtnLeft;
    public Button BtnRight;
    public Button BtnStop;
    public Button BtnReset;

    private TrainScript tComponent;
    private Text tStatistic;

    // Start is called before the first frame update
    void Start()
    {
        BtnLeft.onClick.AddListener(mBtnClickLeft);
        BtnRight.onClick.AddListener(mBtnClickRight);
        BtnStop.onClick.AddListener(mBtnClickStop);
        BtnReset.onClick.AddListener(mBtnClickReset);

        tComponent = GameObject.Find("Train").GetComponent<TrainScript>();
        tStatistic = GameObject.Find("StatisticText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        string tDir = tComponent.Direction == TrainDirection.Left ? "Left" : "Right";
        StringBuilder tResult = new StringBuilder();
        tResult.Append($"Speed: <color=#FF976E>{Math.Round(tComponent.gCurrentSpeed, 2)}</color> \n");
        tResult.Append($"Time: <color=#74FF5E>{Math.Round(tComponent.gCurrentTime, 2)}</color> \n");
        tResult.Append($"Acceleration: <color=#5EC0FF>{Math.Round(tComponent.Acceleration, 2)}</color> \n");
        tResult.Append($"Direction: <color=#FFFD5E>{tDir}</color>");
        tStatistic.text = tResult.ToString();
    }

    public void mBtnClickLeft() {
        Debug.Log("Button Pressed: Left");
        //if (tComponent.Direction != TrainDirection.Left) tComponent.mResetParams();
        tComponent.Direction = TrainDirection.Left;
        tComponent.mStartMove();
    }
    public void mBtnClickRight() {
        Debug.Log("Button Pressed: Right");
        //if (tComponent.Direction != TrainDirection.Right) tComponent.mResetParams();
        tComponent.Direction = TrainDirection.Right;
        tComponent.mStartMove();
    }
    public void mBtnClickStop() {
        Debug.Log("Button Pressed: Stop");
        tComponent.mStopMove();
    }
    public void mBtnClickReset() {
        Debug.Log("Button Pressed: Reset");
        tComponent.mResetParams();
    }
}
