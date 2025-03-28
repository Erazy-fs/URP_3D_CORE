using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LevelNarrator narrator;
    public int colorsCount = 12;
    public int colorStep = 4;
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

    public bool canSpawn = true;

    private PlotControl[] plots = new PlotControl[]{};
    private Dictionary<int, List<PlotControl>> plotGroups = new();
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
                
                if (!plotGroups.TryGetValue(plot.colorIndex, out var plotGroup)){
                    plotGroup = new();
                    plotGroups[plot.colorIndex] = plotGroup;
                }
                plotGroup.Add(plot);
            }

            if (plot.isReady)
                SetParams(plot);
            else
                plot.OnReady += () => SetParams(plot);
        }
    }

    IEnumerator ShakeCamera(float shakeForce) {
        for(int i=0; i<7; i++){
            i++;
            downCamera.transform.position = new Vector3(
                downCamera.transform.position.x + UnityEngine.Random.Range(.5f, shakeForce),
                downCamera.transform.position.y + UnityEngine.Random.Range(.5f, shakeForce),
                downCamera.transform.position.z + UnityEngine.Random.Range(.5f, shakeForce)
            );
            yield return new WaitForSeconds(.1f);
        }
        yield return null;
    }

    private void SetProgress(float progress){
        zeusUIBar.style.width = new Length(progress, LengthUnit.Percent);
    }

    private void SetMessage(string message){
        zeusUIMessage.text = message;        
        Debug.Log(message);
    }

    private void SetWaveMessage(string message) {
        zeusUIWave.text = message;
    }

    private GameObject zeus;
    private Animator zeusAnimator;
    float maxRadius = 0f;
    private List<PlotControl> currentPlotGroup = new();
    private PenetratorInteraction penetratorInteraction;
    IEnumerator LandZEUS(Vector3 point, int plotGroupIndex){
        zeusUI.style.display = DisplayStyle.Flex;
        zeusUIBar.style.backgroundColor = new StyleColor(new Color(1f, 1f, 1f, .55f));
        // SetMessage($"Stand by for Z.E.U.S");
        SetMessage($"вызов Z.E.U.S");
        SetWaveMessage("");
        SetProgress(0f);
        narrator.ZeusCall();

        //spawn zeus
        zeus = Instantiate(zeusPrefab);
        zeus.transform.position = new Vector3(point.x, point.y+100, point.z);
        zeusAnimator = zeus.GetComponentsInChildren<Animator>().First();

        penetratorInteraction = zeus.GetComponentsInChildren<IInteractable>().First() as PenetratorInteraction;
        penetratorInteraction.narrator = narrator;
        
        var radiuses = new Dictionary<PlotControl, float>();
        currentPlotGroup = plotGroups[plotGroupIndex];
        foreach (var plot in currentPlotGroup) {
            Debug.Log($"plot: {plot.transform.parent.name}");
            var r = plot.HitPosition(point);
            radiuses[plot] = r;
            maxRadius = Mathf.Max(maxRadius, r);
            if (plot.doWave)
                yield return null;
        }

        currentPlotGroup.Sort(delegate(PlotControl a, PlotControl b){return radiuses[a].CompareTo(radiuses[b]);});


        //Запуск анимации приземления
        // SetMessage($"landing...");
        SetMessage($"приземление...");
        zeusAnimator.SetBool("isLanding", true);

        //Перенос объекта в нужную точку
        yield return new WaitForSeconds(.5f);
        Debug.Log($"Landing...");
        zeus.transform.position = new Vector3(point.x, point.y-.2f, point.z);

        //Завершение приземления
        yield return new WaitForSeconds(7.8f);
        StartCoroutine(ShakeCamera(2));
        zeusAnimator.SetBool("isLanding", false);
        
        // SetMessage($"landing complete");
        SetMessage($"приземление модуля: УСПЕШНО");

        yield return new WaitForSeconds(2f);
        SetMessage($"ожидание активации Z.E.U.S.");

        narrator.ZeusLand();
        yield return null;
    }

    IEnumerator ActivateZEUS() {
        narrator.ZeusActivate();
        //Запуск волн
        // SetMessage($"initialize G.A.I.A. protocol...");
        SetMessage($"инициализация протокола G.A.I.A....");
        yield return new WaitForSeconds(1f);


        // SetMessage($"executing G.A.I.A. protocol...");
        SetMessage($"выполнение протокола G.A.I.A....");
        int waveIndex = 0;
        while (waveIndex < waveCount) {
            waveIndex++;
            narrator.StageStart(waveIndex, waveCount);
            Debug.Log($"wave: {waveIndex}/{waveCount} ({waveDuratation}sec)");

            // SetWaveMessage($"'stage': {waveIndex}/{waveCount}");
            SetWaveMessage($"этап: {waveIndex}/{waveCount}");

            //Запуск анимации ударов
            zeusAnimator.SetBool("isPumping", true);
            yield return new WaitForSeconds(2.9f); //Подъем перед ударом

            //Запуск волны врагов
            if (canSpawn) {
                StartEnemyWave();
            }

            //Запуск ударов
            int visualWavesCount = (int)Mathf.Ceil(waveDuratation / 3f)+1;   //Кол-во визуальных волн
            foreach (var plot in currentPlotGroup) {
                Color.RGBToHSV(plot.startColor, out _, out var s, out var v);
                var colorIndex = plot.colorIndex + colorStep; //new System.Random().Next(0, colorsCount);
                if (colorIndex >= colorsCount) colorIndex -= colorsCount;
                var newColor = Color.HSVToRGB((float)colorIndex / colorsCount, s, v);
                plot.StartWaves(newColor, visualWavesCount, colorIndex, 3, maxRadius, waveIndex==waveCount);
                if (plot.doWave)
                    yield return null;
            }

            //Ожидани завершения волны
            //yield return new WaitForSeconds(waveDuratation);
            var executeTime = 0f;
            while (executeTime < waveDuratation) {
                executeTime += Time.deltaTime;
                var progress = executeTime/waveDuratation*100;
                SetProgress(progress);
                narrator.StageProgress(progress, waveIndex, waveCount);
                yield return null;
            }

            narrator.StageEnd(waveIndex, waveCount);

            //Остановка ударов
            zeusAnimator.SetBool("isPumping", false);

            //Ожидани завершения волны
            Debug.Log($"stage: {waveIndex}/{waveCount} finished. Waiting {timeBetweenWaves}sec");
            if (waveIndex != waveCount) {
                // SetMessage($"Initializing next stage...");
                SetMessage($"инициализация следующего этапа...");

                //yield return new WaitForSeconds(timeBetweenWaves);
                var waitTime = 0f;
                while (waitTime < timeBetweenWaves) {
                    waitTime += Time.deltaTime;
                    SetProgress(waitTime/timeBetweenWaves*100);
                    yield return null;
                }

                // SetMessage($"executing...");
                SetMessage($"выполнение этапа...");
            }            
        }
        foreach (var plot in currentPlotGroup) {
            plot.isComplete = true;
        }
        
        var countComplete = 0;
        foreach(var plot in plots){
            if (plot.isComplete) countComplete++;
        }
        narrator.ZoneComplete(countComplete, plots.Length);

        if (countComplete == plots.Length) {
            narrator.LevelComplete();
        }

        Debug.Log($"finish");
        penetratorInteraction.Disable();
        landingCoroutine = null;
        activatingCoroutine = null;
        zeus = null;
        zeusAnimator = null;
        penetratorInteraction = null;
        zeusUI.style.display = DisplayStyle.None;
    }

    void StartEnemyWave()
    {
        List<SpawnPoint> spawnPoints = currentPlotGroup.SelectMany(p => p.GetComponentsInChildren<SpawnPoint>()).ToList();
        spawnPoints.ForEach(sp => sp.SetTarget(zeusAnimator.transform));
        LevelManager.StartNextEnemyWave(spawnPoints);
    }

    private IEnumerator activatingCoroutine = null;
    public void Activate() {
        activatingCoroutine = ActivateZEUS();
        StartCoroutine(activatingCoroutine);
    }

    IEnumerator DestroyZEUS(){

        if (landingCoroutine is not null) {
            StopCoroutine(landingCoroutine);

            if (activatingCoroutine is not null)
                StopCoroutine(activatingCoroutine);

            if (penetratorInteraction is not null)
                penetratorInteraction.isDestroyed = true;

            zeusAnimator.SetBool("isLanding", false);
            zeusAnimator.SetBool("isPumping", false);
            zeusAnimator.SetBool("isDestroyed", true);
            StartCoroutine(ShakeCamera(3f));
            foreach (var plot in currentPlotGroup)
                plot.StopWaves();

            // SetMessage("CRITICAL ERROR");
            SetMessage("КРИТИЧЕСКАЯ ОШИБКА");
            zeusUIBar.style.backgroundColor = new StyleColor(new Color(1f, .3f, .1f, .55f));
            yield return new WaitForSeconds(2f);
            // SetMessage("Z.E.U.S. status: OFFLINE");
            SetMessage("статус Z.E.U.S.: OFFLINE");
            yield return new WaitForSeconds(2f);

            landingCoroutine = null;
            activatingCoroutine = null;
            zeusAnimator = null;
            zeus = null;
            penetratorInteraction = null;
            zeusUI.style.display = DisplayStyle.None;
            narrator.ZeusDestroy();
        };
    }

    public void CallInZEUS(Vector3 point, int plotGroupIndex) {
        if (landingCoroutine is not null || !plotGroups.ContainsKey(plotGroupIndex)) return;
        landingCoroutine = LandZEUS(point, plotGroupIndex);
        StartCoroutine(landingCoroutine);
    }

    private IEnumerator landingCoroutine = null;
    void Update() {
        if (landingCoroutine is null) {
            // if (Input.GetMouseButtonDown(0)) {  //ЛКМ
            //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //     if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider != null) {
            //         landingCoroutine = LandZEUS(hit.point);
            //         StartCoroutine(landingCoroutine);
            //     }
            // }
        } else if (Input.GetMouseButtonDown(2)) {
            StartCoroutine(DestroyZEUS());
        }

    }
}
