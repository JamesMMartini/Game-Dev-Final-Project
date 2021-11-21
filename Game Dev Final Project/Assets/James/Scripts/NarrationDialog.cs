using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NarrationDialog", menuName = "UI Objects/Create New Narration Dialog")]
public class NarrationDialog : ScriptableObject
{
    [SerializeField] NarrationType type;
    [SerializeField] string text;
    [SerializeField] NarrationDialog previousAction;
    [SerializeField] NarrationDialog nextAction;

    public NarrationType NarrationType { get { return type; } }

    public string Text { get { return text; } }

    public NarrationDialog Previous { get { return previousAction; } }

    public NarrationDialog Next { get { return nextAction; } }
}

public enum NarrationType
{
    NewDialog,
    MainMenu,
    RunMenu,
    FightMenu
}
