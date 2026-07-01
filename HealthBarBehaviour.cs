// HealthBarBehaviour.cs
// Barra de vida que es mostra sobre qualsevol enemic que implementi IDamageable
// Compatible amb EnemyBehaviour i FinalBossController gràcies a la interfície IDamageable
using UnityEngine;
using UnityEngine.UI;
public class HealthBarBehaviour : MonoBehaviour
{
    public Image healthBarFill;
    public Image healthBarBackground;
    private IDamageable target;
    private float maxHitPoints;

    void Start()
    {
        target = GetComponentInParent<IDamageable>();
        if (target == null) { enabled = false; return; }
        maxHitPoints = target.GetMaxHitPoints();
        SetVisible(false);
    }

    void Update()
    {
        if (target == null) return;
        float ratio = target.GetHitPoints() / maxHitPoints;
        healthBarFill.rectTransform.localScale = new Vector3(ratio, 1f, 1f);
        healthBarFill.color = Color.Lerp(Color.red, Color.green, ratio);
        // Contraresta el flip horitzontal de lenemic perque la barra sempre es vegi be
        Transform parentScale = transform.parent;
        transform.localScale = new Vector3(0.01f / parentScale.localScale.x, 0.01f, 0.01f);
    }

    public void ShowBar() => SetVisible(true);
    void SetVisible(bool visible) { healthBarFill.enabled = visible; healthBarBackground.enabled = visible; }
}