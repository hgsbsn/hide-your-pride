using UnityEngine;
using System.Linq;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class NPCDistanceManager : MonoBehaviour
{
    public Volume volume;
    public Vignette vignette;

    public float vignetteMinDistance = 5f;
    public float vignetteMaxDistance = 100f;
    public float vignetteMaxIntensity = 0.8f;

    private void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    void Update()
    {
        RandomNPCMovement[] npcs = FindObjectsByType<RandomNPCMovement>(FindObjectsSortMode.None);
        if (npcs.Length == 0) return;

        float closestDistance = npcs.Min(npc => npc.playerDistance);
        //Debug.Log(closestDistance);

        // Clamp and normalize distance to 0–1, inverted
        float normalized = Mathf.InverseLerp(vignetteMaxDistance, vignetteMinDistance, closestDistance);

        // Apply to vignette intensity
        vignette.intensity.Override(normalized * vignetteMaxIntensity);
    }
}