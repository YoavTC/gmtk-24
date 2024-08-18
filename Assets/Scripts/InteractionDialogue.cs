using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionDialogue : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private Transform speechBubble;
    
    [Header("Dialogue")]
    [SerializeField] private DialogueMessage[] dialogues;
    private int dialogueIndex;

    [Header("Dialogue Settings")]
    [SerializeField] private float speechSpeed;
    [SerializeField] private AudioClip speechSound;
    
    public void StartInteraction()
    {
        dialogueIndex = 0;
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        DialogueMessage dialogueMessage = dialogues[dialogueIndex];
        
        speechBubble.gameObject.SetActive(true);
        displayText.text = "";
        //Display text
        displayText.text = dialogueMessage.message;

        if (dialogues.Length >= dialogueIndex) yield break;
        if (dialogueMessage.postMessageDelay > 0) yield return HelperFunctions.GetWait(dialogueMessage.postMessageDelay);
        
        dialogueIndex++;
        StartCoroutine(DisplayDialogue());
    }
}

[Serializable]
class DialogueMessage
{
    public string message;
    public float postMessageDelay;
} 