using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour { 
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float maxDistance = 1;
    [SerializeField] float impactForceOnStop = 5;
    [SerializeField] [Range(0, 1)] float lerpInterpolation = 0.5f;
    [SerializeField] [Range(0, 5)] float decreaseSpeed = 1;
    [SerializeField] [Range(0, 5)] float decreaseDistance = 1;
    [SerializeField] string rebootTag;
    [SerializeField] string destroyOnCollisionTag;
    [SerializeField] string bulletTag;
    [SerializeField] bool decreaseSpeedActive = true; //Utilitario
    [SerializeField] bool decreaseDistanceActive = true; //Utilitario
    [SerializeField] bool rebot = true; //Utilitario
    [SerializeField] bool destroyOnCollision = false; //Utilitario
    Rigidbody bullet_rb;
    FauxGravity bullet_gravity;

    void Start(){
        bullet_rb = GetComponent<Rigidbody>();
        bullet_gravity = GetComponent<FauxGravity>();
    }

    void Update(){
        BulletFauxDecay();
        SetDistanceToGround();
        BulletMoviment();
    }

    void BulletMoviment(){
        bullet_rb.MovePosition(bullet_rb.position + transform.forward * bulletSpeed * Time.deltaTime);
    }

    void SetDistanceToGround(){
        bullet_gravity.SetApplyGravity(false);
        Vector3 distAtual = transform.position - bullet_gravity.ReturnGravityRaycastHit().point;
        Vector3 bulletPosition = distAtual.normalized * maxDistance + bullet_gravity.ReturnGravityRaycastHit().point;
        bullet_rb.MovePosition(Vector3.Lerp(transform.position,bulletPosition, lerpInterpolation));
    }

    void BulletFauxDecay(){
        if (decreaseSpeedActive){
            if (bulletSpeed > 0) bulletSpeed -= decreaseSpeed;
            else bulletSpeed = 0;
        }

        if (decreaseDistanceActive){
            if (maxDistance > 0) maxDistance -= decreaseDistance;
            else maxDistance = 0;
        }
    }

    void CollisionInteractions(Collision collision){
        if (rebot){
            if (collision.gameObject.CompareTag(rebootTag) || collision.gameObject.CompareTag(bulletTag))
                Reflection(collision.contacts[0].normal);
        }
        if (destroyOnCollision){
            if (collision.gameObject.CompareTag(destroyOnCollisionTag) || destroyOnCollisionTag == ""){
                Destroy(this.gameObject);
            }
        }
    }

    //Rebot Function
    #region Reboot Function
    public void Reflection(Vector3 colisionPoint){
        float dot = Vector3.Dot(transform.forward.normalized, colisionPoint);
        float arCos = Mathf.Acos(dot) * Mathf.Rad2Deg;
        Vector3 cross = Vector3.Cross(transform.forward.normalized, colisionPoint);
        Vector3 localCross = transform.InverseTransformDirection(cross);
        float angle = arCos;
        if (localCross.y < 0) angle = -angle;
        bullet_rb.MoveRotation(Quaternion.AngleAxis(angle, transform.up) * transform.rotation);
    }
    #endregion

    private void OnCollisionEnter(Collision collision){
        CollisionInteractions(collision);
    }

    //Spawner Configuration Functions
    #region Spawner Configuration Functions
    public void SetVelocity(float velocity){
        bulletSpeed = velocity;
    }

    public void SetMaxDistance(float distance){
        maxDistance = distance;
    }

    public void SetDecreaseSpeedActive(bool active){
        decreaseSpeedActive = active;
    }

    public void SetDecreaseDistanceActive(bool active){
        decreaseDistanceActive = active;
    }

    public void SetDecreaseSpeed(float velocity){
        decreaseSpeed = velocity;
    }

    public void SetDecreaseDistance(float distance){
        decreaseDistance = distance;
    }
    #endregion

}
