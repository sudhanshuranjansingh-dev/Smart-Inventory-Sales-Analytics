
Imports MySql.Data.MySqlClient

Public Class DBConnection

    Public Shared Function GetConnection() As MySqlConnection

        Dim dbPassword As String =
            Environment.GetEnvironmentVariable("AIVEN_DB_PASSWORD")

        If String.IsNullOrWhiteSpace(dbPassword) Then
            Throw New InvalidOperationException(
                "AIVEN_DB_PASSWORD environment variable is missing."
            )
        End If

        Dim builder As New MySqlConnectionStringBuilder With {
            .Server = "mysql-36db1e2a-abc-ec38.j.aivencloud.com",
            .Port = 23471,
            .Database = "smart_inventory",
            .UserID = "avnadmin",
            .Password = dbPassword,
            .SslMode = MySqlSslMode.VerifyCA,
            .SslCa = "C:\Users\wasif\Downloads\ca.pem"
        }

        Return New MySqlConnection(builder.ConnectionString)

    End Function

End Class
