using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dataaccesslaeyr
{
    public class DataAccess
    {
        public MySqlConnection Con;
        //public string CS = @"server=127.0.0.1;SslMode=none;uid=root;pwd=@Admin;database=tailoring;port=3306;Allow User Variables=True;Connect Timeout=30;CharSet=utf8;allowPublicKeyRetrieval=True;";
        //public string CS = @"server=127.0.0.1;SslMode=none;uid=root;pwd=@Admin;database=tailoring;port=3306;Allow User Variables=True;Connect Timeout=30;CharSet=utf8;allowPublicKeyRetrieval=True;";
        public string CS = @"server=127.0.0.1;SslMode=none;uid=root;pwd=@Admin;database=redaa;port=3306;Allow User Variables=True;Connect Timeout=30;CharSet=utf8;allowPublicKeyRetrieval=True;";
        //public string CS = @"server=127.0.0.1;SslMode=none;uid=root;pwd=@Admin;database=wazeria;port=3306;Allow User Variables=True;Connect Timeout=30;CharSet=utf8;allowPublicKeyRetrieval=True;";
        public string Messagedata { get; set; }
        DataAccess DA;
        public MySqlCommand Cmd;
        public MySqlDataReader dr;
        public MySqlDataAdapter da;
        public MySqlDataReader Getdata(string Qur)
        {
            Con = new MySqlConnection(CS);
            try
            {
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dr = Cmd.ExecuteReader();
                if (dr.Read())
                {
                    return dr;
                }
                else
                {
                    Con.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Messagedata = ex.Message.ToString();
            }
            return null;
        }
        public DataTable Getdatatable(string Qur)
        {
            Con = new MySqlConnection(CS);
            DataTable dtbl = new DataTable();
            try
            {
                Con.Open();
                Cmd = new MySqlCommand(Qur, Con);
                dtbl.Load(Cmd.ExecuteReader());
                Con.Close();
                return dtbl;
            }
            catch (Exception ex)
            {
                Messagedata = ex.Message.ToString();
            }
            return null;
        }
        public bool Crudopration(string Qur)
        {
            Con = new MySqlConnection(CS);
            try
            {
                Con.Open();
                MySqlCommand Cmd = new MySqlCommand(Qur, Con);
                Cmd.ExecuteReader();
                Con.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
