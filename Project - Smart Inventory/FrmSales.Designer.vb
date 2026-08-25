<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSales
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
        lblCustomerName = New Label()
        lblProduct = New Label()
        lblQuanity = New Label()
        txtCustomerName = New TextBox()
        txtQuantity = New TextBox()
        cmbProduct = New ComboBox()
        lblAvailability = New Label()
        lblPrice = New Label()
        btnAddToCart = New Button()
        lblAvailabilityAmount = New Label()
        lblPriceAmount = New Label()
        dgvCart = New DataGridView()
        lblPayment = New Label()
        cmbPayment = New ComboBox()
        lblSubTotal = New Label()
        lblDiscount = New Label()
        lblTotalPrice = New Label()
        txtSubtotal = New TextBox()
        txtDiscount = New TextBox()
        txtTotalPrice = New TextBox()
        btnCompleteSale = New Button()
        CType(dgvCart, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(374, 7)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(158, 45)
        lblTitle.TabIndex = 0
        lblTitle.Text = "New Sale"
        ' 
        ' lblCustomerName
        ' 
        lblCustomerName.AutoSize = True
        lblCustomerName.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCustomerName.Location = New Point(19, 80)
        lblCustomerName.Name = "lblCustomerName"
        lblCustomerName.Size = New Size(192, 30)
        lblCustomerName.TabIndex = 1
        lblCustomerName.Text = "Customer Name :"
        ' 
        ' lblProduct
        ' 
        lblProduct.AutoSize = True
        lblProduct.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProduct.Location = New Point(56, 157)
        lblProduct.Name = "lblProduct"
        lblProduct.Size = New Size(108, 30)
        lblProduct.TabIndex = 2
        lblProduct.Text = "Product :"
        ' 
        ' lblQuanity
        ' 
        lblQuanity.AutoSize = True
        lblQuanity.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblQuanity.Location = New Point(56, 237)
        lblQuanity.Name = "lblQuanity"
        lblQuanity.Size = New Size(116, 30)
        lblQuanity.TabIndex = 3
        lblQuanity.Text = "Quantity :"
        ' 
        ' txtCustomerName
        ' 
        txtCustomerName.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtCustomerName.Location = New Point(252, 80)
        txtCustomerName.Margin = New Padding(3, 2, 3, 2)
        txtCustomerName.Multiline = True
        txtCustomerName.Name = "txtCustomerName"
        txtCustomerName.Size = New Size(232, 36)
        txtCustomerName.TabIndex = 4
        ' 
        ' txtQuantity
        ' 
        txtQuantity.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtQuantity.Location = New Point(252, 231)
        txtQuantity.Margin = New Padding(3, 2, 3, 2)
        txtQuantity.Multiline = True
        txtQuantity.Name = "txtQuantity"
        txtQuantity.Size = New Size(232, 36)
        txtQuantity.TabIndex = 5
        ' 
        ' cmbProduct
        ' 
        cmbProduct.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        cmbProduct.FormattingEnabled = True
        cmbProduct.Location = New Point(252, 164)
        cmbProduct.Margin = New Padding(3, 2, 3, 2)
        cmbProduct.Name = "cmbProduct"
        cmbProduct.Size = New Size(232, 33)
        cmbProduct.TabIndex = 6
        ' 
        ' lblAvailability
        ' 
        lblAvailability.AutoSize = True
        lblAvailability.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblAvailability.Location = New Point(578, 82)
        lblAvailability.Name = "lblAvailability"
        lblAvailability.Size = New Size(141, 30)
        lblAvailability.TabIndex = 7
        lblAvailability.Text = "Availability :"
        ' 
        ' lblPrice
        ' 
        lblPrice.AutoSize = True
        lblPrice.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPrice.Location = New Point(608, 139)
        lblPrice.Name = "lblPrice"
        lblPrice.Size = New Size(77, 30)
        lblPrice.TabIndex = 8
        lblPrice.Text = "Price :"
        ' 
        ' btnAddToCart
        ' 
        btnAddToCart.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnAddToCart.Location = New Point(578, 221)
        btnAddToCart.Margin = New Padding(3, 2, 3, 2)
        btnAddToCart.Name = "btnAddToCart"
        btnAddToCart.Size = New Size(215, 46)
        btnAddToCart.TabIndex = 9
        btnAddToCart.Text = "Add To Cart "
        btnAddToCart.UseVisualStyleBackColor = True
        ' 
        ' lblAvailabilityAmount
        ' 
        lblAvailabilityAmount.AutoSize = True
        lblAvailabilityAmount.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblAvailabilityAmount.Location = New Point(772, 83)
        lblAvailabilityAmount.Name = "lblAvailabilityAmount"
        lblAvailabilityAmount.Size = New Size(39, 30)
        lblAvailabilityAmount.TabIndex = 10
        lblAvailabilityAmount.Text = "50"
        ' 
        ' lblPriceAmount
        ' 
        lblPriceAmount.AutoSize = True
        lblPriceAmount.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPriceAmount.Location = New Point(746, 139)
        lblPriceAmount.Name = "lblPriceAmount"
        lblPriceAmount.Size = New Size(65, 30)
        lblPriceAmount.TabIndex = 11
        lblPriceAmount.Text = "₹700"
        ' 
        ' dgvCart
        ' 
        dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCart.Location = New Point(1, 292)
        dgvCart.Margin = New Padding(3, 2, 3, 2)
        dgvCart.Name = "dgvCart"
        dgvCart.RowHeadersWidth = 51
        dgvCart.Size = New Size(955, 200)
        dgvCart.TabIndex = 12
        ' 
        ' lblPayment
        ' 
        lblPayment.AutoSize = True
        lblPayment.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPayment.Location = New Point(32, 530)
        lblPayment.Name = "lblPayment"
        lblPayment.Size = New Size(116, 30)
        lblPayment.TabIndex = 13
        lblPayment.Text = "Payment :"
        ' 
        ' cmbPayment
        ' 
        cmbPayment.DropDownStyle = ComboBoxStyle.DropDownList
        cmbPayment.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbPayment.FormattingEnabled = True
        cmbPayment.Items.AddRange(New Object() {"UPI", "Card", "Cash"})
        cmbPayment.Location = New Point(187, 527)
        cmbPayment.Margin = New Padding(3, 2, 3, 2)
        cmbPayment.Name = "cmbPayment"
        cmbPayment.Size = New Size(232, 38)
        cmbPayment.TabIndex = 14
        ' 
        ' lblSubTotal
        ' 
        lblSubTotal.AutoSize = True
        lblSubTotal.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSubTotal.Location = New Point(590, 508)
        lblSubTotal.Name = "lblSubTotal"
        lblSubTotal.Size = New Size(115, 30)
        lblSubTotal.TabIndex = 15
        lblSubTotal.Text = "SubTotal :"
        ' 
        ' lblDiscount
        ' 
        lblDiscount.AutoSize = True
        lblDiscount.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblDiscount.Location = New Point(590, 547)
        lblDiscount.Name = "lblDiscount"
        lblDiscount.Size = New Size(116, 30)
        lblDiscount.TabIndex = 16
        lblDiscount.Text = "Discount :"
        ' 
        ' lblTotalPrice
        ' 
        lblTotalPrice.AutoSize = True
        lblTotalPrice.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalPrice.Location = New Point(577, 591)
        lblTotalPrice.Name = "lblTotalPrice"
        lblTotalPrice.Size = New Size(128, 30)
        lblTotalPrice.TabIndex = 17
        lblTotalPrice.Text = "TotalPrice :"
        ' 
        ' txtSubtotal
        ' 
        txtSubtotal.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtSubtotal.Location = New Point(737, 505)
        txtSubtotal.Margin = New Padding(3, 2, 3, 2)
        txtSubtotal.Multiline = True
        txtSubtotal.Name = "txtSubtotal"
        txtSubtotal.Size = New Size(162, 32)
        txtSubtotal.TabIndex = 18
        ' 
        ' txtDiscount
        ' 
        txtDiscount.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtDiscount.Location = New Point(737, 547)
        txtDiscount.Margin = New Padding(3, 2, 3, 2)
        txtDiscount.Multiline = True
        txtDiscount.Name = "txtDiscount"
        txtDiscount.Size = New Size(162, 32)
        txtDiscount.TabIndex = 19
        ' 
        ' txtTotalPrice
        ' 
        txtTotalPrice.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalPrice.Location = New Point(737, 597)
        txtTotalPrice.Margin = New Padding(3, 2, 3, 2)
        txtTotalPrice.Multiline = True
        txtTotalPrice.Name = "txtTotalPrice"
        txtTotalPrice.Size = New Size(162, 32)
        txtTotalPrice.TabIndex = 20
        ' 
        ' btnCompleteSale
        ' 
        btnCompleteSale.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnCompleteSale.Location = New Point(64, 583)
        btnCompleteSale.Margin = New Padding(3, 2, 3, 2)
        btnCompleteSale.Name = "btnCompleteSale"
        btnCompleteSale.Size = New Size(355, 46)
        btnCompleteSale.TabIndex = 21
        btnCompleteSale.Text = "Complete Sale"
        btnCompleteSale.UseVisualStyleBackColor = True
        ' 
        ' FrmSales
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(btnCompleteSale)
        Controls.Add(txtTotalPrice)
        Controls.Add(txtDiscount)
        Controls.Add(txtSubtotal)
        Controls.Add(lblTotalPrice)
        Controls.Add(lblDiscount)
        Controls.Add(lblSubTotal)
        Controls.Add(cmbPayment)
        Controls.Add(lblPayment)
        Controls.Add(dgvCart)
        Controls.Add(lblPriceAmount)
        Controls.Add(lblAvailabilityAmount)
        Controls.Add(btnAddToCart)
        Controls.Add(lblPrice)
        Controls.Add(lblAvailability)
        Controls.Add(cmbProduct)
        Controls.Add(txtQuantity)
        Controls.Add(txtCustomerName)
        Controls.Add(lblQuanity)
        Controls.Add(lblProduct)
        Controls.Add(lblCustomerName)
        Controls.Add(lblTitle)
        Margin = New Padding(3, 2, 3, 2)
        Name = "FrmSales"
        StartPosition = FormStartPosition.CenterScreen
        Text = "FrmSales"
        WindowState = FormWindowState.Maximized
        CType(dgvCart, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblCustomerName As Label
    Friend WithEvents lblProduct As Label
    Friend WithEvents lblQuanity As Label
    Friend WithEvents txtCustomerName As TextBox
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents cmbProduct As ComboBox
    Friend WithEvents lblAvailability As Label
    Friend WithEvents lblPrice As Label
    Friend WithEvents btnAddToCart As Button
    Friend WithEvents lblAvailabilityAmount As Label
    Friend WithEvents lblPriceAmount As Label
    Friend WithEvents dgvCart As DataGridView
    Friend WithEvents lblPayment As Label
    Friend WithEvents cmbPayment As ComboBox
    Friend WithEvents lblSubTotal As Label
    Friend WithEvents lblDiscount As Label
    Friend WithEvents lblTotalPrice As Label
    Friend WithEvents txtSubtotal As TextBox
    Friend WithEvents txtDiscount As TextBox
    Friend WithEvents txtTotalPrice As TextBox
    Friend WithEvents btnCompleteSale As Button
End Class
