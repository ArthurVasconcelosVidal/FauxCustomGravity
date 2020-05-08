
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravity : MonoBehaviour{
    [SerializeField] LayerMask gravity_Layer;
    [SerializeField] float magnitudeForce = 50;
    [SerializeField] Collider gameObjectCollider;
    [SerializeField] GameObject gravityReference = null; // work in progress retirar
    [SerializeField] bool applyGravity = true; //Utilitario
    float rotationSpeed = 10f;
    Rigidbody gameObject_rb;
    Vector3 gravityHit;
    Vector3 force;
    Quaternion gravityRotation;
    RaycastHit hit;
    float distToGround;

    // Use this for initialization
    void Start(){
        #region RigidyBody Config
        gameObject_rb = GetComponent<Rigidbody>();
        gameObject_rb.useGravity = false;
        gameObject_rb.freezeRotation = true;
        gameObject_rb.interpolation = RigidbodyInterpolation.Interpolate;
        gameObject_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        #endregion

        #region Collider Config
        if (gameObjectCollider == null) gameObjectCollider = GetComponent<Collider>();
        distToGround = gameObjectCollider.bounds.extents.y;
        #endregion

        if (gravityReference == null){
            gravityReference = this.gameObject;
        }
    }

    void Update(){
        GravityRay();
        GravityRotation();
        GravityForceApplication();
    }

    void GravityRay(){
        //RaycastHit hit;
        if (Physics.Raycast(gravityReference.transform.position, -transform.up, out hit, Mathf.Infinity, gravity_Layer)){
            gravityHit = hit.normal;
        }
    }

    void GravityRotation(){
        gravityRotation = Quaternion.FromToRotation(transform.up, gravityHit) * transform.rotation;
        gameObject_rb.MoveRotation(Quaternion.Slerp(transform.rotation, gravityRotation, rotationSpeed * Time.deltaTime));
    }

    void GravityForceApplication(){
        if (applyGravity){
            force = -transform.up * magnitudeForce;
            gameObject_rb.AddForce(force);
        }
    }

    public bool isGrounded(){
        bool isGround = Physics.Raycast(transform.position, -transform.up, distToGround + 0.1f);
        return isGround;
    }

    public RaycastHit ReturnGravityRaycastHit(){
        return hit;
    }
}
