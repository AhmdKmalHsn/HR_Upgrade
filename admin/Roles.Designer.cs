namespace admin
{
    partial class Roles
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.Label role_nameLabel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Roles));
            this.aK_RolesBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.aK_RolesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.hrDataSet = new admin.HrDataSet();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aK_RolesBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.role_nameTextBox = new System.Windows.Forms.TextBox();
            this.aK_Roles_linesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aK_Roles_linesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.aKModuleslinesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.aK_RolesTableAdapter = new admin.HrDataSetTableAdapters.AK_RolesTableAdapter();
            this.tableAdapterManager = new admin.HrDataSetTableAdapters.TableAdapterManager();
            this.aK_Modules_linesTableAdapter = new admin.HrDataSetTableAdapters.AK_Modules_linesTableAdapter();
            this.aK_Roles_linesTableAdapter = new admin.HrDataSetTableAdapters.AK_Roles_linesTableAdapter();
            idLabel = new System.Windows.Forms.Label();
            role_nameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.aK_RolesBindingNavigator)).BeginInit();
            this.aK_RolesBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aK_RolesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hrDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Roles_linesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Roles_linesDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aKModuleslinesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(32, 38);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(21, 13);
            idLabel.TabIndex = 1;
            idLabel.Text = "Id:";
            // 
            // role_nameLabel
            // 
            role_nameLabel.AutoSize = true;
            role_nameLabel.Location = new System.Drawing.Point(32, 64);
            role_nameLabel.Name = "role_nameLabel";
            role_nameLabel.Size = new System.Drawing.Size(58, 13);
            role_nameLabel.TabIndex = 3;
            role_nameLabel.Text = "role name:";
            // 
            // aK_RolesBindingNavigator
            // 
            this.aK_RolesBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.aK_RolesBindingNavigator.BindingSource = this.aK_RolesBindingSource;
            this.aK_RolesBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.aK_RolesBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.aK_RolesBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.aK_RolesBindingNavigatorSaveItem});
            this.aK_RolesBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.aK_RolesBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.aK_RolesBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.aK_RolesBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.aK_RolesBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.aK_RolesBindingNavigator.Name = "aK_RolesBindingNavigator";
            this.aK_RolesBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.aK_RolesBindingNavigator.Size = new System.Drawing.Size(663, 25);
            this.aK_RolesBindingNavigator.TabIndex = 0;
            this.aK_RolesBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // aK_RolesBindingSource
            // 
            this.aK_RolesBindingSource.DataMember = "AK_Roles";
            this.aK_RolesBindingSource.DataSource = this.hrDataSet;
            // 
            // hrDataSet
            // 
            this.hrDataSet.DataSetName = "HrDataSet";
            this.hrDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // aK_RolesBindingNavigatorSaveItem
            // 
            this.aK_RolesBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aK_RolesBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("aK_RolesBindingNavigatorSaveItem.Image")));
            this.aK_RolesBindingNavigatorSaveItem.Name = "aK_RolesBindingNavigatorSaveItem";
            this.aK_RolesBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.aK_RolesBindingNavigatorSaveItem.Text = "Save Data";
            this.aK_RolesBindingNavigatorSaveItem.Click += new System.EventHandler(this.aK_RolesBindingNavigatorSaveItem_Click_1);
            // 
            // idTextBox
            // 
            this.idTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_RolesBindingSource, "Id", true));
            this.idTextBox.Location = new System.Drawing.Point(96, 35);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(100, 20);
            this.idTextBox.TabIndex = 2;
            // 
            // role_nameTextBox
            // 
            this.role_nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_RolesBindingSource, "role_name", true));
            this.role_nameTextBox.Location = new System.Drawing.Point(96, 61);
            this.role_nameTextBox.Name = "role_nameTextBox";
            this.role_nameTextBox.Size = new System.Drawing.Size(100, 20);
            this.role_nameTextBox.TabIndex = 4;
            // 
            // aK_Roles_linesBindingSource
            // 
            this.aK_Roles_linesBindingSource.DataMember = "FK__AK_Roles___role___544C7222";
            this.aK_Roles_linesBindingSource.DataSource = this.aK_RolesBindingSource;
            // 
            // aK_Roles_linesDataGridView
            // 
            this.aK_Roles_linesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aK_Roles_linesDataGridView.AutoGenerateColumns = false;
            this.aK_Roles_linesDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.aK_Roles_linesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.aK_Roles_linesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewCheckBoxColumn3,
            this.dataGridViewCheckBoxColumn4,
            this.dataGridViewCheckBoxColumn5});
            this.aK_Roles_linesDataGridView.DataSource = this.aK_Roles_linesBindingSource;
            this.aK_Roles_linesDataGridView.Location = new System.Drawing.Point(12, 87);
            this.aK_Roles_linesDataGridView.Name = "aK_Roles_linesDataGridView";
            this.aK_Roles_linesDataGridView.Size = new System.Drawing.Size(639, 283);
            this.aK_Roles_linesDataGridView.TabIndex = 5;
            this.aK_Roles_linesDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.aK_Roles_linesDataGridView_CellEndEdit);
            this.aK_Roles_linesDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.aK_Roles_linesDataGridView_DataError);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "role_id";
            this.dataGridViewTextBoxColumn2.HeaderText = "role_id";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "module_line_id";
            this.dataGridViewTextBoxColumn3.DataSource = this.aKModuleslinesBindingSource;
            this.dataGridViewTextBoxColumn3.DisplayMember = "name";
            this.dataGridViewTextBoxColumn3.HeaderText = "module_line_id";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn3.ValueMember = "id";
            this.dataGridViewTextBoxColumn3.Width = 102;
            // 
            // aKModuleslinesBindingSource
            // 
            this.aKModuleslinesBindingSource.DataMember = "AK_Modules_lines";
            this.aKModuleslinesBindingSource.DataSource = this.hrDataSet;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "access";
            this.dataGridViewCheckBoxColumn1.HeaderText = "access";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 45;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "read";
            this.dataGridViewCheckBoxColumn2.HeaderText = "read";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.Width = 35;
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.DataPropertyName = "create";
            this.dataGridViewCheckBoxColumn3.HeaderText = "create";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.Width = 44;
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.DataPropertyName = "update";
            this.dataGridViewCheckBoxColumn4.HeaderText = "update";
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            this.dataGridViewCheckBoxColumn4.Width = 47;
            // 
            // dataGridViewCheckBoxColumn5
            // 
            this.dataGridViewCheckBoxColumn5.DataPropertyName = "delete";
            this.dataGridViewCheckBoxColumn5.HeaderText = "delete";
            this.dataGridViewCheckBoxColumn5.Name = "dataGridViewCheckBoxColumn5";
            this.dataGridViewCheckBoxColumn5.Width = 43;
            // 
            // aK_RolesTableAdapter
            // 
            this.aK_RolesTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.@__MigrationHistoryTableAdapter = null;
            this.tableAdapterManager.AbsencesTableAdapter = null;
            this.tableAdapterManager.AbsenceTypesTableAdapter = null;
            this.tableAdapterManager.AccessibilityTableAdapter = null;
            this.tableAdapterManager.AClogsTableAdapter = null;
            this.tableAdapterManager.AdditionApprovalsTableAdapter = null;
            this.tableAdapterManager.AddressesTableAdapter = null;
            this.tableAdapterManager.AdministrationsTableAdapter = null;
            this.tableAdapterManager.AK_BasicBayWorks_historyTableAdapter = null;
            this.tableAdapterManager.AK_EmployeesTableAdapter = null;
            this.tableAdapterManager.AK_events_linesTableAdapter = null;
            this.tableAdapterManager.AK_eventsTableAdapter = null;
            this.tableAdapterManager.AK_FieldsTableAdapter = null;
            this.tableAdapterManager.AK_FormsTableAdapter = null;
            this.tableAdapterManager.AK_loginsTableAdapter = null;
            this.tableAdapterManager.AK_MenuTableAdapter = null;
            this.tableAdapterManager.AK_Modules_linesTableAdapter = this.aK_Modules_linesTableAdapter;
            this.tableAdapterManager.AK_ModulesTableAdapter = null;
            this.tableAdapterManager.AK_QueriesTableAdapter = null;
            this.tableAdapterManager.AK_Roles_linesTableAdapter = this.aK_Roles_linesTableAdapter;
            this.tableAdapterManager.AK_RolesTableAdapter = this.aK_RolesTableAdapter;
            this.tableAdapterManager.AK_ShiftsTableAdapter = null;
            this.tableAdapterManager.AK_SimpleTableAdapter = null;
            this.tableAdapterManager.ak_UsersTableAdapter = null;
            this.tableAdapterManager.AK_work_periodsTableAdapter = null;
            this.tableAdapterManager.AllowanceGroupsTableAdapter = null;
            this.tableAdapterManager.AttachmentsTableAdapter = null;
            this.tableAdapterManager.AttendancesTableAdapter = null;
            this.tableAdapterManager.AttendanceTypesTableAdapter = null;
            this.tableAdapterManager.AttTableTableAdapter = null;
            this.tableAdapterManager.AuthoritiesTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.BankAccountsTableAdapter = null;
            this.tableAdapterManager.BasicBayWorkLinesTableAdapter = null;
            this.tableAdapterManager.BasicBayWorks_historyTableAdapter = null;
            this.tableAdapterManager.BasicBayWorksTableAdapter = null;
            this.tableAdapterManager.BonusTableAdapter = null;
            this.tableAdapterManager.CashOrBanksTableAdapter = null;
            this.tableAdapterManager.ChangelogDetailsTableAdapter = null;
            this.tableAdapterManager.changeLogTableAdapter = null;
            this.tableAdapterManager.ClassesTableAdapter = null;
            this.tableAdapterManager.CompaniesTableAdapter = null;
            this.tableAdapterManager.CompanyDetailsTableAdapter = null;
            this.tableAdapterManager.ContractTypesTableAdapter = null;
            this.tableAdapterManager.ContributionPeriodsTableAdapter = null;
            this.tableAdapterManager.CostCentersTableAdapter = null;
            this.tableAdapterManager.CountryOfBirthsTableAdapter = null;
            this.tableAdapterManager.CtizenshipsTableAdapter = null;
            this.tableAdapterManager.CurrenciesTableAdapter = null;
            this.tableAdapterManager.CustomersTableAdapter = null;
            this.tableAdapterManager.DeductionsTableAdapter = null;
            this.tableAdapterManager.DelayFromToesTableAdapter = null;
            this.tableAdapterManager.DelaysTableAdapter = null;
            this.tableAdapterManager.DepartementsTableAdapter = null;
            this.tableAdapterManager.DeptAssignTableAdapter = null;
            this.tableAdapterManager.EducationalStatusTableAdapter = null;
            this.tableAdapterManager.EmpAssignTableAdapter = null;
            this.tableAdapterManager.EmployeeGroupsTableAdapter = null;
            this.tableAdapterManager.EmployeesTableAdapter = null;
            this.tableAdapterManager.EmployeeSubGroupsTableAdapter = null;
            this.tableAdapterManager.EmployeeTransferTableAdapter = null;
            this.tableAdapterManager.EventTypesTableAdapter = null;
            this.tableAdapterManager.FinancesTableAdapter = null;
            this.tableAdapterManager.GeneralsTableAdapter = null;
            this.tableAdapterManager.GradesTableAdapter = null;
            this.tableAdapterManager.HealthInsuranceCompanyNamesTableAdapter = null;
            this.tableAdapterManager.HomeCitiesTableAdapter = null;
            this.tableAdapterManager.HomeCountriesTableAdapter = null;
            this.tableAdapterManager.HomesTableAdapter = null;
            this.tableAdapterManager.IncentivesTableAdapter = null;
            this.tableAdapterManager.InsuranceAuthoritiesTableAdapter = null;
            this.tableAdapterManager.InsuranceDetailsTableAdapter = null;
            this.tableAdapterManager.InsurancePrecentagesTableAdapter = null;
            this.tableAdapterManager.InsuranceTypesTableAdapter = null;
            this.tableAdapterManager.JobHistoryTableAdapter = null;
            this.tableAdapterManager.JobTitlesTableAdapter = null;
            this.tableAdapterManager.LeaveFromToesTableAdapter = null;
            this.tableAdapterManager.LevelsTableAdapter = null;
            this.tableAdapterManager.LiveCoversTableAdapter = null;
            this.tableAdapterManager.LocationsTableAdapter = null;
            this.tableAdapterManager.MacTableAdapter = null;
            this.tableAdapterManager.MedicalCoversTableAdapter = null;
            this.tableAdapterManager.MessagesTableAdapter = null;
            this.tableAdapterManager.MonthsTableAdapter = null;
            this.tableAdapterManager.NationalitiesTableAdapter = null;
            this.tableAdapterManager.OtherAllowancesTableAdapter = null;
            this.tableAdapterManager.PayllorAreasTableAdapter = null;
            this.tableAdapterManager.PaymentDeductionsTableAdapter = null;
            this.tableAdapterManager.PaymentsTableAdapter = null;
            this.tableAdapterManager.PayScaleAreasTableAdapter = null;
            this.tableAdapterManager.PayScaleTypesTableAdapter = null;
            this.tableAdapterManager.PersonalsTableAdapter = null;
            this.tableAdapterManager.PlaceOfBirthsTableAdapter = null;
            this.tableAdapterManager.PostingPeriodsTableAdapter = null;
            this.tableAdapterManager.ProductivityTableAdapter = null;
            this.tableAdapterManager.ProfitsTableAdapter = null;
            this.tableAdapterManager.ReasonForEventsTableAdapter = null;
            this.tableAdapterManager.RecurringOneTimesTableAdapter = null;
            this.tableAdapterManager.ReligionsTableAdapter = null;
            this.tableAdapterManager.RemarksTableAdapter = null;
            this.tableAdapterManager.RequestsTableAdapter = null;
            this.tableAdapterManager.Roles2TableAdapter = null;
            this.tableAdapterManager.RolesTableAdapter = null;
            this.tableAdapterManager.SalarySheetGroupsTableAdapter = null;
            this.tableAdapterManager.SalarySheetsTableAdapter = null;
            this.tableAdapterManager.SalarySheetTableAdapter = null;
            this.tableAdapterManager.sanctions_reasonsTableAdapter = null;
            this.tableAdapterManager.SanctionsTableAdapter = null;
            this.tableAdapterManager.ShiftHolidaysTableAdapter = null;
            this.tableAdapterManager.ShiftsTableAdapter = null;
            this.tableAdapterManager.SocialInsurancesTableAdapter = null;
            this.tableAdapterManager.StatusTableAdapter = null;
            this.tableAdapterManager.TaxesTableAdapter = null;
            this.tableAdapterManager.TempShiftEntryTableAdapter = null;
            this.tableAdapterManager.tempshiftsTableAdapter = null;
            this.tableAdapterManager.TransferTableAdapter = null;
            this.tableAdapterManager.TypeEmployeesTableAdapter = null;
            this.tableAdapterManager.TypeOfHealthInsurancesTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = admin.HrDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.users_AuthoritiesTableAdapter = null;
            this.tableAdapterManager.Users2TableAdapter = null;
            this.tableAdapterManager.UsersTableAdapter = null;
            this.tableAdapterManager.VacationsTableAdapter = null;
            this.tableAdapterManager.WageCodesTableAdapter = null;
            this.tableAdapterManager.XReligionsTableAdapter = null;
            // 
            // aK_Modules_linesTableAdapter
            // 
            this.aK_Modules_linesTableAdapter.ClearBeforeFill = true;
            // 
            // aK_Roles_linesTableAdapter
            // 
            this.aK_Roles_linesTableAdapter.ClearBeforeFill = true;
            // 
            // Roles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 382);
            this.Controls.Add(this.aK_Roles_linesDataGridView);
            this.Controls.Add(idLabel);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(role_nameLabel);
            this.Controls.Add(this.role_nameTextBox);
            this.Controls.Add(this.aK_RolesBindingNavigator);
            this.Name = "Roles";
            this.Text = "Roles";
            this.Load += new System.EventHandler(this.Roles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.aK_RolesBindingNavigator)).EndInit();
            this.aK_RolesBindingNavigator.ResumeLayout(false);
            this.aK_RolesBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aK_RolesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hrDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Roles_linesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Roles_linesDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aKModuleslinesBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HrDataSet hrDataSet;
        private System.Windows.Forms.BindingSource aK_RolesBindingSource;
        private HrDataSetTableAdapters.AK_RolesTableAdapter aK_RolesTableAdapter;
        private HrDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator aK_RolesBindingNavigator;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripButton aK_RolesBindingNavigatorSaveItem;
        private HrDataSetTableAdapters.AK_Roles_linesTableAdapter aK_Roles_linesTableAdapter;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox role_nameTextBox;
        private System.Windows.Forms.BindingSource aK_Roles_linesBindingSource;
        private System.Windows.Forms.DataGridView aK_Roles_linesDataGridView;
        private HrDataSetTableAdapters.AK_Modules_linesTableAdapter aK_Modules_linesTableAdapter;
        private System.Windows.Forms.BindingSource aKModuleslinesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
    }
}