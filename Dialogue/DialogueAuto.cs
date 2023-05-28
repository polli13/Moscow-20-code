using UnityEngine;

public class DialogueAuto : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenReplics = 3f;

    [SerializeField]
    private Dialogue dialogue;

    [SerializeField]
    private string npcName;

    [SerializeField]
    private SoundsPlayer headSource;

    [SerializeField]
    private AudioClip[] npcVoice;

    [SerializeField]
    private UnityEngine.Events.UnityEvent endAction;


    private void OnEnable()
    {
        DialogueManager.npcVoicePlay += PlayVoice;
    }

    private void OnDisable()
    {
        DialogueManager.npcVoicePlay -= PlayVoice;
    }

    public void StartAutoDialogue()
    {
        DialogueManager.m_DialogueManager.GetAutoReplicsTime(timeBetweenReplics, this);
        DialogueManager.m_DialogueManager.GetCurrentText(dialogue, npcName, true);
    }

    public void CheckEndAction()
    {
        endAction?.Invoke();
        DialogueManager.npcVoicePlay -= PlayVoice;
    }

    private void PlayVoice()
    {
        headSource.PlaySound(GetVoice());
    }

    public AudioClip GetVoice()
    {
        var _rand = npcVoice[Random.Range(0, npcVoice.Length)];
        return _rand;
    }
}
