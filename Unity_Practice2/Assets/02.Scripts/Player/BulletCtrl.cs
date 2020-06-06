using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float damage = 20.0f;
    public float speed = 1000.0f;

    Transform tr;
    Rigidbody rb;
    TrailRenderer trail;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        trail = GetComponent<TrailRenderer>();
        damage = GameManager.instance.gameData.damage;
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * speed);
        GameManager.OnItemChange += UpdateSetup;
    }

    void UpdateSetup()
    {
        damage = GameManager.instance.gameData.damage;
    }

    private void OnDisable()
    {
        trail.Clear();
        tr.position = Vector3.zero;
        tr.rotation = Quaternion.identity;
        rb.Sleep();
    }
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
