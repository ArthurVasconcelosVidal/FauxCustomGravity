using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour{
    public void OnSouthButton(){
        PlayerManager.instance.actionManager.Jump();
    }

    public void OnRightStick(InputValue inputValue){
        Vector2 rightStick = inputValue.Get<Vector2>();
        //PlayerManager.instance.movimentManager.RightStick(rightStick);
        PlayerManager.instance.cameraManager.MoveCamValues(rightStick);
    }

    public void OnLeftStick(InputValue inputValue){
        Vector2 leftStick = inputValue.Get<Vector2>();
        PlayerManager.instance.movimentManager.LeftStick(leftStick);
    }

    public void OnDpad(InputValue inputValue){
        //Vector2 dpadValue = inputValue.Get<Vector2>();
        //PlayerManager.instance.cameraManager.Dpad(dpadValue);
    }

}
