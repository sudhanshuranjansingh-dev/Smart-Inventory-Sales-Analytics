Public Class FrmLogin

    Private Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click
        'Check whether username is empty
        If txtname.Text.Trim() = "" Then
            MessageBox.Show("Please enter your username.",
                        "Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

            txtname.Focus()
            Exit Sub
        End If

        'Check whether password is empty
        If txtpass.Text.Trim() = "" Then
            MessageBox.Show("Please enter your password.",
                        "Login",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning)

            txtpass.Focus()
            Exit Sub
        End If

        'Temporary login credentials
        If txtname.Text = "admin" AndAlso txtpass.Text = "admin123" Then

            MessageBox.Show("Login successful!",
                        "Welcome",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information)

            'Open the main form
            FrmMain.Show()

            'Hide the login form
            Me.Hide()

        Else

            MessageBox.Show("Invalid username or password.",
                        "Login Failed",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)

            txtpass.Clear()
            txtpass.Focus()

        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim result As DialogResult

        result = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Exit()
        End If
    End Sub

    Private Sub FrmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If txtname.Text.Trim() = "" Then
            MessageBox.Show("Please enter your username.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            txtname.Focus()
            Exit Sub
        End If

        If txtpass.Text.Trim() = "" Then
            MessageBox.Show("Please enter your password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            txtpass.Focus()
            Exit Sub
        End If

        If txtname.Text = "admin" AndAlso txtpass.Text = "admin123" Then

            MessageBox.Show("Login successful!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information)

            FrmMain.Show()
            Me.Hide()

        Else

            MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

            txtpass.Clear()
            txtpass.Focus()

        End If

    End Sub


    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click

        Dim result As DialogResult

        result = MessageBox.Show("Are you sure you want to exit?", "Exit Application",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Exit()
        End If

    End Sub
End Class