using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Track animation component
    // track animation clips
    // recive animation events
    // function to play animations

    [SerializeField] private Animation mainMenuAnimatior;
    [SerializeField] private AnimationClip fadeOutAnimation;
    [SerializeField] private AnimationClip fadeInAnimation;

    public void OnFadeOutComplite()
    {
        
    }

    public void OnFadeInComplite()
    {
        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        
        mainMenuAnimatior.Stop();
        mainMenuAnimatior.clip = fadeInAnimation;
        mainMenuAnimatior.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);
        mainMenuAnimatior.Stop();
        mainMenuAnimatior.clip = fadeOutAnimation;
        mainMenuAnimatior.Play();
    }
}
