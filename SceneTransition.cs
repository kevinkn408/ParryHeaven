using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] GameObject panelLeft;
    [SerializeField] GameObject panelRight;

    public Camera screenSpace;

    RectTransform leftRect;
    RectTransform rightRect;

    float leftOffset;
    float rightOffset;

    float newLeftOffsetPosition;
    float newRightOffsetPosition;

    Vector3 newLeftPos;
    Vector3 newRightPos;


    int screenWidth;


    // Start is called before the first frame update
    void Start()
    {
        leftRect = panelLeft.GetComponent<RectTransform>();
        rightRect = panelRight.GetComponent<RectTransform>();
        SetUpPosition();
        StartCoroutine(CrossFadeStart());
    }

    private void SetUpPosition()
    {
        screenWidth = Screen.width;

        leftOffset = (screenWidth * 0);
        rightOffset = (screenWidth * 1);


        newLeftOffsetPosition = (screenWidth * -0.5f) - 5;
        newRightOffsetPosition = (screenWidth * 1.5f) + 5;

        newLeftPos = new Vector2(newLeftOffsetPosition, transform.position.y);
        newRightPos = new Vector2(newRightOffsetPosition, transform.position.y);

        leftRect.position = new Vector2(leftOffset, transform.position.y);
        rightRect.position = new Vector2(rightOffset, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    IEnumerator CrossFadeStart()
    {
        var elapsedTime = 0f;
        var time = 0.5f;
        var startingLeftPos = leftRect.position;
        var startingRightPos = rightRect.position;

        yield return new WaitForSeconds(1f);

        while(elapsedTime < time)
        {

            leftRect.position = Vector2.Lerp(startingLeftPos, newLeftPos, elapsedTime / time);
            rightRect.position = Vector2.Lerp(startingRightPos, newRightPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator CrossFadeEnd()
    {
        var elapsedTime = 0f;
        var time = 0.25f;
        var startingLeftPos = leftRect.position;
        var startingRightPos = rightRect.position;

        var oldLeftPos =  new Vector2(leftOffset, transform.position.y);
        var oldRightPos = new Vector2(rightOffset, transform.position.y);


        yield return new WaitForSeconds(1f);

        while (elapsedTime < time)
        {

            leftRect.position = Vector2.Lerp(leftRect.position, oldLeftPos, elapsedTime / time);
            rightRect.position = Vector2.Lerp(rightRect.position, oldRightPos,  elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
