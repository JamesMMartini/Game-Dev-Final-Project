using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumPractice : MonoBehaviour
{
    public enum LerpType {  Move, EaseIn, EaseOut, SmoothStep}

    public Vector3 startPos;
    public Vector3 endPos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        //switch (LerpType)
        {
            //case LerpType.Move:
                    
        }
        StartCoroutine(EaseIn(1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EaseIn(float delay)
    {
        //dtransform.position = startPos;
        yield return new WaitForSeconds(delay);
        float t = 0;
        float currentLearpTime = 0.0f;
        float lerpTime = 1.0f;
        while(1 > 0)
        {
            //currentLearpTime += Time.deltaTime * speed;
            if(currentLearpTime < lerpTime)
            {
                currentLearpTime = lerpTime;
            }
            t = currentLearpTime / lerpTime;
        }
    }

    IEnumerator SmoothStep(float delay)
    {
        transform.position = startPos;
        yield return new WaitForSeconds(delay);
        float t = 0;
        float currentLerpTime = 0.0f;
        float lerpTime = 1.0f;
        while (t < 1.0f)
        {
            currentLerpTime += Time.deltaTime * speed;
            if (currentLerpTime > lerpTime)
            {
                currentLerpTime = lerpTime;
            }
            t = currentLerpTime / lerpTime;
        }
    }
}
