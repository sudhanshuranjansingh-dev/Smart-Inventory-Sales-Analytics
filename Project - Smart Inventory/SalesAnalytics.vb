Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text.Json
Imports System.Globalization

Public Class SalesAnalytics

    Public Sub New()

        InitializeComponent()

        AddHandler Me.Load, AddressOf SalesAnalytics_Load_Test

    End Sub

    Private Sub SalesAnalytics_Load_Test(
    sender As Object,
    e As EventArgs
)

        MessageBox.Show("SalesAnalytics form is loading!")

    End Sub

    Private webView As WebView2
    Private initialLoadDone As Boolean = False

    '=========================================================
    ' FORM LOAD
    '=========================================================
    Private Async Sub SalesAnalytics_Load(
        sender As Object,
        e As EventArgs
    ) Handles MyBase.Load

        Try
            MessageBox.Show("SalesAnalytics_Load is running!")
            Me.Text = "Sales Analytics"
            Me.WindowState = FormWindowState.Maximized

            '-------------------------------------------------
            ' CREATE WEBVIEW2
            '-------------------------------------------------
            webView = New WebView2()

            webView.Dock = DockStyle.Fill

            Me.Controls.Add(webView)

            '-------------------------------------------------
            ' INITIALIZE WEBVIEW2
            '-------------------------------------------------
            Await webView.EnsureCoreWebView2Async(Nothing)

            '-------------------------------------------------
            ' EVENTS
            '-------------------------------------------------
            AddHandler webView.CoreWebView2.WebMessageReceived,
                AddressOf WebView_MessageReceived

            AddHandler webView.CoreWebView2.NavigationCompleted,
                AddressOf WebView_NavigationCompleted

            '-------------------------------------------------
            ' HTML PATH
            '-------------------------------------------------
            Dim htmlPath As String =
                Path.Combine(
                    Application.StartupPath,
                    "WebUI",
                    "sales-analytics.html"
                )

            If Not File.Exists(htmlPath) Then

                MessageBox.Show(
                    "Sales Analytics HTML file not found:" &
                    Environment.NewLine &
                    htmlPath,
                    "File Not Found",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                )

                Return

            End If

            '-------------------------------------------------
            ' LOAD HTML
            '-------------------------------------------------
            webView.CoreWebView2.Navigate(
                New Uri(htmlPath).AbsoluteUri
            )

        Catch ex As Exception

            MessageBox.Show(
                "Sales Analytics Load Error:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' WEBVIEW NAVIGATION COMPLETED
    '=========================================================
    Private Async Sub WebView_NavigationCompleted(
        sender As Object,
        e As CoreWebView2NavigationCompletedEventArgs
    )

        If initialLoadDone Then Return

        initialLoadDone = True

        Try

            Dim fromDate As Date =
                Date.Today.AddDays(-30)

            Dim toDate As Date =
                Date.Today

            Await LoadSalesAnalytics(
                fromDate,
                toDate
            )

        Catch ex As Exception

            MessageBox.Show(
                "Initial Sales Analytics Error:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' RECEIVE MESSAGE FROM JAVASCRIPT
    '=========================================================
    Private Async Sub WebView_MessageReceived(
        sender As Object,
        e As CoreWebView2WebMessageReceivedEventArgs
    )

        Try

            Dim json As String =
                e.WebMessageAsJson

            Using document As JsonDocument =
                JsonDocument.Parse(json)

                Dim root As JsonElement =
                    document.RootElement

                Dim actionElement As JsonElement

                If Not root.TryGetProperty(
                    "action",
                    actionElement
                ) Then
                    Return
                End If

                Dim action As String =
                    actionElement.GetString()

                If action = "loadSalesAnalytics" Then

                    Dim fromDate As Date =
                        Date.ParseExact(
                            root.GetProperty("fromDate").GetString(),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )

                    Dim toDate As Date =
                        Date.ParseExact(
                            root.GetProperty("toDate").GetString(),
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture
                        )

                    Await LoadSalesAnalytics(
                        fromDate,
                        toDate
                    )

                End If

            End Using

        Catch ex As Exception

            MessageBox.Show(
                "WebView Message Error:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Sub


    '=========================================================
    ' LOAD SALES ANALYTICS
    '=========================================================
    Private Async Function LoadSalesAnalytics(
        fromDate As Date,
        toDate As Date
    ) As Task

        Try

            Dim totalSales As Decimal = 0D
            Dim totalOrders As Integer = 0
            Dim averageSale As Decimal = 0D

            Dim dailySales As New List(Of Dictionary(Of String, Object))
            Dim monthlySales As New List(Of Dictionary(Of String, Object))
            Dim paymentMethods As New List(Of Dictionary(Of String, Object))
            Dim statusData As New List(Of Dictionary(Of String, Object))

            Using conn As MySqlConnection =
                DBConnection.GetConnection()

                Await conn.OpenAsync()

                '-------------------------------------------------
                ' TEST - DATABASE CONNECTION
                '-------------------------------------------------
                MessageBox.Show(
                    "Database connected!" &
                    Environment.NewLine &
                    "From: " & fromDate.ToString("yyyy-MM-dd") &
                    Environment.NewLine &
                    "To: " & toDate.ToString("yyyy-MM-dd"),
                    "Sales Analytics Test"
                )

                Dim sqlToDate As Date =
                    toDate.Date.AddDays(1)


                '=================================================
                ' TOTAL SALES
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT COALESCE(SUM(TotalAmount),0) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    totalSales =
                        Convert.ToDecimal(
                            Await cmd.ExecuteScalarAsync()
                        )

                End Using


                '=================================================
                ' TOTAL ORDERS
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT COUNT(*) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    totalOrders =
                        Convert.ToInt32(
                            Await cmd.ExecuteScalarAsync()
                        )

                End Using


                '=================================================
                ' AVERAGE SALE
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT COALESCE(AVG(TotalAmount),0) " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    averageSale =
                        Convert.ToDecimal(
                            Await cmd.ExecuteScalarAsync()
                        )

                End Using


                '=================================================
                ' DAILY SALES
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT DATE(SaleDate) AS SaleDay, " &
                    "SUM(TotalAmount) AS DailySales " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY DATE(SaleDate) " &
                    "ORDER BY DATE(SaleDate)",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    Using reader =
                        Await cmd.ExecuteReaderAsync()

                        While Await reader.ReadAsync()

                            Dim dailyItem As New Dictionary(Of String, Object)

                            dailyItem.Add(
                                "date",
                                Convert.ToDateTime(
                                    reader("SaleDay")
                                ).ToString("dd MMM")
                            )

                            dailyItem.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("DailySales")
                                )
                            )

                            dailySales.Add(dailyItem)

                        End While

                    End Using

                End Using


                '=================================================
                ' MONTHLY SALES
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT DATE_FORMAT(SaleDate,'%b %Y') AS SaleMonth, " &
                    "SUM(TotalAmount) AS MonthlySales " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY YEAR(SaleDate), MONTH(SaleDate) " &
                    "ORDER BY YEAR(SaleDate), MONTH(SaleDate)",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    Using reader =
                        Await cmd.ExecuteReaderAsync()

                        While Await reader.ReadAsync()

                            Dim monthlyItem As New Dictionary(Of String, Object)

                            monthlyItem.Add(
                                "month",
                                reader("SaleMonth").ToString()
                            )

                            monthlyItem.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("MonthlySales")
                                )
                            )

                            monthlySales.Add(monthlyItem)

                        End While

                    End Using

                End Using


                '=================================================
                ' PAYMENT METHODS
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT PaymentMethod, " &
                    "SUM(TotalAmount) AS PaymentSales " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY PaymentMethod " &
                    "ORDER BY PaymentSales DESC",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    Using reader =
                        Await cmd.ExecuteReaderAsync()

                        While Await reader.ReadAsync()

                            Dim paymentItem As New Dictionary(Of String, Object)

                            paymentItem.Add(
                                "method",
                                reader("PaymentMethod").ToString()
                            )

                            paymentItem.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("PaymentSales")
                                )
                            )

                            paymentMethods.Add(paymentItem)

                        End While

                    End Using

                End Using


                '=================================================
                ' SALES STATUS
                '=================================================
                Using cmd As New MySqlCommand(
                    "SELECT Status AS SaleStatus, " &
                    "COUNT(*) AS OrderCount, " &
                    "SUM(TotalAmount) AS StatusSales " &
                    "FROM Sales " &
                    "WHERE SaleDate >= @FromDate " &
                    "AND SaleDate < @ToDate " &
                    "GROUP BY Status " &
                    "ORDER BY OrderCount DESC",
                    conn
                )

                    cmd.Parameters.AddWithValue(
                        "@FromDate",
                        fromDate.Date
                    )

                    cmd.Parameters.AddWithValue(
                        "@ToDate",
                        sqlToDate
                    )

                    Using reader =
                        Await cmd.ExecuteReaderAsync()

                        While Await reader.ReadAsync()

                            Dim statusItem As New Dictionary(Of String, Object)

                            statusItem.Add(
                                "status",
                                reader("SaleStatus").ToString()
                            )

                            statusItem.Add(
                                "orders",
                                Convert.ToInt32(
                                    reader("OrderCount")
                                )
                            )

                            statusItem.Add(
                                "amount",
                                Convert.ToDecimal(
                                    reader("StatusSales")
                                )
                            )

                            statusData.Add(statusItem)

                        End While

                    End Using

                End Using

            End Using


            '=====================================================
            ' PREPARE RESULT
            '=====================================================
            Dim result As New Dictionary(Of String, Object)

            result.Add("totalSales", totalSales)
            result.Add("totalOrders", totalOrders)
            result.Add("averageSale", averageSale)
            result.Add("dailySales", dailySales)
            result.Add("monthlySales", monthlySales)
            result.Add("paymentMethods", paymentMethods)
            result.Add("statusData", statusData)


            '=====================================================
            ' SEND DATA TO JAVASCRIPT
            '=====================================================
            Dim jsonResult As String =
                JsonSerializer.Serialize(result)

            If webView IsNot Nothing AndAlso
               webView.CoreWebView2 IsNot Nothing Then

                webView.CoreWebView2.PostWebMessageAsString(
                    jsonResult
                )

            End If

        Catch ex As Exception

            MessageBox.Show(
                "Load Sales Analytics Error:" &
                Environment.NewLine &
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            )

        End Try

    End Function

End Class