using System.Collections;
using UnityEngine;

public class FlashDamage : MonoBehaviour
{
    [Header("Damage effect")]
    [ColorUsage(true, true)]
    [SerializeField] 
    private Color FlashColor;
    [SerializeField]
    private AnimationCurve FlashCurve;
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private Coroutine _flashCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;
            _materials[i].SetColor("_FlashColor", FlashColor);
        }
    }

    public void PlayFlash(float flashTime)
    {
        _flashCoroutine = StartCoroutine(DamageFlasher(flashTime));
    }
    
    private IEnumerator DamageFlasher(float flashTime)
    {
        float currentFlashAmount = 0.0f;
        float elapsedTime = 0.0f;
        
        while (elapsedTime <= flashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1.0f, FlashCurve.Evaluate(elapsedTime), elapsedTime / flashTime);
            SetFlashAmount(currentFlashAmount);
            
            yield return null;
        }
    }

    private void SetFlashAmount(float flashAmount)
    {
        for (int i = 0; i < _materials.Length; ++i)
        {
            _materials[i].SetFloat("_FlashAmount", flashAmount);
        }
    }
}
