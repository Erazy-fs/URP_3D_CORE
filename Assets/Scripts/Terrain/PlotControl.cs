using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class PlotControl : MonoBehaviour {
    public  int colorIndex;
    public  float waveDuratation;
    public  int waveCount;
    public  float waveHeight = .05f;
    public  Color startColor = Color.red;
    private Color nextColor  = new(0, 0, 255);
    public  bool doWave      = true;
    public float maxRadius;

    private int waveIndex = 0;
    private bool isUpdating = false;
    public bool plotUpdating {get=>isUpdating;}
    private float elapsedTime;
    private Mesh mesh;
    private int[] sortVertices = new int[]{};
    private Dictionary<int, float> vertexDistances = new();
    private Dictionary<int, float> vertexNoise     = new();
    private Vector3[] vertices;
    public Action OnReady;
    public bool isReady = false;

    void Start() {
        isUpdating = false;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        Debug.Log($"{transform.parent.name} x{transform.parent.localScale.x} y{transform.parent.localScale.y} z{transform.parent.localScale.z}");
        waveHeight /= transform.parent.localScale.y;
        vertices = mesh.vertices;
        OnReady?.Invoke();
        isReady = true;
    }
    
    public void SetStartParams(Color color) {
        startColor = color;
        var baseColors = new Color[mesh.vertexCount];
        for (int i = 0; i < baseColors.Length; i++)
            baseColors[i] = startColor;
        mesh.colors = baseColors;
    }

    
    void Update() {
        CalculateWave();
    }

    public float HitPosition(Vector3 position) {
        if (isUpdating) return 0;
        vertexDistances = new Dictionary<int, float>();
        vertexNoise     = new Dictionary<int, float>();
        for (int i = 0; i < mesh.vertexCount; i++) {
            var vertexWorldPosition = transform.TransformPoint(mesh.vertices[i]);
            vertexDistances[i] = Vector3.Distance(vertexWorldPosition, position);
            if (doWave) vertexNoise[i] = Mathf.PerlinNoise(mesh.vertices[i].x*150, mesh.vertices[i].z*150);
        }

        sortVertices = vertexDistances.Keys.ToArray();
        Array.Sort(sortVertices, (a,b) => vertexDistances[a] > vertexDistances[b] ? 1 : (vertexDistances[a] < vertexDistances[b] ? -1 : 0));

        return vertexDistances[sortVertices[mesh.vertexCount-1]];
    }

    private int lastIndex;
    private Color prevWaveColor;
    public void StartWaves(Color color, int waves, int nextColorIndex, float duratation, float radius) {
        nextColor      = color;
        waveCount      = waves;
        colorIndex     = nextColorIndex;
        waveDuratation = duratation;
        maxRadius      = radius;
        waveIndex      = 1;
        elapsedTime    = 0;
        lastIndex      = 0;
        prevTime       = 0;
        prevWaveColor  = startColor;
        isUpdating     = true;
    }

    private float prevTime;
    private void CalculateWave(){
        if (!isUpdating) return;
        elapsedTime += Time.deltaTime;
        if (elapsedTime - prevTime < .015f) return;
        prevTime = elapsedTime;

        var newColors = mesh.colors;

        float t = Mathf.Clamp01(elapsedTime / waveDuratation);
        if (t >= 1) {   //Следующая волна
            prevWaveColor = Color.Lerp(startColor, nextColor, (float)waveIndex/(float)waveCount);
            waveIndex++;
            prevTime = 0;
            elapsedTime = 0;
            lastIndex   = 0;
            t           = 0;
            if (waveIndex > waveCount) {
                isUpdating = false;
                startColor = nextColor;
                for (var i=0; i<mesh.vertexCount; i++) newColors[i] = nextColor;
                mesh.colors = newColors;
                if (doWave) {
                    mesh.vertices = vertices;
                    mesh.RecalculateNormals();
                }
                return;
            }
        }
        var waveColor = Color.Lerp(startColor, nextColor, (float)waveIndex/(float)waveCount);

        var waveStart = maxRadius * t;
        var step      = maxRadius * .2f * t;
        var halfStep  = step*.5f;
        var waveEnd   = waveStart + step;
        int nextLastIndex = 0;

        var newVertices = doWave ? mesh.vertices : null;
        for (var i=lastIndex; i<mesh.vertexCount; i++){
            var j = sortVertices[i];
            var d = vertexDistances[j];
            if (d < waveStart) {
                newColors  [j]   = waveColor;       //Цвет волны
                if (doWave) newVertices[j].y = vertices[j].y;   //Возврат высоты
                nextLastIndex = i;      
            }
            else if (d <= waveEnd) {
                var grad = Mathf.Clamp01(1 - (d - waveStart) / step);             //Градиент волны
                var wave = 1 - Mathf.Abs(waveStart+halfStep - d)/halfStep;  //Волна aAa
                // var wave = Mathf.Abs(waveStart+step*.5f - d);   //Обратная волна AaA
                
                newColors[j] = Color.Lerp(prevWaveColor, waveColor, grad);              //Цвет
                if (doWave) newVertices[j].y = vertices[j].y + waveHeight * wave * vertexNoise[j];  //Волна
            } else break;
        }
        lastIndex = nextLastIndex;

        mesh.colors   = newColors;
        if (doWave) {
            mesh.vertices = newVertices;
            mesh.RecalculateNormals();
        }
    }

}