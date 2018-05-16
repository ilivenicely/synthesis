// Robert Mccormick
// Frameworks
// Term 3
// RobertMcCormick_CE10

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CE10
{
    public partial class Form1 : Form
    {
        //private List<Obj> ListData;

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var dbConn = DBConnection.Instance();
            if (!dbConn.IsConnect())
            {
                MessageBox.Show("Can not connect to mysql.");
                return;
            }


            // mySQL query
            string query = "SELECT dvd_title, studio, price, createddate, publicRating, dvdId FROM exampledatabase.dvd limit 25";



            //summon mysqlcommand
            var cmd = new MySqlCommand(query, dbConn.Connection);
            var reader = cmd.ExecuteReader();
            //convert to Obj
            while (reader.Read())
            {
                var obj = new Obj()
                {
                    Title = reader.GetString(0),
                    Studio = reader.GetString(1),
                    Price = reader.GetDouble(2),
                    CreatedDate = reader.GetDateTime(3),
                    Rating = reader.GetDouble(4),
                    Id = reader.GetInt32(5)
                };



                //add items
                lvDisplay.Items.Add(new ListViewItem() { Text = obj.Title, Tag = obj, ImageIndex = 0 });
            }

            reader.Close();
        }

        //large icon
        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvDisplay.View = View.LargeIcon;
            largeIconToolStripMenuItem.Checked = true;
            smallIconToolStripMenuItem.Checked = false;
        }


        //small icon
        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvDisplay.View = View.SmallIcon;
            largeIconToolStripMenuItem.Checked = false;
            smallIconToolStripMenuItem.Checked = true;
        }

        //double click event handler
        private void lvDisplay_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ItemForm form = new ItemForm(lvDisplay.SelectedItems[0].Tag as Obj);
            if(form.ShowDialog(this) == DialogResult.OK)
            {
                var obj = form.Data;

                //update listview
                var selectedItem = lvDisplay.SelectedItems[0];
                selectedItem.Text = obj.Title;
                selectedItem.Tag = obj;


                //update db
                try
                {
                    var dbConn = DBConnection.Instance();
                    if (!dbConn.IsConnect())
                    {
                        MessageBox.Show("Cant connect to mysql.");
                        return;
                    }



                    var query = "Update exampledatabase.dvd SET dvd_title = @title, studio = @studio, price = @price, createddate=@createddate, publicRating=@publicRating WHERE dvdId = @dvdId";
                    var command = new MySqlCommand(query, dbConn.Connection);
                    command.Parameters.Add("@title",MySqlDbType.String);
                    command.Parameters.Add("@studio", MySqlDbType.String);
                    command.Parameters.Add("@price", MySqlDbType.String);
                    command.Parameters.Add("@createddate", MySqlDbType.String);
                    command.Parameters.Add("@publicRating", MySqlDbType.String);
                    command.Parameters.Add("@dvdId", MySqlDbType.Int32);

                    command.Parameters["@title"].Value = obj.Title;
                    command.Parameters["@studio"].Value = obj.Studio;
                    command.Parameters["@price"].Value = obj.Price.ToString("0.00");
                    command.Parameters["@createddate"].Value = obj.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    command.Parameters["@publicRating"].Value = obj.Rating.ToString("0.00");
                    command.Parameters["@dvdId"].Value = obj.Id;

                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                 {
                    MessageBox.Show("Can not update to Db.");
                    return;
                }
            }
        }

        //close
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
