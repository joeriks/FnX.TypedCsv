using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FnX
{
    public static class CodeGeneration
    {
        public static string GenerateCode(this Type type, string typeName, string filePath, bool addLinqPadHeader)
        {

            var t = new StringBuilder();

            if (addLinqPadHeader)
            {
                t.AppendLine(@"<Query Kind=""Program"">
                    <NuGetReference>CsvHelper</NuGetReference>
                </Query>");
            }

            var listName = typeName.ToLower() + "Enumerable";

            t.AppendLine("// Generated " + DateTime.Now.ToString());

            t.AppendLine("void Main() {");
            t.AppendLine("    var " + listName + @"=Get" + typeName + @"Records(@""" + filePath + @""");");
            t.AppendLine("    " + listName + ".Dump();");

            t.AppendLine("}");

            t.AppendLine(@"public IEnumerable<" + typeName + "> Get" + typeName + "Records(string filePath){");
            t.AppendLine(@"    using (var reader = File.OpenText(filePath)) {");
            t.AppendLine(@"    var csvReader = new CsvHelper.CsvReader(File.OpenText(filePath));");
            t.AppendLine(@"    return csvReader.GetRecords<" + typeName + @">();");
            t.AppendLine("    }");
            t.AppendLine("}");


            t.AppendLine("public class " + typeName + " {");

            type.GetFields().Select(x => new { Name = x.Name, Type = (x.FieldType.IsGenericType && x.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(x.FieldType).Name + "?" : x.FieldType.Name }).ToList().ForEach(row =>
            {
                t.AppendLine("    public " + row.Type + " " + row.Name + " {get;set;}");
            });

            type.GetProperties().Select(x => new { Name = x.Name, Type = (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) ? Nullable.GetUnderlyingType(x.PropertyType).Name + "?" : x.PropertyType.Name }).ToList().ForEach(row =>
            {
                t.AppendLine("    public " + row.Type + " " + row.Name + " {get;set;}");
            });

            t.AppendLine("}");


            return t.ToString();

        }

    }
}
