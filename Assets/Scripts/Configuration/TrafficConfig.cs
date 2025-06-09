[System.Serializable]
public class GameConfig
{
    public Level[] levels;
}

[System.Serializable]
public class Level
{
    public string levelName;
    public LevelConfig levelConfig;
}

[System.Serializable]
public class LevelConfig
{
    public Lane[] lanes;
    public StaticObstacle[] staticObstacles;
}

[System.Serializable]
public class Lane
{
    public string name;
    public float positionX;
    public string spawner;
    public string direction;
}

[System.Serializable]
public class StaticObstacle
{
    public string name;
    public float positionX;
}