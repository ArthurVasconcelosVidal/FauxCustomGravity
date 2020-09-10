using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour{
    [SerializeField] float jumpForce;

    public void Jump() {
        if (PlayerManager.instance.fauxGravity.isGrounded()){
            PlayerManager.instance.rigidyBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }
}
