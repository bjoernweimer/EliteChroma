﻿using System;
using System.Collections.Generic;
using EliteChroma.Core.Internal;
using EliteFiles.Bindings.Devices;
using static EliteChroma.Core.Internal.NativeMethods;

namespace EliteChroma.Elite.Internal
{
    // References:
    // - <EliteRoot>\Products\elite-dangerous-64\ControlSchemes\Help.txt
    // - <EliteRoot>\Products\elite-dangerous-64\EliteDangerous64.exe (binary scraping)
    internal static class KeyMappings
    {
        private static readonly Dictionary<string, VirtualKey> _keys = new Dictionary<string, VirtualKey>(StringComparer.Ordinal)
        {
            { Keyboard.Escape, VirtualKey.VK_ESCAPE },
            { GetKeyName('1'), (VirtualKey)'1' },
            { GetKeyName('2'), (VirtualKey)'2' },
            { GetKeyName('3'), (VirtualKey)'3' },
            { GetKeyName('4'), (VirtualKey)'4' },
            { GetKeyName('5'), (VirtualKey)'5' },
            { GetKeyName('6'), (VirtualKey)'6' },
            { GetKeyName('7'), (VirtualKey)'7' },
            { GetKeyName('8'), (VirtualKey)'8' },
            { GetKeyName('9'), (VirtualKey)'9' },
            { GetKeyName('0'), (VirtualKey)'0' },
            { Keyboard.Backspace, VirtualKey.VK_BACK },
            { Keyboard.Tab, VirtualKey.VK_TAB },
            { GetKeyName('Q'), (VirtualKey)'Q' },
            { GetKeyName('W'), (VirtualKey)'W' },
            { GetKeyName('E'), (VirtualKey)'E' },
            { GetKeyName('R'), (VirtualKey)'R' },
            { GetKeyName('T'), (VirtualKey)'T' },
            { GetKeyName('Y'), (VirtualKey)'Y' },
            { GetKeyName('U'), (VirtualKey)'U' },
            { GetKeyName('I'), (VirtualKey)'I' },
            { GetKeyName('O'), (VirtualKey)'O' },
            { GetKeyName('P'), (VirtualKey)'P' },
            { Keyboard.Enter, VirtualKey.VK_RETURN },
            { Keyboard.LeftControl, VirtualKey.VK_LCONTROL },
            { GetKeyName('A'), (VirtualKey)'A' },
            { GetKeyName('S'), (VirtualKey)'S' },
            { GetKeyName('D'), (VirtualKey)'D' },
            { GetKeyName('F'), (VirtualKey)'F' },
            { GetKeyName('G'), (VirtualKey)'G' },
            { GetKeyName('H'), (VirtualKey)'H' },
            { GetKeyName('J'), (VirtualKey)'J' },
            { GetKeyName('K'), (VirtualKey)'K' },
            { GetKeyName('L'), (VirtualKey)'L' },
            { Keyboard.LeftShift, VirtualKey.VK_LSHIFT },
            { GetKeyName('Z'), (VirtualKey)'Z' },
            { GetKeyName('X'), (VirtualKey)'X' },
            { GetKeyName('C'), (VirtualKey)'C' },
            { GetKeyName('V'), (VirtualKey)'V' },
            { GetKeyName('B'), (VirtualKey)'B' },
            { GetKeyName('N'), (VirtualKey)'N' },
            { GetKeyName('M'), (VirtualKey)'M' },
            { Keyboard.RightShift, VirtualKey.VK_RSHIFT },
            { Keyboard.NumpadMultiply, VirtualKey.VK_MULTIPLY },
            { Keyboard.LeftAlt, VirtualKey.VK_LMENU },
            { Keyboard.Space, VirtualKey.VK_SPACE },
            { Keyboard.CapsLock, VirtualKey.VK_CAPITAL },
            { Keyboard.F1, VirtualKey.VK_F1 },
            { Keyboard.F2, VirtualKey.VK_F2 },
            { Keyboard.F3, VirtualKey.VK_F3 },
            { Keyboard.F4, VirtualKey.VK_F4 },
            { Keyboard.F5, VirtualKey.VK_F5 },
            { Keyboard.F6, VirtualKey.VK_F6 },
            { Keyboard.F7, VirtualKey.VK_F7 },
            { Keyboard.F8, VirtualKey.VK_F8 },
            { Keyboard.F9, VirtualKey.VK_F9 },
            { Keyboard.F10, VirtualKey.VK_F10 },
            { Keyboard.NumLock, VirtualKey.VK_NUMLOCK },
            { Keyboard.ScrollLock, VirtualKey.VK_SCROLL },
            { Keyboard.Numpad7, VirtualKey.VK_NUMPAD7 },
            { Keyboard.Numpad8, VirtualKey.VK_NUMPAD8 },
            { Keyboard.Numpad9, VirtualKey.VK_NUMPAD9 },
            { Keyboard.NumpadSubtract, VirtualKey.VK_SUBTRACT },
            { Keyboard.Numpad4, VirtualKey.VK_NUMPAD4 },
            { Keyboard.Numpad5, VirtualKey.VK_NUMPAD5 },
            { Keyboard.Numpad6, VirtualKey.VK_NUMPAD6 },
            { Keyboard.NumpadAdd, VirtualKey.VK_ADD },
            { Keyboard.Numpad1, VirtualKey.VK_NUMPAD1 },
            { Keyboard.Numpad2, VirtualKey.VK_NUMPAD2 },
            { Keyboard.Numpad3, VirtualKey.VK_NUMPAD3 },
            { Keyboard.Numpad0, VirtualKey.VK_NUMPAD0 },
            { Keyboard.NumpadDecimal, VirtualKey.VK_DECIMAL },
            { Keyboard.F11, VirtualKey.VK_F11 },
            { Keyboard.F12, VirtualKey.VK_F12 },
            { Keyboard.F13, VirtualKey.VK_F13 },
            { Keyboard.F14, VirtualKey.VK_F14 },
            { Keyboard.F15, VirtualKey.VK_F15 },
            { Keyboard.Kana, VirtualKey.VK_KANA },
            { Keyboard.AbntC1, VirtualKey.VK_ABNT_C1 },
            { Keyboard.Convert, VirtualKey.VK_CONVERT },
            { Keyboard.NoConvert, VirtualKey.VK_NONCONVERT },
            { Keyboard.Yen, 0 },
            { Keyboard.AbntC2, VirtualKey.VK_ABNT_C2 },
            { Keyboard.NumpadEquals, 0 },
            { Keyboard.PrevTrack, VirtualKey.VK_MEDIA_PREV_TRACK },
            { Keyboard.AT, 0 },
            { Keyboard.Kanji, VirtualKey.VK_KANJI },
            { Keyboard.Stop, VirtualKey.VK_MEDIA_STOP },
            { Keyboard.AX, VirtualKey.VK_OEM_AX },
            { Keyboard.Unlabeled, VirtualKey.VK_NONAME },
            { Keyboard.NextTrack, VirtualKey.VK_MEDIA_NEXT_TRACK },
            { Keyboard.NumpadEnter, 0 },
            { Keyboard.RightControl, VirtualKey.VK_RCONTROL },
            { Keyboard.Mute, VirtualKey.VK_VOLUME_MUTE },
            { Keyboard.Calculator, 0 },
            { Keyboard.PlayPause, VirtualKey.VK_MEDIA_PLAY_PAUSE },
            { Keyboard.MediaStop, 0 },
            { Keyboard.VolumeDown, VirtualKey.VK_VOLUME_DOWN },
            { Keyboard.VolumeUp, VirtualKey.VK_VOLUME_UP },
            { Keyboard.WebHome, VirtualKey.VK_BROWSER_HOME },
            { Keyboard.NumpadComma, 0 },
            { Keyboard.NumpadDivide, VirtualKey.VK_DIVIDE },
            { Keyboard.SysRQ, VirtualKey.VK_SNAPSHOT },
            { Keyboard.RightAlt, VirtualKey.VK_RMENU },
            { Keyboard.Pause, VirtualKey.VK_PAUSE },
            { Keyboard.Home, VirtualKey.VK_HOME },
            { Keyboard.UpArrow, VirtualKey.VK_UP },
            { Keyboard.PageUp, VirtualKey.VK_PRIOR },
            { Keyboard.LeftArrow, VirtualKey.VK_LEFT },
            { Keyboard.RightArrow, VirtualKey.VK_RIGHT },
            { Keyboard.End, VirtualKey.VK_END },
            { Keyboard.DownArrow, VirtualKey.VK_DOWN },
            { Keyboard.PageDown, VirtualKey.VK_NEXT },
            { Keyboard.Insert, VirtualKey.VK_INSERT },
            { Keyboard.Delete, VirtualKey.VK_DELETE },
            { Keyboard.LeftWin, VirtualKey.VK_LWIN },
            { Keyboard.RightWin, VirtualKey.VK_RWIN },
            { Keyboard.Apps, VirtualKey.VK_APPS },
            { Keyboard.Power, 0 },
            { Keyboard.Sleep, VirtualKey.VK_SLEEP },
            { Keyboard.Wake, 0 },
            { Keyboard.WebSearch, VirtualKey.VK_BROWSER_SEARCH },
            { Keyboard.WebFavourites, VirtualKey.VK_BROWSER_FAVORITES },
            { Keyboard.WebRefresh, VirtualKey.VK_BROWSER_REFRESH },
            { Keyboard.WebStop, VirtualKey.VK_BROWSER_STOP },
            { Keyboard.WebForward, VirtualKey.VK_BROWSER_FORWARD },
            { Keyboard.WebBack, VirtualKey.VK_BROWSER_BACK },
            { Keyboard.MyComputer, 0 },
            { Keyboard.Mail, VirtualKey.VK_LAUNCH_MAIL },
            { Keyboard.MediaSelect, VirtualKey.VK_LAUNCH_MEDIA_SELECT },
            { Keyboard.GreenModifier, 0 },
            { Keyboard.OrangeModifier, 0 },
            { Keyboard.F16, VirtualKey.VK_F16 },
            { Keyboard.F17, VirtualKey.VK_F17 },
            { Keyboard.F18, VirtualKey.VK_F18 },
            { Keyboard.F19, VirtualKey.VK_F19 },
            { Keyboard.F20, VirtualKey.VK_F20 },
            { Keyboard.Section, 0 },
            { Keyboard.Menu, 0 },
            { Keyboard.Help, VirtualKey.VK_HELP },
            { Keyboard.Function, 0 },
            { Keyboard.Clear, VirtualKey.VK_CLEAR },
            { Keyboard.LeftCommand, 0 },
            { Keyboard.RightCommand, 0 },
        };

        public static bool TryGetKey(string keyName, string? keyboardLayout, bool enUSOverride, out VirtualKey key, INativeMethods nativeMethods)
        {
            if (_keys.TryGetValue(keyName, out key))
            {
                return true;
            }

            if (!Keyboard.TryGetKeyChar(keyName, out char c))
            {
                return false;
            }

            return OemKeyMappings.TryGetKey(keyboardLayout, c, enUSOverride, out key, nativeMethods);
        }

        private static string GetKeyName(char c)
        {
            _ = Keyboard.TryGetKeyName(c, out string? keyName);
            return keyName!;
        }
    }
}
