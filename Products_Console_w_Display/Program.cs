// See https://aka.ms/new-console-template for more information


using System;
using System.Data;
using Npgsql;

class Sample
{
    static void Main(string[] args)
    {
        // Connect to a PostgreSQL database
        NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1:5432;User Id=postgres; " +
           "Password=Pixelmon2001;Database=prods;");
        conn.Open();

        // Define a query returning a single row result set
        NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM product", conn);

        // Execute the query and obtain the value of the first column of the first row
        //Int64 count = (Int64)command.ExecuteScalar();
        NpgsqlDataReader reader = command.ExecuteReader();

        DataTable dt = new DataTable();
        dt.Load(reader);

        print_results(dt);

        //second print setup

        
        NpgsqlCommand command1 = new NpgsqlCommand("SELECT * FROM customer", conn);

        NpgsqlDataReader reader1 = command1.ExecuteReader();

        DataTable dt1 = new DataTable();
        dt1.Load(reader1);

        print_results1(dt1);

        conn.Close();
    }


    static void print_results(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();

        //data.Columns.Remove("prod_id");
        //if DataColumn is prod_id

        //for DataColumn col data.Columns(prod_id)
        foreach (DataColumn col in data.Columns)
        {
            if (col.ColumnName is "prod_id" or "prod_desc" or "prod_quantity")
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();



       foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < /*dataRow.ItemArray.Length*/ 3; j++)
            {
                if (30 >= Convert.ToInt32(dataRow.ItemArray[2])) /*12 <= Convert.ToUInt32(dataRow.ItemArray[2])*/
                    if (12 <= Convert.ToUInt32(dataRow.ItemArray[2]))
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        } 
    }

    static void print_results1(DataTable data)
    {
        Console.WriteLine();
        Dictionary<string, int> colWidths = new Dictionary<string, int>();


        Dictionary<string, decimal> rep_sum = new Dictionary<string, decimal>();
        decimal sum; 
        foreach (DataRow dataRow in data.Rows)
        {
            if (!rep_sum.ContainsKey(dataRow["rep_id"].ToString())) //the if sees if the rep_id is in dict
            {
                sum = (decimal)dataRow["cust_balance"];
                rep_sum.Add(dataRow["rep_id"].ToString(), sum); //adds rep_id if unique 
                //Console.WriteLine("ding");
            }
            else
            {
                sum = rep_sum[dataRow["rep_id"].ToString()] + (decimal)dataRow["cust_balance"];
                rep_sum[dataRow["rep_id"].ToString()] = sum; //adds sum
                //Console.WriteLine("dong");
            }

        }

        Console.WriteLine("rep_id" + "   " + "cust_bal_sum");

        foreach (var item in rep_sum)
        {
            if (item.Value > 12000)
            {
                Console.WriteLine(item.Key + "   " + item.Value);
            }
        }

        //data.Columns.Add("bal_sum");

        /*foreach (DataColumn col in data.Columns)
        {
            //if (col.ColumnName is "rep_id" or "cust_balance")
            Console.Write(col.ColumnName);
            var maxLabelSize = data.Rows.OfType<DataRow>()
                    .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                    .OrderByDescending(m => m).FirstOrDefault();

            colWidths.Add(col.ColumnName, maxLabelSize);
            for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 14; i++) Console.Write(" ");
        }

        Console.WriteLine();



        foreach (DataRow dataRow in data.Rows)
        {
            for (int j = 0; j < /*dataRow.ItemArray.Length 3; j++)
            {
                
                Console.Write(dataRow.ItemArray[j]);
                for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 14; i++) Console.Write(" ");
            }
            Console.WriteLine();
        }*/
    }
}

