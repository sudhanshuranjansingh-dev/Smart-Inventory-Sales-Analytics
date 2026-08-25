<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSalesHistory
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
        lblSearch = New Label()
        lblFromDate = New Label()
        lblToDate = New Label()
        txtSearch = New TextBox()
        dtpFromDate = New DateTimePicker()
        dtpToDate = New DateTimePicker()
        lblPayment = New Label()
        cmbPaymentFilter = New ComboBox()
        lblStatus = New Label()
        cmbStatusFilter = New ComboBox()
        btnRefresh = New Button()
        dgvSalesHistory = New DataGridView()
        lblTotalSales = New Label()
        txtTotalSales = New TextBox()
        lblTotalOrders = New Label()
        txtTotalOrders = New TextBox()
        btnViewDetails = New Button()
        btnPrintInvoice = New Button()
        CType(dgvSalesHistory, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(344, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(229, 40)
        lblTitle.TabIndex = 0
        lblTitle.Text = "SALES HISTORY"
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(22, 129)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(103, 32)
        lblSearch.TabIndex = 1
        lblSearch.Text = "Search :"
        ' 
        ' lblFromDate
        ' 
        lblFromDate.AutoSize = True
        lblFromDate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblFromDate.Location = New Point(395, 126)
        lblFromDate.Name = "lblFromDate"
        lblFromDate.Size = New Size(87, 32)
        lblFromDate.TabIndex = 2
        lblFromDate.Text = "From :"
        ' 
        ' lblToDate
        ' 
        lblToDate.AutoSize = True
        lblToDate.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblToDate.Location = New Point(684, 126)
        lblToDate.Name = "lblToDate"
        lblToDate.Size = New Size(55, 32)
        lblToDate.TabIndex = 3
        lblToDate.Text = "To :"
        ' 
        ' txtSearch
        ' 
        txtSearch.Location = New Point(145, 127)
        txtSearch.Multiline = True
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(214, 32)
        txtSearch.TabIndex = 4
        ' 
        ' dtpFromDate
        ' 
        dtpFromDate.Location = New Point(488, 129)
        dtpFromDate.Name = "dtpFromDate"
        dtpFromDate.Size = New Size(190, 23)
        dtpFromDate.TabIndex = 5
        ' 
        ' dtpToDate
        ' 
        dtpToDate.Location = New Point(745, 129)
        dtpToDate.Name = "dtpToDate"
        dtpToDate.Size = New Size(190, 23)
        dtpToDate.TabIndex = 6
        ' 
        ' lblPayment
        ' 
        lblPayment.AutoSize = True
        lblPayment.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPayment.Location = New Point(22, 224)
        lblPayment.Name = "lblPayment"
        lblPayment.Size = New Size(127, 32)
        lblPayment.TabIndex = 7
        lblPayment.Text = "Payment :"
        ' 
        ' cmbPaymentFilter
        ' 
        cmbPaymentFilter.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbPaymentFilter.FormattingEnabled = True
        cmbPaymentFilter.Location = New Point(165, 218)
        cmbPaymentFilter.Name = "cmbPaymentFilter"
        cmbPaymentFilter.Size = New Size(194, 38)
        cmbPaymentFilter.TabIndex = 8
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStatus.Location = New Point(395, 219)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(97, 32)
        lblStatus.TabIndex = 9
        lblStatus.Text = "Status :"
        ' 
        ' cmbStatusFilter
        ' 
        cmbStatusFilter.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbStatusFilter.FormattingEnabled = True
        cmbStatusFilter.Location = New Point(498, 213)
        cmbStatusFilter.Name = "cmbStatusFilter"
        cmbStatusFilter.Size = New Size(180, 38)
        cmbStatusFilter.TabIndex = 10
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(745, 213)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(190, 38)
        btnRefresh.TabIndex = 11
        btnRefresh.Text = "REFRESH"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' dgvSalesHistory
        ' 
        dgvSalesHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSalesHistory.Location = New Point(1, 290)
        dgvSalesHistory.Name = "dgvSalesHistory"
        dgvSalesHistory.Size = New Size(955, 209)
        dgvSalesHistory.TabIndex = 12
        ' 
        ' lblTotalSales
        ' 
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalSales.Location = New Point(22, 534)
        lblTotalSales.Name = "lblTotalSales"
        lblTotalSales.Size = New Size(148, 32)
        lblTotalSales.TabIndex = 13
        lblTotalSales.Text = "Total Sales :"
        ' 
        ' txtTotalSales
        ' 
        txtTotalSales.Location = New Point(209, 534)
        txtTotalSales.Multiline = True
        txtTotalSales.Name = "txtTotalSales"
        txtTotalSales.Size = New Size(214, 32)
        txtTotalSales.TabIndex = 14
        ' 
        ' lblTotalOrders
        ' 
        lblTotalOrders.AutoSize = True
        lblTotalOrders.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalOrders.Location = New Point(530, 534)
        lblTotalOrders.Name = "lblTotalOrders"
        lblTotalOrders.Size = New Size(157, 32)
        lblTotalOrders.TabIndex = 15
        lblTotalOrders.Text = "Total Order :"
        ' 
        ' txtTotalOrders
        ' 
        txtTotalOrders.Location = New Point(721, 534)
        txtTotalOrders.Multiline = True
        txtTotalOrders.Name = "txtTotalOrders"
        txtTotalOrders.Size = New Size(214, 32)
        txtTotalOrders.TabIndex = 16
        ' 
        ' btnViewDetails
        ' 
        btnViewDetails.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnViewDetails.Location = New Point(257, 611)
        btnViewDetails.Name = "btnViewDetails"
        btnViewDetails.Size = New Size(190, 38)
        btnViewDetails.TabIndex = 17
        btnViewDetails.Text = "VIEW DETAILS"
        btnViewDetails.UseVisualStyleBackColor = True
        ' 
        ' btnPrintInvoice
        ' 
        btnPrintInvoice.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnPrintInvoice.Location = New Point(463, 611)
        btnPrintInvoice.Name = "btnPrintInvoice"
        btnPrintInvoice.Size = New Size(190, 38)
        btnPrintInvoice.TabIndex = 18
        btnPrintInvoice.Text = "PRINT INVOICE"
        btnPrintInvoice.UseVisualStyleBackColor = True
        ' 
        ' FrmSalesHistory
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(btnPrintInvoice)
        Controls.Add(btnViewDetails)
        Controls.Add(txtTotalOrders)
        Controls.Add(lblTotalOrders)
        Controls.Add(txtTotalSales)
        Controls.Add(lblTotalSales)
        Controls.Add(dgvSalesHistory)
        Controls.Add(btnRefresh)
        Controls.Add(cmbStatusFilter)
        Controls.Add(lblStatus)
        Controls.Add(cmbPaymentFilter)
        Controls.Add(lblPayment)
        Controls.Add(dtpToDate)
        Controls.Add(dtpFromDate)
        Controls.Add(txtSearch)
        Controls.Add(lblToDate)
        Controls.Add(lblFromDate)
        Controls.Add(lblSearch)
        Controls.Add(lblTitle)
        Margin = New Padding(3, 2, 3, 2)
        Name = "FrmSalesHistory"
        Text = "FrmSalesHistory"
        CType(dgvSalesHistory, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblSearch As Label
    Friend WithEvents lblFromDate As Label
    Friend WithEvents lblToDate As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents lblPayment As Label
    Friend WithEvents cmbPaymentFilter As ComboBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents cmbStatusFilter As ComboBox
    Friend WithEvents btnRefresh As Button
    Friend WithEvents dgvSalesHistory As DataGridView
    Friend WithEvents lblTotalSales As Label
    Friend WithEvents txtTotalSales As TextBox
    Friend WithEvents lblTotalOrders As Label
    Friend WithEvents txtTotalOrders As TextBox
    Friend WithEvents btnViewDetails As Button
    Friend WithEvents btnPrintInvoice As Button
End Class
