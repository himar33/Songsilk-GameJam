using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    Animator anim;
    public GameObject ladder;
    public float fallSize;
    public float animSpeed;

    public Vector3 initialPos;
    public Vector3 downPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
        initialPos = ladder.transform.position;
        downPos = new Vector3(initialPos.x, initialPos.y - fallSize, initialPos.z);
    }

    public override void OnInteract()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Up") || stateInfo.IsName("none"))
        {
            if (ladder.transform.position == initialPos)
            {
                anim.SetTrigger("Down");
                StartCoroutine(SmoothTranlation(downPos, animSpeed));
            }
        }
        else if (stateInfo.IsName("Down"))
        {
            if (ladder.transform.position == downPos)
            {
                anim.SetTrigger("Up");
                StartCoroutine(SmoothTranlation(initialPos, animSpeed));
            }
        }
    }
    IEnumerator SmoothTranlation(Vector3 target, float speed)
    {
        while (ladder.transform.position != target) {
            ladder.transform.position = Vector3.Lerp(ladder.transform.position, target, Time.deltaTime * speed);
            yield return null;
        }
    }
}
