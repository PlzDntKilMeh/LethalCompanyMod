using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectLabel<T> : MonoBehaviour where T : MonoBehaviour
{

    protected T targetObject;
    protected Text NameText;
    protected Camera mainCamera;
    protected Canvas labelsCanvas;
    protected RectTransform PosRect;
    protected virtual Color TextColor => Color.white;
    protected virtual string CanvasName => typeof(T).Name + "_LabelCanvas";
    protected virtual string DefaultText => "TEMP";


    void Awake()
    {
        targetObject = GetComponent<T>();
        if (targetObject == null)
        {
            Debug.LogError($"{typeof(T).Name}_Label: {typeof(T).Name} component is missing!");
            Destroy(this);
            return;
        }

        labelsCanvas = GameObject.Find(CanvasName)?.GetComponent<Canvas>();
        if (labelsCanvas == null)
        {
            CreateCanvas();
        }

        GameObject NameObject = new GameObject("NameObject");
        NameObject.transform.SetParent(labelsCanvas.transform);
        NameText = NameObject.AddComponent<Text>();
        NameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        NameText.fontSize = 13;
        NameText.alignment = TextAnchor.MiddleCenter;
        NameText.color = Color.white; // Default color
        NameText.text = DefaultText;

        PosRect = NameText.GetComponent<RectTransform>() ?? NameText.gameObject.AddComponent<RectTransform>();
        PosRect.localPosition = Vector3.zero;

        NameText.raycastTarget = false;
        mainCamera = GameNetworkManager.Instance.localPlayerController?.gameplayCamera;

    }

    private void CreateCanvas()
    {
        GameObject canvasObject = new GameObject(CanvasName);
        labelsCanvas = canvasObject.AddComponent<Canvas>();
        labelsCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        labelsCanvas.sortingOrder = 0;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        RectTransform rt = labelsCanvas.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    protected virtual void UpdateText()
    {
        NameText.fontSize = 12;
        NameText.text = GetEntityLabel();
        NameText.fontStyle = FontStyle.Normal;
        NameText.color = TextColor;
    }

    protected virtual string GetEntityLabel()
    {
        return targetObject.name;
    }
    protected virtual bool ShouldShow()
    {
        return true;
    }

    protected virtual Vector3 GetWorldPos()
    {
        return targetObject.transform.position;
    }

    void Update()
    {
        if (!ShouldShow())
        {
            NameText.enabled = false;
            return;
        }
        else
        {
            NameText.enabled = true;

        }
        if (targetObject == null) return;

        UpdateText();
        mainCamera = GameNetworkManager.Instance.localPlayerController?.gameplayCamera;


        if (mainCamera != null && labelsCanvas != null && PosRect != null)
        {
            if (WorldToScreen(mainCamera, GetWorldPos(), out Vector3 vector))
            {
                PosRect.position = vector;
            }
        }
    }

    public static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
    {
        screen = camera.WorldToViewportPoint(world);
        screen.x *= Screen.width;
        screen.y *= Screen.height;
        return screen.z > 0f;
    }

    void OnDestroy() => RemoveLabel();
    void OnDisable() => RemoveLabel();

    private void RemoveLabel()
    {
        if (NameText != null)
        {
            Destroy(NameText.gameObject);
        }
    }
}
