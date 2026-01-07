using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    [SerializeField] public float bulletspeed = 1f;
    [SerializeField] public float bulletrange = 2f;
    [SerializeField] public float AimOffsetX = 0.05f;
    [SerializeField] public float AimOffsetY = 0.05f;

    public void ShootRandom(){
        GameObject Bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();

        firePoint.eulerAngles = new UnityEngine.Vector3(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f));
        rb.AddForce(firePoint.up * bulletspeed, ForceMode2D.Impulse);

        Destroy(Bullet, bulletrange);
    }

    public void ShootTarget(){
        Physics.IgnoreLayerCollision(1,9);
        GameObject Bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();

        var plyPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        var shootDir = plyPos - transform.position;
        shootDir.Normalize();
        UnityEngine.Vector2 shootDir2 = new(shootDir.x + AimOffsetX, shootDir.y + AimOffsetY);

        rb.AddForce(shootDir2 * bulletspeed, ForceMode2D.Impulse);
        Destroy(Bullet, bulletrange);
    }
}
