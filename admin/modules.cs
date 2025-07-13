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
    public partial class modules : Form
    {
        public modules()
        {
            InitializeComponent();
        }

        private void aK_ModulesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.aK_ModulesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.hrDataSet);

        }

        private void aK_ModulesBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.aK_ModulesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.hrDataSet);

        }

        private void modules_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules_lines' table. You can move, or remove it, as needed.
            this.aK_Modules_linesTableAdapter.Fill(this.hrDataSet.AK_Modules_lines);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules' table. You can move, or remove it, as needed.
            this.aK_ModulesTableAdapter.Fill(this.hrDataSet.AK_Modules);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules_lines' table. You can move, or remove it, as needed.
            this.aK_Modules_linesTableAdapter.Fill(this.hrDataSet.AK_Modules_lines);
            // TODO: This line of code loads data into the 'hrDataSet.AK_Modules' table. You can move, or remove it, as needed.
            this.aK_ModulesTableAdapter.Fill(this.hrDataSet.AK_Modules);
            
        }

       
        

        private void aK_ModulesBindingNavigatorSaveItem_Click_2(object sender, EventArgs e)
        {
            this.Validate();
            this.aK_ModulesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.hrDataSet);

        }

        private void aK_ModulesBindingNavigatorSaveItem_Click_3(object sender, EventArgs e)
        {
            this.Validate();
            this.aK_ModulesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.hrDataSet);

        }
    }
}
