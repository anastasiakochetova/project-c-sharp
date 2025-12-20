using System;
using System.Collections.Generic;

namespace Autocomplete
{
    public class RightBorderTask
    {
        public static int GetRightBorderIndex(IReadOnlyList<string> phrases, string prefix, int left, int right)
        {
            while (right - left > 1)
            {
                int mid = left + (right - left) / 2;

                if (string.Compare(phrases[mid], prefix, StringComparison.OrdinalIgnoreCase) <= 0
                    || phrases[mid].StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    left = mid;
                }
                else
                {
                    right = mid;
                }
            }

            return right;
        }
    }
}
