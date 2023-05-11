using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    private Animator atkAnim = default;
<<<<<<< HEAD
    public GameObject attackRange = default;
=======
    private GameObject attackRange = default;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498

    public static bool isAttacking = false;
    private bool isAttack = false;

<<<<<<< HEAD
    private void Start() 
    {
        atkAnim = GetComponent<Animator>();
    }

    private void Update() 
    {        
        AtkInput();
    }
    private void FixedUpdate() 
    {
        Attack();        
    }

    // { Player Attack
#region Player Attack
    private void AtkInput()
    {
        if(Input.GetMouseButtonDown(0))
=======
    private void Start()
    {
        atkAnim = GetComponent<Animator>();
        attackRange = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        AtkInput();
    }

    private void FixedUpdate()
    {
        Attack();
    }

    // { Player Attack
    #region Player Attack
    private void AtkInput()
    {
        if (Input.GetMouseButtonDown(0))
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        {
            isAttack = true;
        }
    }
    private void Attack()
    {
<<<<<<< HEAD
        if(isAttack == true && PlayerOther.isInvenOpen == false
            && PlayerOther.isMapOpen == false)
        {
            if(PlayerMove.isGrounded == true && isAttacking == false)
            {
                atkAnim.SetTrigger("Attack");
                isAttacking = true;
                StartCoroutine(AtkDelay());
=======
        if (isAttack == true && PlayerOther.isInvenOpen == false
            && PlayerOther.isMapOpen == false && PlayerMove.isDead == false
            && PlayerOther.isMenuOpen == false && PlayerOther.isStoveOpen == false
            && PlayerOther.isAnvilOpen == false && PlayerOther.isWorkbenchOpen == false
            && PlayerMove.isEating == false)
        {
            if (isAttacking == false)
            {
                atkAnim.SetTrigger("Attack");
                isAttacking = true;
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
            }
        }
    }

<<<<<<< HEAD

    private IEnumerator AtkDelay()
    {
        yield return new WaitForSeconds(0.4f);
        attackRange.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        attackRange.SetActive(false);
        yield return new WaitForSeconds(1f);
=======
    private void AttakCol_T()
    {
        attackRange.SetActive(true);
    }
    private void AttackCol_F()
    {
        attackRange.SetActive(false);
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
        atkAnim.SetTrigger("AtkCancel");
        isAttacking = false;
        isAttack = false;
    }
<<<<<<< HEAD
#endregion
=======
    #endregion
>>>>>>> 906dbeb8e19fb7a93a4a77a683abc26c4204f498
    // } Player Attack
}
