using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace KsbReportTool.Services
{
    public class KsbProcessResult
    {
        public int GeneratedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> SkippedItems { get; set; } = new List<string>();
        public List<string> AllLogs { get; set; } = new List<string>();
    }

    public static class KsbProcessor
    {
        public static KsbProcessResult Process(string table1Path, string table2Path, string templatePath, string outputDir, bool makeZip)
        {
            var result = new KsbProcessResult();
            var logs = new List<string>();

            var table2Index = BuildTable2Index(table2Path, logs);
            if (table2Index == null || table2Index.Count == 0)
            {
                logs.Add("表2解析失败或无有效数据。");
                result.AllLogs = logs;
                return result;
            }

            using (var fs1 = File.OpenRead(table1Path))
            {
                var wb1 = new XSSFWorkbook(fs1);
                for (int i = 0; i < wb1.NumberOfSheets; i++)
                {
                    var sheet = wb1.GetSheetAt(i);
                    if (sheet == null) continue;
                    string serial = sheet.SheetName.Trim();

                    if (IsEmptySheet(sheet))
                    {
                        result.SkippedCount++;
                        result.SkippedItems.Add(serial + " : empty_sheet");
                        continue;
                    }

                    if (!table2Index.ContainsKey(serial))
                    {
                        result.SkippedCount++;
                        result.SkippedItems.Add(serial + " : no_mapping");
                        continue;
                    }

                    using (var fsTemplate = File.OpenRead(templatePath))
                    {
                        var wbT = new XSSFWorkbook(fsTemplate);
                        var wsT = wbT.GetSheet(KsbRules.DataSheetName);
                        if (wsT == null)
                        {
                            result.SkippedCount++;
                            result.SkippedItems.Add(serial + " : no_mapping");
                            continue;
                        }

                        var t2 = table2Index[serial];
                        WriteTextIfAllowed(wsT, "G8", t2.Tag);
                        WriteTextIfAllowed(wsT, "G9", t2.Sap);
                        WriteTextIfAllowed(wsT, "B10", t2.Model);
                        WriteTextIfAllowed(wsT, "B11", t2.SapLine);
                        WriteTextIfAllowed(wsT, "B12", "OH2");

                        WriteIfAllowed(wsT, "G10", SpeedStage(ReadCell(sheet, "C44")));

                        WriteIfAllowed(wsT, "B20", ReadCell(sheet, "K43"));
                        WriteIfAllowed(wsT, "B21", ReadCell(sheet, "G44"));
                        WriteIfAllowed(wsT, "G20", ReadCell(sheet, "K42"));
                        WriteIfAllowed(wsT, "G21", ReadCell(sheet, "G45"));
                        WriteIfAllowed(wsT, "B28", ReadCell(sheet, "C42"));
                        WriteIfAllowed(wsT, "G28", ReadCell(sheet, "C43"));
                        WriteIfAllowed(wsT, "G29", ReadCell(sheet, "C44"));
                        WriteIfAllowed(wsT, "B30", ReadCell(sheet, "C45"));
                        WriteIfAllowed(wsT, "B31", ReadCell(sheet, "G42"));
                        WriteIfAllowed(wsT, "B39", ReadCell(sheet, "N42"));
                        WriteIfAllowed(wsT, "G39", ReadCell(sheet, "N43"));
                        WriteIfAllowed(wsT, "B49", ReadCell(sheet, "G43"));
                        WriteIfAllowed(wsT, "G30", ReadCell(sheet, "R45"));
                        WriteIfAllowed(wsT, "G34", null);
                        WriteIfAllowed(wsT, "I222", ReadCell(sheet, "J29"));

                        var inVals = ReadRange(sheet, "H51", "H57");
                        var outVals = ReadRange(sheet, "I51", "I57");
                        var pairs = ConvertPressurePairs(inVals, outVals);
                        WriteRow(wsT, 63, pairs.Item1);
                        WriteRow(wsT, 64, pairs.Item2);

                        WriteRow(wsT, 60, ReadRange(sheet, "F51", "F57"));
                        WriteRow(wsT, 65, ReadRange(sheet, "G51", "G57"));
                        WriteRow(wsT, 67, ReadRange(sheet, "D51", "D57"));
                        WriteRow(wsT, 172, ReadRange(sheet, "K51", "K57"));
                        WriteRow(wsT, 174, ReadRange(sheet, "J51", "J57"));
                        WriteRow(wsT, 180, ReadRange(sheet, "M51", "M57"));
                        WriteRow(wsT, 185, ReadRange(sheet, "N51", "N57"));
                        WriteRow(wsT, 186, ReadRange(sheet, "Q51", "Q57"));
                        WriteRow(wsT, 187, ReadRange(sheet, "S51", "S57"));
                        WriteRow(wsT, 201, ReadRange(sheet, "O51", "O57"));
                        WriteRow(wsT, 203, ReadRange(sheet, "P51", "P57"));
                        WriteRow(wsT, 207, ReadRange(sheet, "Q51", "Q57"));
                        WriteRow(wsT, 209, ReadRange(sheet, "S51", "S57"));

                        var n45 = ReadCell(sheet, "N45");
                        var k44 = ReadCell(sheet, "K44");
                        WriteRow(wsT, 173, RepeatValue(n45, 7));
                        WriteRow(wsT, 182, RepeatValue(k44, 7));

                        var testDate = ParseExcelDate(ReadCell(sheet, "J29")) ?? DateTime.Today;
                        var tempsPress = GenWaterTempAndPressure(serial, testDate);
                        WriteRow(wsT, 61, tempsPress.Item1.Cast<object>().ToList());
                        WriteRow(wsT, 62, tempsPress.Item2.Cast<object>().ToList());

                        var densities = tempsPress.Item1.Select(t => (object)Math.Round(WaterDensity(t), 3)).ToList();
                        WriteRow(wsT, 170, densities);

                        var outPath = Path.Combine(outputDir, serial + "_KSB初始性能报告.xlsx");
                        using (var fsOut = File.Create(outPath))
                        {
                            wbT.Write(fsOut);
                        }
                        result.GeneratedCount++;
                    }
                }
            }

            if (makeZip)
            {
                var zipPath = Path.Combine(outputDir, "KSB初始性能报告_批量输出.zip");
                if (File.Exists(zipPath)) File.Delete(zipPath);
                ZipFile.CreateFromDirectory(outputDir, zipPath);
            }

            result.AllLogs = logs;
            return result;
        }

        private static bool IsEmptySheet(ISheet sheet)
        {
            string[] keyCells = { "C42", "C43", "C44", "C45", "J29", "R45", "K43", "G44" };
            foreach (var addr in keyCells)
            {
                var v = ReadCell(sheet, addr);
                if (!IsEmpty(v)) return false;
            }

            for (int r = 1; r <= 60; r++)
            {
                var row = sheet.GetRow(r - 1);
                if (row == null) continue;
                for (int c = 0; c < 20; c++)
                {
                    var cell = row.GetCell(c);
                    if (cell != null && !IsEmpty(GetCellValue(cell)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static Dictionary<string, Table2Row> BuildTable2Index(string table2Path, List<string> logs)
        {
            var index = new Dictionary<string, Table2Row>();
            using (var fs = File.OpenRead(table2Path))
            {
                var wb = new XSSFWorkbook(fs);
                var ws = wb.GetSheetAt(0);
                if (ws == null) return index;

                int headerRow = FindHeaderRow(ws, "出厂编号");
                if (headerRow < 0) return index;

                var colMap = new Dictionary<string, int>();
                int maxCol = ws.GetRow(headerRow).LastCellNum;
                for (int c = 0; c < maxCol; c++)
                {
                    var cell = ws.GetRow(headerRow).GetCell(c);
                    var val = cell != null ? GetCellValue(cell) as string : null;
                    if (string.IsNullOrEmpty(val)) continue;
                    if (val == "出厂编号") colMap["serial"] = c;
                    if (val == "位号" || val == "设备位号") colMap["tag"] = c;
                    if (val == "SAP号" || val == "SAP 号" || val == "SAP编号" || val == "SAP 编号") colMap["sap"] = c;
                    if (val == "KSB型号" || val == "KSB 型号" || val == "泵型号") colMap["model"] = c;
                    if (val == "SAP-行号" || val == "SAP 行号" || val == "KSB系列号" || val == "系列号") colMap["sap_line"] = c;
                }

                if (!colMap.ContainsKey("serial") || !colMap.ContainsKey("tag") || !colMap.ContainsKey("sap") || !colMap.ContainsKey("model") || !colMap.ContainsKey("sap_line"))
                {
                    logs.Add("表2缺少必要字段。");
                    return index;
                }

                for (int r = headerRow + 1; r <= ws.LastRowNum; r++)
                {
                    var row = ws.GetRow(r);
                    if (row == null) continue;
                    var serial = ToText(row.GetCell(colMap["serial"]));
                    if (IsEmpty(serial)) continue;

                    var item = new Table2Row
                    {
                        Serial = serial,
                        Tag = ToText(row.GetCell(colMap["tag"])),
                        Sap = ToText(row.GetCell(colMap["sap"])),
                        Model = ToText(row.GetCell(colMap["model"])),
                        SapLine = ToText(row.GetCell(colMap["sap_line"]))
                    };
                    index[serial] = item;
                }
            }
            return index;
        }

        private static int FindHeaderRow(ISheet ws, string header)
        {
            for (int r = 0; r < 5 && r <= ws.LastRowNum; r++)
            {
                var row = ws.GetRow(r);
                if (row == null) continue;
                for (int c = 0; c < row.LastCellNum; c++)
                {
                    var cell = row.GetCell(c);
                    var v = cell != null ? GetCellValue(cell) as string : null;
                    if (!string.IsNullOrEmpty(v) && v.Trim() == header) return r;
                }
            }
            return -1;
        }

        private static object ReadCell(ISheet sheet, string addr)
        {
            ExcelAddress.Parse(addr, out int row, out int col);
            var r = sheet.GetRow(row - 1);
            if (r == null) return null;
            var c = r.GetCell(col - 1);
            if (c == null) return null;
            return GetCellValue(c);
        }

        private static List<object> ReadRange(ISheet sheet, string start, string end)
        {
            ExcelAddress.Parse(start, out int r1, out int c1);
            ExcelAddress.Parse(end, out int r2, out int c2);
            var list = new List<object>();
            for (int r = r1; r <= r2; r++)
            {
                var row = sheet.GetRow(r - 1);
                for (int c = c1; c <= c2; c++)
                {
                    var cell = row != null ? row.GetCell(c - 1) : null;
                    list.Add(cell != null ? GetCellValue(cell) : null);
                }
            }
            return list;
        }

        private static void WriteIfAllowed(ISheet sheet, string addr, object value)
        {
            ExcelAddress.Parse(addr, out int row, out int col);
            var r = sheet.GetRow(row - 1) ?? sheet.CreateRow(row - 1);
            var c = r.GetCell(col - 1) ?? r.CreateCell(col - 1);

            if (c.CellType == CellType.Formula) return;
            if (!IsEmpty(GetCellValue(c))) return;

            if (value == null)
            {
                c.SetCellType(CellType.Blank);
                return;
            }

            if (value is DateTime)
            {
                c.SetCellValue((DateTime)value);
            }
            else if (value is double || value is int || value is float)
            {
                c.SetCellValue(Convert.ToDouble(value));
            }
            else
            {
                c.SetCellValue(value.ToString());
            }
        }

        private static void WriteTextIfAllowed(ISheet sheet, string addr, string value)
        {
            ExcelAddress.Parse(addr, out int row, out int col);
            var r = sheet.GetRow(row - 1) ?? sheet.CreateRow(row - 1);
            var c = r.GetCell(col - 1) ?? r.CreateCell(col - 1);

            if (c.CellType == CellType.Formula) return;
            if (!IsEmpty(GetCellValue(c))) return;

            c.SetCellType(CellType.String);
            c.SetCellValue(value ?? string.Empty);
        }

        private static void WriteRow(ISheet sheet, int row, List<object> values)
        {
            for (int i = 0; i < values.Count; i++)
            {
                var addr = (char)('C' + i) + row.ToString(CultureInfo.InvariantCulture);
                WriteIfAllowed(sheet, addr, values[i]);
            }
        }

        private static bool IsEmpty(object v)
        {
            if (v == null) return true;
            if (v is string) return string.IsNullOrWhiteSpace((string)v);
            return false;
        }

        private static object GetCellValue(ICell cell)
        {
            if (cell == null) return null;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(cell))
                    {
                        return cell.DateCellValue;
                    }
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    return cell.CellFormula;
                default:
                    return cell.ToString();
            }
        }

        private static string ToText(ICell cell)
        {
            if (cell == null) return string.Empty;
            if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString("0", CultureInfo.InvariantCulture);
            }
            if (cell.CellType == CellType.String) return cell.StringCellValue.Trim();
            if (cell.CellType == CellType.Formula) return cell.StringCellValue;
            return cell.ToString().Trim();
        }

        private static int SpeedStage(object val)
        {
            if (val == null) return 0;
            string s = val.ToString().Trim();
            if (s.StartsWith("2")) return 2;
            if (s.StartsWith("1")) return 4;
            if (s.StartsWith("9")) return 6;
            return 0;
        }

        private static Tuple<List<object>, List<object>> ConvertPressurePairs(List<object> inlet, List<object> outlet)
        {
            var inOut = new List<object>();
            var outOut = new List<object>();
            for (int i = 0; i < inlet.Count && i < outlet.Count; i++)
            {
                var inVal = ToDouble(inlet[i]);
                var outVal = ToDouble(outlet[i]);
                if (outVal.HasValue && Math.Abs(outVal.Value) > 20.0)
                {
                    if (inVal.HasValue) inVal = inVal.Value / 100.0;
                    outVal = outVal.Value / 100.0;
                }
                inOut.Add(inVal.HasValue ? (object)inVal.Value : null);
                outOut.Add(outVal.HasValue ? (object)outVal.Value : null);
            }
            return Tuple.Create(inOut, outOut);
        }

        private static double? ToDouble(object v)
        {
            if (v == null) return null;
            if (v is double) return (double)v;
            if (v is float) return Convert.ToDouble(v);
            if (v is int) return Convert.ToDouble(v);
            double d;
            if (double.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d)) return d;
            return null;
        }

        private static DateTime? ParseExcelDate(object v)
        {
            if (v is DateTime) return (DateTime)v;
            return null;
        }

        private static Tuple<List<double>, List<double>> GenWaterTempAndPressure(string serial, DateTime date)
        {
            var rnd = StableRandom.Create(serial);
            double baseline = MonthBaseline(date.Month);

            var temps = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                double t = baseline + rnd.NextDouble() * 0.5;
                t = Clamp(t, 0, 25);
                temps.Add(Math.Round(t, 2));
            }

            var p = new List<double>();
            for (int i = 0; i < 7; i++)
            {
                double v = 1.013 + rnd.NextDouble() * 0.005;
                p.Add(Math.Round(v, 5));
            }

            return Tuple.Create(temps, p);
        }

        private static double MonthBaseline(int month)
        {
            if (month == 12 || month == 1 || month == 2) return 11.5;
            if (month == 3 || month == 4) return 13.5;
            if (month == 5 || month == 6) return 18.0;
            if (month == 7 || month == 8) return 22.0;
            if (month == 9 || month == 10) return 19.0;
            return 15.0;
        }

        private static double Clamp(double v, double lo, double hi)
        {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }

        private static double WaterDensity(double t)
        {
            double rho = 999.842594 + 6.793952e-2 * t - 9.09529e-3 * t * t
                + 1.001685e-4 * Math.Pow(t, 3) - 1.120083e-6 * Math.Pow(t, 4) + 6.536332e-9 * Math.Pow(t, 5);
            return rho;
        }

        private static List<object> RepeatValue(object v, int count)
        {
            var list = new List<object>();
            for (int i = 0; i < count; i++) list.Add(v);
            return list;
        }

        private class Table2Row
        {
            public string Serial;
            public string Tag;
            public string Sap;
            public string Model;
            public string SapLine;
        }
    }
}
