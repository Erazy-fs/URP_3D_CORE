using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int colorsCount = 12;
    private PlotControl[] plots = new PlotControl[]{};
    public GameObject zeusPrefab;

    public float waveDuratation   = 5f;
    public float timeBetweenWaves = 5f;
    public int waveCount = 3;
    public Camera camera;

    public UIDocument zeusUIdoc;
    private VisualElement zeusUI;
    private VisualElement zeusUIBar;
    private Label zeusUIMessage;
    private Label zeusUIWave;

    void Start() {
        zeusUI        = zeusUIdoc.rootVisualElement;
        zeusUI.style.display = DisplayStyle.None;
        zeusUIBar     = zeusUI.Q<VisualElement>("zeus_bar");
        zeusUIMessage = zeusUI.Q<Label>("zeus_message");
        zeusUIWave    = zeusUI.Q<Label>("zeus_wave");

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

    IEnumerator ShakeCamera(float shakeForce = .5f) {
        for(int i=0; i<5; i++){
            i++;
            camera.transform.position = new Vector3(
                camera.transform.position.x + UnityEngine.Random.Range(0f, shakeForce),
                camera.transform.position.y + UnityEngine.Random.Range(0f, shakeForce),
                camera.transform.position.z + UnityEngine.Random.Range(0f, shakeForce)
            );
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    private void SetProgress(float progress){
        zeusUIBar.style.width = Length.Percent(progress);
    }

    private void SetMessage(string message){
        zeusUIMessage.text = message;
    }

    private void SetWaveMessage(int waveIndex, int count) {
        zeusUIWave.text = $"wave: {waveIndex}/{count}";
    }

    private GameObject zeus;
    private Animator zeusAnimator;
    IEnumerator LandZEUS(Vector3 point){
        zeusUI.style.display = DisplayStyle.Flex;
        Debug.Log($"Stand by for Z.E.U.S");
        SetMessage($"Stand by for Z.E.U.S");

        //spawn zeus
        zeus = Instantiate(zeusPrefab);
        zeus.transform.position = new Vector3(point.x, point.y+100, point.z);
        zeusAnimator = zeus.GetComponentsInChildren<Animator>().First();


        var radiuses = new Dictionary<PlotControl, float>();
        var maxRadius = 0f;
        foreach (var plot in plots) {
            var r = plot.HitPosition(point);
            radiuses[plot] = r;
            maxRadius = Mathf.Max(r, r);
            if (plot.doWave)
                yield return null;
        }

        Array.Sort(plots, delegate(PlotControl a, PlotControl b){return radiuses[a].CompareTo(radiuses[b]);});


        //Запуск анимации приземления
        Debug.Log($"Landing...");
        SetMessage($"Landing...");
        zeusAnimator.SetBool("isLanding", true);

        //Перенос объекта в нужную точку
        yield return new WaitForSeconds(.5f);
        Debug.Log($"Landing...");
        zeus.transform.position = new Vector3(point.x, point.y-.2f, point.z);

        //Перенос отмена приземления
        yield return new WaitForSeconds(2.45f);
        StartCoroutine(ShakeCamera());
        zeusAnimator.SetBool("isLanding", false);
        Debug.Log($"Landing complete");
        SetMessage($"Landing complete");
        yield return null;

        //Запуск волн
        Debug.Log($"Initialize G.A.I.A. protocol");
        SetMessage($"Initialize G.A.I.A. protocol");
        yield return new WaitForSeconds(1f);

        
        SetMessage($"Executing G.A.I.A. protocol");
        SetProgress(0f);
        int waveIndex = 0;
        while (waveIndex < waveCount) {
            waveIndex++;
            Debug.Log($"wave: {waveIndex}/{waveCount} ({waveDuratation}sec)");

            //Запуск анимации ударов
            zeusAnimator.SetBool("isPumping", true);
            yield return new WaitForSeconds(2.9f); //Подъем перед ударом

            //Запуск ударов
            int visualWavesCount = (int)Mathf.Ceil(waveDuratation / 3f)+1;   //Кол-во визуальных волн
            foreach (var plot in plots) {
                Color.RGBToHSV(plot.startColor, out _, out var s, out var v);
                var colorIndex = plot.colorIndex + 1; //new System.Random().Next(0, colorsCount);
                if (colorIndex >= colorsCount) colorIndex = 0;
                var newColor = Color.HSVToRGB((float)colorIndex / colorsCount, s, v);
                plot.StartWaves(newColor, visualWavesCount, colorIndex, 3, maxRadius);
                if (plot.doWave)
                    yield return null;
            }

            //Ожидани завершения волны
            //yield return new WaitForSeconds(waveDuratation);
            var elapsedTime = 0f;
            while (elapsedTime < waveDuratation) {
                elapsedTime += Time.deltaTime;
                SetProgress(elapsedTime/waveDuratation*100);
                yield return null;
            }

            //Остановка ударов
            zeusAnimator.SetBool("isPumping", false);

            Debug.Log($"wave: {waveIndex}/{waveCount} finished. Waiting {timeBetweenWaves}sec");
            //Ожидани завершения волны
            if (waveIndex != waveCount)
                yield return new WaitForSeconds(timeBetweenWaves);
        }
        Debug.Log($"finish");
        conquerCoroutine = null;
        zeus = null;
        zeusAnimator = null;
        zeusUI.style.display = DisplayStyle.None;
    }

    public void DestroyZEUS(){
        if (conquerCoroutine is null) return;
        StopCoroutine(conquerCoroutine);
        zeusAnimator.SetBool("isLanding", false);
        zeusAnimator.SetBool("isPumping", false);
        zeusAnimator.SetBool("isDestroyed", true);
        StartCoroutine(ShakeCamera());
        conquerCoroutine = null;
        zeusAnimator = null;
        zeus = null;
        foreach (var plot in plots) {
            plot.StopWaves();
        }
        zeusUI.style.display = DisplayStyle.None;
    }

    private IEnumerator conquerCoroutine = null;
    void Update()
    {
        if (conquerCoroutine is null) {
            if (Input.GetMouseButtonDown(0)) {  //ЛКМ
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider != null) {
                    conquerCoroutine = LandZEUS(hit.point);
                    StartCoroutine(conquerCoroutine);
                }
            }
        } else if (Input.GetMouseButtonDown(1)) {
            DestroyZEUS();
        }

    }
}
