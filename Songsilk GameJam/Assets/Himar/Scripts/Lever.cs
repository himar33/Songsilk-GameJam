using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    Animator anim;
    public GameObject ladder;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void OnInteract()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Up") || stateInfo.IsName("none"))
        {
            anim.SetTrigger("Down");
            ladder.GetComponent<Animator>().SetTrigger("open");
        }
        else if (stateInfo.IsName("Down"))
        {
            anim.SetTrigger("Up");
            ladder.GetComponent<Animator>().SetTrigger("close");
        }
    }
}
