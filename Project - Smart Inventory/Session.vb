Public Module Session

    Public CurrentUserID As Integer = 0

    Public CurrentUsername As String = ""

    Public CurrentFullName As String = ""

    Public CurrentRole As String = ""

    Public CurrentAccountStatus As String = ""


    Public ReadOnly Property IsAdmin As Boolean

        Get

            Return CurrentRole = "Admin" AndAlso
                   CurrentAccountStatus = "Approved"

        End Get

    End Property


    Public Sub ClearSession()

        CurrentUserID = 0

        CurrentUsername = ""

        CurrentFullName = ""

        CurrentRole = ""

        CurrentAccountStatus = ""

    End Sub

End Module