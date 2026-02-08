using System;

namespace KsbReportTool.Services
{
    public static class ExcelAddress
    {
        public static int ColumnToIndex(string col)
        {
            int sum = 0;
            for (int i = 0; i < col.Length; i++)
            {
                sum *= 26;
                sum += (col[i] - 'A' + 1);
            }
            return sum;
        }

        public static void Parse(string addr, out int row, out int col)
        {
            int i = 0;
            while (i < addr.Length && char.IsLetter(addr[i])) i++;
            string colPart = addr.Substring(0, i).ToUpperInvariant();
            string rowPart = addr.Substring(i);
            col = ColumnToIndex(colPart);
            row = int.Parse(rowPart);
        }
    }
}
