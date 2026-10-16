
/* =========================================================
   SMART INVENTORY - SALES ANALYTICS
   ========================================================= */

"use strict";

let dailyChart = null;
let monthlyChart = null;
let paymentChart = null;

let analyticsInitialized = false;


/* =========================================================
   PAGE INITIALIZATION
   ========================================================= */

document.addEventListener("DOMContentLoaded", initializeSalesAnalytics);

function initializeSalesAnalytics() {

    if (analyticsInitialized) return;
    analyticsInitialized = true;

    setDefaultDates();

    // Register response listener before sending the request.
    window.addEventListener("message", handleSalesAnalyticsMessage);

    const btnApply = document.getElementById("btnApply");
    const btnRefresh = document.getElementById("btnRefresh");

    if (btnApply) {
        btnApply.addEventListener("click", loadSalesAnalytics);
    }

    if (btnRefresh) {
        btnRefresh.addEventListener("click", loadSalesAnalytics);
    }

    // Load analytics when the page opens.
    loadSalesAnalytics();
}


/* =========================================================
   DEFAULT DATES
   ========================================================= */

function setDefaultDates() {

    const fromDate = document.getElementById("fromDate");
    const toDate = document.getElementById("toDate");

    if (!fromDate || !toDate) {
        console.error("Sales Analytics: Date inputs not found.");
        return;
    }

    const today = new Date();
    const previousDate = new Date();

    previousDate.setDate(previousDate.getDate() - 30);

    fromDate.value = formatDate(previousDate);
    toDate.value = formatDate(today);
}


/* =========================================================
   DATE FORMAT
   ========================================================= */

function formatDate(date) {

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;
}


/* =========================================================
   LOAD SALES ANALYTICS
   ========================================================= */

function loadSalesAnalytics() {

    const fromDateInput = document.getElementById("fromDate");
    const toDateInput = document.getElementById("toDate");

    if (!fromDateInput || !toDateInput) {
        console.error("Sales Analytics: Date inputs are missing.");
        return;
    }

    const fromDate = fromDateInput.value;
    const toDate = toDateInput.value;

    if (!fromDate || !toDate) {
        alert("Please select both the start date and end date.");
        return;
    }

    if (fromDate > toDate) {
        alert("Start date cannot be later than end date.");
        return;
    }

    if (typeof window.parent === "undefined" || window.parent === window) {
        console.error("Sales Analytics: Parent page is unavailable.");
        return;
    }

    const request = {
        action: "loadSalesAnalytics",
        fromDate: fromDate,
        toDate: toDate
    };

    console.log("Requesting Sales Analytics:", request);

    // Send through the parent page so main.js forwards it to VB.NET.
    window.parent.postMessage(request, "*");
}


/* =========================================================
   RECEIVE SALES ANALYTICS RESPONSE
   ========================================================= */

function handleSalesAnalyticsMessage(event) {

    let data = event.data;

    // Accept either an object or a JSON string.
    if (typeof data === "string") {
        try {
            data = JSON.parse(data);
        } catch (error) {
            return;
        }
    }

    if (!data || typeof data !== "object") {
        return;
    }

    console.log("Sales Analytics received:", data);

    // VB.NET sends type = "salesAnalyticsData".
    // Accept action as well for compatibility with older responses.
    const responseType = String(data.type || data.action || "");

    if (responseType === "salesAnalyticsData") {
        updateDashboard(data);
        return;
    }

    if (
        responseType === "salesAnalyticsError" ||
        responseType === "salesAnalyticsDataError"
    ) {
        console.error(
            "Sales Analytics backend error:",
            data.message || "Unknown error"
        );

        alert(
            "Unable to load Sales Analytics: " +
            (data.message || "Unknown error")
        );
    }
}


/* =========================================================
   UPDATE DASHBOARD
   ========================================================= */

function updateDashboard(data) {

    console.log("Updating Sales Analytics dashboard:", data);

    // KPI cards
    setText("totalSales", formatCurrency(data.totalSales));
    setText("totalOrders", Number(data.totalOrders) || 0);
    setText("averageSale", formatCurrency(data.averageSale));

    // Charts
    createDailySalesChart(
        Array.isArray(data.dailySales) ? data.dailySales : []
    );

    createPaymentChart(
        Array.isArray(data.paymentSales) ? data.paymentSales : []
    );

    createMonthlySalesChart(
        Array.isArray(data.monthlySales) ? data.monthlySales : []
    );

    // Status table
    updateStatusTable(
        Array.isArray(data.statusSales) ? data.statusSales : []
    );

    // Display the returned date range, if available.
    if (data.fromDate && data.toDate) {
        setText(
            "lastUpdated",
            `Showing ${data.fromDate} to ${data.toDate} · Updated ${new Date().toLocaleString()}`
        );
    } else {
        setText(
            "lastUpdated",
            "Last updated: " + new Date().toLocaleString()
        );
    }
}


/* =========================================================
   SAFE TEXT UPDATE
   ========================================================= */

function setText(elementId, value) {

    const element = document.getElementById(elementId);

    if (element) {
        element.textContent = value;
    }
}


/* =========================================================
   DAILY SALES CHART
   ========================================================= */

function createDailySalesChart(data) {

    const canvas = document.getElementById("dailySalesChart");

    if (!canvas) {
        console.warn("Daily sales chart canvas not found.");
        return;
    }

    if (typeof Chart === "undefined") {
        console.error("Chart.js is not loaded.");
        return;
    }

    if (dailyChart) {
        dailyChart.destroy();
        dailyChart = null;
    }

    const labels = data.map(item => item.date || "");
    const values = data.map(item => Number(item.amount) || 0);

    dailyChart = new Chart(canvas, {
        type: "line",

        data: {
            labels,
            datasets: [{
                label: "Daily Sales",
                data: values,
                tension: 0.3,
                fill: false
            }]
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
    });
}


/* =========================================================
   PAYMENT METHOD CHART
   ========================================================= */

function createPaymentChart(data) {

    const canvas = document.getElementById("paymentChart");

    if (!canvas) {
        console.warn("Payment chart canvas not found.");
        return;
    }

    if (typeof Chart === "undefined") {
        console.error("Chart.js is not loaded.");
        return;
    }

    if (paymentChart) {
        paymentChart.destroy();
        paymentChart = null;
    }

    const labels = data.map(item => item.method || "Unknown");
    const values = data.map(item => Number(item.amount) || 0);

    paymentChart = new Chart(canvas, {
        type: "doughnut",

        data: {
            labels,
            datasets: [{
                label: "Sales by Payment Method",
                data: values
            }]
        },

        options: {
            responsive: true,
            maintainAspectRatio: false
        }
    });
}


/* =========================================================
   MONTHLY SALES CHART
   ========================================================= */

function createMonthlySalesChart(data) {

    const canvas = document.getElementById("monthlySalesChart");

    if (!canvas) {
        console.warn("Monthly sales chart canvas not found.");
        return;
    }

    if (typeof Chart === "undefined") {
        console.error("Chart.js is not loaded.");
        return;
    }

    if (monthlyChart) {
        monthlyChart.destroy();
        monthlyChart = null;
    }

    const labels = data.map(item => item.month || "");
    const values = data.map(item => Number(item.amount) || 0);

    monthlyChart = new Chart(canvas, {
        type: "bar",

        data: {
            labels,
            datasets: [{
                label: "Monthly Sales",
                data: values
            }]
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
    });
}


/* =========================================================
   SALES STATUS TABLE
   ========================================================= */

function updateStatusTable(data) {

    const tableBody = document.getElementById("statusTableBody");

    if (!tableBody) {
        console.warn("Sales status table body not found.");
        return;
    }

    tableBody.replaceChildren();

    if (data.length === 0) {
        const row = document.createElement("tr");
        const cell = document.createElement("td");

        cell.colSpan = 3;
        cell.textContent = "No sales status data for this date range.";

        row.appendChild(cell);
        tableBody.appendChild(row);
        return;
    }

    data.forEach(item => {

        const row = document.createElement("tr");

        const statusCell = document.createElement("td");
        const ordersCell = document.createElement("td");
        const amountCell = document.createElement("td");

        statusCell.textContent = item.status || "Unknown";
        ordersCell.textContent = Number(item.orders) || 0;
        amountCell.textContent = formatCurrency(item.amount);

        row.append(statusCell, ordersCell, amountCell);
        tableBody.appendChild(row);
    });
}


/* =========================================================
   CURRENCY FORMAT
   ========================================================= */

function formatCurrency(value) {

    const amount = Number(value) || 0;

    return "₹" + amount.toLocaleString("en-IN", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}
