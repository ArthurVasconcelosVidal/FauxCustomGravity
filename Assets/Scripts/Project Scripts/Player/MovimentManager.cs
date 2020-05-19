using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentManager : MonoBehaviour{
    #region Movimentation
    Vector2 leftStick;
    Vector2 rightStick;
    Vector3 leftStickDirection;
    Vector3 rightStickDirection;
    [SerializeField] float moveSpeed = 7; //Default
    [SerializeField] float rotationSpeed = 5; //Default
    #endregion

    void Update(){
        Movimentation();
    }

    void LateUpdate(){
        //SourcePlayerRotation();
    }

    void Movimentation(){
        leftStick = Vector2.ClampMagnitude(leftStick, 1);
        rightStick = Vector2.ClampMagnitude(rightStick, 1);

        leftStickDirection = (transform.forward * leftStick.y + transform.right * leftStick.x);
        rightStickDirection = (transform.forward * rightStick.y + transform.right * rightStick.x);

        //Movimentar
        if (leftStickDirection != Vector3.zero) PlayerManager.instance.rigidyBody.MovePosition(PlayerManager.instance.rigidyBody.position + leftStickDirection * moveSpeed * Time.deltaTime);

        //Rotacionar o personagem
        if (rightStickDirection != Vector3.zero){
            MeshRotation(rightStickDirection, PlayerManager.instance.meshObject);
        }
        else{
            MeshRotation(leftStickDirection, PlayerManager.instance.meshObject);
        }
    }

    void MeshRotation(Vector3 direction, GameObject mesh){
        if (direction != Vector3.zero){
            var quaternionLook = Quaternion.LookRotation(direction, transform.up);
            mesh.transform.rotation = Quaternion.Slerp(mesh.transform.rotation, quaternionLook, rotationSpeed * Time.deltaTime);
        }
    }

    void SourcePlayerRotation(){ //Not used in moment
        //Rotate like tank
        Vector3 selfRotation_Y = Vector3.up * rightStick.x * rotationSpeed;
        Quaternion deltaRotation = Quaternion.Euler(selfRotation_Y);
        Quaternion targetRotation = PlayerManager.instance.rigidyBody.rotation * deltaRotation;
        PlayerManager.instance.rigidyBody.MoveRotation(Quaternion.Lerp(PlayerManager.instance.rigidyBody.rotation, targetRotation, 0.5f));
    }

    //InputSystem
    #region InputSystem
    public void RightStick(Vector2 stickValue){
        rightStick = stickValue;
    }

    public void LeftStick(Vector2 stickValue){
        leftStick = stickValue;
    }
    #endregion
}
