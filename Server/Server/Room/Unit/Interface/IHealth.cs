using Server.Room.Componets;

namespace Server.Room.Interface;

public interface IHealth
{
    public UHealth? HealthComponent { get; set; }

    public void TackDamage(short damage);
    public void TackHealing(short healing);
    public void Knockdown();

    public void OnDead();
}