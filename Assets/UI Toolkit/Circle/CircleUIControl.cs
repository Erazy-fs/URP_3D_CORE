using UnityEngine;
using UnityEngine.UIElements;

public class SquareController : MonoBehaviour
{
    private VisualElement square;
    private int currentStep = 0;
    private const int totalSteps = 12;
    private float radius = 50f; // Radius of the circular path

    void OnEnable()
    {
        // Get the UIDocument component
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null) return;

        // Get the root VisualElement
        var root = uiDocument.rootVisualElement;

        // Find the square and buttons
        square = root.Q<VisualElement>("square");
        var clockwiseButton = root.Q<Button>("clockwiseButton");
        var counterclockwiseButton = root.Q<Button>("counterclockwiseButton");

        // Assign click handlers to buttons
        clockwiseButton.clicked += MoveClockwise;
        counterclockwiseButton.clicked += MoveCounterclockwise;

        // Initialize the square's position
        UpdateSquarePosition();
    }

    private void MoveClockwise()
    {
        currentStep = (currentStep + 1) % totalSteps;
        UpdateSquarePosition();
    }

    private void MoveCounterclockwise()
    {
        currentStep = (currentStep - 1 + totalSteps) % totalSteps;
        UpdateSquarePosition();
    }

    private void UpdateSquarePosition()
    {
        // Calculate the angle in radians
        float angle = (currentStep / (float)totalSteps) * 2 * Mathf.PI;

        // Calculate the new position using trigonometry
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Apply the position to the square
        square.style.left = Length.Percent(x);
        square.style.top = Length.Percent(y);
    }
}