using Dataaccesslaeyr;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Centeralized
{
    public class Devices
    {
        public int DeviceNo { get; set; }
        public string DeviceName { get; set; }
        public string DeviceMacAddress { get; set; }
        DataAccess DA;
        public MySqlDataReader GetDevice()
        {
            try
            {
                string firstMacAddress = NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                                         .Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                var Qur = @"SELECT * FROM Devices WHERE DeviceMacAddress ='" + firstMacAddress + "' ";
                DA = new DataAccess();
                var GD = DA.Getdata(Qur);
                if (GD== null)
                {
                    return null;
                }
                else
                {
                    return GD;
                }
            }
            catch (Exception ex)
            {
                var Qurcat = @"Insert Into Exception(Exceptionname,Form,Note) value('" + "Devices" + "','" + "Get Devices" + "','" + ex.Message.ToString() + "')";
                DA = new DataAccess();
                var CMO = DA.Crudopration(Qurcat);
                return null;
            }
        }
    }
}
