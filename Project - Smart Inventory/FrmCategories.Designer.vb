<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCategories
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
        lblCategoryManagement = New Label()
        lblCategoryName = New Label()
        txtCategoryName = New TextBox()
        btnAdd = New Button()
        lblSearch = New Label()
        txtSearch = New TextBox()
        dgvCategories = New DataGridView()
        btnClear = New Button()
        btnUpdate = New Button()
        btnDelete = New Button()
        CType(dgvCategories, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblCategoryManagement
        ' 
        lblCategoryManagement.AutoSize = True
        lblCategoryManagement.Font = New Font("Segoe UI", 25.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCategoryManagement.Location = New Point(283, 18)
        lblCategoryManagement.Name = "lblCategoryManagement"
        lblCategoryManagement.Size = New Size(401, 47)
        lblCategoryManagement.TabIndex = 0
        lblCategoryManagement.Text = "Category Management"
        ' 
        ' lblCategoryName
        ' 
        lblCategoryName.AutoSize = True
        lblCategoryName.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCategoryName.Location = New Point(28, 123)
        lblCategoryName.Name = "lblCategoryName"
        lblCategoryName.Size = New Size(233, 37)
        lblCategoryName.TabIndex = 1
        lblCategoryName.Text = "Category Name :"
        ' 
        ' txtCategoryName
        ' 
        txtCategoryName.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtCategoryName.Location = New Point(296, 113)
        txtCategoryName.Margin = New Padding(3, 2, 3, 2)
        txtCategoryName.Multiline = True
        txtCategoryName.Name = "txtCategoryName"
        txtCategoryName.Size = New Size(401, 45)
        txtCategoryName.TabIndex = 2
        ' 
        ' btnAdd
        ' 
        btnAdd.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(89, 192)
        btnAdd.Margin = New Padding(3, 2, 3, 2)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(164, 56)
        btnAdd.TabIndex = 3
        btnAdd.Text = "ADD"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(89, 292)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(116, 37)
        lblSearch.TabIndex = 4
        lblSearch.Text = "Search :"
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtSearch.Location = New Point(296, 292)
        txtSearch.Margin = New Padding(3, 2, 3, 2)
        txtSearch.Multiline = True
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(401, 46)
        txtSearch.TabIndex = 5
        ' 
        ' dgvCategories
        ' 
        dgvCategories.AllowUserToAddRows = False
        dgvCategories.AllowUserToDeleteRows = False
        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvCategories.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCategories.Location = New Point(4, 392)
        dgvCategories.Margin = New Padding(3, 2, 3, 2)
        dgvCategories.Name = "dgvCategories"
        dgvCategories.ReadOnly = True
        dgvCategories.RowHeadersWidth = 51
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCategories.Size = New Size(953, 287)
        dgvCategories.TabIndex = 6
        ' 
        ' btnClear
        ' 
        btnClear.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClear.Location = New Point(305, 192)
        btnClear.Margin = New Padding(3, 2, 3, 2)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(164, 56)
        btnClear.TabIndex = 7
        btnClear.Text = "CLEAR"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(516, 192)
        btnUpdate.Margin = New Padding(3, 2, 3, 2)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(164, 56)
        btnUpdate.TabIndex = 8
        btnUpdate.Text = "UPDATE"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDelete.Location = New Point(738, 192)
        btnDelete.Margin = New Padding(3, 2, 3, 2)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(164, 56)
        btnDelete.TabIndex = 9
        btnDelete.Text = "DELETE"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' FrmCategories
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(btnDelete)
        Controls.Add(btnUpdate)
        Controls.Add(btnClear)
        Controls.Add(dgvCategories)
        Controls.Add(txtSearch)
        Controls.Add(lblSearch)
        Controls.Add(btnAdd)
        Controls.Add(txtCategoryName)
        Controls.Add(lblCategoryName)
        Controls.Add(lblCategoryManagement)
        Margin = New Padding(3, 2, 3, 2)
        Name = "FrmCategories"
        Text = "FrmCategories"
        CType(dgvCategories, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblCategoryManagement As Label
    Friend WithEvents lblCategoryName As Label
    Friend WithEvents txtCategoryName As TextBox
    Friend WithEvents btnAdd As Button
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents dgvCategories As DataGridView
    Friend WithEvents btnClear As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
End Class
