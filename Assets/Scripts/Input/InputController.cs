using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputController 
{
    public static int _game_pad_count;
    static GamePad _game_pad=new GamePad();
    //どっかのStartで呼び出して
    public static void GamePadInit()
    {
        _game_pad.AddButton(ButtonID.A, "Jump");
        _game_pad.AddButton(ButtonID.B, "Fire1");
        _game_pad.AddButton(ButtonID.X, "Fire2");
        _game_pad.AddButton(ButtonID.Y, "Fire3");
        _game_pad.AddButton(ButtonID.Start, "Submit");
        _game_pad.AddButton(ButtonID.Select, "Cancel");

        _game_pad.AddAxis(AxisID.L_Horizontal, "Horizontal");
        _game_pad.AddAxis(AxisID.L_Vertical, "Vertical");
    }
    public static bool GetButtonDown(ButtonID button)
    {
        foreach(KeyValuePair<ButtonID,string> item in _game_pad._button_map)
        {
            if (item.Key == button)
            {
                return Input.GetButtonDown(item.Value);

            }
        }
        return false;
    }
    public static bool GetButton(ButtonID button)
    {
        foreach (KeyValuePair<ButtonID, string> item in _game_pad._button_map)
        {
            if (item.Key == button)
            {
                return Input.GetButton(item.Value);

            }
        }
        return false;
    }
    public static bool GetButtonUp(ButtonID button)
    {
        foreach (KeyValuePair<ButtonID, string> item in _game_pad._button_map)
        {
            if (item.Key == button)
            {
                return Input.GetButtonUp(item.Value);

            }
        }
        return false;
    }
    public static float GetAxis(AxisID axis)
    {

        foreach (KeyValuePair<AxisID, string> item in _game_pad._axis_map)
        {
            if (item.Key == axis)
            {
                return Input.GetAxis(item.Value);

            }
        }
        return 0;
    }

}

class GamePad
{

    public Dictionary<ButtonID, string> _button_map = new Dictionary<ButtonID, string>();
    public Dictionary<AxisID, string> _axis_map = new Dictionary<AxisID, string>();

    public void AddButton(ButtonID button_id,string button_string)
    {
       _button_map.Add(button_id, button_string);
    }
    public void AddAxis(AxisID axis,string axis_string)
    {
        _axis_map.Add(axis, axis_string);
    }
}
public enum ButtonID
{
    A, B, X, Y, L1, R1, L2, R2,L3,R3, Select, Start, 
}

public enum AxisID
{
    R_Horizontal, R_Vertical, L_Horizontal, L_Vertical, Cross_Horizontal, Cross_Vertical, Side, None,
}

public enum GamePad_NUM
{
    One, Two, Three, Four, All, None,
}
