public interface IPoolObject
{
    public PoolObjectType ObjectType { get; }
    public void ChangeLine(int line);
    public void TeleportToPosition();
}

public enum PoolObjectType
{
    ObstacleType1 = 0,
    ObstacleType2 = 1,
    EnemyType1 = 2,
    EnemyType2 = 3
}

