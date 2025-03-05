using UnityEngine;

public class MapMarkerPlacer : MonoBehaviour
{
    public GameObject markerPrefab; // Назначьте ваш префаб маркера в инспекторе
    public MarkerManager markerManager; // Ссылка на MarkerManager

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Обнаружение нажатия левой кнопки мыши
        {
            PlaceMarker();
        }
    }

    void PlaceMarker()
    {
        // Создаем луч из камеры в направлении курсора
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Проверяем, пересекает ли луч какой-либо объект
        if (Physics.Raycast(ray, out hit))
        {
            // Размещаем маркер на поверхности, с которой произошло пересечение
            Vector3 markerPosition = hit.point;
            // Поворачиваем маркер на 90 градусов вокруг оси X
            Quaternion rotation = Quaternion.Euler(90, 0, 0);

            // Инстанцируем маркер с заданной позицией и поворотом
            GameObject newMarker = Instantiate(markerPrefab, markerPosition, rotation);
            markerManager.AddMarker(newMarker);
        }
    }
}
