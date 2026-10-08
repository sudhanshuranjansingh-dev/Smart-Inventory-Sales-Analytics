Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text.Json
Imports Microsoft.Web.WebView2.Core

Public Class FrmLogin


    '=========================================================
    ' FORM LOAD
    '=========================================================
    Private Async Sub FrmLogin_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Me.FormBorderStyle = FormBorderStyle.None

        Try

            '=====================================================
            ' HIDE FORM WHILE WEBVIEW2 IS INITIALIZING
            '=====================================================

            Me.Opacity = 0

            '=====================================================
            ' INITIALIZE WEBVIEW2
            '=====================================================

            Await WebViewLogin.EnsureCoreWebView2Async()

            '=====================================================
            ' LOGIN PAGE PATH
            '=====================================================

            Dim loginPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI",
                    "login.html"
                )

            '=====================================================
            ' CHECK LOGIN PAGE
            '=====================================================

            If Not File.Exists(loginPath) Then

                Me.Opacity = 1

                MessageBox.Show(
                    "Login page was not found." &
                    Environment.NewLine &
                    Environment.NewLine &
                    loginPath,
                    "WebUI Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If

            '=====================================================
            ' LOAD LOGIN PAGE
            '=====================================================

            WebViewLogin.Source =
                New Uri(loginPath)

            '=====================================================
            ' WAIT UNTIL WEBVIEW PAGE IS LOADED
            '=====================================================

            AddHandler WebViewLogin.NavigationCompleted,
                AddressOf WebViewLogin_NavigationCompleted

        Catch ex As Exception

            Me.Opacity = 1

            MessageBox.Show(
                "Unable to load Login page." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Login Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' WEBVIEW2 NAVIGATION COMPLETED
    '=========================================================
    Private Sub WebViewLogin_NavigationCompleted(
        sender As Object,
        e As CoreWebView2NavigationCompletedEventArgs
    )

        Try

            '=====================================================
            ' SHOW LOGIN FORM ONLY AFTER WEBVIEW IS READY
            '=====================================================

            If e.IsSuccess Then

                Me.Opacity = 1

            Else

                Me.Opacity = 1

                MessageBox.Show(
                    "Unable to load the Login page.",
                    "WebView Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

            End If

            '=====================================================
            ' REMOVE HANDLER
            '=====================================================

            RemoveHandler WebViewLogin.NavigationCompleted,
                AddressOf WebViewLogin_NavigationCompleted

        Catch ex As Exception

            Me.Opacity = 1

        End Try

    End Sub


    '=========================================================
    ' WEBVIEW2 MESSAGE RECEIVED
    '=========================================================
    Private Sub WebViewLogin_WebMessageReceived(
        sender As Object,
        e As CoreWebView2WebMessageReceivedEventArgs
    ) Handles WebViewLogin.WebMessageReceived

        Try

            Dim json As String =
                e.WebMessageAsJson

            Using document As JsonDocument =
                JsonDocument.Parse(json)

                Dim root As JsonElement =
                    document.RootElement

                '=================================================
                ' MESSAGE MUST BE OBJECT
                '=================================================

                If root.ValueKind <>
                    JsonValueKind.Object Then

                    Return

                End If

                '=================================================
                ' GET ACTION
                '=================================================

                Dim actionElement As JsonElement

                If Not root.TryGetProperty(
                    "action",
                    actionElement
                ) Then

                    Return

                End If

                If actionElement.ValueKind <>
                    JsonValueKind.String Then

                    Return

                End If

                Dim action As String =
                    actionElement.GetString()

                '=================================================
                ' HANDLE ACTION
                '=================================================

                Select Case action

                    '=================================================
                    ' LOGIN
                    '=================================================

                    Case "login"

                        Dim usernameElement As JsonElement
                        Dim passwordElement As JsonElement

                        If Not root.TryGetProperty(
                            "username",
                            usernameElement
                        ) Then

                            SendMessageToWeb(
                                "Username is required."
                            )

                            Return

                        End If

                        If Not root.TryGetProperty(
                            "password",
                            passwordElement
                        ) Then

                            SendMessageToWeb(
                                "Password is required."
                            )

                            Return

                        End If

                        Dim username As String =
                            usernameElement.GetString()

                        Dim password As String =
                            passwordElement.GetString()

                        LoginFromWeb(
                            username,
                            password
                        )


                    '=================================================
                    ' REGISTER
                    '=================================================

                    Case "register"

                        Dim fullNameElement As JsonElement
                        Dim usernameElement As JsonElement
                        Dim passwordElement As JsonElement
                        Dim roleElement As JsonElement

                        If Not root.TryGetProperty(
                            "fullName",
                            fullNameElement
                        ) Then

                            SendMessageToWeb(
                                "Full name is required."
                            )

                            Return

                        End If

                        If Not root.TryGetProperty(
                            "username",
                            usernameElement
                        ) Then

                            SendMessageToWeb(
                                "Username is required."
                            )

                            Return

                        End If

                        If Not root.TryGetProperty(
                            "password",
                            passwordElement
                        ) Then

                            SendMessageToWeb(
                                "Password is required."
                            )

                            Return

                        End If

                        If Not root.TryGetProperty(
                            "role",
                            roleElement
                        ) Then

                            SendMessageToWeb(
                                "Role is required."
                            )

                            Return

                        End If

                        Dim fullName As String =
                            fullNameElement.GetString()

                        Dim username As String =
                            usernameElement.GetString()

                        Dim password As String =
                            passwordElement.GetString()

                        Dim role As String =
                            roleElement.GetString()

                        RegisterUserFromWeb(
                            fullName,
                            username,
                            password,
                            role
                        )


                    '=================================================
                    ' REGISTER PAGE
                    '=================================================

                    Case "registerPage"

                        LoadRegisterPage()


                    '=================================================
                    ' BACK TO LOGIN
                    '=================================================

                    Case "backToLogin"

                        LoadLoginPage()


                    '=================================================
                    ' EXIT
                    '=================================================

                    Case "exit"

                        ExitApplication()


                        '=================================================
                        ' UNKNOWN ACTION
                        '=================================================

                    Case Else

                        SendMessageToWeb(
                            "Unknown action: " & action
                        )

                End Select

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "WebUI communication error." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Login Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' LOGIN FROM WEB
    '=========================================================
    Private Sub LoginFromWeb(
        username As String,
        password As String
    )

        Try

            '=====================================================
            ' VALIDATE INPUT
            '=====================================================

            If String.IsNullOrWhiteSpace(username) Then

                SendMessageToWeb(
                    "Please enter your username."
                )

                Return

            End If

            If String.IsNullOrWhiteSpace(password) Then

                SendMessageToWeb(
                    "Please enter your password."
                )

                Return

            End If


            '=====================================================
            ' DATABASE CONNECTION
            '=====================================================

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                '=================================================
                ' LOGIN QUERY
                '=================================================

                Dim query As String =
                    "SELECT " &
                    "UserID, " &
                    "Username, " &
                    "FullName, " &
                    "Role, " &
                    "AccountStatus " &
                    "FROM Users " &
                    "WHERE Username = @username " &
                    "AND Password = @password"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@username",
                        username
                    )

                    cmd.Parameters.AddWithValue(
                        "@password",
                        password
                    )


                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        '=========================================
                        ' USER NOT FOUND
                        '=========================================

                        If Not reader.Read() Then

                            SendMessageToWeb(
                                "Invalid username or password."
                            )

                            Return

                        End If


                        '=========================================
                        ' READ USER INFORMATION
                        '=========================================

                        Dim userID As Integer =
                            Convert.ToInt32(
                                reader("UserID")
                            )

                        Dim loggedUsername As String =
                            reader("Username").ToString()

                        Dim fullName As String =
                            reader("FullName").ToString()

                        Dim role As String =
                            reader("Role").ToString()

                        Dim accountStatus As String =
                            reader("AccountStatus").ToString()


                        '=========================================
                        ' ACCOUNT STATUS
                        '=========================================

                        If accountStatus = "Pending" Then

                            SendMessageToWeb(
                                "Your account is pending Admin approval."
                            )

                            Return

                        End If


                        If accountStatus = "Rejected" Then

                            SendMessageToWeb(
                                "Your account request was rejected."
                            )

                            Return

                        End If


                        If accountStatus <> "Approved" Then

                            SendMessageToWeb(
                                "Your account is not approved."
                            )

                            Return

                        End If


                        '=========================================
                        ' CREATE USER SESSION
                        '=========================================

                        Session.CurrentUserID =
                            userID

                        Session.CurrentUsername =
                            loggedUsername

                        Session.CurrentFullName =
                            fullName

                        Session.CurrentRole =
                            role

                        Session.CurrentAccountStatus =
                            accountStatus


                        '=========================================
                        ' LOGIN SUCCESSFUL
                        '=========================================

                        MessageBox.Show(
                            "Welcome, " &
                            fullName &
                            "!" &
                            Environment.NewLine &
                            "Role: " &
                            role,
                            "Login Successful",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        )


                        '=========================================
                        ' CLOSE/HIDE LOGIN FORM FIRST
                        '=========================================

                        Me.Hide()


                        '=========================================
                        ' OPEN MAIN FORM
                        '=========================================

                        FrmMain.Show()

                        FrmMain.WindowState =
                            FormWindowState.Normal

                        FrmMain.BringToFront()

                        FrmMain.Activate()


                    End Using

                End Using

            End Using


        Catch ex As MySqlException

            MessageBox.Show(
                "Database connection error." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Login Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )


        Catch ex As Exception

            MessageBox.Show(
                "Unable to login." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Login Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' REGISTER USER
    '=========================================================
    Private Sub RegisterUserFromWeb(
        fullName As String,
        username As String,
        password As String,
        role As String
    )

        Try

            '=====================================================
            ' VALIDATION
            '=====================================================

            If String.IsNullOrWhiteSpace(fullName) Then

                SendMessageToWeb(
                    "Please enter your full name."
                )

                Return

            End If

            If String.IsNullOrWhiteSpace(username) Then

                SendMessageToWeb(
                    "Please enter a username."
                )

                Return

            End If

            If String.IsNullOrWhiteSpace(password) Then

                SendMessageToWeb(
                    "Please enter a password."
                )

                Return

            End If

            If String.IsNullOrWhiteSpace(role) Then

                SendMessageToWeb(
                    "Please select a role."
                )

                Return

            End If


            '=====================================================
            ' ALLOWED ROLES
            '=====================================================

            If role <> "Staff" AndAlso
               role <> "Manager" AndAlso
               role <> "Admin" Then

                SendMessageToWeb(
                    "Invalid role selected."
                )

                Return

            End If


            '=====================================================
            ' DETERMINE ACCOUNT STATUS
            '=====================================================

            Dim accountStatus As String

            If role = "Admin" Then

                accountStatus = "Pending"

            Else

                accountStatus = "Approved"

            End If


            '=====================================================
            ' DATABASE
            '=====================================================

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                '=================================================
                ' CHECK USERNAME
                '=================================================

                Dim checkQuery As String =
                    "SELECT COUNT(*) " &
                    "FROM Users " &
                    "WHERE Username = @username"


                Using checkCmd As New MySqlCommand(
                    checkQuery,
                    con
                )

                    checkCmd.Parameters.AddWithValue(
                        "@username",
                        username
                    )

                    Dim userCount As Integer =
                        Convert.ToInt32(
                            checkCmd.ExecuteScalar()
                        )


                    If userCount > 0 Then

                        SendMessageToWeb(
                            "Username already exists."
                        )

                        Return

                    End If

                End Using


                '=================================================
                ' INSERT USER
                '=================================================

                Dim insertQuery As String =
                    "INSERT INTO Users " &
                    "(Username, Password, FullName, Role, AccountStatus) " &
                    "VALUES " &
                    "(@username, @password, @fullName, @role, @accountStatus)"


                Using cmd As New MySqlCommand(
                    insertQuery,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@username",
                        username
                    )

                    cmd.Parameters.AddWithValue(
                        "@password",
                        password
                    )

                    cmd.Parameters.AddWithValue(
                        "@fullName",
                        fullName
                    )

                    cmd.Parameters.AddWithValue(
                        "@role",
                        role
                    )

                    cmd.Parameters.AddWithValue(
                        "@accountStatus",
                        accountStatus
                    )


                    Dim rowsAffected As Integer =
                        cmd.ExecuteNonQuery()


                    If rowsAffected > 0 Then

                        '=========================================
                        ' ADMIN REQUEST
                        '=========================================

                        If role = "Admin" Then

                            MessageBox.Show(
                                "Admin access request submitted!" &
                                Environment.NewLine &
                                Environment.NewLine &
                                "Your account is waiting for approval " &
                                "from an existing Admin." &
                                Environment.NewLine &
                                Environment.NewLine &
                                "You cannot login until your request " &
                                "is approved.",
                                "Admin Approval Required",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            )

                        Else

                            '=========================================
                            ' NORMAL ACCOUNT
                            '=========================================

                            MessageBox.Show(
                                "Account created successfully!" &
                                Environment.NewLine &
                                Environment.NewLine &
                                "You can now login.",
                                "Registration Successful",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            )

                        End If


                        LoadLoginPage()

                    Else

                        SendMessageToWeb(
                            "Unable to create account."
                        )

                    End If

                End Using

            End Using


        Catch ex As MySqlException

            If ex.Number = 1062 Then

                SendMessageToWeb(
                    "Username already exists."
                )

            Else

                MessageBox.Show(
                    "Database error during registration." &
                    Environment.NewLine &
                    Environment.NewLine &
                    ex.Message,
                    "Registration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Unable to create account." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Registration Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' LOAD LOGIN PAGE
    '=========================================================
    Private Sub LoadLoginPage()

        Try

            Dim loginPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI",
                    "login.html"
                )


            If Not File.Exists(loginPath) Then

                MessageBox.Show(
                    "Login page was not found." &
                    Environment.NewLine &
                    Environment.NewLine &
                    loginPath,
                    "WebUI Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If


            WebViewLogin.Source =
                New Uri(loginPath)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load Login page." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Login Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' SEND MESSAGE TO WEB
    '=========================================================
    Private Sub SendMessageToWeb(
        message As String
    )

        Try

            If WebViewLogin.CoreWebView2 Is Nothing Then
                Return
            End If


            Dim json As String =
                JsonSerializer.Serialize(
                    New With {
                        .message = message
                    }
                )


            WebViewLogin.CoreWebView2.
                PostWebMessageAsJson(json)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to communicate with Login page." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "WebView Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' EXIT APPLICATION
    '=========================================================
    Private Sub ExitApplication()

        Dim result As DialogResult =
            MessageBox.Show(
                "Are you sure you want to exit?",
                "Exit Application",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            )


        If result = DialogResult.Yes Then

            Application.Exit()

        End If

    End Sub


    '=========================================================
    ' LOAD REGISTER PAGE
    '=========================================================
    Private Sub LoadRegisterPage()

        Try

            Dim registerPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI",
                    "register.html"
                )


            If Not File.Exists(registerPath) Then

                MessageBox.Show(
                    "Registration page was not found." &
                    Environment.NewLine &
                    Environment.NewLine &
                    registerPath,
                    "WebUI Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If


            WebViewLogin.Source =
                New Uri(registerPath)


        Catch ex As Exception

            MessageBox.Show(
                "Unable to load Registration page." &
                Environment.NewLine &
                Environment.NewLine &
                ex.Message,
                "Registration Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

End Class