using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] 
    private GameObject HeartGrid;
    
    [SerializeField] 
    private Sprite EmptyHeart;
    
    [SerializeField] 
    private Sprite Heart;
    
    List<Image> _hearts = new List<Image>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerManager.Instance.OnTakeDamage += UpdateHealth;
        PlayerManager.Instance.OnRestaureHealth += UpdateHealth;
        PlayerManager.Instance.OnUpgradeMaxHealth += UpgradeMaxHealth;
        CreateHealthBar();
    }

    private void OnDestroy()
    {
        // if (PlayerManager.Instance != null)
        // {
        //     PlayerManager.Instance.OnTakeDamage -= UpdateHealth;
        //     PlayerManager.Instance.OnRestaureHealth -= UpdateHealth;
        //     PlayerManager.Instance.OnUpgradeMaxHealth -= UpgradeMaxHealth;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateHealthBar()
    {
        int currentHealth = PlayerManager.Instance.Health;
        int maxHealth = PlayerManager.Instance.MaxHealth;
        
        int numberOfHearts = maxHealth / 4;
        for (int i = 0; i < numberOfHearts; ++i)
        {
            GameObject emptyHeart = new GameObject($"EmptyHeart{i + 1}");
            emptyHeart.transform.SetParent(HeartGrid.transform);
            emptyHeart.AddComponent<Image>().sprite = EmptyHeart;
            
            GameObject heart = new GameObject($"Heart{i + 1}");
            heart.transform.SetParent(emptyHeart.transform);
            Image heartImg = heart.AddComponent<Image>();
            heartImg.sprite = Heart;
            heartImg.type = Image.Type.Filled;
            heartImg.fillClockwise = false;
            heartImg.fillOrigin = 2;
            RectTransform heartRectTransform = heart.GetComponent<RectTransform>();
            heartRectTransform.anchoredPosition = Vector3.zero;
            heartRectTransform.sizeDelta = new Vector2(64, 64);
            _hearts.Add(heartImg);
        }
        UpdateHealth(currentHealth, maxHealth);
    }

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        int leftPiecesOfHeart = currentHealth;
        foreach (Image heart in _hearts)
        {
            heart.fillAmount = Mathf.Clamp01(leftPiecesOfHeart / 4.0f);
            leftPiecesOfHeart -= 4;
        }
    }

    private void UpgradeMaxHealth(int currentHealth, int maxHealth)
    {
        
    }
}
