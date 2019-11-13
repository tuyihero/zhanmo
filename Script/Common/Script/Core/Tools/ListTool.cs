using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ListTool
{

    public static void ExcludeEmpty(IList list)
    {
        list.Remove(null);
    }

    public static void ExcludeEmptyStr(IList list)
    {
        list.Remove("");
        list.Remove(null);
    }
}

