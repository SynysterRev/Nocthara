using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SkillHolder : MonoBehaviour
{
    [SerializedDictionary("Skill", "Unlocked")]
    public SerializedDictionary<Skill, bool> Skills;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
