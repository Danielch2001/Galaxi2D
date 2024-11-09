using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
     _anim = GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            _anim.SetBool("TurnLeft", true);
            _anim.SetBool("TurnRigth", false);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) )
        {
            _anim.SetBool("TurnLeft", false);
            _anim.SetBool("TurnRigth", false);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) )
        {
            _anim.SetBool("TurnRigth", true);
            _anim.SetBool("TurnLeft", false);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow) )
        {
            _anim.SetBool("TurnRigth", false);
            _anim.SetBool("TurnLeft", false);
        }
    }
}
