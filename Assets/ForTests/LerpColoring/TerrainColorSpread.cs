using UnityEngine;

public class TerrainColorSpread : MonoBehaviour
{
    public Material terrainMaterial; // Материал террейна
    public Vector3 spreadOrigin;    // Точка старта распространения, которую можно задать в инспекторе
    public float spreadSpeed = 5f;  // Скорость распространения
    private float spreadRadius = 0f; // Начальный радиус распространения
    public Terrain terrain; // Ссылка на террейн

    void Update()
    {
        if (terrainMaterial && terrain)
        {
            // Увеличиваем радиус с течением времени
            spreadRadius += spreadSpeed * Time.deltaTime;

            // Преобразуем мировые координаты точки в UV-координаты на террейне
            Vector3 terrainPosition = spreadOrigin - terrain.transform.position;
            Vector2 uvPosition = new Vector2(terrainPosition.x / terrain.terrainData.size.x, terrainPosition.z / terrain.terrainData.size.z);

            // Передаем значения в шейдер
            terrainMaterial.SetFloat("_SpreadRadius", spreadRadius);
            terrainMaterial.SetVector("_SpreadCenter", new Vector4(uvPosition.x, uvPosition.y, 0, 0));
        }
    }
}
