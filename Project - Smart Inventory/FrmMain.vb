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
End Class