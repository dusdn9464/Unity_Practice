using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE=0,
        SHOTGUN
    }
    public WeaponType currWeapon = WeaponType.RIFLE;

    public GameObject bullet;
    public ParticleSystem cartridge;
    public Transform firePos;
    private ParticleSystem muzzleFlash;
    private AudioSource _audio;
    public PlayerSfx playerSfx;

    private Shake shake;

    public Image magazineImg;
    public Text magazineText;
    public int maxBullet = 10;
    public int remainingBullet = 10;
    public float reloadTime = 2.0f;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReloading && Input.GetMouseButtonDown(0))
        {
            --remainingBullet;
            Fire();

            if(remainingBullet == 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }


    private void Fire()
    {
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
        //Instantiate(bullet, firePos.position, firePos.rotation);
        var _bullet = GameManager.instance.GetBullet();
        if (_bullet != null)
        {
            _bullet.transform.position = firePos.position;
            _bullet.transform.rotation = firePos.rotation;
            _bullet.SetActive(true);
        }
        cartridge.Play();
        muzzleFlash.Play();
        FireSfx();
        magazineImg.fillAmount = (float)remainingBullet / (float)maxBullet;
        UpdateBulletText();
    }


    private void FireSfx()
    {
        var _sfx = playerSfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }


    IEnumerator Reloading()
    {
        isReloading = true;
        _audio.PlayOneShot(playerSfx.reload[(int)currWeapon], 1.0f);
        yield return new WaitForSeconds(playerSfx.reload[(int)currWeapon].length + 0.3f);

        isReloading = false;
        magazineImg.fillAmount = 1.0f;
        remainingBullet = maxBullet;
        UpdateBulletText();
    }


    private void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet, maxBullet);
    }

}
