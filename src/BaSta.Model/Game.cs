using System;

namespace BaSta.Model;

public interface Game
{
    public Possession? Possession { get; }

    public TimeSpan? GameTime { get; }

    public TimeSpan? TimeSpan { get; }
}

public enum Possession
{
    Home,
    Guest
}

public enum Period
{
    Q1,
    Q2,
    Q3,
    Q4,
    E
}