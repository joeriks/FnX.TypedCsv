####Short info
**Installation:** Nuget FnX.TypedCsv + add FnX namespace.

**Status:** Works on my machine(s).

**The problem it solves:** dump any result from a linq query to a csv-file and automatically generate type (class) and code to be able to get the data back strongly typed.

**When do I use it myself:** when a customer asks me to extract and combine complex data from sql and I need to test and explore to get the correct output. With this I only need to bother the database once, and yet get the strong types to get a nice linqpad experience.

**Alternative:** tsql queries + perhaps temporary tables.

**How much time did I spend creating it:** few hours.

**How much code:** ~50 LOCS.

**Tools:** LinqPad (or VS).

**Dependencies:** CsvHelper.


####More info

Useful when you want to dump a snapshot of data, to be able to process it further later with type information kept, for a nice linq experience. That way you don't need to fetch the data from your database more than necessary. Reading the csv-file snapshot is very fast (using the Nuget CsvHelper).

For example (in LinqPad, with AdventureWorks data):

    VEmployee.Take (100).ToTypedCsv()
    
Result:

    c:\users\jonas\documents\VEmployee.csv
    c:\users\jonas\documents\VEmployee.linq

The first file is the csv with the data. And the other one is a linq-file with a class generated from the query result IEnumerable + a function to read the csv file.

Simply copy the filename and open it in linqpad - then process it further with more linq.


**The generated class (reflection)**

    public class VEmployee {
        public Int32 BusinessEntityID {get;set;}
        public String Title {get;set;}
        public String FirstName {get;set;}
        ...


**The generated sample code to read the csv and get the IEnumerable<T>**
    
    void Main() {
    
        var vemployeeEnumerable=GetVEmployeeRecords(@"c:\users\jonas\documents\VEmployee.csv");
        
        vemployeeEnumerable.Dump();	        
        
    }
    
    
    public IEnumerable<VEmployee> GetVEmployeeRecords(string filePath){
        using (var reader = File.OpenText(filePath)) {
            var csvReader = new CsvHelper.CsvReader(File.OpenText(filePath));
            return csvReader.GetRecords<VEmployee>();
	    }
        }	
    

####Options

You can specify the path (csv gets same path, but .csv extension):

    VEmployee.Take (100).ToTypedCsv(@"c:\data\mydata.linq") 

It works with anonymous types aswell (they also get translated to regular types, with the inferred property types)

You can also run it in Visual Studio (or anywhere) and generate cs-file instead of a linq:

    VEmployee.Take (100).ToTypedCsv(@"c:\data\mydata.cs") 


####Known limitations
Does only work with flat (csv) data (no complex types).
