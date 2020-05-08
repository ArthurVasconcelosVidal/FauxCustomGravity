using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour{
    public void JumpInput(){

    }

    public void OnRightStick(InputValue inputValue){
        Vector2 rightStick = inputValue.Get<Vector2>();
        PlayerManager.instance.movimentManager.RightStick(rightStick);
    }

    public void OnLeftStick(InputValue inputValue){
        Vector2 leftStick = inputValue.Get<Vector2>();
        PlayerManager.instance.movimentManager.LeftStick(leftStick);
    }

}
