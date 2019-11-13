using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Kent.Boogaart.KBCsv;

namespace Tables
{
    public class TableWriter 
    {
        public static void WriteTable(TableFileBase tableFile)
        {
            string path = "./Assets/Resources/Tables/" + tableFile.GetType().Name + ".csv";
            string[] lines = File.ReadAllLines(path);
            lines[0] = lines[0].Replace("\r\n", "\n");

            StringReader rdr = new StringReader(string.Join("\n", lines));
            List<string> titles = new List<string>();
            using (var reader = new CsvReader(rdr))
            {
                HeaderRecord header = reader.ReadHeaderRecord();
                for (int i = 0; i < header.Count; ++i)
                {
                    titles.Add(header[i]);
                }
            }

            File.Delete(path);

            using (StreamWriter SourceStream = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                CsvWriter writer = new CsvWriter(SourceStream);
                writer.WriteRecord(titles.ToArray());
                foreach (var record in tableFile.Records)
                {
                    writer.WriteRecord(record.Value.GetRecordStr());
                }
            }
        }
    }
}
