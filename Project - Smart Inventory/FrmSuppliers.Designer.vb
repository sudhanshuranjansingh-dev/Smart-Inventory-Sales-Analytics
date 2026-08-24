<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSuppliers
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        lblTitle = New Label()
        lblSupplierName = New Label()
        lblPhone = New Label()
        lablEmail = New Label()
        lblAddress = New Label()
        txtSupplierName = New TextBox()
        txtAddress = New TextBox()
        txtEmail = New TextBox()
        txtPhone = New TextBox()
        btnClear = New Button()
        btnDelete = New Button()
        btnUpdate = New Button()
        btnAdd = New Button()
        dgvSuppliers = New DataGridView()
        lblSearch = New Label()
        txtSearch = New TextBox()
        CType(dgvSuppliers, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(388, 32)
        lblTitle.Margin = New Padding(4, 0, 4, 0)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(375, 46)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Supplier Management"
        ' 
        ' lblSupplierName
        ' 
        lblSupplierName.AutoSize = True
        lblSupplierName.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSupplierName.Location = New Point(15, 166)
        lblSupplierName.Margin = New Padding(4, 0, 4, 0)
        lblSupplierName.Name = "lblSupplierName"
        lblSupplierName.Size = New Size(221, 37)
        lblSupplierName.TabIndex = 1
        lblSupplierName.Text = "Supplier Name :"
        ' 
        ' lblPhone
        ' 
        lblPhone.AutoSize = True
        lblPhone.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPhone.Location = New Point(635, 171)
        lblPhone.Margin = New Padding(4, 0, 4, 0)
        lblPhone.Name = "lblPhone"
        lblPhone.Size = New Size(112, 37)
        lblPhone.TabIndex = 2
        lblPhone.Text = "Phone :"
        ' 
        ' lablEmail
        ' 
        lablEmail.AutoSize = True
        lablEmail.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lablEmail.Location = New Point(66, 296)
        lablEmail.Margin = New Padding(4, 0, 4, 0)
        lablEmail.Name = "lablEmail"
        lablEmail.Size = New Size(101, 37)
        lablEmail.TabIndex = 3
        lablEmail.Text = "Email :"
        ' 
        ' lblAddress
        ' 
        lblAddress.AutoSize = True
        lblAddress.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblAddress.Location = New Point(635, 296)
        lblAddress.Margin = New Padding(4, 0, 4, 0)
        lblAddress.Name = "lblAddress"
        lblAddress.Size = New Size(134, 37)
        lblAddress.TabIndex = 4
        lblAddress.Text = "Address :"
        ' 
        ' txtSupplierName
        ' 
        txtSupplierName.Location = New Point(254, 166)
        txtSupplierName.Margin = New Padding(4)
        txtSupplierName.Multiline = True
        txtSupplierName.Name = "txtSupplierName"
        txtSupplierName.Size = New Size(332, 42)
        txtSupplierName.TabIndex = 5
        ' 
        ' txtAddress
        ' 
        txtAddress.Location = New Point(834, 296)
        txtAddress.Margin = New Padding(4)
        txtAddress.Multiline = True
        txtAddress.Name = "txtAddress"
        txtAddress.Size = New Size(332, 44)
        txtAddress.TabIndex = 6
        ' 
        ' txtEmail
        ' 
        txtEmail.Location = New Point(254, 296)
        txtEmail.Margin = New Padding(4)
        txtEmail.Multiline = True
        txtEmail.Name = "txtEmail"
        txtEmail.Size = New Size(332, 36)
        txtEmail.TabIndex = 7
        ' 
        ' txtPhone
        ' 
        txtPhone.Location = New Point(834, 168)
        txtPhone.Margin = New Padding(4)
        txtPhone.Multiline = True
        txtPhone.Name = "txtPhone"
        txtPhone.Size = New Size(332, 40)
        txtPhone.TabIndex = 8
        ' 
        ' btnClear
        ' 
        btnClear.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClear.Location = New Point(312, 474)
        btnClear.Margin = New Padding(4)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(239, 64)
        btnClear.TabIndex = 24
        btnClear.Text = "Clear"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDelete.Location = New Point(928, 474)
        btnDelete.Margin = New Padding(4)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(239, 64)
        btnDelete.TabIndex = 23
        btnDelete.Text = "Delete"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(629, 474)
        btnUpdate.Margin = New Padding(4)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(239, 64)
        btnUpdate.TabIndex = 22
        btnUpdate.Text = "Update"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(15, 474)
        btnAdd.Margin = New Padding(4)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(239, 64)
        btnAdd.TabIndex = 21
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' dgvSuppliers
        ' 
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSuppliers.Location = New Point(0, 592)
        dgvSuppliers.Margin = New Padding(4)
        dgvSuppliers.Name = "dgvSuppliers"
        dgvSuppliers.RowHeadersWidth = 51
        dgvSuppliers.Size = New Size(1195, 256)
        dgvSuppliers.TabIndex = 25
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(254, 374)
        lblSearch.Margin = New Padding(4, 0, 4, 0)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(116, 37)
        lblSearch.TabIndex = 26
        lblSearch.Text = "Search :"
        ' 
        ' txtSearch
        ' 
        txtSearch.Location = New Point(399, 374)
        txtSearch.Margin = New Padding(4)
        txtSearch.Multiline = True
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(479, 42)
        txtSearch.TabIndex = 27
        ' 
        ' FrmSuppliers
        ' 
        AutoScaleDimensions = New SizeF(120F, 120F)
        AutoScaleMode = AutoScaleMode.Dpi
        ClientSize = New Size(1195, 849)
        Controls.Add(txtSearch)
        Controls.Add(lblSearch)
        Controls.Add(dgvSuppliers)
        Controls.Add(btnClear)
        Controls.Add(btnDelete)
        Controls.Add(btnUpdate)
        Controls.Add(btnAdd)
        Controls.Add(txtPhone)
        Controls.Add(txtEmail)
        Controls.Add(txtAddress)
        Controls.Add(txtSupplierName)
        Controls.Add(lblAddress)
        Controls.Add(lablEmail)
        Controls.Add(lblPhone)
        Controls.Add(lblSupplierName)
        Controls.Add(lblTitle)
        Margin = New Padding(4)
        Name = "FrmSuppliers"
        StartPosition = FormStartPosition.Manual
        Text = "FrmSuppliers"
        CType(dgvSuppliers, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblSupplierName As Label
    Friend WithEvents lblPhone As Label
    Friend WithEvents lablEmail As Label
    Friend WithEvents lblAddress As Label
    Friend WithEvents txtSupplierName As TextBox
    Friend WithEvents txtAddress As TextBox
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents txtPhone As TextBox
    Friend WithEvents btnClear As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents dgvSuppliers As DataGridView
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
End Class
