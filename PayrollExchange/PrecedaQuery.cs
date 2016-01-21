using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace PayrollExchange
{
    public class BodPrecedaResult
    {
        public Guid BodId { get; set; }
        public string ProcessingStage { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public string IdNumber { get; set; }
        public string MapperId { get; set; }
    }

    public class BodError
    {
        public int ImportNumber { get; private set; }
        public string Message { get; private set; } 

        public BodError(int importNumber, string message)
        {
            ImportNumber = importNumber;
            Message = message;
        }
    }

    public class PrecedaQuery
    {
        private string _ConnectionString;

        public string Server { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }
        public string FileLibrary { get; private set; }

        public PrecedaQuery(string server, string user, string password, string fileLibrary)
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
            _ConnectionString = connectionStringBuilder.ConnectionString;
        }

        public CustomerCode GetCustomerCode()
        {
            CustomerCode customerCode;

            var connection = new OleDbConnection(_ConnectionString);
            connection.Open();

            OleDbCommand command = new OleDbCommand("SELECT PE03GCC, PE03LCC FROM PEINTERF.PE03 WHERE PE03DB = '" + FileLibrary + "'", connection);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
                customerCode = new CustomerCode(reader.GetFieldValue<string>(0), reader.GetFieldValue<string>(1));
            else
                throw new Exception("Unable to determine GCC/LCC for file library " + FileLibrary);
           
            reader.Close();
            connection.Close();

            return customerCode;
        }

        public BodPrecedaResult GetBodStatus(PayrollExchangeBod bod)
        {
            BodPrecedaResult result = new BodPrecedaResult()
            {
                BodId = bod.Id,
                ProcessingStage = "",
                Status = 0,
                StatusDescription = "",
                IdNumber = "",
                MapperId = ""
            };

            var connection = new OleDbConnection(_ConnectionString);

            connection.Open();
            OleDbCommand command = new OleDbCommand("SELECT C1.CHDESC AS STGDESC, PE2DSTS, C2.CHDESC AS STSDESC, PE2DEMP, PE2DMID FROM " + FileLibrary + ".PE02D "
                                                    + " LEFT OUTER JOIN " + FileLibrary + ".CHOLIST C1 ON (C1.CHTYPE = 'PEI_STG' and C1.CHVAL = PE2DSTG)"
                                                    + " LEFT OUTER JOIN " + FileLibrary + ".CHOLIST C2 ON (C2.CHTYPE = 'PEI_STS' and C2.CHVAL = PE2DSTS)"
                                                    + " WHERE PE2DOBI = '" + bod.Id + "'", connection);
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (! reader.IsDBNull(0))
                    result.ProcessingStage = reader.GetString(0);
                result.Status = reader.GetInt32(1);
                if (!reader.IsDBNull(2))
                    result.StatusDescription = reader.GetString(2);
                result.IdNumber = reader.GetString(3);
                result.MapperId = reader.GetString(4);
            }

            reader.Close();
            connection.Close();

            return result;
        }

        public IReadOnlyCollection<BodError> GetBodErrors(string mapperId)
        {
            var errors = new List<BodError>();

            var connection = new OleDbConnection(_ConnectionString);

            connection.Open();
            OleDbCommand command = new OleDbCommand("SELECT PP26CRN, PP26CEM FROM " + FileLibrary + ".PPF26C WHERE PP26CUI = '" + mapperId + "'", connection);
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
                errors.Add(new BodError(reader.GetInt32(0), reader.GetString(1)));

            reader.Close();
            connection.Close();

            return errors;
        }
    }
}
