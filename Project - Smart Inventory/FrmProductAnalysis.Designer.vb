<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProductAnalysis
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
        lblSoldItems = New Label()
        lblTotalRevenue = New Label()
        lblBestSelling = New Label()
        txtTotalProducts = New TextBox()
        txtTotalSoldItems = New TextBox()
        txtTotalRevenue = New TextBox()
        txtBestSelling = New TextBox()
        lblProductPerformance = New Label()
        dgvProductAnalysis = New DataGridView()
        btnRefresh = New Button()
        CType(dgvProductAnalysis, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(359, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(312, 50)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Product Analysis"
        ' 
        ' lblTotalProducts
        ' 
        lblTotalProducts.AutoSize = True
        lblTotalProducts.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalProducts.Location = New Point(40, 116)
        lblTotalProducts.Name = "lblTotalProducts"
        lblTotalProducts.Size = New Size(204, 38)
        lblTotalProducts.TabIndex = 1
        lblTotalProducts.Text = "Total Products"
        ' 
        ' lblSoldItems
        ' 
        lblSoldItems.AutoSize = True
        lblSoldItems.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSoldItems.Location = New Point(325, 116)
        lblSoldItems.Name = "lblSoldItems"
        lblSoldItems.Size = New Size(156, 38)
        lblSoldItems.TabIndex = 2
        lblSoldItems.Text = "Sold Items"
        ' 
        ' lblTotalRevenue
        ' 
        lblTotalRevenue.AutoSize = True
        lblTotalRevenue.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalRevenue.Location = New Point(591, 116)
        lblTotalRevenue.Name = "lblTotalRevenue"
        lblTotalRevenue.Size = New Size(200, 38)
        lblTotalRevenue.TabIndex = 3
        lblTotalRevenue.Text = "Total Revenue"
        ' 
        ' lblBestSelling
        ' 
        lblBestSelling.AutoSize = True
        lblBestSelling.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblBestSelling.Location = New Point(868, 116)
        lblBestSelling.Name = "lblBestSelling"
        lblBestSelling.Size = New Size(170, 38)
        lblBestSelling.TabIndex = 4
        lblBestSelling.Text = "Best Selling"
        ' 
        ' txtTotalProducts
        ' 
        txtTotalProducts.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalProducts.Location = New Point(40, 192)
        txtTotalProducts.Multiline = True
        txtTotalProducts.Name = "txtTotalProducts"
        txtTotalProducts.ReadOnly = True
        txtTotalProducts.Size = New Size(204, 44)
        txtTotalProducts.TabIndex = 5
        txtTotalProducts.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtTotalSoldItems
        ' 
        txtTotalSoldItems.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalSoldItems.Location = New Point(316, 192)
        txtTotalSoldItems.Multiline = True
        txtTotalSoldItems.Name = "txtTotalSoldItems"
        txtTotalSoldItems.ReadOnly = True
        txtTotalSoldItems.Size = New Size(204, 44)
        txtTotalSoldItems.TabIndex = 6
        txtTotalSoldItems.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtTotalRevenue
        ' 
        txtTotalRevenue.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalRevenue.Location = New Point(591, 192)
        txtTotalRevenue.Multiline = True
        txtTotalRevenue.Name = "txtTotalRevenue"
        txtTotalRevenue.ReadOnly = True
        txtTotalRevenue.Size = New Size(204, 44)
        txtTotalRevenue.TabIndex = 7
        txtTotalRevenue.TextAlign = HorizontalAlignment.Center
        ' 
        ' txtBestSelling
        ' 
        txtBestSelling.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtBestSelling.Location = New Point(852, 192)
        txtBestSelling.Multiline = True
        txtBestSelling.Name = "txtBestSelling"
        txtBestSelling.ReadOnly = True
        txtBestSelling.Size = New Size(204, 44)
        txtBestSelling.TabIndex = 8
        txtBestSelling.TextAlign = HorizontalAlignment.Center
        ' 
        ' lblProductPerformance
        ' 
        lblProductPerformance.AutoSize = True
        lblProductPerformance.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductPerformance.Location = New Point(325, 276)
        lblProductPerformance.Name = "lblProductPerformance"
        lblProductPerformance.Size = New Size(316, 41)
        lblProductPerformance.TabIndex = 9
        lblProductPerformance.Text = "Product Performance"
        ' 
        ' dgvProductAnalysis
        ' 
        dgvProductAnalysis.AllowUserToAddRows = False
        dgvProductAnalysis.AllowUserToDeleteRows = False
        dgvProductAnalysis.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvProductAnalysis.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvProductAnalysis.Location = New Point(40, 365)
        dgvProductAnalysis.MultiSelect = False
        dgvProductAnalysis.Name = "dgvProductAnalysis"
        dgvProductAnalysis.ReadOnly = True
        dgvProductAnalysis.RowHeadersWidth = 51
        dgvProductAnalysis.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvProductAnalysis.Size = New Size(998, 488)
        dgvProductAnalysis.TabIndex = 10
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(819, 275)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(186, 50)
        btnRefresh.TabIndex = 11
        btnRefresh.Text = "Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' FrmProductAnalysis
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1093, 905)
        Controls.Add(btnRefresh)
        Controls.Add(dgvProductAnalysis)
        Controls.Add(lblProductPerformance)
        Controls.Add(txtBestSelling)
        Controls.Add(txtTotalRevenue)
        Controls.Add(txtTotalSoldItems)
        Controls.Add(txtTotalProducts)
        Controls.Add(lblBestSelling)
        Controls.Add(lblTotalRevenue)
        Controls.Add(lblSoldItems)
        Controls.Add(lblTotalProducts)
        Controls.Add(lblTitle)
        Name = "FrmProductAnalysis"
        Text = "FrmProductAnalysis"
        CType(dgvProductAnalysis, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblTotalProducts As Label
    Friend WithEvents lblSoldItems As Label
    Friend WithEvents lblTotalRevenue As Label
    Friend WithEvents lblBestSelling As Label
    Friend WithEvents txtTotalProducts As TextBox
    Friend WithEvents txtTotalSoldItems As TextBox
    Friend WithEvents txtTotalRevenue As TextBox
    Friend WithEvents txtBestSelling As TextBox
    Friend WithEvents lblProductPerformance As Label
    Friend WithEvents dgvProductAnalysis As DataGridView
    Friend WithEvents btnRefresh As Button
End Class
