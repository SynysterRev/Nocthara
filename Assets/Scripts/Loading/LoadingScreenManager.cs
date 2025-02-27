using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenManager : Singleton<LoadingScreenManager>
{
    public delegate void LoadingScreenDelegate();
    public event LoadingScreenDelegate OnFadeInOver;
    public event LoadingScreenDelegate OnFadeOutOver;
    
    [SerializeField]
    private Image LoadingScreen; 

    public void ShowLoadingScreen()
    {
        StartCoroutine(FadeIn(0.5f));
    }

    public void HideLoadingScreen()
    {
        StartCoroutine(FadeOut(0.5f));
    }

    private IEnumerator FadeIn(float duration)
    {
        Color color = LoadingScreen.color;
        color.a = 0;
        while (color.a < 1)
        {
            color.a += Time.deltaTime / duration;
            LoadingScreen.color = color;
            yield return null;
        }
        OnFadeInOver?.Invoke();
    }
    
    private IEnumerator FadeOut(float duration)
    {
        Color color = LoadingScreen.color;
        color.a = 1;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime / duration;
            LoadingScreen.color = color;
            yield return null;
        }
        OnFadeOutOver?.Invoke();
    }
}
