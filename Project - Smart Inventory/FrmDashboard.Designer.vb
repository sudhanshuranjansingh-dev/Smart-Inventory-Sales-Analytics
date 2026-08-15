<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDashboard
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
        PnlHeader = New Panel()
        lbltitle = New Label()
        pnlProducts = New Panel()
        lblTotalProducts = New Label()
        lblProductTitle = New Label()
        pnlLowStock = New Panel()
        lblLowStock = New Label()
        lblLowStockTitle = New Label()
        pnlSales = New Panel()
        lblTotalSales = New Label()
        lblSalesTitle = New Label()
        pnlStock = New Panel()
        lblTotalStock = New Label()
        lblStockTitle = New Label()
        plotSales = New ScottPlot.WinForms.TransparentSKControl()
        PnlHeader.SuspendLayout()
        pnlProducts.SuspendLayout()
        pnlLowStock.SuspendLayout()
        pnlSales.SuspendLayout()
        pnlStock.SuspendLayout()
        SuspendLayout()
        ' 
        ' PnlHeader
        ' 
        PnlHeader.Controls.Add(lbltitle)
        PnlHeader.Dock = DockStyle.Top
        PnlHeader.Location = New Point(0, 0)
        PnlHeader.Name = "PnlHeader"
        PnlHeader.Size = New Size(956, 100)
        PnlHeader.TabIndex = 0
        ' 
        ' lbltitle
        ' 
        lbltitle.AutoSize = True
        lbltitle.Font = New Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lbltitle.Location = New Point(86, 27)
        lbltitle.Name = "lbltitle"
        lbltitle.Size = New Size(375, 37)
        lbltitle.TabIndex = 0
        lbltitle.Text = "Smart Inventory Dashboard"
        ' 
        ' pnlProducts
        ' 
        pnlProducts.Controls.Add(lblTotalProducts)
        pnlProducts.Controls.Add(lblProductTitle)
        pnlProducts.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlProducts.Location = New Point(25, 127)
        pnlProducts.Name = "pnlProducts"
        pnlProducts.Size = New Size(200, 100)
        pnlProducts.TabIndex = 1
        ' 
        ' lblTotalProducts
        ' 
        lblTotalProducts.AutoSize = True
        lblTotalProducts.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalProducts.Location = New Point(83, 57)
        lblTotalProducts.Name = "lblTotalProducts"
        lblTotalProducts.Size = New Size(15, 17)
        lblTotalProducts.TabIndex = 1
        lblTotalProducts.Text = "0"
        ' 
        ' lblProductTitle
        ' 
        lblProductTitle.AutoSize = True
        lblProductTitle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblProductTitle.Location = New Point(40, 10)
        lblProductTitle.Name = "lblProductTitle"
        lblProductTitle.Size = New Size(120, 17)
        lblProductTitle.TabIndex = 0
        lblProductTitle.Text = "TOTAL PRODUCTS"
        ' 
        ' pnlLowStock
        ' 
        pnlLowStock.Controls.Add(lblLowStock)
        pnlLowStock.Controls.Add(lblLowStockTitle)
        pnlLowStock.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlLowStock.Location = New Point(733, 127)
        pnlLowStock.Name = "pnlLowStock"
        pnlLowStock.Size = New Size(200, 100)
        pnlLowStock.TabIndex = 4
        ' 
        ' lblLowStock
        ' 
        lblLowStock.AutoSize = True
        lblLowStock.Location = New Point(71, 57)
        lblLowStock.Name = "lblLowStock"
        lblLowStock.Size = New Size(15, 17)
        lblLowStock.TabIndex = 1
        lblLowStock.Text = "0"
        ' 
        ' lblLowStockTitle
        ' 
        lblLowStockTitle.AutoSize = True
        lblLowStockTitle.Location = New Point(44, 12)
        lblLowStockTitle.Name = "lblLowStockTitle"
        lblLowStockTitle.Size = New Size(125, 17)
        lblLowStockTitle.TabIndex = 0
        lblLowStockTitle.Text = "LOW STOCK ITEMS"
        ' 
        ' pnlSales
        ' 
        pnlSales.Controls.Add(lblTotalSales)
        pnlSales.Controls.Add(lblSalesTitle)
        pnlSales.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlSales.Location = New Point(261, 127)
        pnlSales.Name = "pnlSales"
        pnlSales.Size = New Size(200, 100)
        pnlSales.TabIndex = 5
        ' 
        ' lblTotalSales
        ' 
        lblTotalSales.AutoSize = True
        lblTotalSales.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalSales.Location = New Point(66, 57)
        lblTotalSales.Name = "lblTotalSales"
        lblTotalSales.Size = New Size(40, 17)
        lblTotalSales.TabIndex = 1
        lblTotalSales.Text = "$0.00"
        ' 
        ' lblSalesTitle
        ' 
        lblSalesTitle.AutoSize = True
        lblSalesTitle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSalesTitle.Location = New Point(50, 12)
        lblSalesTitle.Name = "lblSalesTitle"
        lblSalesTitle.Size = New Size(89, 17)
        lblSalesTitle.TabIndex = 0
        lblSalesTitle.Text = "TOTAL SALES"
        ' 
        ' pnlStock
        ' 
        pnlStock.Controls.Add(lblTotalStock)
        pnlStock.Controls.Add(lblStockTitle)
        pnlStock.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        pnlStock.Location = New Point(497, 127)
        pnlStock.Name = "pnlStock"
        pnlStock.Size = New Size(200, 100)
        pnlStock.TabIndex = 2
        ' 
        ' lblTotalStock
        ' 
        lblTotalStock.AutoSize = True
        lblTotalStock.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalStock.Location = New Point(69, 57)
        lblTotalStock.Name = "lblTotalStock"
        lblTotalStock.Size = New Size(15, 17)
        lblTotalStock.TabIndex = 1
        lblTotalStock.Text = "0"
        ' 
        ' lblStockTitle
        ' 
        lblStockTitle.AutoSize = True
        lblStockTitle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStockTitle.Location = New Point(49, 10)
        lblStockTitle.Name = "lblStockTitle"
        lblStockTitle.Size = New Size(93, 17)
        lblStockTitle.TabIndex = 0
        lblStockTitle.Text = "TOTAL STOCK"
        ' 
        ' plotSales
        ' 
        plotSales.Location = New Point(158, 289)
        plotSales.Name = "plotSales"
        plotSales.Size = New Size(600, 300)
        plotSales.TabIndex = 6
        plotSales.Text = "TransparentskControl1"
        ' 
        ' FrmDashboard
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 635)
        Controls.Add(plotSales)
        Controls.Add(pnlStock)
        Controls.Add(pnlSales)
        Controls.Add(pnlLowStock)
        Controls.Add(pnlProducts)
        Controls.Add(PnlHeader)
        Name = "FrmDashboard"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Dashboard"
        PnlHeader.ResumeLayout(False)
        PnlHeader.PerformLayout()
        pnlProducts.ResumeLayout(False)
        pnlProducts.PerformLayout()
        pnlLowStock.ResumeLayout(False)
        pnlLowStock.PerformLayout()
        pnlSales.ResumeLayout(False)
        pnlSales.PerformLayout()
        pnlStock.ResumeLayout(False)
        pnlStock.PerformLayout()
        ResumeLayout(False)
    End Sub

    Friend WithEvents PnlHeader As Panel
    Friend WithEvents lbltitle As Label
    Friend WithEvents pnlProducts As Panel
    Friend WithEvents pnlLowStock As Panel
    Friend WithEvents pnlSales As Panel
    Friend WithEvents pnlStock As Panel
    Friend WithEvents lblTotalProducts As Label
    Friend WithEvents lblProductTitle As Label
    Friend WithEvents lblSalesTitle As Label
    Friend WithEvents lblTotalSales As Label
    Friend WithEvents lblTotalStock As Label
    Friend WithEvents lblStockTitle As Label
    Friend WithEvents lblLowStockTitle As Label
    Friend WithEvents lblLowStock As Label
    Friend WithEvents plotSales As ScottPlot.WinForms.TransparentSKControl
End Class
