using UnityEngine;
using UnityEngine.Events;

public class ActionEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_Action;

    [SerializeField]
    private bool callOnStart;

    [SerializeField]
    private float timer;

    private void Start()
    {
        if (callOnStart)
        {
            if(timer == 0)
                CallEvent();
            else
                Invoke(nameof(CallEvent), timer);
        }
    }

    public void CallEvent()
    {
        m_Action?.Invoke();
    }
}
