using System.Numerics;
using UnityEngine;

public class FireboltProjectile : MonoBehaviour
{
    public float speed = 10f;
    private float damage;
    private UnityEngine.Vector2 direction;

    public void Initialize(UnityEngine.Vector2 dir, float power)
    {
        direction = dir.normalized;
        damage = power;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (UnityEngine.Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
