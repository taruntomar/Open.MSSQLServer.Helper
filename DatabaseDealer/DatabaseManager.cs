using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseDealer
{
    public class DatabaseManager : IDisposable
    {
        #region private fields
        private TelemetryClient _telemetryClient = new TelemetryClient();
        private string _sqlConnectionString;
        private bool _disposed = false;
        private SqlConnection _sqlConnection;
        #endregion

        public DatabaseManager(string connectionString)
        {
            _sqlConnectionString = connectionString;
            try
            {
                _sqlConnection = new SqlConnection(_sqlConnectionString);
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
            }
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {

                }
                //_sqlConnection.Dispose();
                _disposed = true;

            }
            //Dispose(disposing);
        }


        public object[] ExecuteSQLReader(string query, Func<DbDataRecord, dynamic> converterMethod)
        {

            try
            {
                if (_sqlConnection.State != System.Data.ConnectionState.Open)
                    _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, _sqlConnection);
                var data = cmd.ExecuteReader().Cast<DbDataRecord>().Select(converterMethod).ToArray();
                _sqlConnection.Close();
                return data;
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackTrace(ex.StackTrace);
                return null;

            }
        }
        public T[] ExecuteSQLReader<T>(string query, Func<DbDataRecord, dynamic> converterMethod)
        {

            try
            {
                if (_sqlConnection.State != System.Data.ConnectionState.Open)
                    _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, _sqlConnection);
                T[] data = cmd.ExecuteReader().Cast<DbDataRecord>().Select(converterMethod).Cast<T>().ToArray();
                _sqlConnection.Close();
                return data;
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackTrace(ex.StackTrace);
                return null;

            }
        }
        public void ExecuteSQLWriter(string query)
        {

            try
            {
                _sqlConnection.Open();
                SqlCommand cmd = new SqlCommand(query, _sqlConnection);
                var x = cmd.ExecuteNonQuery();
                _sqlConnection.Close();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackTrace(ex.StackTrace);

            }
        }
    }
}
