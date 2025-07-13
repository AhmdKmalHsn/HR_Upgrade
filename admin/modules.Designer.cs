namespace admin
{
    partial class modules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(modules));
            System.Windows.Forms.Label idLabel;
            System.Windows.Forms.Label moduleNameLabel;
            System.Windows.Forms.Label titleLabel;
            System.Windows.Forms.Label urlLabel;
            System.Windows.Forms.Label headerLabel;
            System.Windows.Forms.Label sQLSelectLabel;
            this.hrDataSet = new admin.HrDataSet();
            this.aK_ModulesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aK_ModulesTableAdapter = new admin.HrDataSetTableAdapters.AK_ModulesTableAdapter();
            this.tableAdapterManager = new admin.HrDataSetTableAdapters.TableAdapterManager();
            this.aK_ModulesBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.aK_ModulesBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.moduleNameTextBox = new System.Windows.Forms.TextBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.headerTextBox = new System.Windows.Forms.TextBox();
            this.sQLSelectTextBox = new System.Windows.Forms.TextBox();
            this.aK_Modules_linesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aK_Modules_linesTableAdapter = new admin.HrDataSetTableAdapters.AK_Modules_linesTableAdapter();
            this.aK_Modules_linesDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            idLabel = new System.Windows.Forms.Label();
            moduleNameLabel = new System.Windows.Forms.Label();
            titleLabel = new System.Windows.Forms.Label();
            urlLabel = new System.Windows.Forms.Label();
            headerLabel = new System.Windows.Forms.Label();
            sQLSelectLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.hrDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_ModulesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_ModulesBindingNavigator)).BeginInit();
            this.aK_ModulesBindingNavigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Modules_linesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Modules_linesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // hrDataSet
            // 
            this.hrDataSet.DataSetName = "HrDataSet";
            this.hrDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // aK_ModulesBindingSource
            // 
            this.aK_ModulesBindingSource.DataMember = "AK_Modules";
            this.aK_ModulesBindingSource.DataSource = this.hrDataSet;
            // 
            // aK_ModulesTableAdapter
            // 
            this.aK_ModulesTableAdapter.ClearBeforeFill = true;
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
            this.tableAdapterManager.AK_ModulesTableAdapter = this.aK_ModulesTableAdapter;
            this.tableAdapterManager.AK_QueriesTableAdapter = null;
            this.tableAdapterManager.AK_Roles_linesTableAdapter = null;
            this.tableAdapterManager.AK_RolesTableAdapter = null;
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
            // aK_ModulesBindingNavigator
            // 
            this.aK_ModulesBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
            this.aK_ModulesBindingNavigator.BindingSource = this.aK_ModulesBindingSource;
            this.aK_ModulesBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.aK_ModulesBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.aK_ModulesBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.aK_ModulesBindingNavigatorSaveItem});
            this.aK_ModulesBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.aK_ModulesBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.aK_ModulesBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.aK_ModulesBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.aK_ModulesBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.aK_ModulesBindingNavigator.Name = "aK_ModulesBindingNavigator";
            this.aK_ModulesBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.aK_ModulesBindingNavigator.Size = new System.Drawing.Size(641, 25);
            this.aK_ModulesBindingNavigator.TabIndex = 0;
            this.aK_ModulesBindingNavigator.Text = "bindingNavigator1";
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
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator";
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
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // aK_ModulesBindingNavigatorSaveItem
            // 
            this.aK_ModulesBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aK_ModulesBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("aK_ModulesBindingNavigatorSaveItem.Image")));
            this.aK_ModulesBindingNavigatorSaveItem.Name = "aK_ModulesBindingNavigatorSaveItem";
            this.aK_ModulesBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.aK_ModulesBindingNavigatorSaveItem.Text = "Save Data";
            this.aK_ModulesBindingNavigatorSaveItem.Click += new System.EventHandler(this.aK_ModulesBindingNavigatorSaveItem_Click_3);
            // 
            // idLabel
            // 
            idLabel.AutoSize = true;
            idLabel.Location = new System.Drawing.Point(11, 31);
            idLabel.Name = "idLabel";
            idLabel.Size = new System.Drawing.Size(21, 13);
            idLabel.TabIndex = 1;
            idLabel.Text = "Id:";
            // 
            // idTextBox
            // 
            this.idTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "Id", true));
            this.idTextBox.Location = new System.Drawing.Point(92, 28);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(100, 20);
            this.idTextBox.TabIndex = 2;
            // 
            // moduleNameLabel
            // 
            moduleNameLabel.AutoSize = true;
            moduleNameLabel.Location = new System.Drawing.Point(11, 57);
            moduleNameLabel.Name = "moduleNameLabel";
            moduleNameLabel.Size = new System.Drawing.Size(75, 13);
            moduleNameLabel.TabIndex = 3;
            moduleNameLabel.Text = "Module Name:";
            // 
            // moduleNameTextBox
            // 
            this.moduleNameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "ModuleName", true));
            this.moduleNameTextBox.Location = new System.Drawing.Point(92, 54);
            this.moduleNameTextBox.Name = "moduleNameTextBox";
            this.moduleNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.moduleNameTextBox.TabIndex = 4;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Location = new System.Drawing.Point(11, 83);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(31, 13);
            titleLabel.TabIndex = 5;
            titleLabel.Text = "Title:";
            // 
            // titleTextBox
            // 
            this.titleTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "Title", true));
            this.titleTextBox.Location = new System.Drawing.Point(92, 80);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(100, 20);
            this.titleTextBox.TabIndex = 6;
            // 
            // urlLabel
            // 
            urlLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            urlLabel.AutoSize = true;
            urlLabel.Location = new System.Drawing.Point(438, 31);
            urlLabel.Name = "urlLabel";
            urlLabel.Size = new System.Drawing.Size(23, 13);
            urlLabel.TabIndex = 7;
            urlLabel.Text = "url:";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.urlTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "url", true));
            this.urlTextBox.Location = new System.Drawing.Point(519, 28);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(100, 20);
            this.urlTextBox.TabIndex = 8;
            // 
            // headerLabel
            // 
            headerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            headerLabel.AutoSize = true;
            headerLabel.Location = new System.Drawing.Point(438, 57);
            headerLabel.Name = "headerLabel";
            headerLabel.Size = new System.Drawing.Size(46, 13);
            headerLabel.TabIndex = 9;
            headerLabel.Text = "Header:";
            // 
            // headerTextBox
            // 
            this.headerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headerTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "Header", true));
            this.headerTextBox.Location = new System.Drawing.Point(519, 54);
            this.headerTextBox.Name = "headerTextBox";
            this.headerTextBox.Size = new System.Drawing.Size(100, 20);
            this.headerTextBox.TabIndex = 10;
            // 
            // sQLSelectLabel
            // 
            sQLSelectLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            sQLSelectLabel.AutoSize = true;
            sQLSelectLabel.Location = new System.Drawing.Point(438, 83);
            sQLSelectLabel.Name = "sQLSelectLabel";
            sQLSelectLabel.Size = new System.Drawing.Size(59, 13);
            sQLSelectLabel.TabIndex = 11;
            sQLSelectLabel.Text = "SQLSelect:";
            // 
            // sQLSelectTextBox
            // 
            this.sQLSelectTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sQLSelectTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.aK_ModulesBindingSource, "SQLSelect", true));
            this.sQLSelectTextBox.Location = new System.Drawing.Point(519, 80);
            this.sQLSelectTextBox.Name = "sQLSelectTextBox";
            this.sQLSelectTextBox.Size = new System.Drawing.Size(100, 20);
            this.sQLSelectTextBox.TabIndex = 12;
            // 
            // aK_Modules_linesBindingSource
            // 
            this.aK_Modules_linesBindingSource.DataMember = "FK__AK_Module__Modul__4AC307E8";
            this.aK_Modules_linesBindingSource.DataSource = this.aK_ModulesBindingSource;
            // 
            // aK_Modules_linesTableAdapter
            // 
            this.aK_Modules_linesTableAdapter.ClearBeforeFill = true;
            // 
            // aK_Modules_linesDataGridView
            // 
            this.aK_Modules_linesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.aK_Modules_linesDataGridView.AutoGenerateColumns = false;
            this.aK_Modules_linesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.aK_Modules_linesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn7});
            this.aK_Modules_linesDataGridView.DataSource = this.aK_Modules_linesBindingSource;
            this.aK_Modules_linesDataGridView.Location = new System.Drawing.Point(12, 106);
            this.aK_Modules_linesDataGridView.Name = "aK_Modules_linesDataGridView";
            this.aK_Modules_linesDataGridView.Size = new System.Drawing.Size(617, 241);
            this.aK_Modules_linesDataGridView.TabIndex = 13;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "id";
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn2.HeaderText = "name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "label";
            this.dataGridViewTextBoxColumn3.HeaderText = "label";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Module_id";
            this.dataGridViewTextBoxColumn4.HeaderText = "Module_id";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "sidebar_title";
            this.dataGridViewTextBoxColumn5.HeaderText = "sidebar_title";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "url";
            this.dataGridViewTextBoxColumn6.HeaderText = "url";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "is_sidebar";
            this.dataGridViewCheckBoxColumn1.HeaderText = "is_sidebar";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "ordr";
            this.dataGridViewTextBoxColumn7.HeaderText = "ordr";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // modules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 368);
            this.Controls.Add(this.aK_Modules_linesDataGridView);
            this.Controls.Add(idLabel);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(moduleNameLabel);
            this.Controls.Add(this.moduleNameTextBox);
            this.Controls.Add(titleLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(urlLabel);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(headerLabel);
            this.Controls.Add(this.headerTextBox);
            this.Controls.Add(sQLSelectLabel);
            this.Controls.Add(this.sQLSelectTextBox);
            this.Controls.Add(this.aK_ModulesBindingNavigator);
            this.Name = "modules";
            this.Text = "modules";
            this.Load += new System.EventHandler(this.modules_Load);
            ((System.ComponentModel.ISupportInitialize)(this.hrDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_ModulesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_ModulesBindingNavigator)).EndInit();
            this.aK_ModulesBindingNavigator.ResumeLayout(false);
            this.aK_ModulesBindingNavigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Modules_linesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aK_Modules_linesDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HrDataSet hrDataSet;
        private System.Windows.Forms.BindingSource aK_ModulesBindingSource;
        private HrDataSetTableAdapters.AK_ModulesTableAdapter aK_ModulesTableAdapter;
        private HrDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator aK_ModulesBindingNavigator;
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
        private System.Windows.Forms.ToolStripButton aK_ModulesBindingNavigatorSaveItem;
        private HrDataSetTableAdapters.AK_Modules_linesTableAdapter aK_Modules_linesTableAdapter;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.TextBox moduleNameTextBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.TextBox headerTextBox;
        private System.Windows.Forms.TextBox sQLSelectTextBox;
        private System.Windows.Forms.BindingSource aK_Modules_linesBindingSource;
        private System.Windows.Forms.DataGridView aK_Modules_linesDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}