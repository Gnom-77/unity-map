using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Tile : MonoBehaviour
{
    public int zoom = 15;
    [SerializeField] private float tileSizeWorld = 1f; // Размер тайла в мировых координатах
    [SerializeField] string globalUrl;

    //private void Start()
    //{
    //    StartCoroutine(LoadTileTexture(transform.position.x, -transform.position.y));
    //}


    private IEnumerator LoadTileTexture(float tileX, float tileY)
    {
        string url = $"https://tile.openstreetmap.org/{zoom}/{tileX}/{tileY}.png";
        globalUrl = url;
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {   
            www.SetRequestHeader("User-Agent", "YourAppName/1.0 (your@email.com)");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Tile load error: {www.error}");
            }
            else
            {
                ApplyTexture(DownloadHandlerTexture.GetContent(www));
            }
        }
    }

    public void UpdateTile(int newZoom)
    {
        zoom = newZoom;
        StartCoroutine(LoadTileTexture(transform.position.x, -transform.position.y));
    }

    private void ApplyTexture(Texture2D texture)
    {
        GetComponent<Renderer>().material.mainTexture = texture;
        // Корректировка UV-координат для Quad
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 1);

        // Масштабирование Quad под мировые координаты
        float aspect = (float)texture.width / texture.height;
        transform.localScale = new Vector3(
            tileSizeWorld * aspect,
            tileSizeWorld,
            1
        );
    }
}