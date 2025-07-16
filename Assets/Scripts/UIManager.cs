using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("LODER")]
    public GameObject lodingFillImage;
    public float totalTime;
    public float remainingTime;
    public float speed;
    private Coroutine sliderCoroutine;
    private bool IsStartSlider = false;


    [Space(10)]
    [Header("UI SCREEN")]
    public GameObject lodingScreen;
    public GameObject languageScreen;




    private void Start()
    {
        remainingTime = totalTime;
        IsStartSlider = true;
        StartSlider();
    }

    private void Update()
    {
        if (IsStartSlider)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime * speed;
            }
            else
            {
                IsStartSlider = false;
                remainingTime = 0;
                StopSlider();
            }
        }
    }


    public void StartSlider()
    {
        sliderCoroutine = StartCoroutine(MoveSlider());
    }

    public void StopSlider()
    {
        StopCoroutine(sliderCoroutine);
    }

    public IEnumerator MoveSlider()
    {
        lodingFillImage.transform.DOLocalMoveX(190, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            lodingFillImage.transform.DOLocalMoveX(-190, 1f).SetEase(Ease.Linear);
        });

        yield return new WaitForSeconds(2f);
        StartSlider();
    }



}
