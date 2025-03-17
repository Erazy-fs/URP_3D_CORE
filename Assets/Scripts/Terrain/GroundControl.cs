using System;
using UnityEngine;

public class GroundControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int colorsCount = 12;
    private PlotControl[] plots = new PlotControl[]{};

    void Start()
    {
        plots = GetComponentsInChildren<PlotControl>();
        foreach (var plot in plots) {            
            void SetParams(PlotControl plot){
                Color.RGBToHSV(plot.startColor, out _, out var s, out var v);
                var plotColor = Color.HSVToRGB((float)plot.colorIndex / colorsCount, s, v);
                plot.SetStartParams(plotColor);
            }

            if (plot.isReady) 
                SetParams(plot);
            else 
                plot.OnReady += () => SetParams(plot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {  //ЛКМ
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit)) {
                if (hit.collider != null) {
                    var color = UnityEngine.Random.ColorHSV();
                    var radius = 0f;
                    var flag = false;
                    foreach (var plot in plots) 
                        if (plot.plotUpdating) {
                            flag = true;
                            break;
                        }
                    if (!flag) {
                        foreach (var plot in plots)
                            radius = Mathf.Max(radius, plot.HitPosition(hit.point));
                        
                        foreach (var plot in plots) {
                            plot.StartWaves(color, 3, 0, 5, radius);
                        }
                    }
                }
            }
        }
        
    }
}
