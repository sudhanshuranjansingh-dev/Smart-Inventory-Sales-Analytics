<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReports
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
        lblFrom = New Label()
        lblTo = New Label()
        dtpFromDate = New DateTimePicker()
        dtpToDate = New DateTimePicker()
        btnSearch = New Button()
        btnRefresh = New Button()
        dgvSalesReport = New DataGridView()
        btnPrint = New Button()
        btnExport = New Button()
        lblTotalSales = New Label()
        Label1 = New Label()
        txtTotalSales = New TextBox()
        txtTotalOrders = New TextBox()
        CType(dgvSalesReport, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(365, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(239, 50)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Sales Report"
        ' 
        ' lblFrom
        ' 
        lblFrom.AutoSize = True
        lblFrom.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblFrom.Location = New Point(19, 117)
        lblFrom.Name = "lblFrom"
        lblFrom.Size = New Size(102, 38)
        lblFrom.TabIndex = 1
        lblFrom.Text = "From :"
        ' 
        ' lblTo
        ' 
        lblTo.AutoSize = True
        lblTo.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTo.Location = New Point(382, 117)
        lblTo.Name = "lblTo"
        lblTo.Size = New Size(63, 38)
        lblTo.TabIndex = 2
        lblTo.Text = "To :"
        ' 
        ' dtpFromDate
        ' 
        dtpFromDate.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        dtpFromDate.Location = New Point(127, 121)
        dtpFromDate.Name = "dtpFromDate"
        dtpFromDate.Size = New Size(224, 34)
        dtpFromDate.TabIndex = 3
        ' 
        ' dtpToDate
        ' 
        dtpToDate.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        dtpToDate.Location = New Point(451, 121)
        dtpToDate.Name = "dtpToDate"
        dtpToDate.Size = New Size(224, 34)
        dtpToDate.TabIndex = 4
        ' 
        ' btnSearch
        ' 
        btnSearch.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(739, 117)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(175, 38)
        btnSearch.TabIndex = 5
        btnSearch.Text = "Search"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(670, 179)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(244, 42)
        btnRefresh.TabIndex = 6
        btnRefresh.Text = "Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' dgvSalesReport
        ' 
        dgvSalesReport.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvSalesReport.Location = New Point(50, 262)
        dgvSalesReport.Name = "dgvSalesReport"
        dgvSalesReport.RowHeadersWidth = 51
        dgvSalesReport.Size = New Size(850, 275)
        dgvSalesReport.TabIndex = 7
        ' 
        ' btnPrint
        ' 
        btnPrint.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnPrint.Location = New Point(485, 617)
        btnPrint.Name = "btnPrint"
        btnPrint.Size = New Size(175, 38)
        btnPrint.TabIndex = 8
        btnPrint.Text = "Print"
        btnPrint.UseVisualStyleBackColor = True
        ' 
        ' btnExport
        ' 
        btnExport.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnExport.Location = New Point(694, 618)
        btnExport.Name = "btnExport"
        btnExport.Size = New Size(175, 38)
        btnExport.TabIndex = 9
        btnExport.Text = "Export"
        btnExport.UseVisualStyleBackColor = True
        ' 
        ' lblTotalSales
        ' 
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalSales.Location = New Point(19, 566)
        lblTotalSales.Name = "lblTotalSales"
        lblTotalSales.Size = New Size(171, 38)
        lblTotalSales.TabIndex = 10
        lblTotalSales.Text = "Total Sales :"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(19, 617)
        Label1.Name = "Label1"
        Label1.Size = New Size(192, 38)
        Label1.TabIndex = 11
        Label1.Text = "Total Orders :"
        ' 
        ' txtTotalSales
        ' 
        txtTotalSales.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalSales.Location = New Point(196, 566)
        txtTotalSales.Multiline = True
        txtTotalSales.Name = "txtTotalSales"
        txtTotalSales.Size = New Size(192, 33)
        txtTotalSales.TabIndex = 13
        ' 
        ' txtTotalOrders
        ' 
        txtTotalOrders.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalOrders.Location = New Point(217, 623)
        txtTotalOrders.Multiline = True
        txtTotalOrders.Name = "txtTotalOrders"
        txtTotalOrders.Size = New Size(192, 33)
        txtTotalOrders.TabIndex = 14
        ' 
        ' FrmReports
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(txtTotalOrders)
        Controls.Add(txtTotalSales)
        Controls.Add(Label1)
        Controls.Add(lblTotalSales)
        Controls.Add(btnExport)
        Controls.Add(btnPrint)
        Controls.Add(dgvSalesReport)
        Controls.Add(btnRefresh)
        Controls.Add(btnSearch)
        Controls.Add(dtpToDate)
        Controls.Add(dtpFromDate)
        Controls.Add(lblTo)
        Controls.Add(lblFrom)
        Controls.Add(lblTitle)
        Name = "FrmReports"
        Text = "FrmReports"
        CType(dgvSalesReport, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents dgvSalesReport As DataGridView
    Friend WithEvents btnPrint As Button
    Friend WithEvents btnExport As Button
    Friend WithEvents lblTotalSales As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtTotalSales As TextBox
    Friend WithEvents txtTotalOrders As TextBox
End Class
