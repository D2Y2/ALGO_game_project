using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillSelector : MonoBehaviour
{
    [SerializeField] private Button[] skillButtons = new Button[0];
    private readonly List<int> selectedSkills = new List<int>();

    void Awake()
    {
        for (int i = 0; i < skillButtons.Length; i++)
        {
            int idx = i;
            if (skillButtons[i] != null)
                skillButtons[i].onClick.AddListener(() => ToggleSkill(idx));
        }
    }

    void OnDestroy()
    {
        for (int i = 0; i < skillButtons.Length; i++)
            if (skillButtons[i] != null)
                skillButtons[i].onClick.RemoveAllListeners();
    }

    void ToggleSkill(int index)
    {
        if (selectedSkills.Contains(index)) selectedSkills.Remove(index);
        else selectedSkills.Add(index);
    }

    public List<int> GetSelectedSkills() => new List<int>(selectedSkills);
}
