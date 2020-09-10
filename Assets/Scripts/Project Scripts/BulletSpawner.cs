using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour{
    [SerializeField] GameObject spawnPoint;
    [SerializeField] GameObject bulletPrefab;

    void Update(){
        SpawnBullet();
    }

    void SpawnBullet() {
        if (Input.GetKeyDown(KeyCode.E)){
            var bullet = Instantiate(bulletPrefab, spawnPoint.transform.position, Quaternion.identity);
        }
    }
}
