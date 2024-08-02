public interface IPoolObject
{
    public PoolObjectType ObjectType { get; }
    public void ChangeLine(int line);
    public void TeleportToPosition();
    public void ActionHandler();
    public bool ActionDone();
}

public enum PoolObjectType
{
    ObstacleType1 = 0,
    ObstacleType2 = 1,
    EnemyType1 = 2,
    EnemyType2 = 3,
    FearEssence = 4,
    Immateriality = 5,
    SlowMotion = 6,
    DarkCloud = 7,
    LightsOff = 8,
    PhantomOfTheOpera = 9,
    TownLegend = 10
}

