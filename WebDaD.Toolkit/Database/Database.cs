﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebDaD.Toolkit.Database
{
    public interface Database
    {
        DatabaseType getType();
        string ConnectionString();
        bool Open();
        bool Close();
        string getValue(string table, string field, string filter);
        Result getRow(string table, string[] fields, string filter = "",string orderby="", int limit = 0);
        Result getRow(Joinable j, string[] fields, Condition[] c= null, GroupBy g = null, OrderBy[] o = null, int limit = 0);
        Result getRow(string table, string[] fields, Condition[] c = null, GroupBy g = null, OrderBy[] o = null, int limit = 0);
        bool Update(string table, Dictionary<string, string> fieldset, string filter);
        bool Insert(string table, Dictionary<string, string> fieldset);
        bool Execute(string sql);
        string GetLastInsertedID();
        bool isOpen();

        /// <summary>
        /// Checks, if the Database contains the necessary Tables
        /// </summary>
        /// <param name="tables">A List of Table-Objects to Check</param>
        /// <returns>True or False</returns>
        bool isValid(List<Table> tables);
        Result Select(string sql);
        bool CreateTable(string table, Dictionary<string, FieldType> fields,string primary_field);

        bool CreateTable(Table table);

        Table GetTable(string name);
        Result Join(Joinable[] tables, Condition[] c, GroupBy g, OrderBy[] o);
        /// <summary>
        /// Dumps the Database into a Single restorable File
        /// </summary>
        /// <param name="targetFile">The Name and Path of the File</param>
        /// <returns>If all went well</returns>
        bool Dump(string targetFile);
        /// <summary>
        /// Restores the Database from the File, overwriting all Changes
        /// </summary>
        /// <param name="sourceFile">The File to read From</param>
        /// <returns>If all went well</returns>
        bool Restore(string sourceFile);
    }
}
