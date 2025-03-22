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
    public Camera downCamera;

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

    IEnumerator ShakeCamera(float shakeForce = .7f) {
        for(int i=0; i<5; i++){
            i++;
            downCamera.transform.position = new Vector3(
                downCamera.transform.position.x + UnityEngine.Random.Range(0f, shakeForce),
                downCamera.transform.position.y + UnityEngine.Random.Range(0f, shakeForce),
                downCamera.transform.position.z + UnityEngine.Random.Range(0f, shakeForce)
            );
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    private void SetProgress(float progress){
        zeusUIBar.style.width = new Length(progress, LengthUnit.Percent);
        zeusUIBar.MarkDirtyRepaint();
    }

    private void SetMessage(string message){
        zeusUIMessage.text = message;
    }

    private void SetWaveMessage(string message) {
        zeusUIWave.text = message;
    }

    private GameObject zeus;
    private Animator zeusAnimator;
    float maxRadius = 0f;
    IEnumerator LandZEUS(Vector3 point){
        zeusUI.style.display = DisplayStyle.Flex;
        zeusUIBar.style.backgroundColor = new StyleColor(new Color(1f, 1f, 1f, 1f));
        Debug.Log($"Stand by for Z.E.U.S");
        SetMessage($"Stand by for Z.E.U.S");
        SetWaveMessage("");
        SetProgress(0f);

        //spawn zeus
        zeus = Instantiate(zeusPrefab);
        zeus.transform.position = new Vector3(point.x, point.y+100, point.z);
        zeusAnimator = zeus.GetComponentsInChildren<Animator>().First();


        var radiuses = new Dictionary<PlotControl, float>();
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
        SetMessage($"landing...");
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
        SetMessage($"landing complete");
        yield return null;
    }

        IEnumerator ActivateZEUS()
        {
            //Запуск волн
        Debug.Log($"initialize G.A.I.A. protocol");
        SetMessage($"initialize G.A.I.A. protocol...");
        yield return new WaitForSeconds(1f);


        SetMessage($"executing G.A.I.A. protocol...");
        int waveIndex = 0;
        while (waveIndex < waveCount) {
            waveIndex++;
            Debug.Log($"wave: {waveIndex}/{waveCount} ({waveDuratation}sec)");

            SetWaveMessage($"stage: {waveIndex}/{waveCount}");

            //Запуск анимации ударов
            zeusAnimator.SetBool("isPumping", true);
            yield return new WaitForSeconds(2.9f); //Подъем перед ударом

            //Запуск ударов
            int visualWavesCount = (int)Mathf.Ceil(waveDuratation / 3f)+1;   //Кол-во визуальных волн
            foreach (var plot in plots) {
                Color.RGBToHSV(plot.startColor, out _, out var s, out var v);
                var colorIndex = plot.colorIndex + 4; //new System.Random().Next(0, colorsCount);
                if (colorIndex >= colorsCount) colorIndex -= colorsCount;
                var newColor = Color.HSVToRGB((float)colorIndex / colorsCount, s, v);
                plot.StartWaves(newColor, visualWavesCount, colorIndex, 3, maxRadius);
                if (plot.doWave)
                    yield return null;
            }

            //Ожидани завершения волны
            //yield return new WaitForSeconds(waveDuratation);
            var executeTime = 0f;
            while (executeTime < waveDuratation) {
                executeTime += Time.deltaTime;
                SetProgress(executeTime/waveDuratation*100);
                yield return null;
            }

            //Остановка ударов
            zeusAnimator.SetBool("isPumping", false);

            //Ожидани завершения волны
            Debug.Log($"stage: {waveIndex}/{waveCount} finished. Waiting {timeBetweenWaves}sec");
            if (waveIndex != waveCount) {
                SetMessage($"Initializing next stage...");
                //yield return new WaitForSeconds(timeBetweenWaves);
                var waitTime = 0f;
                while (waitTime < timeBetweenWaves) {
                    waitTime += Time.deltaTime;
                    SetProgress(waitTime/timeBetweenWaves*100);
                    yield return null;
                }
                SetMessage($"executing...");
            }
        }
        Debug.Log($"finish");
        conquerCoroutine = null;
        zeus = null;
        zeusAnimator = null;
        zeusUI.style.display = DisplayStyle.None;
        }

        private IEnumerator activatingCoroutine = null;
        public void Activate()
        {
            activatingCoroutine = ActivateZEUS();
            StartCoroutine(activatingCoroutine);
        }

    IEnumerator DestroyZEUS(){
        if (conquerCoroutine is not null) {
            StopCoroutine(conquerCoroutine);
        if (activatingCoroutine is not null)
            StopCoroutine(activatingCoroutine);

            zeusAnimator.SetBool("isLanding", false);
            zeusAnimator.SetBool("isPumping", false);
            zeusAnimator.SetBool("isDestroyed", true);
            StartCoroutine(ShakeCamera(1.2f));
            foreach (var plot in plots)
                plot.StopWaves();

            SetMessage("CRITICAL ERROR");
            zeusUIBar.style.backgroundColor = new StyleColor(new Color(1f, .3f, .1f, .55f));
            yield return new WaitForSeconds(2f);
            SetMessage("Z.E.U.S. status: OFFLINE");
            yield return new WaitForSeconds(2f);

            conquerCoroutine = null;
            zeusAnimator = null;
            zeus = null;
            zeusUI.style.display = DisplayStyle.None;
        };
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
            StartCoroutine(DestroyZEUS());
        }

    }
}
