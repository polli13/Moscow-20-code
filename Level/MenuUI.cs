using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject menuPanel;

    [SerializeField]
    private GameObject diaryPanel;

    [SerializeField]
    private GameObject pickUpText;

    [SerializeField]
    private TMP_Text examineText;

    [SerializeField]
    private TMP_Text questTargetText;

    [SerializeField]
    private AudioSource noteSounds;

    [SerializeField]
    private Animation noteTextAnim;

    [SerializeField]
    private NotesManager m_Notes;

    private Book m_Book;

    #region Singleton
    public static MenuUI m_MenuUI { get; set; }
    private void Awake()
    {
        if (m_MenuUI == null)
            m_MenuUI = this;

        m_Book = FindObjectOfType<Book>();
    }
    #endregion

    public void OpenCloseMenu(bool _open)
    {
        menuPanel.SetActive(_open);
        InputHandler.m_InputHandler.PauseGame(_open);
    }

    public void OpenCloseDiary(bool _open)
    {
        diaryPanel.SetActive(_open);
        InputHandler.m_InputHandler.PauseGame(_open);
    }

    public void ItemText(string _txt)
    {
        examineText.text = _txt;
        examineText.gameObject.SetActive(true);
    }

    public void HideItemText()
    {
        examineText.gameObject.SetActive(false);
    }

    public void ShowPickupText(bool _show)
    {
        pickUpText.SetActive(_show);
    }

    public void AddNoteText(int _contentPart)
    {
        noteTextAnim.Play();
        noteSounds.Play();

        var _node = m_Notes.contentPart[_contentPart];
        m_Book.AddContent(_node);
    }

    #region Quest
    public void ShowTargetQuest(bool _show)
    {
        questTargetText.gameObject.SetActive(_show);
    }

    public void UpdateQuestUI(string _description)
    {
        questTargetText.text = _description;
    }
    #endregion
}
