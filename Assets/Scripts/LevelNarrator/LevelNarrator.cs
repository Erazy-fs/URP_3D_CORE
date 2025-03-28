using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelNarrator : MonoBehaviour {

    public UIDocument narratorUIdoc;
    public VisualElement narratorUI;
    private Label bottomText;
    private Label blackScreenText;
    private VisualElement blackScreen;
    private VisualElement blackScreenTextBox;
    private VisualElement mainView;
    private VisualElement tasksBox;
    private Label tutorialText;

    public GameObject player;
    private TopDownControl playerControl;

    public string sceneName;
    void Start() {
        playerControl = player.GetComponent<TopDownControl>();
        narratorUI         = narratorUIdoc.rootVisualElement;
        bottomText         = narratorUI.Q<Label>("bottomText");
        blackScreenText    = narratorUI.Q<Label>("blackScreenText");
        tutorialText       = narratorUI.Q<Label>("tutorialText");
        blackScreenTextBox = narratorUI.Q<VisualElement>("blackScreenTextBox");
        blackScreen        = narratorUI.Q<VisualElement>("blackScreen");
        mainView           = narratorUI.Q<VisualElement>("mainView");
        tasksBox           = narratorUI.Q<VisualElement>("tasksBox");
        tasksBox.Clear();

        HideElement(blackScreenTextBox);
        ShowBlackScreen();
        SetBlackScreenMessage("");
        SetBottomMessage("");

        if (sceneName=="basic"){
            StartCoroutine(BasicScene());
        } else if (sceneName == "tutorial") {
            StartCoroutine(TutorialScene());
        }

        OnStart?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)){
            blackScreen.ToggleInClassList("narrator_zero");
        }
    }


    private Label AddTask(string task){
        var t = new Label(task);
        t.AddToClassList("task_text");
        tasksBox.Add(t);
        return t;
    }

    private void RemoveTask(Label task){
        tasksBox.Remove(task);
    }

    private void SetBottomMessage(string message){
        bottomText.text = message;
    }

    private void HideElement(VisualElement e){
        e.AddToClassList("narrator_zero");
    }
    private void ShowElement(VisualElement e){
        e.RemoveFromClassList("narrator_zero");
    }

    private void ShowBlackScreen(){
        blackScreen.style.display = DisplayStyle.Flex;
        ShowElement(blackScreen);
        mainView.style.display    = DisplayStyle.None;
        HideElement(mainView);
        // blackScreen.style.opacity = new StyleFloat(1f);
    }
    private void HideBlackScreen(){
        blackScreen.style.display = DisplayStyle.None;
        HideElement(blackScreen);
        mainView.style.display    = DisplayStyle.Flex;
        ShowElement(mainView);
    }
    private void SetBlackScreenMessage(string message){
        ShowElement(blackScreenTextBox);
        blackScreenText.text = message;
    }


    IEnumerator BasicScene(){
        OnStart = () => {
            Debug.Log("Начало уровня");
        };
        OnZeusCall = () => {
            Debug.Log("ZEUS Вызван");
        };

        var isComplete = false;
        OnZoneComplete = (complete, outof) => {
            Debug.Log($"захвачено {complete}/{outof}");
            isComplete = complete == outof;
        };

        while (!isComplete) {
            yield return null;
        }

        yield return new WaitForSeconds(2);

        yield return null;
    }

    IEnumerator TutorialScene(){
        OnPlayerDeath = ()=>{
            ShowBlackScreen();
            SetBlackScreenMessage("УМЕР");
        };
        
        var foundZone = false;
        actions.Add("foundZone", ()=>{foundZone = true;});

        
        var ZEUScalled = false;
        OnZeusCall = () => {ZEUScalled = true;};

        var ZEUSlanded = false;
        OnZeusLand = () => {ZEUSlanded = true;};

        var ZEUSactivated = false;
        OnZeusActivate = () => {ZEUSactivated = true;};

        playerControl.canMove  = false;
        playerControl.canShoot = false;
        playerControl.canCallZEUS = false;
        ShowBlackScreen();
        HideElement(blackScreenTextBox);
        yield return null;
        yield return new WaitForSeconds(1.5f);


        yield return null;
        SetBlackScreenMessage("Реа́кция (от лат. re… «против» + actio «действие») — действие, возникающее в ответ на какое-либо воздействие.");
        ShowElement(blackScreenTextBox);
        yield return new WaitForSeconds(3f);
        SetBottomMessage("Нажмите [Enter] чтобы продолжить");
        
        while (!Input.GetKeyDown(KeyCode.Return)){
            yield return null;
        }        

        SetBottomMessage("");
        HideElement(blackScreenTextBox);
        yield return new WaitForSeconds(1f);

        HideElement(blackScreen);
        HideElement(tutorialText);

        yield return new WaitForSeconds(1f);
        HideBlackScreen();
        playerControl.canMove = true;
        yield return new WaitForSeconds(.5f);
        tutorialText.text = "Используйте [W] [A] [S] [D] для перемещения";
        var taskW = AddTask("# Нажмите [W]");
        var taskA = AddTask("# Нажмите [A]");
        var taskS = AddTask("# Нажмите [S]");
        var taskD = AddTask("# Нажмите [D]");

        ShowElement(tutorialText);


        var bW = false;
        var bA = false;
        var bS = false;
        var bD = false;
        while (!(bW&&bA&&bS&&bD)) {
            if (Input.GetKeyDown(KeyCode.W)) {bW = true; RemoveTask(taskW);}
            if (Input.GetKeyDown(KeyCode.A)) {bA = true; RemoveTask(taskA);}
            if (Input.GetKeyDown(KeyCode.S)) {bS = true; RemoveTask(taskS);}
            if (Input.GetKeyDown(KeyCode.D)) {bD = true; RemoveTask(taskD);}
            yield return null;
        }
        tutorialText.text = "";
        yield return new WaitForSeconds(2f);

        SetBottomMessage("Вшш... ВШШ шш ъъъ....");
        yield return new WaitForSeconds(2f);

        SetBottomMessage("....");
        yield return new WaitForSeconds(1f);

        SetBottomMessage("...ууУуУУ...ФФФП");
        yield return new WaitForSeconds(1f);
        SetBottomMessage("");
        yield return new WaitForSeconds(2f);


        SetBottomMessage("...бъясню вашу задачу и буду направлять вас по ходу выполнения. Слушайте внимательно.");
        yield return new WaitForSeconds(7f);
        SetBottomMessage("зЗЗзз...П");
        yield return new WaitForSeconds(3f);


        SetBottomMessage("Наша цель — установка модуля Z.E.U.S. в подходящей зоне.");
        yield return new WaitForSeconds(7f);
        var taskZEUSland = AddTask("# Установить модуль Z.E.U.S.");
        yield return new WaitForSeconds(3f);


        SetBottomMessage("Любое место не подойдет. Ищите ровную, не слишком твердую, НЕКАМЕНИСТУЮ поверхность");
        yield return new WaitForSeconds(7f);
        var taskFindLand = AddTask("# Найти подходящую поверхность");
        yield return new WaitForSeconds(3f);


        SetBottomMessage("Предположително такая зона находится НА СЕВЕРЕ");
        yield return new WaitForSeconds(7f);
        taskFindLand.text += " НА СЕВЕРЕ";
        SetBottomMessage("");

        while (!foundZone) {
            yield return null;
        }

        SetBottomMessage("...");
        yield return new WaitForSeconds(1f);
        SetBottomMessage("Да...");
        yield return new WaitForSeconds(1f);
        
        SetBottomMessage("Да ДА! Эта зона подойдет.");
        RemoveTask(taskFindLand);

        playerControl.canCallZEUS = true;
        tutorialText.text = "Встань в подходящую зону и вызовите модуль Z.E.U.S. нажав [F]";

        yield return new WaitForSeconds(3f);
        SetBottomMessage("");
        while (!ZEUScalled) {
            yield return null;
        }

        tutorialText.text = "";
        SetBottomMessage("Модуль на подходе. Ожидайте.");

        yield return new WaitForSeconds(3f);

        while(!ZEUSlanded){
            yield return null;
        }
        RemoveTask(taskZEUSland);        
        yield return new WaitForSeconds(1f);

        SetBottomMessage("Вы уже близки к завершению работы. Сейчас я объясню, как правильно запустить устройство терраформирования. Слушайте внимательно — этот этап крайне важен для успеха всей миссии.");
        yield return new WaitForSeconds(6f);

        var taskActivate = AddTask("# Активируйте модуль");
        SetBottomMessage("Начните с включения основного энергетического контура. Для этого найдите панель управления на правой стороне устройства — она обозначена синей полосой. Откройте её и нажмите кнопку активации. Вы должны увидеть зелёный световой сигнал — это означает, что энергия поступает в систему. Если сигнала нет, проверьте соединения и перезапустите контур. После активации энергосистемы переходите к загрузке реагентов.На задней панели устройства расположены капсулы с химическими веществами и микроорганизмами. Убедитесь, что все они закреплены плотно и не повреждены. Если всё в порядке, поверните рычаг активации на панели управления — он находится слева от главного дисплея. Это запустит процесс распределения реагентов в атмосферу и почву. Важно: после запуска процесса устройство начнет работать автономно, но первые несколько часов требуют вашего внимания. Наблюдайте за показателями на главном дисплее.");
        yield return new WaitForSeconds(7f);
        SetBottomMessage("Надеюсь понятно объяснил");
        yield return new WaitForSeconds(2f);
        tutorialText.text = "Подойдите к модулю и нажмите [E]";

        // Для появления кнопки активации [E]
        OnReadyForActivation?.Invoke(true);

        while (!ZEUSactivated) {
            yield return null;
        }
        RemoveTask(taskActivate);
        tutorialText.text = "";
        SetBottomMessage("");



        playerControl.canShoot = true;

        yield return null;
    }

    private Action OnStart;

    private Dictionary<string, Action> actions = new();
    public void CallAction(string actionName){
        if (actions.TryGetValue(actionName, out var action)) action();
    }

    //При начале этапа
    private Action<int, int> OnStageStart;
    public void StageStart(int stageIndex, int stageCount) {
        OnStageStart?.Invoke(stageIndex, stageCount);
    }

    //При проходе этапа
    private Action<float, int, int> OnStageProgress;
    public void StageProgress(float value, int stageIndex, int stageCount){
        OnStageProgress?.Invoke(value, stageIndex, stageCount);
    }

    //При завершении этапа
    private Action<int, int> OnStageEnd;
    public void StageEnd(int stageIndex, int stageCount){
        OnStageEnd?.Invoke(stageIndex, stageCount);
    }

    private Action OnZeusCall;
    public void ZeusCall(){
        OnZeusCall?.Invoke();
    }

    private Action OnZeusLand;
    public void ZeusLand(){
        OnZeusLand?.Invoke();
    }

    private Action OnZeusActivate;
    public void ZeusActivate(){
        OnZeusActivate?.Invoke();
    }

    private Action OnZeusDestroy;
    public void ZeusDestroy(){
        OnZeusDestroy?.Invoke();
    }

    
    private Action<int, int> OnZoneComplete;
    public void ZoneComplete(int completeCount, int count){
        OnZoneComplete?.Invoke(completeCount, count);
    }

    private Action OnLevelComplete;
    public void LevelComplete(){
        OnLevelComplete?.Invoke();
    }

    private Action OnPlayerDeath;
    public void PlayerDeath(){
        OnPlayerDeath?.Invoke();
    }

    public event Action<bool> OnReadyForActivation;




}
