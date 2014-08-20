####Nuget : FnX.TypedCsv 

####Basic idea 
Dump any result from a linq query to a csv-file and automatically generate type (class) and code to be able to get the data back strongly typed.

Useful when you want to dump a snapshot of data, to be able to process it further later with type information kept, for a nice linq experience. That way you don't need to fetch the data from your database more than necessary. Reading the csv-file snapshot is very fast (using the Nuget CsvHelper).

For example (in LinqPad, with AdventureWorks data):

    VEmployee.Take (100).ToTypedCsv()
    
(First install the nuget FnX.TypedCsv in linqpad, and add the FnX namespace.)    

####Result:

    c:\users\jonas\documents\VEmployee.csv
    c:\users\jonas\documents\VEmployee.linq

The first file is the csv with the data. And the other one is a linq-file with class and function to read the csv file.
Simply copy the filename and open it in linqpad - then process it further with more linq:

    // Generated 2014-08-20 08:08:41

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
    
    public class VEmployee {
        public Int32 BusinessEntityID {get;set;}
        public String Title {get;set;}
        public String FirstName {get;set;}
        ...
        

Naturally you can specify the path (csv gets same path, but .csv extension):

    VEmployee.Take (100).ToTypedCsv(@"c:\data\mydata.linq") 

It works with anonymous types aswell (they also get translated to regular types, with the inferred property types)

You can also run it in Visual Studio (or anywhere) and generate cs-file instead of a linq:

    VEmployee.Take (100).ToTypedCsv(@"c:\data\mydata.cs") 


####Known limitations
Does only work with flat (csv) data (no complex types).
