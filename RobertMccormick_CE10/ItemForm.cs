//Robert Mccormick
//Frameworks
//Term 3
//RobertMccormick_CE10

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CE10
{
    public partial class ItemForm : Form
    {
        public Obj Data { get; set; }


        public ItemForm(Obj data)
        {
            InitializeComponent();
            Data = data;

            var index = 0;
            foreach (var prop in Data.GetType().GetProperties())
            {
                //do not generate Id field
                if (prop.Name.Equals("Id"))
                    continue;

                AutoGenerateControl(prop, index);
                index++;
            }

            //display first row
            DisplayData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //collect data
            foreach (var prop in Data.GetType().GetProperties())
            {
                //do not generate Id field
                if (prop.Name.Equals("Id"))
                    continue;

                var control = this.Controls.Find(prop.Name, true)[0];

                switch (prop.PropertyType.Name)
                {
                    case "String":
                        prop.SetValue(Data,(control as TextBox).Text);
                        break;
                    case "Double":
                        prop.SetValue(Data, Convert.ToDouble((control as NumericUpDown).Value));
                        break;
                    case "DateTime":
                        prop.SetValue(Data, (control as DateTimePicker).Value);
                        break;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void AutoGenerateControl(PropertyInfo prop, int index)
        {
            //laber
            Label lbl = new Label();
            lbl.Name = "label" + index.ToString();
            lbl.Location = new Point(12, 43 + 26 * index);
            lbl.Text = prop.Name;
            lbl.AutoSize = true;
            this.Controls.Add(lbl);

            //if String -> generate textbox
            if (prop.PropertyType.Name.Equals("String"))
            {
                TextBox txt = new TextBox();
                txt.Name = prop.Name;
                txt.Location = new Point(95, 43 + 26 * index);
                txt.Size = new Size(100, 20);
                this.Controls.Add(txt);
            }
            //double -> NumericUpDown
            else if (prop.PropertyType.Name.Equals("Double"))
            {
                NumericUpDown num = new NumericUpDown();
                num.Name = prop.Name;
                num.Location = new Point(95, 43 + 26 * index);
                num.Size = new Size(100, 20);
                num.ThousandsSeparator = true;
                num.DecimalPlaces = 2;
                this.Controls.Add(num);
            }
            //datetime -> Datetimepicker
            else if (prop.PropertyType.Name.Equals("DateTime"))
            {
                DateTimePicker dtp = new DateTimePicker();
                dtp.Format = DateTimePickerFormat.Short;
                dtp.Name = prop.Name;
                dtp.Location = new Point(95, 43 + 26 * index);
                dtp.Size = new Size(100, 20);
                this.Controls.Add(dtp);
            }
        }

        private void DisplayData()
        {

            foreach (var prop in Data.GetType().GetProperties())
            {
                //do not generate Id field
                if (prop.Name.Equals("Id"))
                    continue;

                var control = this.Controls.Find(prop.Name, true)[0];

                switch (prop.PropertyType.Name)
                {
                    case "String":
                        (control as TextBox).Text = (string)prop.GetValue(Data);
                        break;
                    case "Double":
                        (control as NumericUpDown).Value = Convert.ToDecimal(prop.GetValue(Data));
                        break;
                    case "DateTime":
                        (control as DateTimePicker).Value = (DateTime)prop.GetValue(Data);
                        break;
                }
            }
        }
    }
}
