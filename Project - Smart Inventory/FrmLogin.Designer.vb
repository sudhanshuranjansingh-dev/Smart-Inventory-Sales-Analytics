<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLogin
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmLogin))
        Label2 = New Label()
        Label3 = New Label()
        Label4 = New Label()
        txtname = New TextBox()
        txtpass = New TextBox()
        btnlogin = New Button()
        btnExit = New Button()
        PictureBox1 = New PictureBox()
        FolderBrowserDialog1 = New FolderBrowserDialog()
        btnRegister = New Button()
        WebViewLogin = New Microsoft.Web.WebView2.WinForms.WebView2()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(WebViewLogin, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(150, 149)
        Label2.Name = "Label2"
        Label2.Size = New Size(176, 15)
        Label2.TabIndex = 1
        Label2.Text = "Sales & Business Analytics System"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(44, 201)
        Label3.Name = "Label3"
        Label3.Size = New Size(83, 20)
        Label3.TabIndex = 2
        Label3.Text = "UserName"
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.Font = New Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label4.Location = New Point(44, 286)
        Label4.Name = "Label4"
        Label4.Size = New Size(76, 20)
        Label4.TabIndex = 3
        Label4.Text = "Password"
        ' 
        ' txtname
        ' 
        txtname.BorderStyle = BorderStyle.FixedSingle
        txtname.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtname.Location = New Point(44, 238)
        txtname.Margin = New Padding(3, 2, 3, 2)
        txtname.Multiline = True
        txtname.Name = "txtname"
        txtname.Size = New Size(354, 33)
        txtname.TabIndex = 4
        ' 
        ' txtpass
        ' 
        txtpass.BorderStyle = BorderStyle.FixedSingle
        txtpass.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        txtpass.Location = New Point(44, 319)
        txtpass.Margin = New Padding(3, 2, 3, 2)
        txtpass.Multiline = True
        txtpass.Name = "txtpass"
        txtpass.Size = New Size(354, 33)
        txtpass.TabIndex = 5
        txtpass.UseSystemPasswordChar = True
        ' 
        ' btnlogin
        ' 
        btnlogin.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnlogin.Location = New Point(163, 394)
        btnlogin.Margin = New Padding(3, 2, 3, 2)
        btnlogin.Name = "btnlogin"
        btnlogin.Size = New Size(142, 52)
        btnlogin.TabIndex = 6
        btnlogin.Text = "Login"
        btnlogin.UseVisualStyleBackColor = True
        ' 
        ' btnExit
        ' 
        btnExit.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnExit.Location = New Point(163, 469)
        btnExit.Margin = New Padding(3, 2, 3, 2)
        btnExit.Name = "btnExit"
        btnExit.Size = New Size(142, 52)
        btnExit.TabIndex = 7
        btnExit.Text = "EXIT"
        btnExit.UseVisualStyleBackColor = True
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(88, 9)
        PictureBox1.Margin = New Padding(3, 2, 3, 2)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(318, 190)
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        PictureBox1.TabIndex = 8
        PictureBox1.TabStop = False
        ' 
        ' btnRegister
        ' 
        btnRegister.Font = New Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        btnRegister.Location = New Point(396, 9)
        btnRegister.Name = "btnRegister"
        btnRegister.Size = New Size(101, 29)
        btnRegister.TabIndex = 9
        btnRegister.Text = "Register"
        btnRegister.UseVisualStyleBackColor = True
        ' 
        ' WebViewLogin
        ' 
        WebViewLogin.AllowExternalDrop = True
        WebViewLogin.CreationProperties = Nothing
        WebViewLogin.DefaultBackgroundColor = Color.White
        WebViewLogin.Dock = DockStyle.Fill
        WebViewLogin.Location = New Point(0, 0)
        WebViewLogin.Name = "WebViewLogin"
        WebViewLogin.Size = New Size(500, 538)
        WebViewLogin.TabIndex = 10
        WebViewLogin.ZoomFactor = 1R
        ' 
        ' FrmLogin
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.White
        ClientSize = New Size(500, 538)
        Controls.Add(WebViewLogin)
        Controls.Add(btnRegister)
        Controls.Add(btnExit)
        Controls.Add(btnlogin)
        Controls.Add(txtpass)
        Controls.Add(txtname)
        Controls.Add(Label4)
        Controls.Add(Label3)
        Controls.Add(Label2)
        Controls.Add(PictureBox1)
        Margin = New Padding(3, 2, 3, 2)
        Name = "FrmLogin"
        StartPosition = FormStartPosition.CenterScreen
        Text = "FrmLogin"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(WebViewLogin, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtname As TextBox
    Friend WithEvents txtpass As TextBox
    Friend WithEvents btnlogin As Button
    Friend WithEvents btnExit As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents btnRegister As Button
    Friend WithEvents WebViewLogin As Microsoft.Web.WebView2.WinForms.WebView2
End Class
