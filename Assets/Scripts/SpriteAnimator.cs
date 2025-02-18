using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class SpriteAnimator : MonoBehaviour
{
    public delegate void OnAnimationFinished();
    public event OnAnimationFinished onAnimationFinished;
    
    [System.Serializable]
    public class AnimationTrigger
    {
        public int Frame;
        public string Name;
    }
 
    [System.Serializable]
    public class Animation
    {
        public string Name;
        [HideInInspector]
        public int Fps;
        [HideInInspector]
        public Sprite[] Frames;
        public AnimationClip AnimationClip;
        
        public UnityEvent OnAnimationFinished;
 
        public AnimationTrigger[] Triggers;
    }
 
    public SpriteRenderer SpriteRenderer;
    public Animation[] Animations;
 
    public bool Playing { get; private set; }
    public Animation CurrentAnimation { get; private set; }
    public int CurrentFrame { get; private set; }
    public bool Loop { get; private set; }
 
    public string PlayAnimationOnStart;
 
    void Awake()
    {
        if (!SpriteRenderer)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
 
    void OnEnable()
    {
        if (PlayAnimationOnStart != "")
            Play(PlayAnimationOnStart);
    }
 
    void OnDisable()
    {
        Playing = false;
        CurrentAnimation = null;
    }
 
    public void Play(string animName, bool loop = true, int startFrame = 0)
    {
        Animation currentAnim = GetAnimation(animName);
        if (currentAnim != null )
        {
            if (currentAnim != CurrentAnimation)
            {
                ForcePlay(animName, loop, startFrame);
            }
        }
        else
        {
            Debug.LogWarning("could not find animation: " + animName);
        }
    }
 
    public void ForcePlay(string animName, bool loop = true, int startFrame = 0)
    {
        Animation currentAnim = GetAnimation(animName);
        if (currentAnim != null)
        {
            this.Loop = loop;
            CurrentAnimation = currentAnim;
            Playing = true;
            CurrentFrame = startFrame;
            SpriteRenderer.sprite = currentAnim.Frames[CurrentFrame];
            StopAllCoroutines();
            StartCoroutine(PlayAnimation(CurrentAnimation));
        }
    }
 
    public void SlipPlay(string animName, int wantFrame, params string[] otherNames)
    {
        for (int i = 0; i < otherNames.Length; i++)
        {
            if (CurrentAnimation != null && CurrentAnimation.Name == otherNames[i])
            {
                Play(animName, true, CurrentFrame);
                break;
            }
        }
        Play(animName, true, wantFrame);
    }
 
    public bool IsPlaying(string animName)
    {
        return (CurrentAnimation != null && CurrentAnimation.Name == animName);
    }
 
    public Animation GetAnimation(string animName)
    {
        foreach (Animation currentAnim in Animations)
        {
            if (currentAnim.Name == animName)
            {
                currentAnim.Fps = (int)currentAnim.AnimationClip.frameRate;
                currentAnim.Frames = GetSpritesFromClip(currentAnim.AnimationClip).ToArray();
                return currentAnim;
            }
        }
        return null;
    }
 
    IEnumerator PlayAnimation(Animation wantedAnimation)
    {
        float timer = 0f;
        float delay = 1f / (float)wantedAnimation.Fps;
        while (Loop || CurrentFrame < wantedAnimation.Frames.Length-1)
        {
 
            while (timer < delay)
            {
                timer += Time.deltaTime;
                yield return 0f;
            }
            while (timer > delay)
            {
                timer -= delay;
                NextFrame(wantedAnimation);
            }
 
            SpriteRenderer.sprite = wantedAnimation.Frames[CurrentFrame];
        }
 
        CurrentAnimation = null;
    }
 
    void NextFrame(Animation currentAnim)
    {
        CurrentFrame++;
        foreach (AnimationTrigger animationTrigger in CurrentAnimation.Triggers)
        {
            if (animationTrigger.Frame == CurrentFrame)
            {
                gameObject.SendMessageUpwards(animationTrigger.Name);
            }
        }
        if (CurrentFrame >= currentAnim.Frames.Length - 1)
        {
            if (Loop)
                CurrentFrame = 0;
            else
            {
                CurrentFrame = currentAnim.Frames.Length - 1;
                onAnimationFinished?.Invoke();
                currentAnim.OnAnimationFinished?.Invoke();
            }
        }
    }
 
    public int GetFacing()
    {
        return (int)Mathf.Sign(SpriteRenderer.transform.localScale.x);
    }
 
    public void FlipTo(float dir)
    {
        if (dir < 0f)
            SpriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            SpriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
 
    public void FlipTo(Vector3 position)
    {
        float diff = position.x - transform.position.x;
        if (diff < 0f)
            SpriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        else
            SpriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
    public List<Sprite> GetSpritesFromClip(AnimationClip clip)
    {
        var sprites = new List<Sprite> ();
        if(clip != null)
        {
            foreach(var binding in AnimationUtility.GetObjectReferenceCurveBindings(clip))
            {
                var keyframes = AnimationUtility.GetObjectReferenceCurve (clip, binding);
                foreach(var frame in keyframes)
                {
                    sprites.Add((Sprite) frame.value);
                }
            }
        }
        return sprites;
    }
}