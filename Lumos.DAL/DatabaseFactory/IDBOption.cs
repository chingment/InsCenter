﻿using System;
using System.Data;


namespace Lumos.DbRelay
{
    public enum DatabaseType
    {
        SqlServer = 0,
        Oracle = 1
    }

    public interface IDBOption
    {
        IDbConnection Connection(string connectionString, DatabaseType databaseType);

        string ConnectionString { get; set; }
    }
}
