using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempClass
{
    // Start is called before the first frame update
    private static float value_width = 20;
    private static float value_height = 20;
    private static int factionCount = 2;
    private static bool loadLevel = true;

    public static void set(float convert_value_width, float convert_value_height, int convert_factionCount, bool convert_loadLevel)
    {
        value_width = convert_value_width;
        value_height = convert_value_height;
        factionCount = convert_factionCount;
        loadLevel = convert_loadLevel;
    }

    public static bool get_loadLevel()
    {
        return loadLevel;
    }

    public static float get_width()
    {
        return value_width;
    }

    public static float get_height()
    {
        return value_height;
    }

    public static int get_factionCount()
    {
        return factionCount;
    }
}
