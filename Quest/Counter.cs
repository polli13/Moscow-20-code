using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField]
    private int currentCount;
    [SerializeField]
    private int needCount;

    [SerializeField]
    private ActionEvent aEvent;

    public void PlusCount()
    {
        currentCount++;
        if(needCount == currentCount)
        {
            aEvent.CallEvent();
        }
    }
}
