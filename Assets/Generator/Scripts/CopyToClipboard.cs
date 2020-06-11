using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CopyToClipboard
{
    public static void copyToClipboard(this string s)
    {
        TextEditor te = new TextEditor();
        te.text = s;
        te.SelectAll();
        te.Copy();
    }
}
