using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneSo")]
public class GameSceneSo : ScriptableObject
{
    public Scenetype sceneType;
    public AssetReference sceneReference;
}
