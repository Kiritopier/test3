/// <summary>
/// Generic Mono singleton.
/// </summary>
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    public static bool IsClosing = false;
    private static T m_Instance = null;

    public static T _Instance
    {
        get
        {
            if (IsClosing)
            {
                return default(T);
            }
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType<T>();
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("Singleton of " + typeof(T).ToString()).AddComponent<T>();

                }
                m_Instance.Init();
            }
            return m_Instance;
        }
    }

    private void Awake()
    {

        if (m_Instance == null)
        {
            m_Instance = _Instance;
        }
    }

    public virtual void Init() { }


    protected virtual void OnApplicationQuit()
    {
        IsClosing = true;
        m_Instance = null;
    }
}