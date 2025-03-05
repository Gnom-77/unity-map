using UnityEngine;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public GameObject tilePrefab;
    public Camera mainCamera;
    public float tileSpacing = 1.0f;
    public int extraTileBorder = 1; // Number of extra tiles to generate around the visible area

    private Dictionary<Vector2Int, GameObject> activeTiles = new Dictionary<Vector2Int, GameObject>();
    private Vector3 lastCameraPosition;
    public MarkerManager markerManager;
    public int zoom = 4;

    void Start()
    {
        lastCameraPosition = mainCamera.transform.position;
        GenerateGrid();
    }

    void Update()
    {
        if (Vector3.Distance(mainCamera.transform.position, lastCameraPosition) > tileSpacing * 0.5f)
        {
            lastCameraPosition = mainCamera.transform.position;
            GenerateGrid();
        }
    }

    // Добавьте метод для изменения масштаба
    public void SetZoomLevel(int newZoom)
    {
        // Обновляем все тайлы
        foreach (var tile in activeTiles.Values)
        {
            tile.GetComponent<Tile>().UpdateTile(newZoom);
        }
        zoom = newZoom;
        // Обновляем маркеры
        markerManager.SetZoom(newZoom);
    }

    void GenerateGrid()
    {
        Vector3[] cameraCorners = GetCameraCorners();
        HashSet<Vector2Int> requiredTiles = CalculateRequiredTiles(cameraCorners);

        CleanupUnusedTiles(requiredTiles);
        CreateNewTiles(requiredTiles);
    }

    Vector3[] GetCameraCorners()
    {
        Vector3[] corners = new Vector3[4];
        float depth = mainCamera.nearClipPlane; // Use near clip plane for depth

        corners[0] = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, depth));
        corners[1] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, depth));
        corners[2] = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, depth));
        corners[3] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));

        return corners;
    }

    HashSet<Vector2Int> CalculateRequiredTiles(Vector3[] corners)
    {
        HashSet<Vector2Int> tiles = new HashSet<Vector2Int>();

        // Calculate the min and max tile positions based on the camera corners
        int minX = Mathf.FloorToInt(corners[0].x / tileSpacing) - extraTileBorder;
        int maxX = Mathf.CeilToInt(corners[1].x / tileSpacing) + extraTileBorder;
        int minY = Mathf.FloorToInt(corners[2].y / tileSpacing) - extraTileBorder;
        int maxY = Mathf.CeilToInt(corners[3].y / tileSpacing) + extraTileBorder;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                tiles.Add(new Vector2Int(x, y));
            }
        }

        return tiles;
    }

    Vector2Int WorldToTilePosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / tileSpacing);
        int y = Mathf.FloorToInt(worldPosition.y / tileSpacing);
        return new Vector2Int(x, y);
    }

    void CleanupUnusedTiles(HashSet<Vector2Int> requiredTiles)
    {
        List<Vector2Int> tilesToRemove = new List<Vector2Int>();

        foreach (var tile in activeTiles)
        {
            if (!requiredTiles.Contains(tile.Key))
            {
                Destroy(tile.Value);
                tilesToRemove.Add(tile.Key);
            }
        }

        foreach (var key in tilesToRemove)
        {
            activeTiles.Remove(key);
        }
    }

    void CreateNewTiles(HashSet<Vector2Int> requiredTiles)
    {
        foreach (Vector2Int tilePos in requiredTiles)
        {
            if (!activeTiles.ContainsKey(tilePos))
            {
                Vector3 position = new Vector3(
                    tilePos.x * tileSpacing,
                    tilePos.y * tileSpacing,
                    0
                );

                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                newTile.GetComponent<Tile>().UpdateTile(zoom);
                activeTiles.Add(tilePos, newTile);
            }
        }
    }
}
