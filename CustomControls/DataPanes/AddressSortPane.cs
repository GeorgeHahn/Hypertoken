using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sniffer
{
    public partial class AddressSortPane : UserControl, DataPane
    {
        public AddressSortPane()
        {
            InitializeComponent();
        }

        Dictionary<byte, int[]> addrData = new Dictionary<byte,int[]>();

        private String ByteArrayToString(byte[] ar)
        {
            String s = String.Empty;
            for (int j = 0; j < ar.Length; j++)
                s += " " + ar[j].ToString("X2");
            return s;
        }

        public void AddData(long[] timeStamp, byte[] addr, byte[][] data)
        {
            int[] vals;

            for(int i = 0; i < addr.Length; i++)
            {
                bool found = false;
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[1].Value.ToString() == addr[i].ToString("X"))
                    {
                        try
                        {
                            row.Cells[2].Value = ByteArrayToString(data[i]);
                            addrData.TryGetValue(addr[i], out vals);
                            row.Cells[3].Value = Environment.TickCount - vals[0];
                            row.Cells[4].Value = vals[1];
                            addrData.Remove(addr[i]);
                            addrData.Add(addr[i], new int[2] { Environment.TickCount, vals[1] + 1 });
                        }
                        catch (Exception e)
                        {
                            e.ToString();
                        }
                        found = true;
                    }
                }

                if(!found)
                {
                    dataGridView1.Rows.Add(1);
                    int index = dataGridView1.RowCount - 1;
                    try
                    {
                        addrData.Add(addr[i], new int[2]{Environment.TickCount, 1});
                        dataGridView1.Rows[index].Cells[1].Value = addr[i].ToString("X");
                        dataGridView1.Rows[index].Cells[2].Value = ByteArrayToString(data[i]);
                        dataGridView1.Rows[index].Cells[3].Value = 0;
                        dataGridView1.Rows[index].Cells[4].Value = 1;
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
                }
            }


            if (dataGridView1.SortOrder.Equals(SortOrder.None))
                return;

            if (dataGridView1.SortOrder.Equals(SortOrder.Ascending))
                dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Ascending);
            else
                dataGridView1.Sort(dataGridView1.SortedColumn, ListSortDirection.Descending);
            
            return;
        }
    }
}
