/* =========================================================
   SMART INVENTORY
   SALES ANALYTICS
   ========================================================= */

let dailyChart = null;
let monthlyChart = null;
let paymentChart = null;


/* =========================================================
   PAGE LOAD
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    setDefaultDates();

    const btnApply =
        document.getElementById("btnApply");

    const btnRefresh =
        document.getElementById("btnRefresh");


    if (btnApply) {

        btnApply.addEventListener(
            "click",
            function () {
                loadSalesAnalytics();
            }
        );

    }


    if (btnRefresh) {

        btnRefresh.addEventListener(
            "click",
            function () {
                loadSalesAnalytics();
            }
        );

    }


    /* =====================================================
       RECEIVE DATA FROM VB.NET
       ===================================================== */

   /* =====================================================
   RECEIVE DATA FROM PARENT INDEX PAGE
   ===================================================== */

window.addEventListener("message", function (event) {

    let data = event.data;


    /* -----------------------------------------
       Convert JSON string if necessary
       ----------------------------------------- */

    if (typeof data === "string") {

        try {

            data = JSON.parse(data);

        }
        catch (error) {

            console.error(
                "Invalid JSON received:",
                error
            );

            return;
        }

    }


    console.log(
        "Sales Analytics received:",
        data
    );


    /* -----------------------------------------
       Check response
       ----------------------------------------- */

    if (
        data &&
        data.action === "salesAnalyticsData"
    ) {

        updateDashboard(data);

    }

});
      // Load analytics automatically
    loadSalesAnalytics();

});


/* =========================================================
   DEFAULT DATES
   ========================================================= */

function setDefaultDates() {

    const fromDate =
        document.getElementById("fromDate");

    const toDate =
        document.getElementById("toDate");


    if (!fromDate || !toDate) {
        return;
    }


    const today = new Date();

    const previousDate = new Date();

    previousDate.setDate(
        today.getDate() - 30
    );


    fromDate.value =
        formatDate(previousDate);

    toDate.value =
        formatDate(today);

}


/* =========================================================
   FORMAT DATE
   ========================================================= */

function formatDate(date) {

    const year =
        date.getFullYear();

    const month =
        String(
            date.getMonth() + 1
        ).padStart(2, "0");

    const day =
        String(
            date.getDate()
        ).padStart(2, "0");


    return `${year}-${month}-${day}`;

}


/* =========================================================
   LOAD SALES ANALYTICS
   ========================================================= */

function loadSalesAnalytics() {

    const fromDate =
        document.getElementById("fromDate").value;

    const toDate =
        document.getElementById("toDate").value;


    console.log(
        "Requesting Sales Analytics:",
        fromDate,
        toDate
    );


   /* =====================================================
   SEND REQUEST TO PARENT INDEX PAGE
   ===================================================== */

window.parent.postMessage({

    action: "loadSalesAnalytics",

    fromDate: fromDate,

    toDate: toDate

}, "*");

}


/* =========================================================
   UPDATE DASHBOARD
   ========================================================= */

function updateDashboard(data) {

    console.log(
        "Updating Sales Analytics:",
        data
    );


    /* =====================================================
       KPI CARDS
       ===================================================== */

    const totalSales =
        document.getElementById("totalSales");

    const totalOrders =
        document.getElementById("totalOrders");

    const averageSale =
        document.getElementById("averageSale");


    if (totalSales) {

        totalSales.textContent =
            formatCurrency(
                data.totalSales
            );

    }


    if (totalOrders) {

        totalOrders.textContent =
            data.totalOrders;

    }


    if (averageSale) {

        averageSale.textContent =
            formatCurrency(
                data.averageSale
            );

    }


    /* =====================================================
       DAILY SALES CHART
       ===================================================== */

    createDailySalesChart(
        data.dailySales || []
    );


    /* =====================================================
       PAYMENT CHART
       ===================================================== */

    createPaymentChart(
        data.paymentSales || []
    );


    /* =====================================================
       MONTHLY SALES CHART
       ===================================================== */

    createMonthlySalesChart(
        data.monthlySales || []
    );


    /* =====================================================
       STATUS TABLE
       ===================================================== */

    updateStatusTable(
        data.statusSales || []
    );


    /* =====================================================
       LAST UPDATED
       ===================================================== */

    const lastUpdated =
        document.getElementById("lastUpdated");


    if (lastUpdated) {

        lastUpdated.textContent =
            "Last updated: " +
            new Date().toLocaleString();

    }

}


/* =========================================================
   DAILY SALES CHART
   ========================================================= */

function createDailySalesChart(data) {

    const canvas =
        document.getElementById(
            "dailySalesChart"
        );


    if (!canvas) {
        return;
    }


    const labels =
        data.map(
            item => item.date
        );


    const values =
        data.map(
            item => Number(item.amount)
        );


    if (dailyChart) {

        dailyChart.destroy();

    }


    dailyChart =
        new Chart(
            canvas,
            {

                type: "line",

                data: {

                    labels: labels,

                    datasets: [
                        {
                            label: "Daily Sales",

                            data: values,

                            tension: 0.3,

                            fill: false
                        }
                    ]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false,

                    plugins: {

                        legend: {
                            display: true
                        }

                    },

                    scales: {

                        y: {

                            beginAtZero: true

                        }

                    }

                }

            }
        );

}


/* =========================================================
   PAYMENT METHOD CHART
   ========================================================= */

function createPaymentChart(data) {

    const canvas =
        document.getElementById(
            "paymentChart"
        );


    if (!canvas) {
        return;
    }


    const labels =
        data.map(
            item => item.method
        );


    const values =
        data.map(
            item => Number(item.amount)
        );


    if (paymentChart) {

        paymentChart.destroy();

    }


    paymentChart =
        new Chart(
            canvas,
            {

                type: "doughnut",

                data: {

                    labels: labels,

                    datasets: [
                        {
                            label: "Payment Method",

                            data: values
                        }
                    ]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false

                }

            }
        );

}


/* =========================================================
   MONTHLY SALES CHART
   ========================================================= */

function createMonthlySalesChart(data) {

    const canvas =
        document.getElementById(
            "monthlySalesChart"
        );


    if (!canvas) {
        return;
    }


    const labels =
        data.map(
            item => item.month
        );


    const values =
        data.map(
            item => Number(item.amount)
        );


    if (monthlyChart) {

        monthlyChart.destroy();

    }


    monthlyChart =
        new Chart(
            canvas,
            {

                type: "bar",

                data: {

                    labels: labels,

                    datasets: [
                        {
                            label: "Monthly Sales",

                            data: values
                        }
                    ]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false,

                    scales: {

                        y: {

                            beginAtZero: true

                        }

                    }

                }

            }
        );

}


/* =========================================================
   STATUS TABLE
   ========================================================= */

function updateStatusTable(data) {

    const tableBody =
        document.getElementById(
            "statusTableBody"
        );


    if (!tableBody) {
        return;
    }


    tableBody.innerHTML = "";


    data.forEach(function (item) {

        const row =
            document.createElement("tr");


        const statusCell =
            document.createElement("td");

        const ordersCell =
            document.createElement("td");

        const amountCell =
            document.createElement("td");


        statusCell.textContent =
            item.status;

        ordersCell.textContent =
            item.orders;

        amountCell.textContent =
            formatCurrency(
                item.amount
            );


        row.appendChild(statusCell);

        row.appendChild(ordersCell);

        row.appendChild(amountCell);


        tableBody.appendChild(row);

    });

}


/* =========================================================
   CURRENCY FORMAT
   ========================================================= */

function formatCurrency(value) {

    const amount =
        Number(value) || 0;


    return "₹" +
        amount.toLocaleString(
            "en-IN",
            {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }
        );

}