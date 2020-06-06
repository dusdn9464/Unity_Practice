using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour
{
    private float h = 0.0f;
    private float v = 0.0f;
    private float r = 0.0f;

    private Transform tr;
    public float moveSpeed = 10.0f;
    public float rotSpeed = 80.0f;

    public PlayerAnim playerAnim;
    public Animation anim;

    private void OnEnable()
    {
        GameManager.OnItemChange += UpdateSetup;
    }

    void UpdateSetup()
    {
        moveSpeed = GameManager.instance.gameData.speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.clip = playerAnim.idle;
        anim.Play();

        moveSpeed = GameManager.instance.gameData.speed;
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        Debug.Log("h = " + h.ToString());
        Debug.Log("v = " + v.ToString());

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        if( v >= 0.1f)
        {
            anim.CrossFade(playerAnim.runF.name, 0.3f);    //전진 애니메이션
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);    //후진 애니메이션
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);    //오른쪽이동 애니메이션
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);    //왼쪽이동 애니메이션
        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);    //정지 시 Idle 애니메이션
        }

    }

}
