using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnX
{
    public static class TypedCsvHelperExtensions
    {

        public static List<string> ToTypedCsv<TIn>(this IEnumerable<TIn> records, string filePath = "", string typeName = "", bool generateCodeFile = true, bool generateCsvFile = true)
        {
            if (!generateCodeFile && !generateCsvFile) return null;

            var type = typeof(TIn);

            if (typeName == "")
                typeName = type.Name;

            if (typeName.StartsWith("<>")) typeName = "AnonymousType";
            var path = "";

            if (filePath == "")
                path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                path = Path.GetDirectoryName(filePath);

            var codeFilePath = "";
            var csvFilePath = "";

            path = path.ToLower();
            var generateCodeType = ".linq";

            if (filePath.EndsWith(".csv"))
            {
                csvFilePath = filePath;
                codeFilePath = csvFilePath.Replace(".csv", ".linq");
            }
            else if (filePath.EndsWith(".cs"))
            {
                codeFilePath = filePath;
                csvFilePath = filePath.Replace(".cs", ".csv");
                generateCodeType = ".cs";
            }
            else if (filePath.EndsWith(".linq"))
            {
                codeFilePath = filePath;
                csvFilePath = filePath.Replace(".linq", ".csv");
            }
            else
            {
                if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    path = path + "\\" + typeName;
                }
                csvFilePath = path + ".csv";
                codeFilePath = path + generateCodeType;
            }

            Directory.CreateDirectory(path);
            var result = new List<String>();

            if (generateCsvFile)
            {
                TypedCsvHelper.WriteCsv(csvFilePath, records);
                result.Add(csvFilePath);
            }

            if (generateCodeFile)
            {
                var code = CodeGeneration.GenerateCode(records.First().GetType(), typeName, csvFilePath, generateCodeType == ".linq");
                File.WriteAllText(codeFilePath, code);
                result.Add(codeFilePath);
            }

            return result;

        }

    }
}
