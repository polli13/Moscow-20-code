using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue File")]
public class Dialogue : ScriptableObject
{
    public Node[] nodes;
}

[System.Serializable]
public struct Node 
{
    [TextArea(3, 5)]
    public string npcText;      //����� �������
    public bool isPlayer;       //���� �� ��, �� ��� ��� ��������
    public bool isExit;         //�����
    public bool action;
    public int actionID;
    public string npcName;
    public AudioClip voice;
}

