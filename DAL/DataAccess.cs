using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace Disorder.DAL;
public class DataAccess
{
    public static string dbpath1;
    public static string dbpath2;
    public static bool Init(string dbpaht1, string dbpath2)
    {
        if (DataAccess.dbpath1 == dbpaht1 && DataAccess.dbpath2 == dbpath2) return true;
        DataAccess.dbpath1 = dbpaht1;
        DataAccess.dbpath2 = dbpath2;
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath1}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                ("ATTACH DATABASE '"+dbpath2+"' AS history;", db);
            SqliteDataReader query;
            try
            {
                query = selectCommand.ExecuteReader();
            }
            catch
            {
                return false;
            }

            db.Close();
        }
        return true;
    }

    public static List<List<string>> Query(string sql)
    {
        List<List<string>> entries = new List<List<string>>();
        using (SqliteConnection db =
           new SqliteConnection($"Filename={dbpath1}"))
        {
            db.Open();

            SqliteCommand selectCommand = new SqliteCommand
                (sql, db);
            SqliteDataReader query;
            try
            {
                query = selectCommand.ExecuteReader();
            }
            catch
            {
                return entries;
            }


            while (query.Read())
            {
                entries.Add(new List<string>());
                for (int i = 0; i < query.FieldCount; i++)
                {
                    if (query.IsDBNull(i))
                    {
                        entries[entries.Count - 1].Add("null");
                    }
                    else entries[entries.Count - 1].Add(query.GetString(i));
                }

            }
            db.Close();
        }

        return entries;
    }
}
