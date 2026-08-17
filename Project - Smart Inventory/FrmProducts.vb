Imports MySql.Data.MySqlClient
Public Class FrmProducts
    Private Sub FrmProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadCategories()
        LoadSuppliers()
        LoadProducts()
    End Sub
    Private Sub LoadCategories()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String = "SELECT CategoryID, CategoryName " & "FROM Categories " & "ORDER BY CategoryName"

                Using adapter As New MySqlDataAdapter(query, con)

                    Dim table As New DataTable()

                    adapter.Fill(table)

                    cmbCategory.DataSource = table
                    cmbCategory.DisplayMember = "CategoryName"
                    cmbCategory.ValueMember = "CategoryID"

                    cmbCategory.SelectedIndex = -1

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "Error loading categories:" & vbCrLf & ex.Message,
            "Database Error",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        )

        End Try

    End Sub
    Private Sub LoadProducts()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT p.ProductID, p.ProductCode, p.ProductName, " &
                    "c.CategoryName, s.SupplierName, " &
                    "p.PurchasePrice, p.SellingPrice, " &
                    "p.StockQuantity, p.MinimumStock " &
                    "FROM Products p " &
                    "LEFT JOIN Categories c ON p.CategoryID = c.CategoryID " &
                    "LEFT JOIN Suppliers s ON p.SupplierID = s.SupplierID " &
                    "ORDER BY p.ProductID DESC"

                Using adapter As New MySqlDataAdapter(query, con)

                    Dim table As New DataTable()

                    adapter.Fill(table)

                    dgvProducts.DataSource = table

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading products: " & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Sub LoadSuppliers()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT SupplierID, SupplierName FROM Suppliers ORDER BY SupplierName"

                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader = cmd.ExecuteReader()

                        cmbSupplier.Items.Clear()

                        While reader.Read()

                            cmbSupplier.Items.Add(
                                New ComboBoxItem(
                                    reader("SupplierName").ToString(),
                                    Convert.ToInt32(reader("SupplierID"))
                                )
                            )

                        End While

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error loading suppliers: " & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    Private Sub cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategory.SelectedIndexChanged

    End Sub
End Class

Public Class ComboBoxItem

    Public Property Text As String
    Public Property Value As Integer

    Public Sub New(text As String, value As Integer)

        Me.Text = text
        Me.Value = value

    End Sub

    Public Overrides Function ToString() As String

        Return Text

    End Function

End Class