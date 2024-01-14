using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BioPanelHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject RulePanel;
    [SerializeField]
    private GameObject HowToPanel;
    [SerializeField]
    private GameObject StoryPanel;

    public void PushRule()
    {
        AllClosePanel();
        RulePanel.SetActive(true);
    }

    public void PushHowTo()
    {
        AllClosePanel();
        HowToPanel.SetActive(true);
    }

    public void PushStory()
    {
        AllClosePanel();
        StoryPanel.SetActive(true);
    }

    private void AllClosePanel()
    {
        RulePanel.SetActive(false);
        HowToPanel.SetActive(false);
        StoryPanel.SetActive(false);
    }
}
