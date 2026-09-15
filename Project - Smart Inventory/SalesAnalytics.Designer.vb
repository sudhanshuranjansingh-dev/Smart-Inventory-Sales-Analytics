<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SalesAnalytics
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
        lblTotalSales = New Label()
        lblTotalOrders = New Label()
        lblAverageOrders = New Label()
        lblFrom = New Label()
        lblTo = New Label()
        dtpFromDate = New DateTimePicker()
        dtpToDate = New DateTimePicker()
        btnRefresh = New Button()
        plotSales = New ScottPlot.WinForms.FormsPlot()
        lblCategory = New Label()
        cmbCategory = New ComboBox()
        pnlTotalSales = New Panel()
        pnlTotalOrders = New Panel()
        pnlAverageOrder = New Panel()
        dgvPaymentSales = New DataGridView()
        btnExportReport = New Button()
        CType(dgvPaymentSales, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 21.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(385, 12)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(337, 50)
        lblTitle.TabIndex = 0
        lblTitle.Text = "SALES ANALYTICS"
        ' 
        ' lblTotalSales
        ' 
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalSales.Location = New Point(85, 269)
        lblTotalSales.Name = "lblTotalSales"
        lblTotalSales.Size = New Size(154, 37)
        lblTotalSales.TabIndex = 1
        lblTotalSales.Text = "Total Sales"
        ' 
        ' lblTotalOrders
        ' 
        lblTotalOrders.AutoSize = True
        lblTotalOrders.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalOrders.Location = New Point(440, 269)
        lblTotalOrders.Name = "lblTotalOrders"
        lblTotalOrders.Size = New Size(175, 37)
        lblTotalOrders.TabIndex = 2
        lblTotalOrders.Text = "Total Orders"
        ' 
        ' lblAverageOrders
        ' 
        lblAverageOrders.AutoSize = True
        lblAverageOrders.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblAverageOrders.Location = New Point(807, 269)
        lblAverageOrders.Name = "lblAverageOrders"
        lblAverageOrders.Size = New Size(216, 37)
        lblAverageOrders.TabIndex = 3
        lblAverageOrders.Text = "Average Orders"
        ' 
        ' lblFrom
        ' 
        lblFrom.AutoSize = True
        lblFrom.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblFrom.Location = New Point(14, 117)
        lblFrom.Name = "lblFrom"
        lblFrom.Size = New Size(98, 37)
        lblFrom.TabIndex = 4
        lblFrom.Text = "From :"
        ' 
        ' lblTo
        ' 
        lblTo.AutoSize = True
        lblTo.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTo.Location = New Point(419, 116)
        lblTo.Name = "lblTo"
        lblTo.Size = New Size(62, 37)
        lblTo.TabIndex = 5
        lblTo.Text = "To :"
        ' 
        ' dtpFromDate
        ' 
        dtpFromDate.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        dtpFromDate.Location = New Point(121, 117)
        dtpFromDate.Margin = New Padding(3, 4, 3, 4)
        dtpFromDate.Name = "dtpFromDate"
        dtpFromDate.Size = New Size(258, 34)
        dtpFromDate.TabIndex = 6
        ' 
        ' dtpToDate
        ' 
        dtpToDate.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        dtpToDate.Location = New Point(504, 116)
        dtpToDate.Margin = New Padding(3, 4, 3, 4)
        dtpToDate.Name = "dtpToDate"
        dtpToDate.Size = New Size(270, 34)
        dtpToDate.TabIndex = 7
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(841, 116)
        btnRefresh.Margin = New Padding(3, 4, 3, 4)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(198, 40)
        btnRefresh.TabIndex = 8
        btnRefresh.Text = "REFRESH"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' plotSales
        ' 
        plotSales.Location = New Point(37, 416)
        plotSales.Margin = New Padding(3, 4, 3, 4)
        plotSales.Name = "plotSales"
        plotSales.Size = New Size(1032, 245)
        plotSales.TabIndex = 9
        ' 
        ' lblCategory
        ' 
        lblCategory.AutoSize = True
        lblCategory.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCategory.Location = New Point(24, 185)
        lblCategory.Name = "lblCategory"
        lblCategory.Size = New Size(150, 37)
        lblCategory.TabIndex = 11
        lblCategory.Text = "Category :"
        ' 
        ' cmbCategory
        ' 
        cmbCategory.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbCategory.FormattingEnabled = True
        cmbCategory.Location = New Point(171, 195)
        cmbCategory.Margin = New Padding(3, 4, 3, 4)
        cmbCategory.Name = "cmbCategory"
        cmbCategory.Size = New Size(289, 36)
        cmbCategory.TabIndex = 12
        ' 
        ' pnlTotalSales
        ' 
        pnlTotalSales.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlTotalSales.Location = New Point(37, 329)
        pnlTotalSales.Margin = New Padding(3, 4, 3, 4)
        pnlTotalSales.Name = "pnlTotalSales"
        pnlTotalSales.Size = New Size(261, 60)
        pnlTotalSales.TabIndex = 13
        ' 
        ' pnlTotalOrders
        ' 
        pnlTotalOrders.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlTotalOrders.Location = New Point(409, 329)
        pnlTotalOrders.Margin = New Padding(3, 4, 3, 4)
        pnlTotalOrders.Name = "pnlTotalOrders"
        pnlTotalOrders.Size = New Size(261, 60)
        pnlTotalOrders.TabIndex = 14
        ' 
        ' pnlAverageOrder
        ' 
        pnlAverageOrder.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlAverageOrder.Location = New Point(778, 329)
        pnlAverageOrder.Margin = New Padding(3, 4, 3, 4)
        pnlAverageOrder.Name = "pnlAverageOrder"
        pnlAverageOrder.Size = New Size(261, 60)
        pnlAverageOrder.TabIndex = 15
        ' 
        ' dgvPaymentSales
        ' 
        dgvPaymentSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPaymentSales.Location = New Point(235, 669)
        dgvPaymentSales.Margin = New Padding(3, 4, 3, 4)
        dgvPaymentSales.Name = "dgvPaymentSales"
        dgvPaymentSales.RowHeadersWidth = 51
        dgvPaymentSales.Size = New Size(493, 232)
        dgvPaymentSales.TabIndex = 16
        ' 
        ' btnExportReport
        ' 
        btnExportReport.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnExportReport.Location = New Point(790, 725)
        btnExportReport.Margin = New Padding(3, 4, 3, 4)
        btnExportReport.Name = "btnExportReport"
        btnExportReport.Size = New Size(238, 65)
        btnExportReport.TabIndex = 17
        btnExportReport.Text = "EXPORT REPORT"
        btnExportReport.UseVisualStyleBackColor = True
        ' 
        ' SalesAnalytics
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1093, 905)
        Controls.Add(btnExportReport)
        Controls.Add(dgvPaymentSales)
        Controls.Add(pnlAverageOrder)
        Controls.Add(pnlTotalOrders)
        Controls.Add(pnlTotalSales)
        Controls.Add(cmbCategory)
        Controls.Add(lblCategory)
        Controls.Add(plotSales)
        Controls.Add(btnRefresh)
        Controls.Add(dtpToDate)
        Controls.Add(dtpFromDate)
        Controls.Add(lblTo)
        Controls.Add(lblFrom)
        Controls.Add(lblAverageOrders)
        Controls.Add(lblTotalOrders)
        Controls.Add(lblTotalSales)
        Controls.Add(lblTitle)
        Margin = New Padding(3, 4, 3, 4)
        Name = "SalesAnalytics"
        Text = "SalesAnalytics"
        CType(dgvPaymentSales, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTotalSales As Label
    Friend WithEvents lblTotalOrders As Label
    Friend WithEvents lblAverageOrders As Label
    Friend WithEvents lblFrom As Label
    Friend WithEvents lblTo As Label
    Friend WithEvents dtpFromDate As DateTimePicker
    Friend WithEvents dtpToDate As DateTimePicker
    Friend WithEvents btnRefresh As Button
    Friend WithEvents plotSales As ScottPlot.WinForms.FormsPlot
    Friend WithEvents lblCategory As Label
    Friend WithEvents cmbCategory As ComboBox
    Friend WithEvents pnlTotalSales As Panel
    Friend WithEvents pnlTotalOrders As Panel
    Friend WithEvents pnlAverageOrder As Panel
    Friend WithEvents dgvPaymentSales As DataGridView
    Friend WithEvents btnExportReport As Button
End Class
