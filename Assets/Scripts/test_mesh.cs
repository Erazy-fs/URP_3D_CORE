using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil.Cil;
using Unity.Collections;
using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class GradualColorChange : MonoBehaviour
{

    public int duration = 3;
    public int hitCount  = 3;
    public  Color startColor = new(255, 0, 0);
    private Color nextColor  = new(0, 0, 255);

    private int hitStep = 0;
    private bool isUpdating;
    private float elapsedTime;
    private Mesh mesh;
    private Color[] baseColors;
    private int[] sortVertices = new int[]{};
    private Dictionary<int, float> vertDistances = new();
    private Dictionary<int, float> vertNoise     = new();

    void Start() {
        isUpdating = false;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;

        //Стартовый цвет
        baseColors = new Color[mesh.vertexCount];
        for (int i = 0; i < baseColors.Length; i++)
            baseColors[i] = startColor;
        mesh.colors = baseColors;
    }

    private int lastIndex;
    private Color prevHitColor = Color.white;

    void Update() {

        if (!isUpdating && Input.GetMouseButtonDown(0)) {  //ЛКМ

            Debug.Log($"1 {transform.name}");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider != null && hit.collider.gameObject == gameObject) {
                    var clickPosition = hit.point;

                    elapsedTime    = 0;
                    lastIndex      = 0;
                    isUpdating     = true;
                    hitStep = 1;
                    prevHitColor = startColor;

                    vertDistances = new Dictionary<int, float>();
                    vertNoise     = new Dictionary<int, float>();
                    for (int i = 0; i < mesh.vertexCount; i++) {
                        var vertexWorldPosition = transform.TransformPoint(mesh.vertices[i]);
                        vertDistances[i] = Vector3.Distance(vertexWorldPosition, clickPosition);
                        vertNoise[i] = Mathf.PerlinNoise(mesh.vertices[i].x*100, mesh.vertices[i].y*100);
                    }

                    sortVertices = vertDistances.Keys.ToArray();
                    Array.Sort(sortVertices, (a,b) => vertDistances[a] > vertDistances[b] ? 1 : (vertDistances[a] < vertDistances[b] ? -1 : 0));

                    nextColor = UnityEngine.Random.ColorHSV();
                }
            }
        }

        if (isUpdating) {

            elapsedTime += Time.deltaTime;
            var newColors = mesh.colors;

            var hitColor = startColor;
            float t = Mathf.Clamp01(elapsedTime / duration);
            if (t>=1){
                prevHitColor = Color.Lerp(startColor, nextColor, (float)hitStep/(float)hitCount);
                hitStep++;
                elapsedTime = 0;
                lastIndex = 0;
                if (hitStep >= hitCount) {
                    isUpdating = false;
                    startColor = nextColor;
                    Debug.Log("Stop");
                }
                Debug.Log($"step={hitStep}/{hitCount} ({(float)hitStep/(float)hitCount}) === {Color.Lerp(startColor, nextColor, hitStep/hitCount)} === {startColor} === {nextColor}");
            }
            hitColor = Color.Lerp(startColor, nextColor, (float)hitStep/(float)hitCount);

            int nextIndex = Mathf.RoundToInt(mesh.vertexCount * t);

            
            {
                var fullRadius    = vertDistances[sortVertices[mesh.vertexCount-1]];
                var cur_radius    = fullRadius * t;
                var step          = fullRadius * .2f;
                var step_radius   = cur_radius + step;
                int nextLastIndex = 0;
                for (var i=lastIndex; i<mesh.vertexCount; i++){
                    var j = sortVertices[i];
                    var d = vertDistances[j];
                    if (d < cur_radius) {
                        newColors[j] = hitColor;
                        nextLastIndex = i;
                    }
                    else if (d <= step_radius) {
                        newColors[j] = Color.Lerp(prevHitColor, hitColor,
                            /*(10 * (*/ 1 - (d - cur_radius) / step//)  + 2*vertNoise[i]) /12
                        );
                    } else break;
                }
                // Debug.Log($"[{lastIndex}-{nextLastIndex}]    full={fullRadius}  (t={t})  [{cur_radius} - {step_radius}] (step={step})");
                lastIndex = nextLastIndex;

            }

            mesh.colors = newColors;
        }
    }
}