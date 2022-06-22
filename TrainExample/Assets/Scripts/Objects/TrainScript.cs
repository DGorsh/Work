using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public enum TrainDirection
{
    Left = 0,
    Right = 1
}

public enum TrainState
{
    Stand = 0,
    Move = 1
}
public class TrainScript : MonoBehaviour
{
    #region Private Variables
    private float _CurrentSpeed;
    private float _CurrentTime;
    private TrainState _CurrentTrainState;
    #endregion

    #region Public Variables
    public float BaseSpeed;
    public float MaxSpeed;
    public float Acceleration;
    public TrainDirection Direction;
    #endregion

    #region Getters/Setters
    public float gCurrentSpeed => _CurrentSpeed;
    public float gCurrentTime => _CurrentTime;
    #endregion

    #region Constructors
    public TrainScript()
    {
        _CurrentSpeed = 0.0f;
        _CurrentTime = 0.0f;
        _CurrentTrainState = TrainState.Stand;

        BaseSpeed = 0.0f;
        MaxSpeed = 80.0f;
        Acceleration = 1.0f;
        Direction = TrainDirection.Right;
    }
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        mStartMove();
    }

    // Update is called once per frame
    void Update()
    {
        if (_CurrentTrainState == TrainState.Move)
        {
            mSetTrainPosition(mOffsetX());
            _CurrentTime += 0.01f;
            Thread.Sleep(10);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        mStopMove();
        //Debug.Log($"Collided!");
    }

    // ”становка координат позиции (центра т€жести) поезда относительно текущих координат (по ’)
    private void mSetTrainPosition(float ppX)
    {
        Vector3 tCurrentPos = new Vector3(0.0f, 0.0f, 0.0f);
        tCurrentPos.x += (Direction == TrainDirection.Right) ? ppX : -ppX;
        transform.position += tCurrentPos;
    }
    #endregion

    #region Base Methods
    public void mStartMove() {
        _CurrentTrainState = TrainState.Move;
    }
    public void mStopMove() {
        _CurrentTrainState = TrainState.Stand;
        mResetParams();
    }

    public void mResetParams() {
        _CurrentSpeed = 0.0f;
        _CurrentTime = 0.0f;
    }
    private float mOffsetX ()
    {
        _CurrentSpeed = BaseSpeed + Acceleration * _CurrentTime;
        if (_CurrentSpeed >= MaxSpeed) _CurrentSpeed = MaxSpeed;

        //Debug.Log($"Current = {_CurrentSpeed}");
        return _CurrentSpeed / 100;
    }
    #endregion
}
