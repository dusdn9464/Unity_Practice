using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    public Mesh[] meshes;
    public Texture[] textures;

    int hitCount = 0;
    Rigidbody rb;
    MeshFilter meshFilter;
    MeshRenderer _renderer;
    AudioSource _audio;

    public float expRadius = 10.0f;
    public AudioClip expSfx;
    public Shake shake;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        _audio = GetComponent<AudioSource>();
        //shake = GameObject.Find("CameraRig").GetComponent<Shake>();
        StartCoroutine(GetShake());
        _renderer.material.mainTexture = textures[UnityEngine.Random.Range(0, textures.Length)];
    }

    IEnumerator GetShake()
    {
        while(!UnityEngine.SceneManagement.SceneManager.GetSceneByName("Play").isLoaded)
        {
            yield return null;

        }
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.collider.CompareTag("BULLET"))
        {
            if(++hitCount == 3)
            {
                ExpBarrel();
            }
        }
    }

    private void ExpBarrel()
    {
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);
        //rb.mass = 1.0f;
        //rb.AddForce(Vector3.up * 1000.0f);
        IndirectDamage(transform.position);

        int idx = UnityEngine.Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        _audio.PlayOneShot(expSfx, 1.0f);
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    private void IndirectDamage(Vector3 pos)
    {
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1.0f;
            _rb.AddExplosionForce(1200.0f, pos, expRadius, 1000.0f);
        }
    }
}
