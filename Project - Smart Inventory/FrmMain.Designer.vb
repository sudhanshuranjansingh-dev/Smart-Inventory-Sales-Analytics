<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
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
        MenuStrip1 = New MenuStrip()
        DashBoardToolStripMenuItem = New ToolStripMenuItem()
        InventoryToolStripMenuItem = New ToolStripMenuItem()
        mnuProduct = New ToolStripMenuItem()
        mnuCategories = New ToolStripMenuItem()
        mnuSuppliers = New ToolStripMenuItem()
        mnuStock = New ToolStripMenuItem()
        TransactionsToolStripMenuItem = New ToolStripMenuItem()
        mnuPurchases = New ToolStripMenuItem()
        mnuNewSale = New ToolStripMenuItem()
        mnuSalesHistory = New ToolStripMenuItem()
        AnalyticsToolStripMenuItem = New ToolStripMenuItem()
        mnuSalesAnalytics = New ToolStripMenuItem()
        mnuProductAnalytics = New ToolStripMenuItem()
        ReportsToolStripMenuItem = New ToolStripMenuItem()
        mnuSalesReport = New ToolStripMenuItem()
        mnuInventoryReport = New ToolStripMenuItem()
        SystemToolStripMenuItem = New ToolStripMenuItem()
        mnuLogout = New ToolStripMenuItem()
        mnuExit = New ToolStripMenuItem()
        MenuStrip1.SuspendLayout()
        SuspendLayout()
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Font = New Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        MenuStrip1.ImageScalingSize = New Size(20, 20)
        MenuStrip1.Items.AddRange(New ToolStripItem() {DashBoardToolStripMenuItem, InventoryToolStripMenuItem, TransactionsToolStripMenuItem, AnalyticsToolStripMenuItem, ReportsToolStripMenuItem, SystemToolStripMenuItem})
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Padding = New Padding(7, 3, 0, 3)
        MenuStrip1.Size = New Size(1093, 42)
        MenuStrip1.TabIndex = 0
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' DashBoardToolStripMenuItem
        ' 
        DashBoardToolStripMenuItem.Name = "DashBoardToolStripMenuItem"
        DashBoardToolStripMenuItem.Size = New Size(152, 36)
        DashBoardToolStripMenuItem.Text = "DashBoard"
        ' 
        ' InventoryToolStripMenuItem
        ' 
        InventoryToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuProduct, mnuCategories, mnuSuppliers, mnuStock})
        InventoryToolStripMenuItem.Name = "InventoryToolStripMenuItem"
        InventoryToolStripMenuItem.Size = New Size(140, 36)
        InventoryToolStripMenuItem.Text = "Inventory"
        ' 
        ' mnuProduct
        ' 
        mnuProduct.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuProduct.Name = "mnuProduct"
        mnuProduct.Size = New Size(183, 28)
        mnuProduct.Text = "Products"
        ' 
        ' mnuCategories
        ' 
        mnuCategories.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuCategories.Name = "mnuCategories"
        mnuCategories.Size = New Size(183, 28)
        mnuCategories.Text = "Categories"
        ' 
        ' mnuSuppliers
        ' 
        mnuSuppliers.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuSuppliers.Name = "mnuSuppliers"
        mnuSuppliers.Size = New Size(183, 28)
        mnuSuppliers.Text = "Suppliers"
        ' 
        ' mnuStock
        ' 
        mnuStock.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuStock.Name = "mnuStock"
        mnuStock.Size = New Size(183, 28)
        mnuStock.Text = "Stock"
        ' 
        ' TransactionsToolStripMenuItem
        ' 
        TransactionsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuPurchases, mnuNewSale, mnuSalesHistory})
        TransactionsToolStripMenuItem.Name = "TransactionsToolStripMenuItem"
        TransactionsToolStripMenuItem.Size = New Size(171, 36)
        TransactionsToolStripMenuItem.Text = "Transactions"
        ' 
        ' mnuPurchases
        ' 
        mnuPurchases.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuPurchases.Name = "mnuPurchases"
        mnuPurchases.Size = New Size(202, 28)
        mnuPurchases.Text = "Purchases"
        ' 
        ' mnuNewSale
        ' 
        mnuNewSale.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuNewSale.Name = "mnuNewSale"
        mnuNewSale.Size = New Size(202, 28)
        mnuNewSale.Text = "New Sale"
        ' 
        ' mnuSalesHistory
        ' 
        mnuSalesHistory.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuSalesHistory.Name = "mnuSalesHistory"
        mnuSalesHistory.Size = New Size(202, 28)
        mnuSalesHistory.Text = "Sales History"
        ' 
        ' AnalyticsToolStripMenuItem
        ' 
        AnalyticsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuSalesAnalytics, mnuProductAnalytics})
        AnalyticsToolStripMenuItem.Name = "AnalyticsToolStripMenuItem"
        AnalyticsToolStripMenuItem.Size = New Size(132, 36)
        AnalyticsToolStripMenuItem.Text = "Analytics"
        ' 
        ' mnuSalesAnalytics
        ' 
        mnuSalesAnalytics.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuSalesAnalytics.Name = "mnuSalesAnalytics"
        mnuSalesAnalytics.Size = New Size(238, 28)
        mnuSalesAnalytics.Text = "Sales Analytics"
        ' 
        ' mnuProductAnalytics
        ' 
        mnuProductAnalytics.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuProductAnalytics.Name = "mnuProductAnalytics"
        mnuProductAnalytics.Size = New Size(238, 28)
        mnuProductAnalytics.Text = "Product Analytics"
        ' 
        ' ReportsToolStripMenuItem
        ' 
        ReportsToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuSalesReport, mnuInventoryReport})
        ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        ReportsToolStripMenuItem.Size = New Size(117, 36)
        ReportsToolStripMenuItem.Text = "Reports"
        ' 
        ' mnuSalesReport
        ' 
        mnuSalesReport.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuSalesReport.Name = "mnuSalesReport"
        mnuSalesReport.Size = New Size(235, 28)
        mnuSalesReport.Text = "Sales Report"
        ' 
        ' mnuInventoryReport
        ' 
        mnuInventoryReport.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuInventoryReport.Name = "mnuInventoryReport"
        mnuInventoryReport.Size = New Size(235, 28)
        mnuInventoryReport.Text = "Inventory Report"
        ' 
        ' SystemToolStripMenuItem
        ' 
        SystemToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {mnuLogout, mnuExit})
        SystemToolStripMenuItem.Name = "SystemToolStripMenuItem"
        SystemToolStripMenuItem.Size = New Size(108, 36)
        SystemToolStripMenuItem.Text = "System"
        ' 
        ' mnuLogout
        ' 
        mnuLogout.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuLogout.Name = "mnuLogout"
        mnuLogout.Size = New Size(155, 28)
        mnuLogout.Text = "Logout"
        ' 
        ' mnuExit
        ' 
        mnuExit.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        mnuExit.Name = "mnuExit"
        mnuExit.Size = New Size(155, 28)
        mnuExit.Text = "Exit"
        ' 
        ' FrmMain
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1093, 905)
        Controls.Add(MenuStrip1)
        IsMdiContainer = True
        MainMenuStrip = MenuStrip1
        Margin = New Padding(3, 4, 3, 4)
        Name = "FrmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "FrmMain"
        MenuStrip1.ResumeLayout(False)
        MenuStrip1.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DashBoardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents InventoryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TransactionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AnalyticsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SystemToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents mnuProduct As ToolStripMenuItem
    Friend WithEvents mnuCategories As ToolStripMenuItem
    Friend WithEvents mnuSuppliers As ToolStripMenuItem
    Friend WithEvents mnuStock As ToolStripMenuItem
    Friend WithEvents mnuPurchases As ToolStripMenuItem
    Friend WithEvents mnuNewSale As ToolStripMenuItem
    Friend WithEvents mnuSalesHistory As ToolStripMenuItem
    Friend WithEvents mnuSalesAnalytics As ToolStripMenuItem
    Friend WithEvents mnuProductAnalytics As ToolStripMenuItem
    Friend WithEvents mnuSalesReport As ToolStripMenuItem
    Friend WithEvents mnuInventoryReport As ToolStripMenuItem
    Friend WithEvents mnuLogout As ToolStripMenuItem
    Friend WithEvents mnuExit As ToolStripMenuItem
End Class
