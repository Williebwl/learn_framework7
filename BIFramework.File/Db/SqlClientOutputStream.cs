using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using BIStudio.Framework.Core;

namespace BIStudio.Framework.File
{
    public sealed class SqlClientOutputStream : Stream
    {
        private SqlConnection _conn;
        private long _datasize; //要读取数据的字节数
        private SqlCommand _com;
        private SqlParameter _sqlpar;
        private long _datalength; //数据所占的字节数
        private SqlParameter _sqlpar2;

        public SqlClientOutputStream(string table, string dataField, string whereCriteria)
        {
            byte[] buffer;
            this._conn = new SqlConnection(AppConfig.CnStr);
            this._com = this._conn.CreateCommand();
            this._com.CommandText = "SELECT DATALENGTH(" + dataField + ") FROM " + table + " WHERE " + whereCriteria;
            try
            {
                this._conn.Open();
                this._datalength = (int)this._com.ExecuteScalar();
            }
            finally
            {
                this._conn.Close();
            }
            this._com.CommandText = "SELECT TEXTPTR(" + dataField + ") FROM " + table + " WHERE " + whereCriteria;
            try
            {
                this._conn.Open();
                buffer = (byte[])this._com.ExecuteScalar();
            }
            finally
            {
                this._conn.Close();
            }
            this._com.CommandText = "READTEXT " + table + "." + dataField + " @ptr @offset @size;";
            SqlParameter parameter = this._com.CreateParameter();
            parameter.DbType = DbType.Binary;
            parameter.ParameterName = "@ptr";
            parameter.Size = 0x10;
            parameter.Value = buffer;
            this._com.Parameters.Add(parameter);
            this._sqlpar2 = this._com.CreateParameter();
            this._sqlpar2.DbType = DbType.Int32;
            this._sqlpar2.ParameterName = "@offset";
            this._sqlpar2.Size = 4;
            this._com.Parameters.Add(this._sqlpar2);
            this._sqlpar = this._com.CreateParameter();
            this._sqlpar.DbType = DbType.Int32;
            this._sqlpar.ParameterName = "@size";
            this._sqlpar.Size = 4;
            this._com.Parameters.Add(this._sqlpar);
        }

        public SqlClientOutputStream(string table, string dataField, string idField, int idValue)
            : this(table, dataField, idField + "=" + idValue.ToString())
        {
        }

        public override void Close()
        {
            base.Close();
            this._conn.Dispose();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int num;
            if (this._datasize > this._datalength)
            {
                throw new InvalidOperationException("Tried to read past end of stream.");
            }
            if (this._datasize == this._datalength)
            {
                return 0;
            }
            if ((this._datasize + count) > this._datalength)
            {
                num = (int)(this._datalength - this._datasize);
            }
            else
            {
                num = count;
            }
            this._sqlpar.Value = num;
            this._sqlpar2.Value = this._datasize;
            byte[] src = null;
            try
            {
                this._conn.Open();
                src = (byte[])this._com.ExecuteScalar();
            }
            finally
            {
                this._conn.Close();
            }
            Buffer.BlockCopy(src, 0, buffer, offset, num);
            this._datasize += num;
            return num;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if ((offset < 0L) || (offset > this._datalength))
                    {
                        throw new ArgumentOutOfRangeException("offset");
                    }
                    this._datasize = offset;
                    break;

                case SeekOrigin.Current:
                    if (((this._datasize + offset) < 0L) || ((this._datasize + offset) > this._datalength))
                    {
                        throw new ArgumentOutOfRangeException("offset");
                    }
                    this._datasize += offset;
                    break;

                case SeekOrigin.End:
                    if ((offset > 0L) || ((this._datalength + offset) < 0L))
                    {
                        throw new ArgumentOutOfRangeException("offset");
                    }
                    this._datasize = this._datalength + offset;
                    break;
            }
            return this._datasize;
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return true;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return this._datalength;
            }
        }

        public override long Position
        {
            get
            {
                return this._datasize;
            }
            set
            {
                if ((this._datasize < 0L) || (this._datasize > this._datalength))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                this._datasize = value;
            }
        }
    }
}
