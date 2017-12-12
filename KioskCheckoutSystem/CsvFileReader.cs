/*****************************************************************
Filename:       CsvFileReader.cs
Revised:        Date: 2017/02/23
Revision:       Revision: 1.0.0

Description:    To read data in csv file

Revision log:
* 2017-02-21: Created
******************************************************************/
using System;
using System.IO;

namespace KioskCheckoutSystem
{
    public class CsvFileReader : StreamReader
    {
        public CsvFileReader(Stream stream) : base(stream)
        {
        }

        public CsvFileReader(string filename) : base(filename)
        {
        }
        public bool ReadRow(CsvRow row)
        {
            try
            {
                row.LineText = ReadLine();
                if (string.IsNullOrEmpty(row.LineText))
                    return false;

                var pos = 0;
                var rows = 0;

                while (pos < row.LineText.Length)
                {
                    string value;

                    if (row.LineText[pos] == '"')
                    {
                        pos++;
                        var start = pos;
                        while (pos < row.LineText.Length)
                        {
                            if (row.LineText[pos] == '"')
                            {
                                pos++;

                                if (pos >= row.LineText.Length || row.LineText[pos] != '"')
                                {
                                    pos--;
                                    break;
                                }
                            }
                            pos++;
                        }
                        value = row.LineText.Substring(start, pos - start);
                        value = value.Replace("\"\"", "\"");
                    }
                    else
                    {
                        var start = pos;
                        while (pos < row.LineText.Length && row.LineText[pos] != ',')
                            pos++;
                        value = row.LineText.Substring(start, pos - start);
                    }

                    if (rows < row.Count)
                        row[rows] = value;
                    else
                        row.Add(value);
                    rows++;

                    while (pos < row.LineText.Length && row.LineText[pos] != ',')
                        pos++;
                    if (pos < row.LineText.Length)
                        pos++;
                }
                while (row.Count > rows)
                    row.RemoveAt(rows);
                return (row.Count > 0);
            }
            catch (Exception ex)
            {
                CollectError.CollectErrorToFile(ex, Program.ErrorFile);
                return false;
            }
        } // END: ReadRow(CsvRow row)
    }
}
