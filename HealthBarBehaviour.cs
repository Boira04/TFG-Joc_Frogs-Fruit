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
        maxHitPoints = target.GetMaxHitPoints();
        healthBarFill.sprite = null;
        SetVisible(false);
    }

    void Update()
    {
        float ratio = target.GetHitPoints() / maxHitPoints;
        healthBarFill.rectTransform.localScale = new Vector3(ratio, 1f, 1f);
        healthBarFill.color = Color.Lerp(Color.red, Color.green, ratio);

        Transform parentScale = transform.parent;
        transform.localScale = new Vector3(0.01f / parentScale.localScale.x, 0.01f, 0.01f);
    }

    public void ShowBar()
    {
        SetVisible(true);
    }

    void SetVisible(bool visible)
    {
        healthBarFill.enabled = visible;
        healthBarBackground.enabled = visible;
    }
}