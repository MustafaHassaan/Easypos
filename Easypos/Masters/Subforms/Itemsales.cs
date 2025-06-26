using jdk.@internal.org.objectweb.asm.tree.analysis;
using Querylaeyr.Modelmaster;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Easypos.Masters.Subforms
{
    public partial class Itemsales : Form
    {
        Items Item;
        public Itemsales()
        {
            InitializeComponent();
            Item = new Items();

            var CDpur = Item.Getpurinvdata();
            foreach (DataRow item in CDpur.Rows)
            {
                DGV.Rows.Add(item["TDetailNo"].ToString(),
                             item["TDDesc"].ToString(),
                             item["TDate"].ToString(),
                             item["Quantity"].ToString(),
                             0,
                             0,
                             item["Invoiceno"].ToString(),
                             "فاتورة مشتريات");
            }
            var CD = Item.Getsalesinvdata();
            foreach (DataRow item in CD.Rows)
            {
                DGV.Rows.Add(item["Id"].ToString(),
                             item["Itemname"].ToString(),
                             item["Date"].ToString(),
                             0,
                             item["Quantity"].ToString(),
                             0,
                             item["invoiceno"].ToString(),
                             "فاتورة مبيعات");
            }
            DGV.Sort(DGV.Columns["Date"],System.ComponentModel.ListSortDirection.Ascending);
            var Balance = 0.0;
            for (int i = 0; i < DGV.Rows.Count; i++)
            {
                var Det = DGV.Rows[i].Cells[7].Value.ToString();
                var Stock = Convert.ToDouble(DGV.Rows[i].Cells[3].Value.ToString());
                var Qty = Convert.ToDouble(DGV.Rows[i].Cells[4].Value.ToString());
                var Rem = Convert.ToDouble(DGV.Rows[i].Cells[5].Value.ToString());
                if (i > 0)
                {
                    if (Det == "فاتورة مبيعات")
                    {
                        DGV.Rows[i].Cells[3].Value = Balance;
                    }
                }
                if (Det == "فاتورة مشتريات")
                {
                    Balance = Balance + (Stock + Qty);
                    DGV.Rows[i].Cells[5].Value = Balance;
                }
                if (Det == "فاتورة مبيعات")
                {
                    Balance = Balance + (Stock - Qty);
                    DGV.Rows[i].Cells[5].Value = Balance;
                }
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
