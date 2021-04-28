using System;

public class TDRandom
{
    private static Random random = new Random(0);

    public static int RandomInt(int maxValue = 2)
    {
        return random.Next(maxValue);
    }

    public static int RandomInt(int minValue, int maxValue)
    {
        return minValue + RandomInt(maxValue - minValue);
    }

    public static float RandomFloat(float maxValue = 1f)
    {
        return maxValue * (float)random.NextDouble();
    }

    public static float RandomFloat(float minValue, float maxValue)
    {
        return minValue + (maxValue - minValue) * RandomFloat();
    }
}