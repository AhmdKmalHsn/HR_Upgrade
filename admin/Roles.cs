using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace admin
{
    public partial class Roles : Form
    {
        public Roles()
        {
            InitializeComponent();
        }
        private void Roles_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules_lines' table. You can move, or remove it, as needed.
            this.aK_Modules_linesTableAdapter.Fill(this.hrDataSet.AK_Modules_lines);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Roles_lines' table. You can move, or remove it, as needed.
            this.aK_Roles_linesTableAdapter.Fill(this.hrDataSet.AK_Roles_lines);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Roles' table. You can move, or remove it, as needed.
            this.aK_RolesTableAdapter.Fill(this.hrDataSet.AK_Roles);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules_lines' table. You can move, or remove it, as needed.

        }
        private void aK_RolesBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.aK_RolesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.hrDataSet);

        }
        private void aK_Roles_linesDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            
        }
    }
}
