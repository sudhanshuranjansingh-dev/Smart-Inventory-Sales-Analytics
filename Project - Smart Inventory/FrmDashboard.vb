Imports MySql.Data.MySqlClient
Imports ScottPlot

Public Class FrmDashboard


    ' FORM LOAD

    Private Sub FrmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        LoadDashboardData()
        LoadMonthlySalesChart()

    End Sub



    ' LOAD DASHBOARD CARDS

    Private Sub LoadDashboardData()

        Try

            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()



                ' TOTAL PRODUCTS


                Dim queryProducts As String =
                    "SELECT COUNT(*) FROM Products"

                Using cmd As New MySqlCommand(queryProducts, con)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        lblTotalProducts.Text = "0"
                    Else
                        lblTotalProducts.Text =
                            Convert.ToInt32(result).ToString()
                    End If

                End Using



                ' TOTAL STOCK


                Dim queryStock As String =
                    "SELECT COALESCE(SUM(StockQuantity), 0) FROM Products"

                Using cmd As New MySqlCommand(queryStock, con)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        lblTotalStock.Text = "0"
                    Else
                        lblTotalStock.Text =
                            Convert.ToInt32(result).ToString()
                    End If

                End Using



                ' LOW STOCK


                Dim queryLowStock As String =
                    "SELECT COUNT(*) " &
                    "FROM Products " &
                    "WHERE StockQuantity > 0 " &
                    "AND StockQuantity <= MinimumStock"

                Using cmd As New MySqlCommand(queryLowStock, con)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then
                        lblLowStockItems.Text = "0"
                    Else
                        lblLowStockItems.Text =
                            Convert.ToInt32(result).ToString()
                    End If

                End Using



                ' TOTAL SALES


                Dim querySales As String =
                    "SELECT COALESCE(SUM(TotalAmount), 0) FROM Sales"

                Using cmd As New MySqlCommand(querySales, con)

                    Dim result As Object = cmd.ExecuteScalar()

                    If result Is Nothing OrElse IsDBNull(result) Then

                        lblTotalSales.Text = "₹0.00"

                    Else

                        Dim totalSales As Decimal =
                            Convert.ToDecimal(result)

                        lblTotalSales.Text =
                            "₹" & totalSales.ToString("N2")

                    End If

                End Using

            End Using


        Catch ex As Exception

            MessageBox.Show(
                "Error loading dashboard data:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Dashboard Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' MONTHLY SALES CHART

    Private Sub LoadMonthlySalesChart()

        Try


            ' Clear previous graph


            plotSales.Plot.Clear()



            ' Array for 12 months


            Dim monthlySales(11) As Double


            Using con As MySqlConnection = DBConnection.GetConnection()

                con.Open()



                ' Get monthly sales


                Dim query As String =
                    "SELECT MONTH(SaleDate) AS SaleMonth, " &
                    "COALESCE(SUM(TotalAmount), 0) AS TotalSales " &
                    "FROM Sales " &
                    "WHERE YEAR(SaleDate) = YEAR(CURDATE()) " &
                    "GROUP BY MONTH(SaleDate) " &
                    "ORDER BY MONTH(SaleDate)"


                Using cmd As New MySqlCommand(query, con)

                    Using reader As MySqlDataReader =
                        cmd.ExecuteReader()

                        While reader.Read()

                            Dim monthNumber As Integer =
                                Convert.ToInt32(reader("SaleMonth"))

                            Dim salesAmount As Double =
                                Convert.ToDouble(reader("TotalSales"))


                            ' Month 1 = January
                            ' Array starts at 0

                            monthlySales(monthNumber - 1) =
                                salesAmount

                        End While

                    End Using

                End Using

            End Using



            ' ADD BAR CHART


            Dim bars = plotSales.Plot.Add.Bars(monthlySales)



            ' MONTH NAMES
            

            Dim monthNames() As String =
            {
                "Jan",
                "Feb",
                "Mar",
                "Apr",
                "May",
                "Jun",
                "Jul",
                "Aug",
                "Sep",
                "Oct",
                "Nov",
                "Dec"
            }



            ' Create X-axis labels


            Dim ticks As New List(Of ScottPlot.Tick)

            For i As Integer = 0 To 11

                ticks.Add(
                    New ScottPlot.Tick(
                        i,
                        monthNames(i)
                    )
                )

            Next


            plotSales.Plot.Axes.Bottom.TickGenerator =
                New ScottPlot.TickGenerators.NumericManual(
                    ticks.ToArray()
                )



            ' GRAPH SETTINGS


            plotSales.Plot.Title("Monthly Sales")

            plotSales.Plot.YLabel("Sales (₹)")

            plotSales.Plot.XLabel("Month")


            ' Start graph from zero

            plotSales.Plot.Axes.Margins(
                bottom:=0,
                top:=0.15
            )


            ' Hide unnecessary grid

            plotSales.Plot.HideGrid()



            ' REFRESH GRAPH


            plotSales.Refresh()


        Catch ex As Exception

            MessageBox.Show(
                "Error loading monthly sales chart:" &
                vbCrLf & vbCrLf &
                ex.Message,
                "Chart Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub



    ' REFRESH DASHBOARD

    Public Sub RefreshDashboard()

        LoadDashboardData()
        LoadMonthlySalesChart()

    End Sub

End Class