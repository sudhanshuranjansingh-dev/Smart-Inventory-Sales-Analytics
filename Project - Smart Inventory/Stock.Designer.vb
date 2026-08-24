<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Stock
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
        lblTotalProducts = New Label()
        lblLowStocks = New Label()
        lblOutofStocks = New Label()
        lblTotalUnits = New Label()
        pnlTotalProducts = New Panel()
        pnlLowStock = New Panel()
        pnlOutOfStock = New Panel()
        pnlTotalUnits = New Panel()
        lblSearch = New Label()
        txtSearch = New TextBox()
        btnRefresh = New Button()
        lblFilter = New Label()
        cmbStockFilter = New ComboBox()
        dgvStock = New DataGridView()
        CType(dgvStock, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(321, 24)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(265, 37)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Stock Management"
        ' 
        ' lblTotalProducts
        ' 
        lblTotalProducts.AutoSize = True
        lblTotalProducts.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalProducts.Location = New Point(22, 113)
        lblTotalProducts.Margin = New Padding(4, 0, 4, 0)
        lblTotalProducts.Name = "lblTotalProducts"
        lblTotalProducts.Size = New Size(166, 30)
        lblTotalProducts.TabIndex = 2
        lblTotalProducts.Text = "Total Products :"
        ' 
        ' lblLowStocks
        ' 
        lblLowStocks.AutoSize = True
        lblLowStocks.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblLowStocks.Location = New Point(277, 113)
        lblLowStocks.Margin = New Padding(4, 0, 4, 0)
        lblLowStocks.Name = "lblLowStocks"
        lblLowStocks.Size = New Size(135, 30)
        lblLowStocks.TabIndex = 3
        lblLowStocks.Text = "Low Stocks :"
        ' 
        ' lblOutofStocks
        ' 
        lblOutofStocks.AutoSize = True
        lblOutofStocks.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblOutofStocks.Location = New Point(526, 113)
        lblOutofStocks.Margin = New Padding(4, 0, 4, 0)
        lblOutofStocks.Name = "lblOutofStocks"
        lblOutofStocks.Size = New Size(161, 30)
        lblOutofStocks.TabIndex = 4
        lblOutofStocks.Text = "Out Of Stocks :"
        ' 
        ' lblTotalUnits
        ' 
        lblTotalUnits.AutoSize = True
        lblTotalUnits.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalUnits.Location = New Point(757, 113)
        lblTotalUnits.Margin = New Padding(4, 0, 4, 0)
        lblTotalUnits.Name = "lblTotalUnits"
        lblTotalUnits.Size = New Size(130, 30)
        lblTotalUnits.TabIndex = 5
        lblTotalUnits.Text = "Total Units :"
        ' 
        ' pnlTotalProducts
        ' 
        pnlTotalProducts.Location = New Point(22, 172)
        pnlTotalProducts.Margin = New Padding(3, 2, 3, 2)
        pnlTotalProducts.Name = "pnlTotalProducts"
        pnlTotalProducts.Size = New Size(191, 94)
        pnlTotalProducts.TabIndex = 6
        ' 
        ' pnlLowStock
        ' 
        pnlLowStock.Location = New Point(262, 172)
        pnlLowStock.Margin = New Padding(3, 2, 3, 2)
        pnlLowStock.Name = "pnlLowStock"
        pnlLowStock.Size = New Size(191, 94)
        pnlLowStock.TabIndex = 7
        ' 
        ' pnlOutOfStock
        ' 
        pnlOutOfStock.Location = New Point(514, 172)
        pnlOutOfStock.Margin = New Padding(3, 2, 3, 2)
        pnlOutOfStock.Name = "pnlOutOfStock"
        pnlOutOfStock.Size = New Size(191, 94)
        pnlOutOfStock.TabIndex = 7
        ' 
        ' pnlTotalUnits
        ' 
        pnlTotalUnits.Location = New Point(740, 172)
        pnlTotalUnits.Margin = New Padding(3, 2, 3, 2)
        pnlTotalUnits.Name = "pnlTotalUnits"
        pnlTotalUnits.Size = New Size(191, 94)
        pnlTotalUnits.TabIndex = 7
        ' 
        ' lblSearch
        ' 
        lblSearch.AutoSize = True
        lblSearch.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearch.Location = New Point(59, 328)
        lblSearch.Margin = New Padding(4, 0, 4, 0)
        lblSearch.Name = "lblSearch"
        lblSearch.Size = New Size(90, 30)
        lblSearch.TabIndex = 8
        lblSearch.Text = "Search :"
        ' 
        ' txtSearch
        ' 
        txtSearch.Location = New Point(214, 322)
        txtSearch.Margin = New Padding(3, 2, 3, 2)
        txtSearch.Multiline = True
        txtSearch.Name = "txtSearch"
        txtSearch.Size = New Size(454, 48)
        txtSearch.TabIndex = 9
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(721, 319)
        btnRefresh.Margin = New Padding(3, 2, 3, 2)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(130, 46)
        btnRefresh.TabIndex = 10
        btnRefresh.Text = "REFERESH"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' lblFilter
        ' 
        lblFilter.AutoSize = True
        lblFilter.Font = New Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblFilter.Location = New Point(59, 418)
        lblFilter.Margin = New Padding(4, 0, 4, 0)
        lblFilter.Name = "lblFilter"
        lblFilter.Size = New Size(75, 30)
        lblFilter.TabIndex = 11
        lblFilter.Text = "Filter :"
        ' 
        ' cmbStockFilter
        ' 
        cmbStockFilter.FormattingEnabled = True
        cmbStockFilter.Location = New Point(214, 425)
        cmbStockFilter.Margin = New Padding(3, 2, 3, 2)
        cmbStockFilter.Name = "cmbStockFilter"
        cmbStockFilter.Size = New Size(280, 23)
        cmbStockFilter.TabIndex = 12
        ' 
        ' dgvStock
        ' 
        dgvStock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvStock.Location = New Point(0, 478)
        dgvStock.Margin = New Padding(3, 2, 3, 2)
        dgvStock.Name = "dgvStock"
        dgvStock.RowHeadersWidth = 51
        dgvStock.Size = New Size(957, 201)
        dgvStock.TabIndex = 13
        ' 
        ' Stock
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(dgvStock)
        Controls.Add(cmbStockFilter)
        Controls.Add(lblFilter)
        Controls.Add(btnRefresh)
        Controls.Add(txtSearch)
        Controls.Add(lblSearch)
        Controls.Add(pnlLowStock)
        Controls.Add(pnlOutOfStock)
        Controls.Add(pnlTotalUnits)
        Controls.Add(pnlTotalProducts)
        Controls.Add(lblTotalUnits)
        Controls.Add(lblOutofStocks)
        Controls.Add(lblLowStocks)
        Controls.Add(lblTotalProducts)
        Controls.Add(lblTitle)
        Margin = New Padding(3, 2, 3, 2)
        Name = "Stock"
        Text = "Stock"
        CType(dgvStock, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTotalProducts As Label
    Friend WithEvents lblLowStocks As Label
    Friend WithEvents lblOutofStocks As Label
    Friend WithEvents lblTotalUnits As Label
    Friend WithEvents pnlTotalProducts As Panel
    Friend WithEvents pnlLowStock As Panel
    Friend WithEvents pnlOutOfStock As Panel
    Friend WithEvents pnlTotalUnits As Panel
    Friend WithEvents lblSearch As Label
    Friend WithEvents txtSearch As TextBox
    Friend WithEvents btnRefresh As Button
    Friend WithEvents lblFilter As Label
    Friend WithEvents cmbStockFilter As ComboBox
    Friend WithEvents dgvStock As DataGridView
End Class
