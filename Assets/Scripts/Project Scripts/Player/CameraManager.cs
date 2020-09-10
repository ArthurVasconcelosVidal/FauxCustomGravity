using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour{
    #region Cinemachine Camera
    [SerializeField] CinemachineFreeLook freeLookCM;
    [SerializeField] float camSpeedY = 1; //Default
    [SerializeField] float camSpeedX = 80; //Default
    Vector2 stick;
    #endregion

    // Update is called once per frame
    void Update(){
        CameraMoviment();
    }

    void CameraMoviment(){
        freeLookCM.m_YAxis.Value += stick.y * camSpeedY * Time.deltaTime;
        freeLookCM.m_XAxis.Value += stick.x * camSpeedX * Time.deltaTime;
    }

    public void MoveCamValues(Vector2 stickValue){
        stick = stickValue;
    }
}
