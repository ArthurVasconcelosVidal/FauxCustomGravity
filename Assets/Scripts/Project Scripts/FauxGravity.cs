
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;

public class FauxGravity : MonoBehaviour{
    [SerializeField] LayerMask gravity_Layer;
    [SerializeField] string gravityTag;
    [SerializeField] string groundTag;
    [SerializeField] float magnitudeForce = 50;
    [SerializeField] Collider gameObjectCollider;
    [SerializeField] GameObject gravityUser = null;
    [SerializeField] GameObject planetReference = null;
    [SerializeField] [Range(0, 1)] float forceDirectionInterpolation = 0.5f;
    [SerializeField] bool applyGravity = true; //Utilitario
    [SerializeField] bool canChangePlanets = false; //Utilitarios
    [SerializeField] bool transitionForceCorrection = true; //Utilitarios
    [SerializeField] bool setRigidyBodyRotation = true; //Utilitarios
    GameObject auxPlanetReference = null;
    float rotationSpeed = 10f;
    float distToGround;
    float directionInterpolation;
    Rigidbody gameObject_rb;
    Vector3 gravityHit;
    Vector3 gravityForceDirection;
    Quaternion gravityRotation;
    RaycastHit hit;


    // Use this for initialization
    void Start(){
        #region RigidyBody Config
        gameObject_rb = GetComponent<Rigidbody>();
        gameObject_rb.useGravity = false;
        gameObject_rb.freezeRotation = setRigidyBodyRotation;
        gameObject_rb.interpolation = RigidbodyInterpolation.Interpolate;
        gameObject_rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        #endregion

        #region Collider Config
        if (gameObjectCollider == null) gameObjectCollider = GetComponent<Collider>();
        distToGround = gameObjectCollider.bounds.extents.y;
        #endregion

        if (gravityUser == null){
            gravityUser = this.gameObject;
        }

        if (planetReference == null){
            if(Physics.Raycast(gravityUser.transform.position, -transform.up, out hit, Mathf.Infinity, gravity_Layer)){
                planetReference = hit.collider.gameObject;   
            }
        }
    }

    void Update(){
        NewGravityRay();
        GravityRotation();
        GravityForceApplication();
    }

    //Gravity system
    #region Gravity System
    void NewGravityRay(){
        Vector3 rayDirection = planetReference.GetComponent<Collider>().ClosestPoint(transform.position); 
        rayDirection = rayDirection - transform.position;
        if (Physics.Raycast(gravityUser.transform.position, rayDirection.normalized, out hit, Mathf.Infinity, gravity_Layer)){
            gravityHit = hit.normal;
        }
    }

    void ChangingPlanet(GameObject newPlanet) {
        if (!isGrounded() && newPlanet != auxPlanetReference && canChangePlanets){
            auxPlanetReference = planetReference;
            planetReference = newPlanet;
            directionInterpolation = 1;
            if (transitionForceCorrection && applyGravity)
                StartCoroutine("GravityTransitionForce", magnitudeForce);
        }    
    }

    void ClearPreviousPlanet(){
        auxPlanetReference = null;
    }

    void ClearForce(){
        gameObject_rb.velocity = Vector3.zero;
        gameObject_rb.angularVelocity = Vector3.zero;
        directionInterpolation = forceDirectionInterpolation;
    }

    IEnumerator GravityTransitionForce(float iniForce){
        while (auxPlanetReference != null){
            magnitudeForce += 0.5f;
            yield return null;
        }
        magnitudeForce = iniForce;
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag(gravityTag)){
            ClearPreviousPlanet();
            ClearForce();
        }    
    }

    void OnTriggerEnter(Collider other){
        ChangingPlanet(other.transform.parent.gameObject);
    }

    void GravityRotation(){
        gravityRotation = Quaternion.FromToRotation(transform.up, gravityHit) * transform.rotation;
        gameObject_rb.MoveRotation(Quaternion.Slerp(transform.rotation, gravityRotation, rotationSpeed * Time.deltaTime));
    }

    void GravityForceApplication(){
        if (applyGravity){
            gravityForceDirection = Vector3.Lerp(-transform.up, -gravityHit, directionInterpolation);
            gravityForceDirection = gravityForceDirection * magnitudeForce;
            gameObject_rb.AddForce(gravityForceDirection);
        }
    }
    //Gravity System
    #endregion

    //Public Utilities
    #region Public Utilities
    public bool isGrounded(){
        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, -transform.up, out hitGround, distToGround + 0.1f) && hitGround.transform.gameObject.CompareTag(groundTag))
            return true;
        else
            return false;
    }

    public RaycastHit ReturnGravityRaycastHit() {
        return hit;
    }

    public void SetApplyGravity(bool value){
        applyGravity = value;
    }

    public void SetCanChangePlanets(bool value) {
        canChangePlanets = value;
    }

    void SetTransitionForceFix(bool value) {
        transitionForceCorrection = value;
    }

    void SetFreezeRotation(bool value) {
        gameObject_rb.freezeRotation = value;
    }
    //Public Utilities
    #endregion
}
