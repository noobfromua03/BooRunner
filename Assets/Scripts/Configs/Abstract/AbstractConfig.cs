using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public abstract class AbstractConfig<TConfig> : ScriptableObject where TConfig : AbstractConfig<TConfig>
{
    #region Singleton
    private readonly static string AssetName = typeof(TConfig).Name;

    private static TConfig _instance;
    public static TConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load("Configs/" + AssetName) as TConfig;

#if UNITY_EDITOR
                if (_instance == null)
                {
                    #region Create And Save in Assets
                    _instance = CreateInstance<TConfig>();
                    AssetDatabase.CreateAsset(_instance, "Assets/Resources/Configs/" + AssetName + ".asset");
                    AssetDatabase.SaveAssets();
                    #endregion
                }
#endif
            }
            return _instance;
        }
    }
    #endregion
}
