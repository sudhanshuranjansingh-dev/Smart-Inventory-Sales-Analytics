Imports MySql.Data.MySqlClient
Public Class FrmLogin

    Private Sub btnlogin_Click(sender As Object, e As EventArgs) Handles btnlogin.Click

        'Check username
        If txtname.Text.Trim() = "" Then

            MessageBox.Show(
            "Please enter your username.",
            "Login",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        )

            txtname.Focus()
            Exit Sub

        End If


        'Check password
        If txtpass.Text.Trim() = "" Then

            MessageBox.Show(
            "Please enter your password.",
            "Login",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning
        )

            txtpass.Focus()
            Exit Sub

        End If


        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                "SELECT UserID, Username, FullName, Role " &
                "FROM Users " &
                "WHERE Username = @username " &
                "AND Password = @password"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue("@username", txtname.Text.Trim())
                    cmd.Parameters.AddWithValue("@password", txtpass.Text)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        If reader.Read() Then

                            MessageBox.Show(
                            "Welcome, " & reader("FullName").ToString() & "!",
                            "Login Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        )

                            FrmMain.Show()
                            Me.Hide()

                        Else

                            MessageBox.Show(
                            "Invalid username or password.",
                            "Login Failed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        )

                            txtpass.Clear()
                            txtpass.Focus()

                        End If

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "Database connection error:" & vbCrLf & ex.Message,
            "Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Dim result As DialogResult

        result = MessageBox.Show("Are you sure you want to exit?", "Exit Application", MessageBoxButtons.YesNo,
                                  MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Application.Exit()
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