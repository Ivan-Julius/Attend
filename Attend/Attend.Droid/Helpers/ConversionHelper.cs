

using System;

namespace Attend.Droid.Helpers
{
    class ConversionHelper
    {
        public static int GetSize(float pixelValue, float density)
        {
            return (int)((pixelValue) / density);
        }

        public static double GetScreenSizeInInches(float width, float height)
        {
            return Math.Sqrt(Math.Pow(width, 2) + Math.Pow(height, 2));
        }
    }
}