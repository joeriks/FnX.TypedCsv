using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnX
{
    public static class TypedCsvHelper
    {
        public static IEnumerable<T> ReadCsv<T>(string filePath)
        {
            using (var reader = File.OpenText(filePath))
            {
                var csvReader = new CsvHelper.CsvReader(reader);
                return csvReader.GetRecords<T>();
            }
        }
        public static void WriteCsv<T>(string filePath, IEnumerable<T> records)
        {
            using (var textWriter = File.CreateText(filePath) as TextWriter)
            {
                var writer = new CsvHelper.CsvWriter(textWriter);
                writer.WriteRecords(records);
            }

        }
    }

}
