using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate; //공격 속도
    public int maxAmmo;
    public int curAmmo;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public void Use()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if (type == Type.Range && curAmmo > 0)
        {
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing()
    {
        // 1
        yield return new WaitForSeconds(0.1f); // yield: 결과를 전달하는 키워드 -> 0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        // 2
        yield return new WaitForSeconds(0.3f);
        meleeArea.enabled = false;

        // 3
        yield return new WaitForSeconds(0.3f);
        trailEffect.enabled = false;

    }

    // 일반 함수: Use() 메인루틴 -> Swing() 서브루틴 -> Use() 메인루틴
    // 코루틴: Use() 메인루틴 + Swing() Co-루틴
    // yield 키워드를 여러개 사용해 시간차 로직 작성 가능

    IEnumerator Shot()
    {
        // #1. 총알 발사
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;

        //#2. 탄피 배출
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3); //탄피 튕겨나갈 때 랜덤 값
        caseRigid.AddForce(caseVec,ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
