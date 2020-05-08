using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour{
    static public PlayerManager instance;

    public Rigidbody rigidyBody;
    public FauxGravity fauxGravity;
    public MovimentManager movimentManager;
    public GameObject meshObject; //inspector

    void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
        DontDestroyOnLoad(instance);
    }
}
