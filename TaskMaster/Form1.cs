using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TaskMaster.Model;


namespace TaskMaster
{
    public partial class Form1 : Form
    {
        private tmDBContext tmContext;

        public Form1()
        {
            InitializeComponent();

            tmContext = new tmDBContext();

            var statuses = tmContext.Statuses.ToList();

            foreach (Status s in statuses)
            {
                cboStatus.Items.Add(s);
            }

            refreshGrid();
        }

        private void refreshGrid()
        {
            BindingSource bi = new BindingSource();
            
            
            var query = from t in tmContext.Tasks
                        orderby t.DueDate
                        select new { t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.DueDate };

            bi.DataSource = query.ToList();

            dataGridView1.DataSource = bi;
            dataGridView1.Refresh();
        }

        private void cmdCreateTask_Click(object sender, EventArgs e)
        {
            if (cboStatus.SelectedItem != null && txtTask.Text != String.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateTimePicker1.Value
                };

                tmContext.Tasks.Add(newTask);

                tmContext.SaveChanges();
                refreshGrid();
            }
            else
            {
                MessageBox.Show("You must select a Task Status and enter a Task description.");
            }

        }

        private void cmdDeleteTask_Click(object sender, EventArgs e)
        {
            var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);
            tmContext.Tasks.Remove(t);
            tmContext.SaveChanges();

            refreshGrid();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            cmdUpdateTask.Text = "Update";
            txtTask.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.Text = "Please Select...";
        }

        private void cmdUpdateTask_Click(object sender, EventArgs e)
        {
            if (cmdUpdateTask.Text == "Update")
            {
                
                txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString();
                dateTimePicker1.Value = (DateTime)dataGridView1.SelectedCells[3].Value;
                foreach (Status s in cboStatus.Items)
                {
                    if (s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                    {
                        cboStatus.SelectedItem = s;
                    }
                }
                

               cmdUpdateTask.Text = "Save";
            }
            else if (cmdUpdateTask.Text == "Save")
            {
                var updateTask = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

                updateTask.Name = txtTask.Text;
                updateTask.StatusId = (cboStatus.SelectedItem as Status).Id;
                updateTask.DueDate = dateTimePicker1.Value;

                tmContext.SaveChanges();

                refreshGrid();

                cmdUpdateTask.Text = "Update";
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cboStatus.Text = "Please Select...";
            }
        }
    }
}
