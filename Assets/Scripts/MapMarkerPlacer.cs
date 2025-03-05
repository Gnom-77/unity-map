using UnityEngine;

public class MapMarkerPlacer : MonoBehaviour
{
    public GameObject markerPrefab; // ��������� ��� ������ ������� � ����������
    public MarkerManager markerManager; // ������ �� MarkerManager

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ����������� ������� ����� ������ ����
        {
            PlaceMarker();
        }
    }

    void PlaceMarker()
    {
        // ������� ��� �� ������ � ����������� �������
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ���������, ���������� �� ��� �����-���� ������
        if (Physics.Raycast(ray, out hit))
        {
            // ��������� ������ �� �����������, � ������� ��������� �����������
            Vector3 markerPosition = hit.point;
            // ������������ ������ �� 90 �������� ������ ��� X
            Quaternion rotation = Quaternion.Euler(90, 0, 0);

            // ������������ ������ � �������� �������� � ���������
            GameObject newMarker = Instantiate(markerPrefab, markerPosition, rotation);
            markerManager.AddMarker(newMarker);
        }
    }
}
