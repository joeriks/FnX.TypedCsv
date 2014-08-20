using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FnX;
using System.Linq;

namespace FnX.TypedCsv.Tests
{
    public class TestType
    {
        public string Name { get; set; }
        public DateTime? RegDate { get; set; }
        public int Id { get; set; }
    }

    [TestClass]
    public class BasicFileWriteTests_NeedsManualApproval
    {

        public IEnumerable<TestType> getTestTypes()
        {
            var x = new List<TestType>();
            x.Add(new TestType { Name = "foo", Id = 0, RegDate = new DateTime(2010, 1, 1) });
            x.Add(new TestType { Name = "bar", Id = 1, RegDate = new DateTime(2011, 1, 1) });
            x.Add(new TestType { Name = "baz", Id = 2, RegDate = new DateTime(2012, 1, 1) });
            return x;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var x = getTestTypes();
            var fileName1 = x.ToTypedCsv(@"c:\admin\foo.csv");
            Assert.AreEqual(@"c:\admin\foo.csv",fileName1[0]);

            var fileName2 = x.ToTypedCsv(@"c:\admin\");
            Assert.AreEqual(@"c:\admin\TestType.csv", fileName2[0]);

            var fileName3 = x.ToTypedCsv(@"c:\admin\", "Foo");
            Assert.AreEqual(@"c:\admin\Foo.csv", fileName3[0]);

            var myDocumentsFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToLower();

            var fileName4 = x.ToTypedCsv();
            Assert.AreEqual(myDocumentsFolder + @"\TestType.csv".ToLower(), fileName4[0].ToLower());

            var fileName5 = x.ToTypedCsv(typeName: "TypeName");
            Assert.AreEqual(myDocumentsFolder + @"\TypeName.csv".ToLower(), fileName5[0].ToLower());

        }

        [TestMethod]
        public void AnonymousTypesLinq()
        {

            var y = getTestTypes().Select(t => new { t.Name, t.RegDate });

            var fileName1 = y.ToTypedCsv(@"c:\admin\AnonymousTypesLinq.linq");
            Console.WriteLine(fileName1);
        }

        [TestMethod]
        public void AnonymousTypesCs()
        {

            var y = getTestTypes().Select(t => new { t.Name, t.RegDate });

            var fileName1 = y.ToTypedCsv(@"c:\admin\AnonymousTypesCs.cs");
            Console.WriteLine(fileName1);
        }

        [TestMethod]
        public void Only_Create_Csv()
        {

            var y = getTestTypes().Select(t => new { t.Name, t.RegDate });

            var fileName1 = y.ToTypedCsv(@"c:\admin\Only_Create_Csv.csv", generateCodeFile: false);
            Assert.AreEqual(@"c:\admin\Only_Create_Csv.csv", fileName1);

        }

        [TestMethod]
        public void Create_Cs()
        {

            var y = getTestTypes().Select(t => new { t.Name, t.RegDate });

            var fileName1 = y.ToTypedCsv(@"c:\admin\Create_Cs.cs", generateCsvFile: false);

            Assert.AreEqual(@"c:\admin\Create_Cs.cs", fileName1);

        }

    }
}
