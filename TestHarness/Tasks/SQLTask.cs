using System;
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
            return FileComparer.Compare(DataFileName, value.DataFileName);       
        }
    }

    public enum SQLRunMode { Execute, Query}

    public class SQLTask : ITask
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Description
        {
            get
            {
                if (RunMode == SQLRunMode.Query)
                    return "Execute SQL query";
                else
                    return "Execute SQL statement";
            }
        }
        public string Message { get; private set; }
        public TaskResult Result { get; private set; }

        public string SQLStatement { get; set; }
        public SQLRunMode RunMode { get; set; }
        public SQLTaskResult ExpectedResult;
        public SQLTaskResult ActualResult;

        public SQLTask()
        {
            Message = "";
            Result = TaskResult.NotRun;
            RunMode = SQLRunMode.Execute;

            ExpectedResult = new SQLTaskResult();
            ActualResult = null;
        }

        public async Task<bool> RunAsync(Dictionary<string, string> variables, TestOutputFileNameGenerator fileNameGenerator, CancellationToken cancellationToken, IProgress<TestProgress> progress)
        {
            try
            {
                Result =  TaskResult.InProgress;
                Message = "";

                if (progress != null)
                {
                    progress.Report(new TestProgress(Id, TestResult.InProgress));
                }

                SQLQuery sqlQuery = new SQLQuery(variables["SERVER"], variables["USER"], variables["PASSWORD"], variables["FILELIBRARY"]);
                if (RunMode == SQLRunMode.Execute)
                {

                    var successfull = await sqlQuery.Execute(SQLStatement);

                    Result = TaskResult.Passed;

                    if (progress != null)
                    {
                        progress.Report(new TestProgress(Id, TestResult.Passed));
                    }

                    return true;

                }
                else if (RunMode == SQLRunMode.Query)
                {
                    ActualResult = new SQLTaskResult()
                    {
                        DataFileName = fileNameGenerator.GetOutputFileName("data", "csv")                        
                    };

                    var successfull = await sqlQuery.RunQuery(SQLStatement, ActualResult.DataFileName);

                    // Output results
                    if (successfull)
                    {
                        if (ExpectedResult.Equals(ActualResult))
                            Result = TaskResult.Passed;
                        else
                        {
                            Message = "Result did not match expected result";
                            Result = TaskResult.Failed;
                        }
                    }
                    else
                        Result = TaskResult.Failed;

                }

                if (progress != null)
                {
                    if (Result == TaskResult.Passed)
                        progress.Report(new TestProgress(Id, TestResult.Passed));
                    else
                        progress.Report(new TestProgress(Id, TestResult.Failed));
                }

                return (Result == TaskResult.Passed);
            }
            catch (Exception e)
            {
                Result = TaskResult.ExceptionOccurred;
                Message = "An exception occurred: " + e.Message;


                if (progress != null)
                {
                    progress.Report(new TestProgress(Id, TestResult.Failed));
                }

                return false;
            }
        }

        public void ViewResult()
        {

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
            connectionStringBuilder["Naming Convention"] = 1;  // System naming
            _ConnectionString = connectionStringBuilder.ConnectionString;
        }

        private async Task <string> GetLibraryList(OleDbConnection connection, string fileLibrary, string user)
        {
            string libraryList = "";

            var sql = string.Format("SELECT INLLBL FROM {0}.PAYUSR WHERE USRPRF = '{1}'", fileLibrary, user);
            OleDbCommand command = new OleDbCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                libraryList = reader.GetString(0);
            
            reader.Close();

            return libraryList;
        }

        private async Task<int> SetLibraryList(OleDbConnection connection)
        {
            var libraryList = await GetLibraryList(connection, FileLibrary, User);
            if (libraryList == "")
                libraryList = await GetLibraryList(connection, FileLibrary, "*ANY");

            var clCommand = string.Format("'CHGLIBL LIBL({0})'", libraryList);
            var sql = string.Format("CALL QSYS/QCMDEXC({0}, {1:0000000000.00000})", clCommand, clCommand.Length - 2);
            OleDbCommand command = new OleDbCommand(sql, connection);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<bool> Execute(string sql)
        {
            var connection = new OleDbConnection(_ConnectionString);
            await connection.OpenAsync();

            await SetLibraryList(connection);

            OleDbCommand command = new OleDbCommand(sql, connection);
            var result = await command.ExecuteNonQueryAsync();

            connection.Close();

            return true; 
        }

        public async Task<bool> RunQuery(string sql, string outputFile)
        {
            var connection = new OleDbConnection(_ConnectionString);
            await connection.OpenAsync();

            await SetLibraryList(connection);

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
