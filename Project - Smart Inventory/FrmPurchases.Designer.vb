<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPurchases
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
        lblOutofStocks = New Label()
        lblSupplier = New Label()
        lblPurchaseNo = New Label()
        txtPurchaseNo = New TextBox()
        dtpPurchaseDate = New DateTimePicker()
        lblProduct = New Label()
        lblQuantity = New Label()
        lblPurchasePrice = New Label()
        lblTotalAmount = New Label()
        txtQuantity = New TextBox()
        txtTotalAmount = New TextBox()
        txtPurchasePrice = New TextBox()
        btnClear = New Button()
        btnDelete = New Button()
        btnUpdate = New Button()
        btnAdd = New Button()
        cmbProduct = New ComboBox()
        dgvPurchases = New DataGridView()
        txtSearch = New TextBox()
        lblSearch = New Label()
        cmbSupplier = New ComboBox()
        CType(dgvPurchases, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(299, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(310, 37)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Purchase Management"
        ' 
        ' lblOutofStocks
        ' 
        lblOutofStocks.AutoSize = True
        lblOutofStocks.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblOutofStocks.Location = New Point(478, 95)
        lblOutofStocks.Margin = New Padding(4, 0, 4, 0)
        lblOutofStocks.Name = "lblOutofStocks"
        lblOutofStocks.Size = New Size(161, 30)
        lblOutofStocks.TabIndex = 8
        lblOutofStocks.Text = "Out Of Stocks :"
        ' 
        ' lblSupplier
        ' 
        lblSupplier.AutoSize = True
        lblSupplier.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSupplier.Location = New Point(39, 189)
        lblSupplier.Margin = New Padding(4, 0, 4, 0)
        lblSupplier.Name = "lblSupplier"
        lblSupplier.Size = New Size(107, 30)
        lblSupplier.TabIndex = 7
        lblSupplier.Text = "Supplier :"
        ' 
        ' lblPurchaseNo
        ' 
        lblPurchaseNo.AutoSize = True
        lblPurchaseNo.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPurchaseNo.Location = New Point(27, 95)
        lblPurchaseNo.Margin = New Padding(4, 0, 4, 0)
        lblPurchaseNo.Name = "lblPurchaseNo"
        lblPurchaseNo.Size = New Size(149, 30)
        lblPurchaseNo.TabIndex = 6
        lblPurchaseNo.Text = "Purchase No :"
        ' 
        ' txtPurchaseNo
        ' 
        txtPurchaseNo.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtPurchaseNo.Location = New Point(216, 89)
        txtPurchaseNo.Multiline = True
        txtPurchaseNo.Name = "txtPurchaseNo"
        txtPurchaseNo.Size = New Size(211, 36)
        txtPurchaseNo.TabIndex = 9
        ' 
        ' dtpPurchaseDate
        ' 
        dtpPurchaseDate.Location = New Point(674, 101)
        dtpPurchaseDate.Name = "dtpPurchaseDate"
        dtpPurchaseDate.Size = New Size(203, 23)
        dtpPurchaseDate.TabIndex = 11
        ' 
        ' lblProduct
        ' 
        lblProduct.AutoSize = True
        lblProduct.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProduct.Location = New Point(488, 182)
        lblProduct.Margin = New Padding(4, 0, 4, 0)
        lblProduct.Name = "lblProduct"
        lblProduct.Size = New Size(103, 30)
        lblProduct.TabIndex = 13
        lblProduct.Text = "Product :"
        ' 
        ' lblQuantity
        ' 
        lblQuantity.AutoSize = True
        lblQuantity.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblQuantity.Location = New Point(39, 286)
        lblQuantity.Margin = New Padding(4, 0, 4, 0)
        lblQuantity.Name = "lblQuantity"
        lblQuantity.Size = New Size(111, 30)
        lblQuantity.TabIndex = 14
        lblQuantity.Text = "Quantity :"
        ' 
        ' lblPurchasePrice
        ' 
        lblPurchasePrice.AutoSize = True
        lblPurchasePrice.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPurchasePrice.Location = New Point(478, 265)
        lblPurchasePrice.Margin = New Padding(4, 0, 4, 0)
        lblPurchasePrice.Name = "lblPurchasePrice"
        lblPurchasePrice.Size = New Size(167, 30)
        lblPurchasePrice.TabIndex = 15
        lblPurchasePrice.Text = "Purchase Price :"
        ' 
        ' lblTotalAmount
        ' 
        lblTotalAmount.AutoSize = True
        lblTotalAmount.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalAmount.Location = New Point(39, 371)
        lblTotalAmount.Margin = New Padding(4, 0, 4, 0)
        lblTotalAmount.Name = "lblTotalAmount"
        lblTotalAmount.Size = New Size(160, 30)
        lblTotalAmount.TabIndex = 16
        lblTotalAmount.Text = "Total Amount :"
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtQuantity.Location = New Point(216, 280)
        txtQuantity.Multiline = True
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(211, 36)
        txtQuantity.TabIndex = 17
        ' 
        ' txtTotalAmount
        ' 
        txtTotalAmount.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalAmount.Location = New Point(216, 371)
        txtTotalAmount.Multiline = True
        txtTotalAmount.Name = "txtTotalAmount"
        txtTotalAmount.Size = New Size(211, 36)
        txtTotalAmount.TabIndex = 18
        ' 
        ' txtPurchasePrice
        ' 
        txtPurchasePrice.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtPurchasePrice.Location = New Point(674, 265)
        txtPurchasePrice.Multiline = True
        txtPurchasePrice.Name = "txtPurchasePrice"
        txtPurchasePrice.Size = New Size(211, 36)
        txtPurchasePrice.TabIndex = 19
        ' 
        ' btnClear
        ' 
        btnClear.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnClear.Location = New Point(722, 324)
        btnClear.Name = "btnClear"
        btnClear.Size = New Size(191, 51)
        btnClear.TabIndex = 24
        btnClear.Text = "Clear"
        btnClear.UseVisualStyleBackColor = True
        ' 
        ' btnDelete
        ' 
        btnDelete.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnDelete.Location = New Point(722, 394)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New Size(191, 51)
        btnDelete.TabIndex = 23
        btnDelete.Text = "Delete"
        btnDelete.UseVisualStyleBackColor = True
        ' 
        ' btnUpdate
        ' 
        btnUpdate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnUpdate.Location = New Point(478, 394)
        btnUpdate.Name = "btnUpdate"
        btnUpdate.Size = New Size(191, 51)
        btnUpdate.TabIndex = 22
        btnUpdate.Text = "Update"
        btnUpdate.UseVisualStyleBackColor = True
        ' 
        ' btnAdd
        ' 
        btnAdd.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAdd.Location = New Point(478, 324)
        btnAdd.Name = "btnAdd"
        btnAdd.Size = New Size(191, 51)
        btnAdd.TabIndex = 21
        btnAdd.Text = "Add"
        btnAdd.UseVisualStyleBackColor = True
        ' 
        ' cmbProduct
        ' 
        cmbProduct.FormattingEnabled = True
        cmbProduct.Location = New Point(674, 182)
        cmbProduct.Name = "cmbProduct"
        cmbProduct.Size = New Size(203, 23)
        cmbProduct.TabIndex = 25
        ' 
        ' dgvPurchases
        ' 
        dgvPurchases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPurchases.Location = New Point(1, 517)
        dgvPurchases.Name = "dgvPurchases"
        dgvPurchases.Size = New Size(954, 162)
        dgvPurchases.TabIndex = 26
        ' 
        ' txtSearch
        ' 
        txtSearch.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtSearch.Location = New Point(216, 444)
        txtSearch.Multiline = True
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(217, 44)
        txtSearch.TabIndex = 28
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(60, 444)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(103, 32)
        lblSearch.TabIndex = 27
        lblSearch.Text = "Search :"
        ' 
        ' cmbSupplier
        ' 
        cmbSupplier.FormattingEnabled = True
        cmbSupplier.Location = New Point(216, 191)
        cmbSupplier.Name = "cmbSupplier"
        cmbSupplier.Size = New Size(203, 23)
        cmbSupplier.TabIndex = 29
        ' 
        ' FrmPurchases
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(cmbSupplier)
        Controls.Add(txtSearch)
        Controls.Add(lblSearch)
        Controls.Add(dgvPurchases)
        Controls.Add(cmbProduct)
        Controls.Add(btnClear)
        Controls.Add(btnDelete)
        Controls.Add(btnUpdate)
        Controls.Add(btnAdd)
        Controls.Add(txtPurchasePrice)
        Controls.Add(txtTotalAmount)
        Controls.Add(txtQuantity)
        Controls.Add(lblTotalAmount)
        Controls.Add(lblPurchasePrice)
        Controls.Add(lblQuantity)
        Controls.Add(lblProduct)
        Controls.Add(dtpPurchaseDate)
        Controls.Add(txtPurchaseNo)
        Controls.Add(lblOutofStocks)
        Controls.Add(lblSupplier)
        Controls.Add(lblPurchaseNo)
        Controls.Add(lblTitle)
        Name = "FrmPurchases"
        Text = "FrmPurchases"
        CType(dgvPurchases, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblOutofStocks As Label
    Friend WithEvents lblSupplier As Label
    Friend WithEvents lblPurchaseNo As Label
    Friend WithEvents txtPurchaseNo As TextBox
    Friend WithEvents dtpPurchaseDate As DateTimePicker
    Friend WithEvents lblAddPurchaseItem As Label
    Friend WithEvents lblProduct As Label
    Friend WithEvents lblQuantity As Label
    Friend WithEvents lblPurchasePrice As Label
    Friend WithEvents lblTotalAmount As Label
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents txtTotalAmount As TextBox
    Friend WithEvents txtPurchasePrice As TextBox
    Friend WithEvents btnClear As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnAdd As Button
    Friend WithEvents cmbProduct As ComboBox
    Friend WithEvents dgvPurchases As DataGridView
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents lblSearch As Label
    Friend WithEvents cmbSupplier As ComboBox
End Class
