using UnityEngine;

[CreateAssetMenu(fileName = "NotesManager")]
public class NotesManager : ScriptableObject
{
    [TextArea(5, 10)]
    public string[] contentPart;
}
