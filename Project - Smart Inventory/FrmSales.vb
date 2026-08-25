Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing

Public Class FrmSales


    ' VARIABLES


    Private WithEvents PrintDoc As New PrintDocument()

    Private InvoiceNo As String



    ' FORM LOAD


    Private Sub FrmSales_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Try

            ' Configure payment ComboBox
            cmbPayment.Items.Clear()

            cmbPayment.Items.Add("Cash")
            cmbPayment.Items.Add("Card")
            cmbPayment.Items.Add("UPI")

            cmbPayment.SelectedIndex = 0


            ' Configure calculated TextBoxes

            txtSubtotal.ReadOnly = True

            txtTotalPrice.ReadOnly = True

            txtSubtotal.Text = "0.00"

            txtDiscount.Text = "0.00"

            txtTotalPrice.Text = "0.00"


            ' Configure cart

            ConfigureCart()


            ' Load products directly from MySQL

            LoadProducts()


            lblAvailability.Text = "0"

            lblPrice.Text = "₹0.00"


        Catch ex As Exception

            MessageBox.Show(
                "Error loading Sales Form:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Sales Form Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' CONFIGURE CART


    Private Sub ConfigureCart()

        dgvCart.Columns.Clear()

        dgvCart.AllowUserToAddRows = False

        dgvCart.AllowUserToDeleteRows = False

        dgvCart.ReadOnly = True

        dgvCart.MultiSelect = False

        dgvCart.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect

        dgvCart.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill


        ' Product ID
        Dim idColumn As New DataGridViewTextBoxColumn()

        idColumn.Name = "ProductID"

        idColumn.HeaderText = "Product ID"

        idColumn.Visible = False

        dgvCart.Columns.Add(idColumn)


        ' Product
        dgvCart.Columns.Add(
            "Product",
            "Product"
        )


        ' Quantity
        dgvCart.Columns.Add(
            "Qty",
            "Qty"
        )


        ' Price
        dgvCart.Columns.Add(
            "Price",
            "Price"
        )


        ' Total
        dgvCart.Columns.Add(
            "Total",
            "Total"
        )

    End Sub



    ' LOAD PRODUCTS FROM MYSQL DATABASE


    Private Sub LoadProducts()

        Try

            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Dim query As String =
                    "SELECT ProductID, ProductName, " &
                    "SellingPrice, StockQuantity " &
                    "FROM Products " &
                    "ORDER BY ProductName ASC"


                Using cmd As New MySqlCommand(
                    query,
                    con
                )


                    Using adapter As New MySqlDataAdapter(cmd)

                        Dim table As New DataTable()

                        adapter.Fill(table)


                        ' Connect database table
                        ' directly to ComboBox

                        cmbProduct.DataSource = Nothing

                        cmbProduct.DisplayMember =
                            "ProductName"

                        cmbProduct.ValueMember =
                            "ProductID"

                        cmbProduct.DataSource =
                            table


                    End Using

                End Using

            End Using


            ' Don't select anything initially

            cmbProduct.SelectedIndex = -1

            lblAvailability.Text = "0"

            lblPrice.Text = "₹0.00"


        Catch ex As Exception

            MessageBox.Show(
                "Error loading products from database:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Database Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' PRODUCT SELECTED


    Private Sub cmbProduct_SelectedIndexChanged(
        sender As Object,
        e As EventArgs
    ) Handles cmbProduct.SelectedIndexChanged

        Try

            If cmbProduct.SelectedIndex = -1 Then

                lblAvailability.Text = "0"

                lblPrice.Text = "₹0.00"

                Return

            End If


            If cmbProduct.SelectedItem Is Nothing Then
                Return
            End If


            Dim row As DataRowView =
                TryCast(
                    cmbProduct.SelectedItem,
                    DataRowView
                )


            If row Is Nothing Then
                Return
            End If


            ' Get stock

            Dim stock As Integer =
                Convert.ToInt32(
                    row("StockQuantity")
                )


            ' Get selling price

            Dim price As Decimal =
                Convert.ToDecimal(
                    row("SellingPrice")
                )


            ' Display stock

            lblAvailability.Text =
                stock.ToString()


            ' Display price

            lblPrice.Text =
                "₹" & price.ToString("0.00")


        Catch ex As Exception

            MessageBox.Show(
                "Error selecting product:" &
                vbCrLf & ex.Message,
                "Product Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' ADD TO CART


    Private Sub btnAddToCart_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnAddToCart.Click

        Try


            ' CHECK PRODUCT


            If cmbProduct.SelectedIndex = -1 Then

                MessageBox.Show(
                    "Please select a product.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                cmbProduct.Focus()

                Return

            End If



            ' CHECK QUANTITY


            Dim quantity As Integer


            If Not Integer.TryParse(
                txtQuantity.Text.Trim(),
                quantity
            ) OrElse quantity <= 0 Then

                MessageBox.Show(
                    "Please enter a valid quantity.",
                    "Invalid Quantity",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                txtQuantity.Focus()

                Return

            End If



            ' GET SELECTED DATABASE ROW


            Dim row As DataRowView =
                TryCast(
                    cmbProduct.SelectedItem,
                    DataRowView
                )


            If row Is Nothing Then
                Return
            End If


            Dim productID As Integer =
                Convert.ToInt32(
                    row("ProductID")
                )


            Dim productName As String =
                row("ProductName").ToString()


            Dim price As Decimal =
                Convert.ToDecimal(
                    row("SellingPrice")
                )


            Dim availableStock As Integer =
                Convert.ToInt32(
                    row("StockQuantity")
                )



            ' CHECK STOCK


            If quantity > availableStock Then

                MessageBox.Show(
                    "Available stock: " &
                    availableStock.ToString(),
                    "Insufficient Stock",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Return

            End If



            ' CHECK IF PRODUCT ALREADY EXISTS


            For Each cartRow As DataGridViewRow
                In dgvCart.Rows

                If Convert.ToInt32(
                    cartRow.Cells("ProductID").Value
                ) = productID Then


                    Dim oldQuantity As Integer =
                        Convert.ToInt32(
                            cartRow.Cells("Qty").Value
                        )


                    Dim newQuantity As Integer =
                        oldQuantity + quantity


                    If newQuantity > availableStock Then

                        MessageBox.Show(
                            "You cannot add more than " &
                            availableStock &
                            " units.",
                            "Insufficient Stock",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        )

                        Return

                    End If


                    cartRow.Cells("Qty").Value =
                        newQuantity


                    cartRow.Cells("Total").Value =
                        newQuantity * price


                    CalculateTotal()


                    txtQuantity.Clear()

                    Return

                End If

            Next



            ' ADD NEW PRODUCT


            Dim total As Decimal =
                quantity * price


            dgvCart.Rows.Add(
                productID,
                productName,
                quantity,
                price.ToString("0.00"),
                total.ToString("0.00")
            )


            CalculateTotal()


            txtQuantity.Clear()


        Catch ex As Exception

            MessageBox.Show(
                "Error adding product:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Cart Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' CALCULATE TOTAL


    Private Sub CalculateTotal()

        Dim subtotal As Decimal = 0D


        For Each row As DataGridViewRow
            In dgvCart.Rows

            If row.Cells("Total").Value IsNot Nothing Then

                subtotal +=
                    Convert.ToDecimal(
                        row.Cells("Total").Value
                    )

            End If

        Next


        txtSubtotal.Text =
            subtotal.ToString("0.00")



        ' DISCOUNT


        Dim discount As Decimal = 0D


        If txtDiscount.Text.Trim() <> "" Then

            Decimal.TryParse(
                txtDiscount.Text.Trim(),
                discount
            )

        End If


        If discount < 0 Then
            discount = 0
        End If


        If discount > subtotal Then
            discount = subtotal
        End If



        ' GRAND TOTAL


        Dim grandTotal As Decimal =
            subtotal - discount


        txtTotalPrice.Text =
            grandTotal.ToString("0.00")

    End Sub



    ' DISCOUNT CHANGED


    Private Sub txtDiscount_TextChanged(
        sender As Object,
        e As EventArgs
    ) Handles txtDiscount.TextChanged

        CalculateTotal()

    End Sub



    ' COMPLETE SALE


    Private Sub btnCompleteSale_Click(
        sender As Object,
        e As EventArgs
    ) Handles btnCompleteSale.Click

        Try


            ' CUSTOMER VALIDATION


            If txtCustomerName.Text.Trim() = "" Then

                MessageBox.Show(
                    "Please enter customer name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                txtCustomerName.Focus()

                Return

            End If



            ' CART VALIDATION


            If dgvCart.Rows.Count = 0 Then

                MessageBox.Show(
                    "Please add products to the cart.",
                    "Empty Cart",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Return

            End If



            ' PAYMENT VALIDATION


            If cmbPayment.SelectedIndex = -1 Then

                MessageBox.Show(
                    "Please select payment method.",
                    "Payment Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                )

                Return

            End If


            CalculateTotal()


            Dim subtotal As Decimal =
                Convert.ToDecimal(
                    txtSubtotal.Text
                )


            Dim discount As Decimal =
                Convert.ToDecimal(
                    txtDiscount.Text
                )


            Dim totalAmount As Decimal =
                Convert.ToDecimal(
                    txtTotalPrice.Text
                )



            ' CONFIRM


            Dim answer As DialogResult =
                MessageBox.Show(
                    "Complete this sale?" &
                    vbCrLf & vbCrLf &
                    "Total: ₹" &
                    totalAmount.ToString("0.00"),
                    "Confirm Sale",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                )


            If answer <> DialogResult.Yes Then
                Return
            End If



            ' DATABASE TRANSACTION


            Using con As MySqlConnection =
                DBConnection.GetConnection()

                con.Open()


                Using transaction As MySqlTransaction =
                    con.BeginTransaction()

                    Try


                        ' INSERT SALES


                        Dim saleQuery As String =
                            "INSERT INTO Sales " &
                            "(CustomerName, SaleDate, " &
                            "PaymentMethod, TotalAmount) " &
                            "VALUES " &
                            "(@CustomerName, NOW(), " &
                            "@PaymentMethod, @TotalAmount)"


                        Dim saleID As Integer


                        Using cmd As New MySqlCommand(
                            saleQuery,
                            con,
                            transaction
                        )

                            cmd.Parameters.AddWithValue(
                                "@CustomerName",
                                txtCustomerName.Text.Trim()
                            )


                            cmd.Parameters.AddWithValue(
                                "@PaymentMethod",
                                cmbPayment.Text
                            )


                            cmd.Parameters.AddWithValue(
                                "@TotalAmount",
                                totalAmount
                            )


                            cmd.ExecuteNonQuery()


                            saleID =
                                Convert.ToInt32(
                                    cmd.LastInsertedId
                                )

                        End Using



                        ' SALE DETAILS + STOCK UPDATE


                        For Each cartRow As DataGridViewRow
                            In dgvCart.Rows


                            Dim productID As Integer =
                                Convert.ToInt32(
                                    cartRow.Cells("ProductID").Value
                                )


                            Dim quantity As Integer =
                                Convert.ToInt32(
                                    cartRow.Cells("Qty").Value
                                )


                            Dim price As Decimal =
                                Convert.ToDecimal(
                                    cartRow.Cells("Price").Value
                                )


                            Dim itemTotal As Decimal =
                                Convert.ToDecimal(
                                    cartRow.Cells("Total").Value
                                )



                            ' INSERT SALE DETAIL


                            Dim detailQuery As String =
                                "INSERT INTO SaleDetails " &
                                "(SaleID, ProductID, Quantity, " &
                                "SellingPrice, TotalPrice) " &
                                "VALUES " &
                                "(@SaleID, @ProductID, @Quantity, " &
                                "@SellingPrice, @TotalPrice)"


                            Using cmdDetail As New MySqlCommand(
                                detailQuery,
                                con,
                                transaction
                            )

                                cmdDetail.Parameters.AddWithValue(
                                    "@SaleID",
                                    saleID
                                )


                                cmdDetail.Parameters.AddWithValue(
                                    "@ProductID",
                                    productID
                                )


                                cmdDetail.Parameters.AddWithValue(
                                    "@Quantity",
                                    quantity
                                )


                                cmdDetail.Parameters.AddWithValue(
                                    "@SellingPrice",
                                    price
                                )


                                cmdDetail.Parameters.AddWithValue(
                                    "@TotalPrice",
                                    itemTotal
                                )


                                cmdDetail.ExecuteNonQuery()

                            End Using



                            ' UPDATE STOCK


                            Dim stockQuery As String =
                                "UPDATE Products " &
                                "SET StockQuantity = " &
                                "StockQuantity - @Quantity " &
                                "WHERE ProductID = @ProductID " &
                                "AND StockQuantity >= @Quantity"


                            Using cmdStock As New MySqlCommand(
                                stockQuery,
                                con,
                                transaction
                            )

                                cmdStock.Parameters.AddWithValue(
                                    "@Quantity",
                                    quantity
                                )


                                cmdStock.Parameters.AddWithValue(
                                    "@ProductID",
                                    productID
                                )


                                Dim affected As Integer =
                                    cmdStock.ExecuteNonQuery()


                                If affected = 0 Then

                                    Throw New Exception(
                                        "Insufficient stock for product ID " &
                                        productID
                                    )

                                End If

                            End Using

                        Next



                        ' COMMIT


                        transaction.Commit()



                        ' INVOICE


                        InvoiceNo =
                            "INV-" &
                            saleID.ToString("00000")


                        MessageBox.Show(
                            "Sale completed successfully!" &
                            vbCrLf & vbCrLf &
                            "Invoice No: " &
                            InvoiceNo,
                            "Sale Completed",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        )


                        ' Show invoice

                        ShowInvoice()


                        ' Clear form

                        ClearSaleForm()


                        ' Reload products from database

                        LoadProducts()


                    Catch ex As Exception

                        transaction.Rollback()

                        Throw

                    End Try

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error completing sale:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Sale Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' SHOW INVOICE


    Private Sub ShowInvoice()

        PrintDoc.DefaultPageSettings.PaperSize =
            New PaperSize(
                "Invoice",
                850,
                1100
            )


        Dim preview As New PrintPreviewDialog()

        preview.Document = PrintDoc

        preview.Width = 900

        preview.Height = 700

        preview.ShowDialog()

    End Sub



    ' PRINT INVOICE


    Private Sub PrintDoc_PrintPage(
        sender As Object,
        e As PrintPageEventArgs
    ) Handles PrintDoc.PrintPage


        Dim g As Graphics =
            e.Graphics


        Dim titleFont As New Font(
            "Arial",
            18,
            FontStyle.Bold
        )


        Dim headingFont As New Font(
            "Arial",
            11,
            FontStyle.Bold
        )


        Dim normalFont As New Font(
            "Arial",
            10
        )


        Dim y As Integer = 40



        ' HEADER


        g.DrawString(
            "SMART INVENTORY",
            titleFont,
            Brushes.Black,
            270,
            y
        )


        y += 35


        g.DrawString(
            "INVOICE",
            headingFont,
            Brushes.Black,
            360,
            y
        )


        y += 40


        g.DrawString(
            "Invoice No: " & InvoiceNo,
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 25


        g.DrawString(
            "Date: " &
            DateTime.Now.ToString("dd-MMM-yyyy"),
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 25


        g.DrawString(
            "Customer: " &
            txtCustomerName.Text,
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 35


        g.DrawString(
            "-------------------------------------", normalFont, Brushes.Black, 60, y)
        y += 25



        ' TABLE HEADER


        g.DrawString("Product", headingFont, Brushes.Black, 60, y)


        g.DrawString("Qty", headingFont, Brushes.Black, 330, y)


        g.DrawString("Price", headingFont, Brushes.Black, 410, y)


        g.DrawString("Total", headingFont, Brushes.Black, 520, y)


        y += 25


        g.DrawString(
            "-------------------------------------",
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 25



        ' CART ITEMS


        For Each row As DataGridViewRow
            In dgvCart.Rows


            If row.IsNewRow Then
                Continue For
            End If


            Dim productName As String =
                Convert.ToString(
                    row.Cells("Product").Value
                )


            Dim quantity As String =
                Convert.ToString(
                    row.Cells("Qty").Value
                )


            Dim price As String =
                Convert.ToString(
                    row.Cells("Price").Value
                )


            Dim total As String =
                Convert.ToString(
                    row.Cells("Total").Value
                )


            g.DrawString(
                productName,
                normalFont,
                Brushes.Black,
                60,
                y
            )


            g.DrawString(
                quantity,
                normalFont,
                Brushes.Black,
                330,
                y
            )


            g.DrawString(
                price,
                normalFont,
                Brushes.Black,
                410,
                y
            )


            g.DrawString(
                total,
                normalFont,
                Brushes.Black,
                520,
                y
            )


            y += 25

        Next


        y += 10


        g.DrawString(
            "-------------------------------------",
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 30



        ' TOTAL


        g.DrawString(
            "Total:",
            headingFont,
            Brushes.Black,
            400,
            y
        )


        g.DrawString(
            "₹" & txtTotalPrice.Text,
            headingFont,
            Brushes.Black,
            520,
            y
        )


        y += 30



        ' PAYMENT


        g.DrawString(
            "Payment:",
            headingFont,
            Brushes.Black,
            400,
            y
        )


        g.DrawString(
            cmbPayment.Text,
            normalFont,
            Brushes.Black,
            520,
            y
        )


        y += 60



        ' FOOTER


        g.DrawString(
            "=====================================",
            normalFont,
            Brushes.Black,
            60,
            y
        )


        y += 30


        g.DrawString(
            "Thank You!",
            headingFont,
            Brushes.Black,
            350,
            y
        )

    End Sub



    ' CLEAR FORM


    Private Sub ClearSaleForm()

        txtCustomerName.Clear()

        cmbProduct.SelectedIndex = -1

        txtQuantity.Clear()

        txtDiscount.Text = "0.00"

        txtSubtotal.Text = "0.00"

        txtTotalPrice.Text = "0.00"

        lblAvailability.Text = "0"

        lblPrice.Text = "₹0.00"

        dgvCart.Rows.Clear()

        cmbPayment.SelectedIndex = 0

        txtCustomerName.Focus()

    End Sub

End Class