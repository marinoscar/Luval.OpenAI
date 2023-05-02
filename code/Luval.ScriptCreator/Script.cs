using Luval.DataStore;
using Luval.DataStore.Database.SqlServer;
using System.Data;

namespace Luval.ScriptCreator
{
    public class Script
    {
        private SqlDataStore _dataStore;
        private string _tableQuery = @"
SELECT 
 Tables.TABLE_SCHEMA As TableSchema,
 Tables.TABLE_NAME As TableName,
 Columns.COLUMN_NAME As ColumnName,
 Columns.DATA_TYPE As DataType,
 Columns.CHARACTER_MAXIMUM_LENGTH As MaxLength
FROM INFORMATION_SCHEMA.TABLES As Tables
INNER JOIN INFORMATION_SCHEMA.COLUMNS As Columns ON Columns.TABLE_SCHEMA = Tables.TABLE_SCHEMA and Columns.TABLE_NAME = Tables.TABLE_NAME
WHERE
	Tables.TABLE_TYPE = 'BASE TABLE'
";

        public Script(string connectionString)
        {
            _dataStore = new SqlDataStore(connectionString);
        }

        private List<TableInformation> GetTableQuery()
        {
            return _dataStore.ExecuteToEntityList<TableInformation>(_tableQuery).ToList();
        }

        public string CreateFullScript()
        {
            var sw = new StringWriter();
            var tables = GetTableQuery().Where(i => i.TableSchema == "Sales").ToList();
            foreach (var tableName in tables.Select(i => i.TableName).Distinct())
            {
                var colums = tables.Where(i => i.TableName == tableName).ToList();
                sw.WriteLine("/**** {0} ****/", tableName);
                sw.WriteLine("CREATE TABLE {0}.{1} (", colums.First().TableSchema, tableName);
                foreach (var column in colums)
                {
                    sw.WriteLine("{0} {1} NULL", column.ColumnName, GetColType(column));
                }
                sw.WriteLine(");");
            }
            return sw.ToString();
        }

        public string GetCompressedScript()
        {
            var sw = new StringWriter();
            var tables = GetTableQuery().Where(i => i.TableSchema == "Sales").ToList();
            foreach (var tableName in tables.Select(i => i.TableName).Distinct())
            {
                var colums = tables.Where(i => i.TableName == tableName).ToList();
                sw.Write("{0}.{1}(", colums.First().TableSchema, tableName);
                sw.Write(string.Join(",", colums.Select(i => i.ColumnName)));
                sw.WriteLine(")");
            }
            return sw.ToString();
        }

        private string GetColType(TableInformation col)
        {
            if (col.MaxLength == null || col.MaxLength < 1) return col.DataType;
            return string.Format("{0}({1})", col.ColumnName, col.MaxLength);
        }




    }
}