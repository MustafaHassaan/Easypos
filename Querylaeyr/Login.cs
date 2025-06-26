using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querylaeyr
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        DataAccess DA;
        public MySqlDataReader Signin()
        {
            try
            {
                var Qur = @"SELECT * FROM Staff WHERE Username = '" + Username + "' AND UPassword = '" + Password + "' ";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD != null)
                {
                    return GD;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
