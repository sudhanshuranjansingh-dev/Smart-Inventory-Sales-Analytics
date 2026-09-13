<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmInventory
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
        pnlTotalProducts = New FlowLayoutPanel()
        lblTotalProductsTitles = New Label()
        txtTotalProducts = New TextBox()
        FlowLayoutPanel1 = New FlowLayoutPanel()
        lblLowStocks = New Label()
        txtLowStocks = New TextBox()
        FlowLayoutPanel2 = New FlowLayoutPanel()
        lblOutOfStock = New Label()
        txtOutOfStocks = New TextBox()
        FlowLayoutPanel3 = New FlowLayoutPanel()
        Label2 = New Label()
        TextBox2 = New TextBox()
        FlowLayoutPanel4 = New FlowLayoutPanel()
        Label3 = New Label()
        TextBox3 = New TextBox()
        FlowLayoutPanel5 = New FlowLayoutPanel()
        Label4 = New Label()
        TextBox4 = New TextBox()
        TextBox1 = New TextBox()
        FlowLayoutPanel7 = New FlowLayoutPanel()
        lblTotalUnits = New Label()
        txtTotalUnits = New TextBox()
        lblSearchProducts = New Label()
        txtSearchProducts = New TextBox()
        btnSearch = New Button()
        btnRefresh = New Button()
        lblStockFilter = New Label()
        cmbStockFilter = New ComboBox()
        dgvInventory = New DataGridView()
        pnlTotalProducts.SuspendLayout()
        FlowLayoutPanel1.SuspendLayout()
        FlowLayoutPanel2.SuspendLayout()
        FlowLayoutPanel3.SuspendLayout()
        FlowLayoutPanel4.SuspendLayout()
        FlowLayoutPanel5.SuspendLayout()
        FlowLayoutPanel7.SuspendLayout()
        CType(dgvInventory, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' lblTitle
        ' 
        lblTitle.AutoSize = True
        lblTitle.Font = New Font("Segoe UI", 22.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTitle.Location = New Point(234, 9)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(431, 50)
        lblTitle.TabIndex = 0
        lblTitle.Text = "Inventory Management"
        ' 
        ' pnlTotalProducts
        ' 
        pnlTotalProducts.BorderStyle = BorderStyle.FixedSingle
        pnlTotalProducts.Controls.Add(lblTotalProductsTitles)
        pnlTotalProducts.Controls.Add(txtTotalProducts)
        pnlTotalProducts.Location = New Point(12, 96)
        pnlTotalProducts.Name = "pnlTotalProducts"
        pnlTotalProducts.Size = New Size(214, 129)
        pnlTotalProducts.TabIndex = 1
        ' 
        ' lblTotalProductsTitles
        ' 
        lblTotalProductsTitles.AutoSize = True
        lblTotalProductsTitles.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalProductsTitles.Location = New Point(3, 0)
        lblTotalProductsTitles.Name = "lblTotalProductsTitles"
        lblTotalProductsTitles.Size = New Size(204, 38)
        lblTotalProductsTitles.TabIndex = 0
        lblTotalProductsTitles.Text = "Total Products"
        ' 
        ' txtTotalProducts
        ' 
        txtTotalProducts.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalProducts.Location = New Point(3, 41)
        txtTotalProducts.Multiline = True
        txtTotalProducts.Name = "txtTotalProducts"
        txtTotalProducts.Size = New Size(210, 87)
        txtTotalProducts.TabIndex = 2
        ' 
        ' FlowLayoutPanel1
        ' 
        FlowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel1.Controls.Add(lblLowStocks)
        FlowLayoutPanel1.Controls.Add(txtLowStocks)
        FlowLayoutPanel1.Location = New Point(253, 96)
        FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        FlowLayoutPanel1.Size = New Size(214, 129)
        FlowLayoutPanel1.TabIndex = 3
        ' 
        ' lblLowStocks
        ' 
        lblLowStocks.AutoSize = True
        lblLowStocks.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblLowStocks.Location = New Point(3, 0)
        lblLowStocks.Name = "lblLowStocks"
        lblLowStocks.Size = New Size(162, 38)
        lblLowStocks.TabIndex = 0
        lblLowStocks.Text = "Low Stocks"
        lblLowStocks.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtLowStocks
        ' 
        txtLowStocks.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtLowStocks.Location = New Point(3, 41)
        txtLowStocks.Multiline = True
        txtLowStocks.Name = "txtLowStocks"
        txtLowStocks.Size = New Size(210, 87)
        txtLowStocks.TabIndex = 2
        ' 
        ' FlowLayoutPanel2
        ' 
        FlowLayoutPanel2.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel2.Controls.Add(lblOutOfStock)
        FlowLayoutPanel2.Controls.Add(txtOutOfStocks)
        FlowLayoutPanel2.Controls.Add(FlowLayoutPanel3)
        FlowLayoutPanel2.Controls.Add(FlowLayoutPanel4)
        FlowLayoutPanel2.Location = New Point(494, 96)
        FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        FlowLayoutPanel2.Size = New Size(214, 129)
        FlowLayoutPanel2.TabIndex = 4
        ' 
        ' lblOutOfStock
        ' 
        lblOutOfStock.AutoSize = True
        lblOutOfStock.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblOutOfStock.Location = New Point(3, 0)
        lblOutOfStock.Name = "lblOutOfStock"
        lblOutOfStock.Size = New Size(186, 38)
        lblOutOfStock.TabIndex = 0
        lblOutOfStock.Text = "Out Of Stock"
        lblOutOfStock.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtOutOfStocks
        ' 
        txtOutOfStocks.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtOutOfStocks.Location = New Point(3, 41)
        txtOutOfStocks.Multiline = True
        txtOutOfStocks.Name = "txtOutOfStocks"
        txtOutOfStocks.Size = New Size(210, 87)
        txtOutOfStocks.TabIndex = 2
        ' 
        ' FlowLayoutPanel3
        ' 
        FlowLayoutPanel3.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel3.Controls.Add(Label2)
        FlowLayoutPanel3.Controls.Add(TextBox2)
        FlowLayoutPanel3.Location = New Point(3, 134)
        FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        FlowLayoutPanel3.Size = New Size(214, 129)
        FlowLayoutPanel3.TabIndex = 3
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(3, 0)
        Label2.Name = "Label2"
        Label2.Size = New Size(204, 38)
        Label2.TabIndex = 0
        Label2.Text = "Total Products"
        ' 
        ' TextBox2
        ' 
        TextBox2.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox2.Location = New Point(3, 41)
        TextBox2.Multiline = True
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(210, 87)
        TextBox2.TabIndex = 2
        ' 
        ' FlowLayoutPanel4
        ' 
        FlowLayoutPanel4.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel4.Controls.Add(Label3)
        FlowLayoutPanel4.Controls.Add(TextBox3)
        FlowLayoutPanel4.Controls.Add(FlowLayoutPanel5)
        FlowLayoutPanel4.Location = New Point(3, 269)
        FlowLayoutPanel4.Name = "FlowLayoutPanel4"
        FlowLayoutPanel4.Size = New Size(214, 129)
        FlowLayoutPanel4.TabIndex = 5
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(3, 0)
        Label3.Name = "Label3"
        Label3.Size = New Size(204, 38)
        Label3.TabIndex = 0
        Label3.Text = "Total Products"
        ' 
        ' TextBox3
        ' 
        TextBox3.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox3.Location = New Point(3, 41)
        TextBox3.Multiline = True
        TextBox3.Name = "TextBox3"
        TextBox3.Size = New Size(210, 87)
        TextBox3.TabIndex = 2
        ' 
        ' FlowLayoutPanel5
        ' 
        FlowLayoutPanel5.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel5.Controls.Add(Label4)
        FlowLayoutPanel5.Controls.Add(TextBox4)
        FlowLayoutPanel5.Location = New Point(3, 134)
        FlowLayoutPanel5.Name = "FlowLayoutPanel5"
        FlowLayoutPanel5.Size = New Size(214, 129)
        FlowLayoutPanel5.TabIndex = 3
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(3, 0)
        Label4.Name = "Label4"
        Label4.Size = New Size(204, 38)
        Label4.TabIndex = 0
        Label4.Text = "Total Products"
        ' 
        ' TextBox4
        ' 
        TextBox4.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox4.Location = New Point(3, 41)
        TextBox4.Multiline = True
        TextBox4.Name = "TextBox4"
        TextBox4.Size = New Size(210, 87)
        TextBox4.TabIndex = 2
        ' 
        ' TextBox1
        ' 
        TextBox1.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        TextBox1.Location = New Point(3, 134)
        TextBox1.Multiline = True
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(210, 87)
        TextBox1.TabIndex = 2
        ' 
        ' FlowLayoutPanel7
        ' 
        FlowLayoutPanel7.BorderStyle = BorderStyle.FixedSingle
        FlowLayoutPanel7.Controls.Add(lblTotalUnits)
        FlowLayoutPanel7.Controls.Add(txtTotalUnits)
        FlowLayoutPanel7.Controls.Add(TextBox1)
        FlowLayoutPanel7.Location = New Point(730, 96)
        FlowLayoutPanel7.Name = "FlowLayoutPanel7"
        FlowLayoutPanel7.Size = New Size(214, 129)
        FlowLayoutPanel7.TabIndex = 3
        ' 
        ' lblTotalUnits
        ' 
        lblTotalUnits.AutoSize = True
        lblTotalUnits.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTotalUnits.Location = New Point(3, 0)
        lblTotalUnits.Name = "lblTotalUnits"
        lblTotalUnits.Size = New Size(157, 38)
        lblTotalUnits.TabIndex = 0
        lblTotalUnits.Text = "Total Units"
        ' 
        ' txtTotalUnits
        ' 
        txtTotalUnits.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtTotalUnits.Location = New Point(3, 41)
        txtTotalUnits.Multiline = True
        txtTotalUnits.Name = "txtTotalUnits"
        txtTotalUnits.Size = New Size(210, 87)
        txtTotalUnits.TabIndex = 2
        ' 
        ' lblSearchProducts
        ' 
        lblSearchProducts.AutoSize = True
        lblSearchProducts.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblSearchProducts.Location = New Point(16, 303)
        lblSearchProducts.Name = "lblSearchProducts"
        lblSearchProducts.Size = New Size(243, 38)
        lblSearchProducts.TabIndex = 3
        lblSearchProducts.Text = "Search Products :"
        lblSearchProducts.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' txtSearchProducts
        ' 
        txtSearchProducts.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtSearchProducts.Location = New Point(257, 301)
        txtSearchProducts.Multiline = True
        txtSearchProducts.Name = "txtSearchProducts"
        txtSearchProducts.Size = New Size(239, 40)
        txtSearchProducts.TabIndex = 5
        ' 
        ' btnSearch
        ' 
        btnSearch.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnSearch.Location = New Point(554, 285)
        btnSearch.Name = "btnSearch"
        btnSearch.Size = New Size(174, 56)
        btnSearch.TabIndex = 6
        btnSearch.Text = "Search"
        btnSearch.UseVisualStyleBackColor = True
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRefresh.Location = New Point(756, 285)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(174, 56)
        btnRefresh.TabIndex = 7
        btnRefresh.Text = "Refresh"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' lblStockFilter
        ' 
        lblStockFilter.AutoSize = True
        lblStockFilter.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblStockFilter.Location = New Point(16, 385)
        lblStockFilter.Name = "lblStockFilter"
        lblStockFilter.Size = New Size(181, 38)
        lblStockFilter.TabIndex = 8
        lblStockFilter.Text = "Stock Filter :"
        lblStockFilter.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' cmbStockFilter
        ' 
        cmbStockFilter.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        cmbStockFilter.FormattingEnabled = True
        cmbStockFilter.Location = New Point(213, 384)
        cmbStockFilter.Name = "cmbStockFilter"
        cmbStockFilter.Size = New Size(279, 39)
        cmbStockFilter.TabIndex = 9
        ' 
        ' dgvInventory
        ' 
        dgvInventory.AllowUserToAddRows = False
        dgvInventory.AllowUserToDeleteRows = False
        dgvInventory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        dgvInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvInventory.Location = New Point(31, 460)
        dgvInventory.MultiSelect = False
        dgvInventory.Name = "dgvInventory"
        dgvInventory.ReadOnly = True
        dgvInventory.RowHeadersVisible = False
        dgvInventory.RowHeadersWidth = 51
        dgvInventory.Size = New Size(899, 188)
        dgvInventory.TabIndex = 10
        ' 
        ' FrmInventory
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(956, 679)
        Controls.Add(dgvInventory)
        Controls.Add(cmbStockFilter)
        Controls.Add(lblStockFilter)
        Controls.Add(btnRefresh)
        Controls.Add(btnSearch)
        Controls.Add(txtSearchProducts)
        Controls.Add(FlowLayoutPanel7)
        Controls.Add(FlowLayoutPanel2)
        Controls.Add(lblSearchProducts)
        Controls.Add(pnlTotalProducts)
        Controls.Add(lblTitle)
        Controls.Add(FlowLayoutPanel1)
        Name = "FrmInventory"
        Text = "FrmInventory"
        pnlTotalProducts.ResumeLayout(False)
        pnlTotalProducts.PerformLayout()
        FlowLayoutPanel1.ResumeLayout(False)
        FlowLayoutPanel1.PerformLayout()
        FlowLayoutPanel2.ResumeLayout(False)
        FlowLayoutPanel2.PerformLayout()
        FlowLayoutPanel3.ResumeLayout(False)
        FlowLayoutPanel3.PerformLayout()
        FlowLayoutPanel4.ResumeLayout(False)
        FlowLayoutPanel4.PerformLayout()
        FlowLayoutPanel5.ResumeLayout(False)
        FlowLayoutPanel5.PerformLayout()
        FlowLayoutPanel7.ResumeLayout(False)
        FlowLayoutPanel7.PerformLayout()
        CType(dgvInventory, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents pnlTotalProducts As FlowLayoutPanel
    Friend WithEvents lblTotalProductsTitles As Label
    Friend WithEvents txtTotalProducts As TextBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents lblLowStocks As Label
    Friend WithEvents txtLowStocks As TextBox
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents lblOutOfStock As Label
    Friend WithEvents txtOutOfStocks As TextBox
    Friend WithEvents FlowLayoutPanel3 As FlowLayoutPanel
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents FlowLayoutPanel4 As FlowLayoutPanel
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents FlowLayoutPanel5 As FlowLayoutPanel
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents FlowLayoutPanel7 As FlowLayoutPanel
    Friend WithEvents lblTotalUnits As Label
    Friend WithEvents txtTotalUnits As TextBox
    Friend WithEvents lblSearchProducts As Label
    Friend WithEvents txtSearchProducts As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnRefresh As Button
    Friend WithEvents lblStockFilter As Label
    Friend WithEvents cmbStockFilter As ComboBox
    Friend WithEvents dgvInventory As DataGridView
End Class
