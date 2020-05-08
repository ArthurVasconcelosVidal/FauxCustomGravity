using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawn : MonoBehaviour{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] int numberOfBullets;
    [SerializeField] float timeBtweenSpawn;


    void Start(){
        StartCoroutine("SpawnBulletByTime");
    }

    IEnumerator SpawnBulletByTime(){
        for (int i = 0; i < numberOfBullets; i++){
            var bullet = Instantiate(bulletPrefab, transform);
            yield return new WaitForSeconds(timeBtweenSpawn);
        }
    }
}
