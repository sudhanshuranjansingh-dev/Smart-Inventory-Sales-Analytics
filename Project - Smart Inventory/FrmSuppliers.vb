Imports MySql.Data.MySqlClient
Public Class FrmSuppliers
    ' FORM LOAD
    Private Sub FrmSuppliers_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        LoadSuppliers()
        ClearFields()
        ConfigureDataGridView()

    End Sub

    ' CONFIGURE DATAGRIDVIEW
    Private Sub ConfigureDataGridView()

        dgvSuppliers.ReadOnly = True

        dgvSuppliers.AllowUserToAddRows = False

        dgvSuppliers.AllowUserToDeleteRows = False

        dgvSuppliers.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvSuppliers.MultiSelect = False

        dgvSuppliers.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill

        dgvSuppliers.RowTemplate.Height = 35

    End Sub

    ' LOAD SUPPLIERS

    Private Sub LoadSuppliers()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()

                Dim query As String =
                    "SELECT SupplierID, SupplierName, Phone, Email, Address " &
                    "FROM Suppliers " &
                    "ORDER BY SupplierID DESC"

                Using adapter As New MySqlDataAdapter(query, con)

                    Dim table As New DataTable()

                    adapter.Fill(table)

                    dgvSuppliers.DataSource = table

                End Using

            End Using


            'Set column headers
            If dgvSuppliers.Columns.Count > 0 Then

                dgvSuppliers.Columns("SupplierID").HeaderText = "ID"

                dgvSuppliers.Columns("SupplierName").HeaderText = "Supplier Name"

                dgvSuppliers.Columns("Phone").HeaderText = "Phone"

                dgvSuppliers.Columns("Email").HeaderText = "Email"

                dgvSuppliers.Columns("Address").HeaderText = "Address"

            End If


        Catch ex As Exception

            MessageBox.Show(
                "Error loading suppliers:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

    ' ADD SUPPLIER

    Private Sub btnAdd_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnAdd.Click

        'Check supplier name
        If txtSupplierName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the supplier name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtSupplierName.Focus()

            Exit Sub

        End If


        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                'Check duplicate supplier
                Dim checkQuery As String =
                    "SELECT COUNT(*) FROM Suppliers " &
                    "WHERE SupplierName = @SupplierName"

                Using checkCmd As New MySqlCommand(
                    checkQuery,
                    con
                )

                    checkCmd.Parameters.AddWithValue(
                        "@SupplierName",
                        txtSupplierName.Text.Trim()
                    )

                    Dim count As Integer =
                        Convert.ToInt32(
                            checkCmd.ExecuteScalar()
                        )


                    If count > 0 Then

                        MessageBox.Show(
                            "This supplier already exists.",
                            "Duplicate Supplier",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        txtSupplierName.Focus()

                        Exit Sub

                    End If

                End Using


                'Insert supplier
                Dim query As String =
                    "INSERT INTO Suppliers " &
                    "(SupplierName, Phone, Email, Address) " &
                    "VALUES " &
                    "(@SupplierName, @Phone, @Email, @Address)"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@SupplierName",
                        txtSupplierName.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Phone",
                        txtPhone.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Email",
                        txtEmail.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Address",
                        txtAddress.Text.Trim()
                    )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Supplier added successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadSuppliers()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Error adding supplier:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' SELECT SUPPLIER FROM DATAGRIDVIEW

    Private Sub dgvSuppliers_CellClick(
        sender As Object,
        e As DataGridViewCellEventArgs
    ) Handles dgvSuppliers.CellClick

        'Ignore header
        If e.RowIndex < 0 Then

            Exit Sub

        End If


        Try

            Dim row As DataGridViewRow =
                dgvSuppliers.Rows(e.RowIndex)


            txtSupplierName.Text =
                If(
                    row.Cells("SupplierName").Value Is Nothing,
                    "",
                    row.Cells("SupplierName").Value.ToString()
                )


            txtPhone.Text =
                If(
                    row.Cells("Phone").Value Is Nothing,
                    "",
                    row.Cells("Phone").Value.ToString()
                )


            txtEmail.Text =
                If(
                    row.Cells("Email").Value Is Nothing,
                    "",
                    row.Cells("Email").Value.ToString()
                )


            txtAddress.Text =
                If(
                    row.Cells("Address").Value Is Nothing,
                    "",
                    row.Cells("Address").Value.ToString()
                )


        Catch ex As Exception

            MessageBox.Show(
                "Unable to select supplier:" &
                vbCrLf & ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' UPDATE SUPPLIER

    Private Sub btnUpdate_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnUpdate.Click

        'Check selection
        If dgvSuppliers.CurrentRow Is Nothing Then

            MessageBox.Show(
                "Please select a supplier first.",
                "Update Supplier",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        'Check supplier name
        If txtSupplierName.Text.Trim() = "" Then

            MessageBox.Show(
                "Please enter the supplier name.",
                "Validation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            txtSupplierName.Focus()

            Exit Sub

        End If


        Try

            Dim supplierID As Integer =
                Convert.ToInt32(
                    dgvSuppliers.CurrentRow.Cells(
                        "SupplierID"
                    ).Value
                )


            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                'Check duplicate supplier
                Dim checkQuery As String =
                    "SELECT COUNT(*) FROM Suppliers " &
                    "WHERE SupplierName = @SupplierName " &
                    "AND SupplierID <> @SupplierID"


                Using checkCmd As New MySqlCommand(
                    checkQuery,
                    con
                )

                    checkCmd.Parameters.AddWithValue(
                        "@SupplierName",
                        txtSupplierName.Text.Trim()
                    )

                    checkCmd.Parameters.AddWithValue(
                        "@SupplierID",
                        supplierID
                    )


                    Dim count As Integer =
                        Convert.ToInt32(
                            checkCmd.ExecuteScalar()
                        )


                    If count > 0 Then

                        MessageBox.Show(
                            "Another supplier with this name already exists.",
                            "Duplicate Supplier",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Exit Sub

                    End If

                End Using


                'Update supplier
                Dim query As String =
                    "UPDATE Suppliers SET " &
                    "SupplierName = @SupplierName, " &
                    "Phone = @Phone, " &
                    "Email = @Email, " &
                    "Address = @Address " &
                    "WHERE SupplierID = @SupplierID"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@SupplierName",
                        txtSupplierName.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Phone",
                        txtPhone.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Email",
                        txtEmail.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@Address",
                        txtAddress.Text.Trim()
                    )

                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        supplierID
                    )


                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Supplier updated successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadSuppliers()

            ClearFields()


        Catch ex As Exception

            MessageBox.Show(
                "Error updating supplier:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' DELETE SUPPLIER

    Private Sub btnDelete_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnDelete.Click

        'Check selection
        If dgvSuppliers.CurrentRow Is Nothing Then

            MessageBox.Show(
                "Please select a supplier first.",
                "Delete Supplier",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            )

            Exit Sub

        End If


        Try

            Dim supplierID As Integer =
                Convert.ToInt32(
                    dgvSuppliers.CurrentRow.Cells(
                        "SupplierID"
                    ).Value
                )


            Dim supplierName As String =
                dgvSuppliers.CurrentRow.Cells(
                    "SupplierName"
                ).Value.ToString()


            'Confirm deletion
            Dim result As DialogResult =
                MessageBox.Show(
                    "Are you sure you want to delete:" &
                    vbCrLf & vbCrLf &
                    supplierName & "?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )


            If result <> DialogResult.Yes Then

                Exit Sub

            End If


            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                'Check Products
                Dim productQuery As String =
                    "SELECT COUNT(*) FROM Products " &
                    "WHERE SupplierID = @SupplierID"


                Using cmd As New MySqlCommand(
                    productQuery,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        supplierID
                    )


                    Dim productCount As Integer =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        )


                    If productCount > 0 Then

                        MessageBox.Show(
                            "This supplier cannot be deleted " &
                            "because it is being used by one or more products.",
                            "Cannot Delete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Exit Sub

                    End If

                End Using


                'Check Purchases
                Dim purchaseQuery As String =
                    "SELECT COUNT(*) FROM Purchases " &
                    "WHERE SupplierID = @SupplierID"


                Using cmd As New MySqlCommand(
                    purchaseQuery,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        supplierID
                    )


                    Dim purchaseCount As Integer =
                        Convert.ToInt32(
                            cmd.ExecuteScalar()
                        )


                    If purchaseCount > 0 Then

                        MessageBox.Show(
                            "This supplier cannot be deleted " &
                            "because purchase records exist for this supplier.",
                            "Cannot Delete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Exit Sub

                    End If

                End Using


                'Delete supplier
                Dim query As String =
                    "DELETE FROM Suppliers " &
                    "WHERE SupplierID = @SupplierID"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@SupplierID",
                        supplierID
                    )

                    cmd.ExecuteNonQuery()

                End Using

            End Using


            MessageBox.Show(
                "Supplier deleted successfully.",
                "Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            )


            LoadSuppliers()

            ClearFields()


        Catch ex As MySqlException

            MessageBox.Show(
                "Database error while deleting supplier:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )


        Catch ex As Exception

            MessageBox.Show(
                "Error deleting supplier:" &
                vbCrLf & ex.Message,
                "Error",
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

        txtSupplierName.Clear()

        txtPhone.Clear()

        txtEmail.Clear()

        txtAddress.Clear()

        dgvSuppliers.ClearSelection()

        txtSupplierName.Focus()

    End Sub



    ' SEARCH SUPPLIERS

    Private Sub txtSearch_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtSearch.TextChanged

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "SELECT SupplierID, SupplierName, Phone, Email, Address " &
                    "FROM Suppliers " &
                    "WHERE SupplierName LIKE @Search " &
                    "OR Phone LIKE @Search " &
                    "OR Email LIKE @Search " &
                    "OR Address LIKE @Search " &
                    "ORDER BY SupplierID DESC"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )

                    cmd.Parameters.AddWithValue(
                        "@Search",
                        "%" & txtSearch.Text.Trim() & "%"
                    )


                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)

                        dgvSuppliers.DataSource = table

                    End Using

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error searching suppliers:" &
                vbCrLf & ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub

End Class