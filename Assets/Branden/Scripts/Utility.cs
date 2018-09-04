using UnityEngine;
using System.Collections;

//Note: If using with textmesh pro, don't forget to set "parse escape characters"
public static class Utility {
    public static string AddNewLines(string str, int charLimit) {
        string newStr = "";
        for (int i = 0, numCharLeft = str.Length, count = str.Length;
            (i * charLimit) < count; ++i, numCharLeft -= charLimit) {

            int numChar = Mathf.Min(charLimit, numCharLeft);
            newStr += (i == 0) ? str.Substring(i * charLimit, numChar) :
                 "\n" + str.Substring(i * charLimit, numChar);
        }

        return newStr;
    }
}