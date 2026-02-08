using System;
using System.Security.Cryptography;
using System.Text;

namespace KsbReportTool.Services
{
    public static class StableRandom
    {
        public static int SeedFromString(string input)
        {
            using (var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToInt32(data, 0);
            }
        }

        public static Random Create(string seedText)
        {
            int seed = SeedFromString(seedText);
            return new Random(seed);
        }
    }
}
