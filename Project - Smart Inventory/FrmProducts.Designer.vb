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
        lblTitle.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(293, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(263, 32)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Product Management"
        ' 
        ' lblProductCode
        ' 
        lblProductCode.AutoSize = True
        lblProductCode.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductCode.Location = New Point(23, 83)
        lblProductCode.Name = "lblProductCode"
        lblProductCode.Size = New Size(145, 25)
        lblProductCode.TabIndex = 1
        lblProductCode.Text = "Product Code :"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(190, 72)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(211, 36)
        TextBox1.TabIndex = 2
        ' 
        ' txtProductName
        ' 
        txtProductName.Location = New Point(623, 60)
        txtProductName.Multiline = True
        txtProductName.Name = "txtProductName"
        txtProductName.Size = New Size(211, 36)
        txtProductName.TabIndex = 4
        ' 
        ' lblProductName
        ' 
        lblProductName.AutoSize = True
        lblProductName.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductName.Location = New Point(456, 71)
        lblProductName.Name = "lblProductName"
        lblProductName.Size = New Size(151, 25)
        lblProductName.TabIndex = 3
        lblProductName.Text = "Product Name :"
        ' 
        ' lblCategory
        ' 
        lblCategory.AutoSize = True
        lblCategory.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCategory.Location = New Point(26, 153)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(104, 25)
        lblCategory.TabIndex = 5
        lblCategory.Text = "Category :"
        ' 
        ' cmbCategory
        ' 
        cmbCategory.AllowDrop = True
        cmbCategory.FormattingEnabled = True
        cmbCategory.Location = New Point(190, 158)
        cmbCategory.Name = "cmbCategory"
        cmbCategory.Size = New Size(211, 23)
        cmbCategory.TabIndex = 6
        ' 
        ' cmbSupplier
        ' 
        cmbSupplier.FormattingEnabled = True
        cmbSupplier.Location = New Point(623, 158)
        cmbSupplier.Name = "cmbSupplier"
        cmbSupplier.Size = New Size(211, 23)
        cmbSupplier.TabIndex = 8
        ' 
        ' lblSupplier
        ' 
        lblSupplier.AutoSize = True
        lblSupplier.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSupplier.Location = New Point(459, 153)
        lblSupplier.Name = "lblSupplier"
        lblSupplier.Size = New Size(97, 25)
        lblSupplier.TabIndex = 7
        lblSupplier.Text = "Supplier :"
        ' 
        ' txtPurchasePrice
        ' 
        txtPurchasePrice.Location = New Point(190, 222)
        txtPurchasePrice.Multiline = True
        txtPurchasePrice.Name = "txtPurchasePrice"
        txtPurchasePrice.Size = New Size(211, 36)
        txtPurchasePrice.TabIndex = 10
        ' 
        ' lblPurchasePrice
        ' 
        lblPurchasePrice.AutoSize = True
        lblPurchasePrice.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPurchasePrice.Location = New Point(23, 233)
        lblPurchasePrice.Name = "lblPurchasePrice"
        lblPurchasePrice.Size = New Size(151, 25)
        lblPurchasePrice.TabIndex = 9
        lblPurchasePrice.Text = "Purchase Price :"
        ' 
        ' txtSellingPrice
        ' 
        txtSellingPrice.Location = New Point(623, 222)
        txtSellingPrice.Multiline = True
        txtSellingPrice.Name = "txtSellingPrice"
        txtSellingPrice.Size = New Size(211, 36)
        txtSellingPrice.TabIndex = 12
        ' 
        ' lblSellingPrice
        ' 
        lblSellingPrice.AutoSize = True
        lblSellingPrice.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSellingPrice.Location = New Point(456, 233)
        lblSellingPrice.Name = "lblSellingPrice"
        lblSellingPrice.Size = New Size(131, 25)
        lblSellingPrice.TabIndex = 11
        lblSellingPrice.Text = "Selling Price :"
        ' 
        ' txtStock
        ' 
        txtStock.Location = New Point(190, 308)
        txtStock.Multiline = True
        txtStock.Name = "txtStock"
        txtStock.Size = New Size(211, 36)
        txtStock.TabIndex = 14
        ' 
        ' lblStock
        ' 
        lblStock.AutoSize = True
        lblStock.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStock.Location = New Point(54, 319)
        lblStock.Name = "lblStock"
        lblStock.Size = New Size(72, 25)
        lblStock.TabIndex = 13
        lblStock.Text = "Stock :"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(623, 319)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(211, 36)
        TextBox2.TabIndex = 16
        ' 
        ' lblMinimumStock
        ' 
        lblMinimumStock.AutoSize = True
        lblMinimumStock.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblMinimumStock.Location = New Point(444, 330)
        lblMinimumStock.Name = "lblMinimumStock"
        lblMinimumStock.Size = New Size(163, 25)
        lblMinimumStock.TabIndex = 15
        lblMinimumStock.Text = "Minimum Stock :"
        ' 
        ' btnAdd
        ' 
        btnAdd.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(23, 394)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(191, 51)
        btnAdd.TabIndex = 17
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(456, 394)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(191, 51)
        btnUpdate.TabIndex = 18
        btnUpdate.Text = "Update"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDelete.Location = New Point(656, 394)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(191, 51)
        btnDelete.TabIndex = 19
        btnDelete.Text = "Delete"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' btnClear
        ' 
        btnClear.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClear.Location = New Point(242, 394)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(191, 51)
        btnClear.TabIndex = 20
        btnClear.Text = "Clear"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' TextBox3
        ' 
        TextBox3.Location = New Point(242, 472)
        TextBox3.Multiline = True
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(405, 36)
        TextBox3.TabIndex = 22
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(104, 472)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(90, 30)
        lblSearch.TabIndex = 21
        lblSearch.Text = "Search :"
        ' 
        ' dgvProducts
        ' 
        dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvProducts.Location = New Point(0, 526)
        dgvProducts.Name = "dgvProducts"
        dgvProducts.Size = New Size(859, 150)
        dgvProducts.TabIndex = 23
        ' 
        ' FrmProducts
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(859, 709)
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
        Name = "FrmProducts"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Product Management"
        WindowState = FormWindowState.Maximized
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
