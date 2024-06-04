
using Microsoft.Data.SqlClient;
using System.Data;

class PrograAdo
{
    static void MainAdo()
    {
        string connectionString = GetConnectionString();
        using SqlConnection connection = new(connectionString);
        // Connect to the database then retrieve the schema information.  
        connection.Open();
        DataTable table = connection.GetSchema("Tables");

        // Display the contents of the table.  
        DisplayData(table);
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    private static string GetConnectionString()
    {
        // To avoid storing the connection string in your code,  
        // you can retrieve it from a configuration file.  
        return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SaaSPlatform;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server " +
            "Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    }

    private static void DisplayData(System.Data.DataTable table)
    {
        foreach (System.Data.DataRow row in table.Rows)
        {
            foreach (System.Data.DataColumn col in table.Columns)
            {
                Console.WriteLine("{0} = {1}, type: {2}", col.ColumnName, row[col], col.DataType.Name);
            }
            Console.WriteLine("============================");
        }
    }
}