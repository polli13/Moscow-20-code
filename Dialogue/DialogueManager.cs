using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogueText;
    private string talkingNameText;
    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private string playerName;

    private Dialogue currentDialogue;
    private string currentNPCName;

    [SerializeField]
    private float typingTextDelay;
    private bool typingReplic = false;
    private string currentDialogueText;
    private int currentNodeIndex = 0;
    private Node currentNode;

    private WaitForSeconds waitSec => new WaitForSeconds(typingTextDelay);
    private WaitForSeconds waitAutoSec;
    private Color newCol = new Color(0.8352942f, 0.7803922f, 0.6470588f, 1);
    private string colorText;

    private bool autoReplics = false;
    private float timeAutoReplics;
    private DialogueAuto auto;

    [SerializeField]
    private ActionEvent endAction;

    [SerializeField]
    private SoundsPlayer playerHeadSource;

    public static System.Action npcVoicePlay;

    #region Singleton
    public static DialogueManager m_DialogueManager { get; set; }
    private void Awake()
    {
        if (m_DialogueManager == null)
            m_DialogueManager = this;

        m_DialogueManager = GetComponent<DialogueManager>();

        colorText = "<color=#" + ColorUtility.ToHtmlStringRGBA(newCol) + ">";
    }
    #endregion

    public void GetCurrentText(Dialogue _dialogue, string _npcName, bool _auto)
    {
        if (currentDialogue != null && currentDialogue == _dialogue) return;

        currentNodeIndex = 0;
        currentDialogue = _dialogue;
        currentNPCName = _npcName;
        autoReplics = _auto;
        StartReplic();
    }
    
    public void GetCurrentVoice(System.Action _action)
    {
        npcVoicePlay = _action;
    }

    public void GetAutoReplicsTime(float _time, DialogueAuto _auto)
    {
        timeAutoReplics = _time;
        waitAutoSec = new WaitForSeconds(timeAutoReplics);
        auto = _auto;
    }

    public void StartReplic()
    {
        currentNode = currentDialogue.nodes[currentNodeIndex];

        if (currentNode.isPlayer)
        {
            talkingNameText = playerName;
            playerHeadSource.PlaySound(currentNode.voice);
        }
        else
        {
            if (string.IsNullOrEmpty(currentNPCName))
            {
                talkingNameText = currentNode.npcName;
            }
            else
            {
                talkingNameText = currentNPCName;
            }

            npcVoicePlay?.Invoke();
        }

        OpenClosePanel(true);
        typingReplic = true;

        StartCoroutine(DisplayString(currentNode.npcText, dialogueText));
    }

    public void TypeSomeText(TMP_Text txt, string _text) 
    {
        StartCoroutine(DisplayString(_text, txt));
    }

    public void NextButton()
    {
        if (typingReplic)
        {
            StopAllCoroutines();
            var _textName = string.IsNullOrEmpty(talkingNameText) ? string.Empty : colorText + talkingNameText + ":</color> ";
            dialogueText.text = _textName + currentNode.npcText;
            typingReplic = false;
        }
        else
        {
            if (currentNode.isExit)
            {
                playerTalkAnim = false;
                autoReplics = false;
                OpenClosePanel(false);
                return;
            }

            NextNode();
        }
    }

    public void ChangeTypingSpeed(float _delay)
    {
        typingTextDelay = _delay;
    }

    public void NextNode()
    {
        currentNodeIndex++;
        StartReplic();
    }

    private void OpenClosePanel(bool _open)
    {
        dialoguePanel.SetActive(_open);

        if (!_open)     //exit
        {
            endAction?.CallEvent();
            if (auto) auto.CheckEndAction();
            currentDialogue = null;
        }
    }

    IEnumerator DisplayString(string stringToDisplay, TMP_Text txt)
    {
        for (int i = 0; i <= stringToDisplay.Length; i++)
        {
            currentDialogueText = stringToDisplay.Substring(0, i);

            var _textName = string.IsNullOrEmpty(talkingNameText) ? string.Empty : colorText + talkingNameText + ":</color> ";
            txt.text = _textName + currentDialogueText;
            yield return waitSec;
        }

        if (currentDialogueText.Length == stringToDisplay.Length)
        {
            typingReplic = false;

            if (autoReplics)
            {
                yield return waitAutoSec;
                NextButton();
            }
        }
    }
}