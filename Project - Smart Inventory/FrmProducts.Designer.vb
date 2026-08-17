<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProducts
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
        lblProductCode = New Label()
        TextBox1 = New TextBox()
        txtProductName = New TextBox()
        lblProductName = New Label()
        lblCategory = New Label()
        cmbCategory = New ComboBox()
        cmbSupplier = New ComboBox()
        lblSupplier = New Label()
        txtPurchasePrice = New TextBox()
        lblPurchasePrice = New Label()
        txtSellingPrice = New TextBox()
        lblSellingPrice = New Label()
        txtStock = New TextBox()
        lblStock = New Label()
        TextBox2 = New TextBox()
        lblMinimumStock = New Label()
        btnAdd = New Button()
        btnUpdate = New Button()
        btnDelete = New Button()
        btnClear = New Button()
        TextBox3 = New TextBox()
        lblSearch = New Label()
        dgvProducts = New DataGridView()
        CType(dgvProducts, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(397, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(368, 46)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Product Management"
        ' 
        ' lblProductCode
        ' 
        lblProductCode.AutoSize = True
        lblProductCode.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductCode.Location = New Point(26, 111)
        lblProductCode.Name = "lblProductCode"
        lblProductCode.Size = New Size(184, 32)
        lblProductCode.TabIndex = 1
        lblProductCode.Text = "Product Code :"
        ' 
        ' TextBox1
        ' 
        TextBox1.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox1.Location = New Point(217, 96)
        TextBox1.Margin = New Padding(3, 4, 3, 4)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(241, 47)
        TextBox1.TabIndex = 2
        ' 
        ' txtProductName
        ' 
        txtProductName.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtProductName.Location = New Point(838, 84)
        txtProductName.Margin = New Padding(3, 4, 3, 4)
        txtProductName.Multiline = True
        txtProductName.Name = "txtProductName"
        txtProductName.Size = New Size(241, 47)
        txtProductName.TabIndex = 4
        ' 
        ' lblProductName
        ' 
        lblProductName.AutoSize = True
        lblProductName.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductName.Location = New Point(647, 99)
        lblProductName.Name = "lblProductName"
        lblProductName.Size = New Size(193, 32)
        lblProductName.TabIndex = 3
        lblProductName.Text = "Product Name :"
        ' 
        ' lblCategory
        ' 
        lblCategory.AutoSize = True
        lblCategory.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCategory.Location = New Point(30, 204)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(132, 32)
        lblCategory.TabIndex = 5
        lblCategory.Text = "Category :"
        ' 
        ' cmbCategory
        ' 
        cmbCategory.AllowDrop = True
        cmbCategory.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbCategory.FormattingEnabled = True
        cmbCategory.Location = New Point(217, 211)
        cmbCategory.Margin = New Padding(3, 4, 3, 4)
        cmbCategory.Name = "cmbCategory"
        cmbCategory.Size = New Size(241, 29)
        cmbCategory.TabIndex = 6
        ' 
        ' cmbSupplier
        ' 
        cmbSupplier.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbSupplier.FormattingEnabled = True
        cmbSupplier.Location = New Point(838, 215)
        cmbSupplier.Margin = New Padding(3, 4, 3, 4)
        cmbSupplier.Name = "cmbSupplier"
        cmbSupplier.Size = New Size(241, 29)
        cmbSupplier.TabIndex = 8
        ' 
        ' lblSupplier
        ' 
        lblSupplier.AutoSize = True
        lblSupplier.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSupplier.Location = New Point(651, 208)
        lblSupplier.Name = "lblSupplier"
        lblSupplier.Size = New Size(123, 32)
        lblSupplier.TabIndex = 7
        lblSupplier.Text = "Supplier :"
        ' 
        ' txtPurchasePrice
        ' 
        txtPurchasePrice.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtPurchasePrice.Location = New Point(217, 296)
        txtPurchasePrice.Margin = New Padding(3, 4, 3, 4)
        txtPurchasePrice.Multiline = True
        txtPurchasePrice.Name = "txtPurchasePrice"
        txtPurchasePrice.Size = New Size(241, 47)
        txtPurchasePrice.TabIndex = 10
        ' 
        ' lblPurchasePrice
        ' 
        lblPurchasePrice.AutoSize = True
        lblPurchasePrice.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPurchasePrice.Location = New Point(26, 311)
        lblPurchasePrice.Name = "lblPurchasePrice"
        lblPurchasePrice.Size = New Size(195, 32)
        lblPurchasePrice.TabIndex = 9
        lblPurchasePrice.Text = "Purchase Price :"
        ' 
        ' txtSellingPrice
        ' 
        txtSellingPrice.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtSellingPrice.Location = New Point(838, 300)
        txtSellingPrice.Margin = New Padding(3, 4, 3, 4)
        txtSellingPrice.Multiline = True
        txtSellingPrice.Name = "txtSellingPrice"
        txtSellingPrice.Size = New Size(241, 47)
        txtSellingPrice.TabIndex = 12
        ' 
        ' lblSellingPrice
        ' 
        lblSellingPrice.AutoSize = True
        lblSellingPrice.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSellingPrice.Location = New Point(647, 315)
        lblSellingPrice.Name = "lblSellingPrice"
        lblSellingPrice.Size = New Size(169, 32)
        lblSellingPrice.TabIndex = 11
        lblSellingPrice.Text = "Selling Price :"
        ' 
        ' txtStock
        ' 
        txtStock.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtStock.Location = New Point(217, 411)
        txtStock.Margin = New Padding(3, 4, 3, 4)
        txtStock.Multiline = True
        txtStock.Name = "txtStock"
        txtStock.Size = New Size(241, 47)
        txtStock.TabIndex = 14
        ' 
        ' lblStock
        ' 
        lblStock.AutoSize = True
        lblStock.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStock.Location = New Point(62, 425)
        lblStock.Name = "lblStock"
        lblStock.Size = New Size(89, 32)
        lblStock.TabIndex = 13
        lblStock.Text = "Stock :"
        ' 
        ' TextBox2
        ' 
        TextBox2.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox2.Location = New Point(838, 429)
        TextBox2.Margin = New Padding(3, 4, 3, 4)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(241, 47)
        TextBox2.TabIndex = 16
        ' 
        ' lblMinimumStock
        ' 
        lblMinimumStock.AutoSize = True
        lblMinimumStock.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblMinimumStock.Location = New Point(633, 444)
        lblMinimumStock.Name = "lblMinimumStock"
        lblMinimumStock.Size = New Size(207, 32)
        lblMinimumStock.TabIndex = 15
        lblMinimumStock.Text = "Minimum Stock :"
        ' 
        ' btnAdd
        ' 
        btnAdd.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(26, 525)
        btnAdd.Margin = New Padding(3, 4, 3, 4)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(218, 68)
        btnAdd.TabIndex = 17
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(578, 525)
        btnUpdate.Margin = New Padding(3, 4, 3, 4)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(218, 68)
        btnUpdate.TabIndex = 18
        btnUpdate.Text = "Update"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDelete.Location = New Point(861, 525)
        btnDelete.Margin = New Padding(3, 4, 3, 4)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(218, 68)
        btnDelete.TabIndex = 19
        btnDelete.Text = "Delete"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' btnClear
        ' 
        btnClear.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClear.Location = New Point(301, 525)
        btnClear.Margin = New Padding(3, 4, 3, 4)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(218, 68)
        btnClear.TabIndex = 20
        btnClear.Text = "Clear"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox3.Location = New Point(277, 629)
        TextBox3.Margin = New Padding(3, 4, 3, 4)
        TextBox3.Multiline = True
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(580, 47)
        TextBox3.TabIndex = 22
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(119, 629)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(116, 37)
        lblSearch.TabIndex = 21
        lblSearch.Text = "Search :"
        ' 
        ' dgvProducts
        ' 
        dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvProducts.Location = New Point(0, 701)
        dgvProducts.Margin = New Padding(3, 4, 3, 4)
        dgvProducts.Name = "dgvProducts"
        dgvProducts.RowHeadersWidth = 51
        dgvProducts.Size = New Size(1096, 200)
        dgvProducts.TabIndex = 23
        ' 
        ' FrmProducts
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1093, 905)
        Controls.Add(dgvProducts)
        Controls.Add(TextBox3)
        Controls.Add(lblSearch)
        Controls.Add(btnClear)
        Controls.Add(btnDelete)
        Controls.Add(btnUpdate)
        Controls.Add(btnAdd)
        Controls.Add(TextBox2)
        Controls.Add(lblMinimumStock)
        Controls.Add(txtStock)
        Controls.Add(lblStock)
        Controls.Add(txtSellingPrice)
        Controls.Add(lblSellingPrice)
        Controls.Add(txtPurchasePrice)
        Controls.Add(lblPurchasePrice)
        Controls.Add(cmbSupplier)
        Controls.Add(lblSupplier)
        Controls.Add(cmbCategory)
        Controls.Add(lblCategory)
        Controls.Add(txtProductName)
        Controls.Add(lblProductName)
        Controls.Add(TextBox1)
        Controls.Add(lblProductCode)
        Controls.Add(lblTitle)
        Margin = New Padding(3, 4, 3, 4)
        Name = "FrmProducts"
        StartPosition = FormStartPosition.CenterParent
        Text = "Product Management"
        CType(dgvProducts, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblProductCode As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents txtProductName As TextBox
    Friend WithEvents lblProductName As Label
    Friend WithEvents lblCategory As Label
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents cmbSupplier As ComboBox
    Friend WithEvents lblSupplier As Label
    Friend WithEvents txtPurchasePrice As TextBox
    Friend WithEvents lblPurchasePrice As Label
    Friend WithEvents txtSellingPrice As TextBox
    Friend WithEvents lblSellingPrice As Label
    Friend WithEvents txtStock As TextBox
    Friend WithEvents lblStock As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents lblMinimumStock As Label
    Friend WithEvents btnAdd As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClear As Button
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents dgvProducts As DataGridView
End Class
