Public Class FrmMain
    Private Sub FrmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowDashboard()
    End Sub

    Private Sub ShowDashboard()

        For Each frm As Form In Me.MdiChildren
            frm.Close()
        Next

        Dim dashboard As New FrmDashboard()

        dashboard.MdiParent = Me
        dashboard.Dock = DockStyle.Fill
        dashboard.Show()
    End Sub

    Private Sub DashBoardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashBoardToolStripMenuItem.Click
        ShowDashboard()
    End Sub

    Private Sub mnuProduct_Click(sender As Object, e As EventArgs) Handles mnuProduct.Click


        Dim products As New FrmProducts()

        products.MdiParent = Me
        products.WindowState = FormWindowState.Maximized
        products.Show()
    End Sub

    Private Sub SystemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SystemToolStripMenuItem.Click

    End Sub

    Private Sub mnuExit_Click(sender As Object, e As EventArgs) Handles mnuExit.Click
        Dim result As DialogResult

        result = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo,
                              MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub mnuLogout_Click(sender As Object, e As EventArgs) Handles mnuLogout.Click
        Dim result As DialogResult

        result = MessageBox.Show(
            "Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then

            'Show Login Form
            FrmLogin.Show()

            'Close Main Form
            Me.Close()

        End If
    End Sub

    Private Sub mnuCategories_Click(sender As Object, e As EventArgs) Handles mnuCategories.Click
        Dim frm As New FrmCategories()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuSuppliers_Click(sender As Object, e As EventArgs) Handles mnuSuppliers.Click
        Dim frm As New FrmSuppliers()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()

    End Sub

    Private Sub mnuStock_Click(sender As Object, e As EventArgs) Handles mnuStock.Click
        Dim frm As New Stock()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuPurchases_Click(sender As Object, e As EventArgs) Handles mnuPurchases.Click
        Dim frm As New FrmPurchases()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuNewSale_Click(sender As Object, e As EventArgs) Handles mnuNewSale.Click
        Dim frm As New FrmSales()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuSalesHistory_Click(sender As Object, e As EventArgs) Handles mnuSalesHistory.Click
        Dim frm As New FrmSalesHistory()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuSalesAnalytics_Click(sender As Object, e As EventArgs) Handles mnuSalesAnalytics.Click
        Dim frm As New SalesAnalytics()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuProductAnalytics_Click(sender As Object, e As EventArgs) Handles mnuProductAnalytics.Click
        Dim frm As New FrmProductAnalysis()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuSalesReport_Click(sender As Object, e As EventArgs) Handles mnuSalesReport.Click
        Dim frm As New FrmReports()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub

    Private Sub mnuInventoryReport_Click(sender As Object, e As EventArgs) Handles mnuInventoryReport.Click
        Dim frm As New FrmInventory()

        frm.MdiParent = Me
        frm.Dock = DockStyle.Fill
        frm.Show()
    End Sub
End Class