using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 1.0f;
    public Vector2 velocity;
    public float maxDistance = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (maxDistance > 0)
        {
            transform.Translate(new Vector2(bulletSpeed * Time.fixedDeltaTime * velocity.x, bulletSpeed * Time.fixedDeltaTime * velocity.y));
            maxDistance -= bulletSpeed * Time.fixedDeltaTime;
        }
        else Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        Destroy(gameObject);
    }
}
