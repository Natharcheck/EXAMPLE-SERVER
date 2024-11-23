namespace Server.Room.Componets;

public class UHealth(short health, short maxHealth, short rigen)
{
    private short _maxHealth = maxHealth;
    private short _health = health;
    private short _rigen = rigen;

    public bool IsLive = true;

    public delegate void WhenMiddleHealth();
    public delegate void WhenMinimumHealth();
    public delegate void WhenMaximumHealth();

    public event WhenMiddleHealth? MiddleHealth;
    public event WhenMinimumHealth? MinimumHealth;
    public event WhenMaximumHealth? MaximumHealth;

    public short Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= 0)
            {
                IsLive = false;
                _health = 0;

                OnMinimumHealth();
            }

            if (IsLive)
            {
                if (_health == _maxHealth - 1)
                {
                    OnMiddleHealth();
                }

                if (_health >= _maxHealth)
                {
                    _health = _maxHealth;
                    OnMaximumHealth();
                }
            }
        }
    }

    public short MaxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    public short Rigen
    {
        get => _rigen;
        set
        {
            if (IsLive)
            {
                if (value < 5000) _rigen = value;   
            }
        }
    }

    private void OnMinimumHealth()
    {
        MinimumHealth?.Invoke();
    }

    private void OnMaximumHealth()
    {
        MaximumHealth?.Invoke();
    }

    private void OnMiddleHealth()
    {
        MiddleHealth?.Invoke();
    }
}