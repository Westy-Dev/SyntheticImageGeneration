using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Perception.Randomization.Randomizers.Utilities;
using UnityEngine.Perception.Randomization.Samplers;

[Serializable]
[UnityEngine.Perception.Randomization.Randomizers.AddRandomizerMenu("My Randomizers/Cluster Randomizer")]
public class ClusterRandomizer : Randomizer
{
    /// <summary>
    /// The Z offset component applied to the generated layer of GameObjects
    /// </summary>
    [Tooltip("The Z offset applied to positions of all placed objects.")]
    public float Depth;

    /// <summary>
    /// The minimum distance between all placed objects
    /// </summary>
    [Tooltip("The minimum distance between the centers of the placed objects.")]
    public float SeparationDistance = 2f;

    /// <summary>
    /// The size of the 2D area designated for object placement
    /// </summary>
    [Tooltip("The width and height of the area in which objects will be placed. These should be positive numbers and sufficiently large in relation with the Separation Distance specified.")]
    public Vector2 PlacementArea;

    /// <summary>
    /// The list of prefabs clusters to sample and randomly place
    /// </summary>
    [Tooltip("The list of Prefabs to be placed by this Randomizer.")]
    public PrefabCluster[] Clusters;

    private GameObject Container;
    private GameObjectOneWayCache GameObjectOneWayCache;

    /// <inheritdoc/>
    protected override void OnAwake()
    {
        Container = new GameObject("Foreground Objects");
        Container.transform.parent = scenario.transform;
    }

    protected override void OnIterationStart()
    {
        var seed = SamplerState.NextRandomState();
        var placementSamples = PoissonDiskSampling.GenerateSamples(
            PlacementArea.x, PlacementArea.y, SeparationDistance, seed);
        var offset = new Vector3(PlacementArea.x, PlacementArea.y, 0f) * -0.5f;


        //select a random prefab from each cluster
        foreach (var cluster in Clusters)
        {
            GameObjectOneWayCache = new GameObjectOneWayCache(
            Container.transform, cluster.clusterPrefabs.categories.Select(element => element.Item1).ToArray());

            var prefab = cluster.clusterPrefabs.Sample();

            var instance = GameObjectOneWayCache.GetOrInstantiate(prefab);

            System.Random rnd = new System.Random();

            int r = rnd.Next(placementSamples.Length);

            instance.transform.position = new Vector3(placementSamples[r].x, placementSamples[r].y, Depth) + offset;
            //do things with this prefab, e.g. create instances of it, etc. 
        }

        placementSamples.Dispose();
    }

    /// <summary>
    /// Deletes generated foreground objects after each scenario iteration is complete
    /// </summary>
    protected override void OnIterationEnd()
    {
        GameObjectOneWayCache.ResetAllObjects();
    }
}