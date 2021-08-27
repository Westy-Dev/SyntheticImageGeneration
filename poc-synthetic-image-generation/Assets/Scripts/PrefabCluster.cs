using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;

[CreateAssetMenu(fileName = "NewPrefabCluster", menuName = "Test/PrefabCluster")]
public class PrefabCluster : ScriptableObject
{
    public GameObjectParameter clusterPrefabs;
}