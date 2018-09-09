using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using BIStudio.Framework.Core;
namespace BIStudio.Framework.File
{
    public sealed class SqlClientInputStream : Stream
    {
        private SqlConnection _SqlConnection;
        private long _Position;
        private SqlCommand _SqlCommand;
        private SqlParameter _DataSqlParameter;
        private SqlParameter _OffsetSqlParameter;

        public SqlClientInputStream(string table, string dataField, string whereCriteria)
        {
            byte[] buffer;
            this._SqlConnection = new SqlConnection(AppConfig.CnStr);
            this._SqlCommand = this._SqlConnection.CreateCommand();
            this._SqlCommand.CommandText = "UPDATE " + table + " SET " + dataField + "=NULL WHERE " + whereCriteria + ";SELECT TEXTPTR(" + dataField + ") FROM " + table + " WHERE " + whereCriteria;
            try
            {
                this._SqlConnection.Open();
                buffer = (byte[])this._SqlCommand.ExecuteScalar();
            }
            finally
            {
                this._SqlConnection.Close();
            }
            this._SqlCommand.CommandText = "UPDATETEXT " + table + "." + dataField + " @ptr @offset NULL @data;";
            SqlParameter parameter = this._SqlCommand.CreateParameter();
            parameter.DbType = DbType.Binary;
            parameter.ParameterName = "@ptr";
            parameter.Size = 0x10;
            parameter.Value = buffer;
            this._SqlCommand.Parameters.Add(parameter);
            this._OffsetSqlParameter = this._SqlCommand.CreateParameter();
            this._OffsetSqlParameter.DbType = DbType.Int32;
            this._OffsetSqlParameter.ParameterName = "@offset";
            this._OffsetSqlParameter.Size = 4;
            this._SqlCommand.Parameters.Add(this._OffsetSqlParameter);
            this._DataSqlParameter = this._SqlCommand.CreateParameter();
            this._DataSqlParameter.SqlDbType = SqlDbType.Image;
            this._DataSqlParameter.ParameterName = "@data";
            this._DataSqlParameter.Size = 0x1f68;
            this._SqlCommand.Parameters.Add(this._DataSqlParameter);
        }

        public SqlClientInputStream(string table, string dataField, string idField, int idValue) : this(table, dataField, idField + "=" + idValue.ToString())
        {
        }

        public override void Close()
        {
            base.Close();
            this._SqlConnection.Dispose();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int start, int count)
        {
            int num = 0x1f68;
            int num2 = 0;
            this._DataSqlParameter.Value = buffer;
            try
            {
                this._SqlConnection.Open();
                while (num2 < count)
                {
                    if (num > (count - num2))
                    {
                        num = count - num2;
                    }
                    this._DataSqlParameter.Offset = start + num2;
                    this._DataSqlParameter.Size = num;
                    this._OffsetSqlParameter.Value = this._Position;
                    this._SqlCommand.ExecuteNonQuery();
                    this._Position += num;
                    num2 += num;
                }
            }
            finally
            {
                this._SqlConnection.Close();
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                return this._Position;
            }
        }

        public override long Position
        {
            get
            {
                return this._Position;
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}

