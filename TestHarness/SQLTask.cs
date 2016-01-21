﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Data.OleDb;

using CsvHelper;

namespace TestHarness
{

    public class SQLTaskResult
    {
        public string DataFileName { get; set; }

        public bool Equals(SQLTaskResult value)
        {
          
            var file1 = new StreamReader(DataFileName);
            var file2 = new StreamReader(value.DataFileName);

            var line1 = file1.ReadLine();
            var line2 = file2.ReadLine();
            while (line1 != null)
            {
                if (line1 != line2)
                {
                    file1.Close();
                    file2.Close();
                    return false;
                }

                line1 = file1.ReadLine();
                line2 = file2.ReadLine();
            }

            if (line2 == null)
                return true;
            else
                return false;

        }
    }

    public enum SQLRunMode { Execute, Query}

    public class SQLTask : ITask
    {
        public string SQLStatement { get; private set; }
        public SQLRunMode RunMode {get; private set; }
        public SQLTaskResult ExpectedResult;
        public SQLTaskResult ActualResult;

        public SQLTask(XmlNode xml, string directory)
        {
            SQLStatement = xml.SelectSingleNode("statement").InnerText;
            RunMode = SQLRunMode.Execute;

            ExpectedResult = new SQLTaskResult();
            ActualResult = null;

            var expectedResultNode = xml.SelectSingleNode("expectedresult");
            if (expectedResultNode != null)
            {
                var dataNode = expectedResultNode.SelectSingleNode("data");
                if (dataNode != null)
                {
                    RunMode = SQLRunMode.Query;
                    ExpectedResult.DataFileName = Path.Combine(directory, dataNode.Attributes["file"].Value);                    
                }
            }                                 
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, string outputFolder, StringWriter output, CancellationToken cancellationToken)
        {

            /* Do any replacement needed */
            var sqlStatement = SQLStatement;
            foreach (var variable in variables)
                sqlStatement = sqlStatement.Replace("%" + variable.Key + "%", variable.Value);

            output.WriteLine("Executing SQL {0}", sqlStatement);

            SQLQuery sqlQuery = new SQLQuery(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);
            if (RunMode == SQLRunMode.Execute)
            {

                var successfull = await sqlQuery.Execute(sqlStatement);

                // Output results
                if (successfull)
                    output.WriteLine("SQL successfully completed");
                else
                    output.WriteLine("SQL failed");

                return true;
   
            }
            else if (RunMode == SQLRunMode.Query)
            {
                ActualResult = new SQLTaskResult()
                {
                    DataFileName = Path.Combine(outputFolder, "data.csv")
                };

                var successfull = await sqlQuery.RunQuery(sqlStatement, ActualResult.DataFileName);

                // Output results
                if (successfull)
                    output.WriteLine("SQL successfully completed");
                else
                    output.WriteLine("SQL failed");

                if (ExpectedResult.Equals(ActualResult))
                    return true;
                else
                    return false;
            }

            return false;
        }
    }

    public class SQLQuery
    {
        private string _ConnectionString;

        public string Server { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string FileLibrary { get; private set; }

        public SQLQuery(string server, string user, string password, string fileLibrary)
        {
            Server = server;
            User = user;
            Password = password;
            FileLibrary = fileLibrary;

            // Setup OLEDB connection string
            var connectionStringBuilder = new OleDbConnectionStringBuilder();
            connectionStringBuilder["Provider"] = "IBMDA400";
            connectionStringBuilder["Data Source"] = Server;
            connectionStringBuilder["User Id"] = User;
            connectionStringBuilder["Password"] = Password;
            connectionStringBuilder["Library List"] = FileLibrary + ",PRECLP15,PRECPP15,QGPL,QTEMP"; 
            _ConnectionString = connectionStringBuilder.ConnectionString;
        }

        public async Task<bool> Execute(string sql)
        {
            var connection = new OleDbConnection(_ConnectionString);
            await connection.OpenAsync();

            OleDbCommand command = new OleDbCommand(sql, connection);
            var result = await command.ExecuteNonQueryAsync();

            connection.Close();

            return true; 
        }

        public async Task<bool> RunQuery(string sql, string outputFile)
        {
            var connection = new OleDbConnection(_ConnectionString);
            await connection.OpenAsync();

            OleDbCommand command = new OleDbCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();

            var csvFile = File.CreateText(outputFile);

            var csvWriter = new CsvWriter(csvFile);

            // Write out header
            for (var i = 0; i < reader.FieldCount; i++)
                csvWriter.WriteField(reader.GetName(i));
            csvWriter.NextRecord();

            while (await reader.ReadAsync())
            {
                for (var i = 0; i < reader.FieldCount; i++)
                    csvWriter.WriteField(reader.GetValue(i));

                csvWriter.NextRecord();
            }

            csvFile.Close();

            connection.Close();

            return true;                      
        }
    }
}
