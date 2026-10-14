Imports MySql.Data.MySqlClient
Public Class DBConnection

    Private Shared connectionString As String =
        "Server=localhost;Database=SmartInventory;Uid=root;Pwd=Sudhanshu;"

    Public Shared Function GetConnection() As MySqlConnection
        Return New MySqlConnection(connectionString)
    End Function

End Class
