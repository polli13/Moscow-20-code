using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private PlayerEffects effects;

    private void OnTriggerEnter(Collider other)
    {
        var _enemy = other.GetComponent<EnemyAI>();
        if (_enemy != null)
        {
            _enemy.GetDamage();
            effects.LightAttackEffect();
        }   
        else if (other.GetComponent<ExamineObject>())
        {
            ExamineSystem.m_ExamineSystem.currentPickUp = other.GetComponent<ExamineObject>();
            MenuUI.m_MenuUI.ShowPickupText(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ExamineObject>())
        {
            ExamineSystem.m_ExamineSystem.currentPickUp = null;
            MenuUI.m_MenuUI.ShowPickupText(false);
        } 
    }
}
