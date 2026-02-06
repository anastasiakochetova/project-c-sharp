using System;
using System.Collections.Generic;
using System.Linq;

public static class MedianFilterTask
{
    public static double[,] MedianFilter(double[,] img)
    {
        int h = img.GetLength(0), w = img.GetLength(1);
        double[,] res = new double[h, w];

        for (int i = 0; i < h; i++)
            for (int j = 0; j < w; j++)
            {
                var vals = new List<double>();
                for (int di = -1; di <= 1; di++)
                    for (int dj = -1; dj <= 1; dj++)
                    {
                        int ni = i + di, nj = j + dj;
                        if (ni >= 0 && ni < h && nj >= 0 && nj < w)
                            vals.Add(img[ni, nj]);
                    }
                vals.Sort();
                int n = vals.Count;
                res[i, j] = (n % 2 == 1) ? vals[n / 2] : (vals[n / 2 - 1] + vals[n / 2]) / 2.0;
            }

        return res;
    }
}
