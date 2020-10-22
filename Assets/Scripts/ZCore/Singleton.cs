/// <summary>单例实现    classA:Singleton<classA>  </summary>
public class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null) _instance = new T();

            return _instance;
        }
    }
}
