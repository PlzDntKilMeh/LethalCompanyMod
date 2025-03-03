using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using pdkmMenu;

public class guiBase : MonoBehaviour
{
    public Color CustomRed = new Color(0.5f, 0.2f, 0.2f, 1.0f);
    public Color CustomBlue = new Color(0.2f, 0.3f, 0.8f, 1.0f);
    public Color MenuColor;
    public Color TextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);


    public float ButtonWidthPercentage = 0.06f;
    public float ButtonHeightPercentage = 0.025f;
    public float PaddingYPercentage = 0.03f;

    public float XPercentage = 0.0f;
    public float YPercentage = 0.0f;
    public int ButtonIndex;


    public float infoWidthPercentage = 0.06f;
    public float infoHeighthPercentage = 0.015f;
    public int InfoIndex;

    public int ContainerX;
    public int ContainterY;
    public int ContainterIndex_x;
    public int ContainterIndex_y;
    public int ContainterImgX = 75;
    public int ContainterImgY = 75;



    public float AddSlider(float minValue, float maxValue, float currentValue, string label = "", bool toggle = true)
    {
        Color OriginalColor = MenuColor;
        if (toggle == false)
        {
            MenuColor = CustomRed;
        }

        GUI.backgroundColor = MenuColor;

        GUI.contentColor = TextColor;

        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);
        Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        Rect buttonRect2 = new Rect(buttonX, buttonY+buttonHeight/3, buttonWidth, buttonHeight-buttonHeight/3);


        GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);




        float result = GUI.HorizontalSlider(
            buttonRect,
            (float)currentValue,
            (float)minValue,
            (float)maxValue);

        GUI.Label(buttonRect2, label);

        MenuColor = OriginalColor;

        ButtonIndex++;
        return result;
    }
    public int AddSlider(int minValue, int maxValue, int currentValue, string label = "", bool toggle = true)
    {
        Color OriginalColor = MenuColor;
        if (toggle == false)
        {
            MenuColor = CustomRed;
        }

        GUI.backgroundColor = MenuColor;

        GUI.contentColor = TextColor;
        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);
        Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        Rect buttonRect2 = new Rect(buttonX, buttonY + buttonHeight / 3, buttonWidth, buttonHeight - buttonHeight / 3);

        GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);




        int result = Mathf.RoundToInt(
            GUI.HorizontalSlider(
                buttonRect,
                (float)currentValue,
                (float)minValue,
                (float)maxValue));
        GUI.Label(buttonRect2, label);

        MenuColor = OriginalColor;
        ButtonIndex++;
        return result;
    }


    public void AddButtonList(Dictionary<string, Tuple<bool, Action>> keyValuePairs, ref Vector2 scrollPosition, int index = 0, int RowDisplayCount = 10)
    {
        int xOffset = index * (int)((Screen.width * ButtonWidthPercentage) + (Screen.width * 0.015f)); // Horizontal offset for each category
        int visibleHeight = (int)(Screen.height * (PaddingYPercentage)) * RowDisplayCount; // Height of the scrollable area (adjust as needed)
        int contentHeight = (int)(Screen.height * (keyValuePairs.Count * PaddingYPercentage)); // Total height of the content

        scrollPosition = GUI.BeginScrollView(
            new Rect(
            (Screen.width * XPercentage) + xOffset,
            (Screen.height * YPercentage),

            (Screen.width * ButtonWidthPercentage) + (Screen.width * 0.01f),
            visibleHeight),

            scrollPosition,

            new Rect(0, 0,
            (Screen.width * ButtonWidthPercentage),
            contentHeight
            ));


        int i = 0;
        foreach (var button in keyValuePairs)
        {
            Color originalColor = MenuColor;
            MenuColor = button.Value.Item1 ? CustomBlue : CustomRed;

            //button.Key
            Rect buttonRect = new Rect(
                0,
                Screen.height * (i * PaddingYPercentage),

                (Screen.width * ButtonWidthPercentage),
                (Screen.height * ButtonHeightPercentage));

            GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);
            GUI.backgroundColor = MenuColor;
            if (GUI.Button(
                buttonRect,
                $"{button.Key}"))
            {
                button.Value.Item2.Invoke();
            }
            MenuColor = originalColor;
            GUI.backgroundColor = originalColor;
            i++;
        }
        GUI.EndScrollView();
    }

    public void AddInfo(string info)
    {
        GUI.backgroundColor = MenuColor;
        GUI.contentColor = TextColor;
        float infoWidth = Screen.width * infoWidthPercentage;
        float infoHeight = Screen.height * infoHeighthPercentage;
        float infoX = Screen.width * XPercentage;
        float infoY = Screen.height * (YPercentage + InfoIndex * infoHeighthPercentage);

        Rect InfoRect = new Rect(infoX, infoY, infoWidth, infoHeight);

        GUI.DrawTexture(InfoRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);

        if (GUI.Button(InfoRect, info))
        {
            // Copy the info to the clipboard
            GUIUtility.systemCopyBuffer = info;
        }

        InfoIndex++;
    }

    public void AddButton(string Name, UnityAction buttonAction, bool toggle = true)
    {
        Color OriginalColor = MenuColor;
        if (toggle == false)
        {
            MenuColor = CustomRed;
        }

        GUI.backgroundColor = MenuColor;

        GUI.contentColor = TextColor;

        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);
        Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);

        try
        {
            if (GUI.Button(buttonRect, Name))
            {
                buttonAction();
            }
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError(e);
        }
        
        MenuColor = OriginalColor;

        ButtonIndex++;
    }

    public void AddHoldButton(string Name, UnityAction action)
    {
        GUI.backgroundColor = MenuColor;
        GUI.contentColor = TextColor;

        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);
        Rect buttonRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);
        GUI.Button(buttonRect, Name);

        // Check if the mouse is interacting with the button
        if (buttonRect.Contains(Event.current.mousePosition))
        {
            for (int mouseButton = 0; mouseButton <= 2; mouseButton++)
            {
                if (Event.current.type == EventType.MouseDown ||
                    (Event.current.type == EventType.Repaint && Input.GetMouseButton(mouseButton)))
                {
                    action?.Invoke();
                    break; // Prevent invoking the action multiple times per frame
                }
            }
        }

        ButtonIndex++;
    }


    public string TextBox(string currentValue)
    {
        GUI.backgroundColor = MenuColor;
        GUI.contentColor = TextColor;

        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);

        // Create a unique name for the text box
        string controlName = $"TextField_{ButtonIndex}";
        GUI.SetNextControlName(controlName);

        // Check if the Tab key is pressed and block focus
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab)
        {
            Event.current.Use(); // Consume the Tab key event
        }

        // Render the text field
        Rect textRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        GUI.DrawTexture(textRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);
        currentValue = GUI.TextField(
            textRect,
            currentValue, 25);

        // Prevent focus from being assigned to this control
        if (GUI.GetNameOfFocusedControl() == controlName && Event.current.keyCode == KeyCode.Tab)
        {
            GUI.FocusControl(null); // Explicitly remove focus
        }

        ButtonIndex++;
        return currentValue;
    }


    public int IntTextBox(int currentValue)
    {
        GUI.backgroundColor = MenuColor;
        GUI.contentColor = TextColor;

        float buttonWidth = Screen.width * ButtonWidthPercentage;
        float buttonHeight = Screen.height * ButtonHeightPercentage;
        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);

        // Convert current value to string for the text field
        string inputValue = currentValue.ToString();

        Rect textRect = new Rect(buttonX, buttonY, buttonWidth, buttonHeight);
        GUI.DrawTexture(textRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);
        inputValue = GUI.TextField(
            textRect,
            inputValue, 25);

        // Try to parse the input as an integer
        if (int.TryParse(inputValue, out int newValue))
        {
            currentValue = newValue;
        }
        ButtonIndex++;
        return currentValue;

    }



    public void AddImgListItem(Texture2D img, string itemname, UnityAction buttonAction, int rowLen)
    {
        GUI.backgroundColor = MenuColor;
        GUI.contentColor = TextColor;


        float buttonX = Screen.width * XPercentage;
        float buttonY = Screen.height * (YPercentage + ButtonIndex * PaddingYPercentage);


        Rect buttonRect = new Rect((float)(ContainerX + ContainterImgX * ContainterIndex_x),
                                (float)(ContainterY + ContainterImgY * ContainterIndex_y),
                                ContainterImgX,
                                ContainterImgY);

        GUI.DrawTexture(buttonRect, Texture2D.whiteTexture, ScaleMode.StretchToFill, true, 0, MenuColor, 0, 0);
        GUI.Label(buttonRect, itemname);
        if (GUI.Button(buttonRect, img))
        {
            buttonAction();
        }
        ContainterIndex_x++;
        if (ContainterIndex_x > rowLen)
        {
            ContainterIndex_y++;
            ContainterIndex_x = 0;
        }
    }
}