using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCtrl : MonoBehaviour
{
    private Animator anim;

    private readonly int hashWaterOn = Animator.StringToHash("WaterOn");

    private bool isWet = false;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GAMEMANAGER").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        WaterStateCheck();
    }


    void WaterStateCheck()
    {

        if(this.isWet != gameManager.isWet)
        {
            this.isWet = gameManager.isWet;
            StartCoroutine(AnimStateUpdate());
        }
    }
    
    IEnumerator AnimStateUpdate()
    {
        if (this.isWet)
        {
            //Turn Anim hash bool On and Rendering turn off 
            anim.SetBool(hashWaterOn, true);
            yield return new WaitForSeconds(2.0f);
            SetAnimRendererState(this.anim, !isWet);
        }
        else
        {
            //Turn Anim hash bool Off and Rendering turn on.
            anim.SetBool(hashWaterOn, false);
            SetAnimRendererState(this.anim, !isWet);
        }
        yield break;
    }

    void SetAnimRendererState(Animator anim, bool state)
    {
        anim.transform.Find("Flower Pot Monster").gameObject.SetActive(state);
    }
}
