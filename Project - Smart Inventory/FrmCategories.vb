Imports MySql.Data.MySqlClient

Public Class FrmCategories



    ' FORM LOAD

    Private Sub FrmCategories_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dgvCategories.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvCategories.MultiSelect = False
        dgvCategories.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCategories.ReadOnly = True
        dgvCategories.AllowUserToAddRows = False

        LoadCategories()

        txtCategoryName.Focus()

    End Sub



    ' LOAD CATEGORIES

    Private Sub LoadCategories()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT CategoryID, CategoryName " &
                    "FROM Categories " &
                    "ORDER BY CategoryID DESC"

                Using adapter As New MySqlDataAdapter(query, con)

                    Dim table As New DataTable()

                    adapter.Fill(table)

                    dgvCategories.DataSource = table

                End Using

            End Using

            'DataGridView settings
            If dgvCategories.Columns.Count > 0 Then

                dgvCategories.Columns("CategoryID").HeaderText = "ID"
                dgvCategories.Columns("CategoryName").HeaderText = "Category Name"

                dgvCategories.Columns("CategoryID").Width = 80
                dgvCategories.Columns("CategoryName").AutoSizeMode =
                    DataGridViewAutoSizeColumnMode.Fill

            End If

        Catch ex As Exception

            MessageBox.Show(
                "Error loading categories:" & vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' ADD CATEGORY

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        'Check empty
        If txtCategoryName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter a category name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtCategoryName.Focus()
            Exit Sub

        End If


        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                'Check duplicate category
                Dim checkQuery As String =
                    "SELECT COUNT(*) FROM Categories " &
                    "WHERE CategoryName = @CategoryName"

                Using checkCmd As New MySqlCommand(checkQuery, con)

                    checkCmd.Parameters.AddWithValue(
                        "@CategoryName",
                        txtCategoryName.Text.Trim()
                    )

                    Dim count As Integer =
                        Convert.ToInt32(checkCmd.ExecuteScalar())

                    If count > 0 Then

                        MessageBox.Show(
                            "This category already exists.",
                            "Duplicate Category",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        txtCategoryName.Focus()
                        Exit Sub

                    End If

                End Using


                'Insert category
                Dim query As String =
                    "INSERT INTO Categories(CategoryName) " &
                    "VALUES(@CategoryName)"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@CategoryName",
                        txtCategoryName.Text.Trim()
                    )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Category added successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadCategories()
            ClearFields()

        Catch ex As Exception

            MessageBox.Show(
                "Error adding category:" & vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' DATA GRID VIEW CLICK

    Private Sub dgvCategories_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    ) Handles dgvCategories.CellClick

        'Ignore header row
        If e.RowIndex < 0 Then
            Exit Sub
        End If


        Try

            Dim row As DataGridViewRow =
                dgvCategories.Rows(e.RowIndex)

            txtCategoryName.Text =
                row.Cells("CategoryName").Value.ToString()

        Catch ex As Exception

            MessageBox.Show(
                "Unable to select category:" & vbCrLf & ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' UPDATE CATEGORY

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click

        'Check category selected
        If dgvCategories.CurrentRow Is Nothing Then

            MessageBox.Show(
                "Please select a category from the list first.",
                "Update Category",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        'Check name
        If txtCategoryName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter a category name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtCategoryName.Focus()
            Exit Sub

        End If


        Try

            Dim categoryID As Integer =
                Convert.ToInt32(
                    dgvCategories.CurrentRow.Cells("CategoryID").Value
                )


            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()


                'Check duplicate name
                Dim checkQuery As String =
                    "SELECT COUNT(*) FROM Categories " &
                    "WHERE CategoryName = @CategoryName " &
                    "AND CategoryID <> @CategoryID"

                Using checkCmd As New MySqlCommand(checkQuery, con)

                    checkCmd.Parameters.AddWithValue(
                        "@CategoryName",
                        txtCategoryName.Text.Trim()
                    )

                    checkCmd.Parameters.AddWithValue(
                        "@CategoryID",
                        categoryID
                    )

                    Dim count As Integer =
                        Convert.ToInt32(checkCmd.ExecuteScalar())

                    If count > 0 Then

                        MessageBox.Show(
                            "Another category with this name already exists.",
                            "Duplicate Category",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Exit Sub

                    End If

                End Using


                'Update
                Dim query As String =
                    "UPDATE Categories " &
                    "SET CategoryName = @CategoryName " &
                    "WHERE CategoryID = @CategoryID"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@CategoryName",
                        txtCategoryName.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@CategoryID",
                        categoryID
                    )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Category updated successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadCategories()
            ClearFields()

        Catch ex As Exception

            MessageBox.Show(
                "Error updating category:" & vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    ' DELETE CATEGORY

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click

        'Check selection
        If dgvCategories.CurrentRow Is Nothing Then

            MessageBox.Show(
                "Please select a category first.",
                "Delete Category",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        Try

            Dim categoryID As Integer =
                Convert.ToInt32(
                    dgvCategories.CurrentRow.Cells("CategoryID").Value
                )

            Dim categoryName As String =
                dgvCategories.CurrentRow.Cells("CategoryName").Value.ToString()


            'Confirmation
            Dim result As DialogResult =
                MessageBox.Show(
                    "Are you sure you want to delete the category:" &
                    vbCrLf & vbCrLf &
                    categoryName & "?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )


            If result <> DialogResult.Yes Then
                Exit Sub
            End If


            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()


                'Check whether category is being used by Products
                Dim checkQuery As String =
                    "SELECT COUNT(*) FROM Products " &
                    "WHERE CategoryID = @CategoryID"

                Using checkCmd As New MySqlCommand(checkQuery, con)

                    checkCmd.Parameters.AddWithValue(
                        "@CategoryID",
                        categoryID
                    )

                    Dim productCount As Integer =
                        Convert.ToInt32(checkCmd.ExecuteScalar())

                    If productCount > 0 Then

                        MessageBox.Show(
                            "This category cannot be deleted because " &
                            "it is being used by one or more products.",
                            "Cannot Delete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Exit Sub

                    End If

                End Using


                'Delete
                Dim query As String =
                    "DELETE FROM Categories " &
                    "WHERE CategoryID = @CategoryID"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@CategoryID",
                        categoryID
                    )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Category deleted successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )

            LoadCategories()
            ClearFields()

        Catch ex As Exception

            MessageBox.Show(
                "Error deleting category:" & vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' CLEAR BUTTON

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click

        ClearFields()

    End Sub



    ' CLEAR FIELDS

    Private Sub ClearFields()

        txtCategoryName.Clear()

        dgvCategories.ClearSelection()

        txtCategoryName.Focus()

    End Sub



    ' SEARCH CATEGORY

    Private Sub txtSearch_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtSearch.TextChanged

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT CategoryID, CategoryName " &
                    "FROM Categories " &
                    "WHERE CategoryName LIKE @Search " &
                    "ORDER BY CategoryID DESC"

                Using cmd As New MySqlCommand(query, con)

                    cmd.Parameters.AddWithValue(
                        "@Search",
                        "%" & txtSearch.Text.Trim() & "%"
                    )


                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvCategories.DataSource = table

                    End Using

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "Error searching categories:" & vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

End Class