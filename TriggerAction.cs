using UnityEngine;

public class TriggerAction : MonoBehaviour
{
    [SerializeField]
    private ActionEvent actioneEvent;

    private void Start()
    {
        actioneEvent = GetComponent<ActionEvent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(StaticStrings.PlayerTag))
            actioneEvent?.CallEvent();
    }
}
