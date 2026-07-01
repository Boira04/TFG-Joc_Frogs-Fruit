// IDamageable.cs
// Interficie que implementen tots els objectes que tenen barra de vida (EnemyBehaviour, FinalBossController)
// Permet que HealthBarBehaviour funcioni amb qualsevol tipus d'enemic sense acoblament directe
public interface IDamageable
{
    float GetHitPoints();
    float GetMaxHitPoints();
}