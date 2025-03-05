using UnityEngine;
using System.Collections.Generic;

public class MarkerManager : MonoBehaviour
{
    public List<GameObject> markers = new List<GameObject>();
    public int currentZoom = 15; // Используем целочисленные уровни масштаба

    private Dictionary<GameObject, Vector3> originalWorldPositions = new Dictionary<GameObject, Vector3>();

    public void SetZoom(int newZoom)
    {
        if (currentZoom == newZoom || !ZoomLevels.MetersPerPixel.ContainsKey(newZoom)) return;

        // Рассчитываем коэффициент изменения масштаба
        float scaleFactor = ZoomLevels.MetersPerPixel[currentZoom] / ZoomLevels.MetersPerPixel[newZoom];
        UpdateMarkerPositions(scaleFactor);
        currentZoom = newZoom;
    }

    private void UpdateMarkerPositions(float scaleFactor)
    {
        foreach (var marker in markers)
        {
            if (originalWorldPositions.ContainsKey(marker))
            {
                // Пересчитываем позицию относительно нового масштаба
                marker.transform.position = originalWorldPositions[marker] * scaleFactor;
            }
        }
    }

    public void AddMarker(GameObject marker)
    {
        markers.Add(marker);
        originalWorldPositions[marker] = marker.transform.position;
    }

    public void RemoveMarker(GameObject marker)
    {
        markers.Remove(marker);
        originalWorldPositions.Remove(marker);
        Destroy(marker);
    }
}