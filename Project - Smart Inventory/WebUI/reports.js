
/* =========================================================
   SMART INVENTORY - REPORTS MODULE
   ========================================================= */


/* =========================================================
   GLOBAL DATA
   ========================================================= */

let reportData = [];
let currentReportType = "sales";


/* =========================================================
   PAGE INITIALIZATION
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {
    setDefaultDates();
    setupEvents();
    updateReportUI();
    loadReports();
});


/* =========================================================
   DOM HELPER
   ========================================================= */

function getElement(id) {
    return document.getElementById(id);
}


/* =========================================================
   EVENTS
   ========================================================= */

function setupEvents() {
    const reportType = getElement("reportType");
    const generateBtn = getElement("generateBtn");
    const printBtn = getElement("printBtn");
    const searchInput = getElement("searchInput");

    if (reportType) {
        reportType.addEventListener("change", function () {
            currentReportType = this.value;
            updateReportUI();
            loadReports();
        });
    }

    if (generateBtn) {
        generateBtn.addEventListener("click", loadReports);
    }

    if (printBtn) {
        printBtn.addEventListener("click", function () {
            window.print();
        });
    }

    if (searchInput) {
        searchInput.addEventListener("input", renderFilteredReport);
    }
}


/* =========================================================
   DEFAULT DATES
   ========================================================= */

function setDefaultDates() {
    const fromDate = getElement("fromDate");
    const toDate = getElement("toDate");

    const today = new Date();

    const todayString = [
        today.getFullYear(),
        String(today.getMonth() + 1).padStart(2, "0"),
        String(today.getDate()).padStart(2, "0")
    ].join("-");

    const firstDay = new Date(
        today.getFullYear(),
        today.getMonth(),
        1
    );

    const firstDayString = [
        firstDay.getFullYear(),
        String(firstDay.getMonth() + 1).padStart(2, "0"),
        String(firstDay.getDate()).padStart(2, "0")
    ].join("-");

    if (fromDate) fromDate.value = firstDayString;
    if (toDate) toDate.value = todayString;
}


/* =========================================================
   UPDATE REPORT UI
   ========================================================= */

function updateReportUI() {
    const dateControls = getElement("dateControls");
    const title = getElement("reportTitle");

    const reportTitles = {
        "sales": "Sales Report",
        "inventory": "Inventory Report",
        "low-stock": "Low Stock Report",
        "product-performance": "Product Performance"
    };

    if (dateControls) {
        dateControls.style.display =
            currentReportType === "sales" ? "flex" : "none";
    }

    if (title) {
        title.textContent =
            reportTitles[currentReportType] || "Reports";
    }
}


/* =========================================================
   LOAD REPORTS
   ========================================================= */

function loadReports() {
    const status = getElement("reportStatus");
    const reportType = getElement("reportType");
    const fromDate = getElement("fromDate");
    const toDate = getElement("toDate");

    if (reportType) {
        currentReportType = reportType.value;
    }

    if (status) {
        status.textContent = "Generating report...";
    }

    // Validate date range for Sales Report.
    if (currentReportType === "sales" && fromDate && toDate) {
        if (
            fromDate.value &&
            toDate.value &&
            fromDate.value > toDate.value
        ) {
            if (status) {
                status.textContent =
                    "From date cannot be later than To date.";
            }
            return;
        }
    }

    const request = {
        action: "loadReports",
        reportType: currentReportType,
        fromDate: fromDate ? fromDate.value : "",
        toDate: toDate ? toDate.value : ""
    };

    console.log("[Reports] Sending request:", request);

    // Reports page is inside the main page iframe.
    if (
        window.parent &&
        window.parent !== window
    ) {
        window.parent.postMessage(request, "*");
        return;
    }

    // Fallback when running directly inside WebView2.
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.postMessage(request);
        return;
    }

    if (status) {
        status.textContent =
            "Unable to communicate with the application.";
    }
}


/* =========================================================
   RECEIVE MESSAGES FROM MAIN.JS
   ========================================================= */

window.addEventListener("message", function (event) {
    const message = event.data;

    if (!message || typeof message !== "object") {
        return;
    }

    console.log("[Reports] Message received:", message);

    if (message.type === "reports") {
        reportData = Array.isArray(message.data)
            ? message.data
            : [];

        if (message.reportType) {
            currentReportType = message.reportType;

            const reportType = getElement("reportType");

            if (reportType) {
                reportType.value = currentReportType;
            }
        }

        updateReportUI();
        updateSummary(message.summary);
        renderFilteredReport();

        const status = getElement("reportStatus");

        if (status) {
            status.textContent =
                `${reportData.length} record(s) loaded.`;
        }

        return;
    }

    if (message.type === "reportsError") {
        reportData = [];

        updateSummary(null);
        renderTable([]);

        const status = getElement("reportStatus");

        if (status) {
            status.textContent =
                message.message || "Unable to generate report.";
        }

        console.error("[Reports] Backend error:", message.message);
    }
});


/* =========================================================
   SUMMARY - OPTIONAL ELEMENTS
   ========================================================= */

function updateSummary(summary) {
    const totalRecords = getElement("totalRecords");
    const totalUnits = getElement("totalUnits");
    const totalAmount = getElement("totalAmount");
    const totalAlerts = getElement("totalAlerts");

    // Summary cards are optional in reports.html.
    if (
        !totalRecords ||
        !totalUnits ||
        !totalAmount ||
        !totalAlerts
    ) {
        return;
    }

    if (!summary) {
        totalRecords.textContent = "0";
        totalUnits.textContent = "0";
        totalAmount.textContent = formatCurrency(0);
        totalAlerts.textContent = "0";
        return;
    }

    totalRecords.textContent =
        formatNumber(summary.totalRecords);

    totalUnits.textContent =
        formatNumber(summary.totalUnits);

    totalAmount.textContent =
        formatCurrency(summary.totalAmount);

    totalAlerts.textContent =
        formatNumber(summary.totalAlerts);
}


/* =========================================================
   SEARCH AND FILTER
   ========================================================= */

function renderFilteredReport() {
    const searchInput = getElement("searchInput");

    const search = searchInput
        ? searchInput.value.trim().toLowerCase()
        : "";

    if (!search) {
        renderTable(reportData);
        return;
    }

    const filtered = reportData.filter(function (item) {
        return Object.values(item).some(function (value) {
            return String(value ?? "")
                .toLowerCase()
                .includes(search);
        });
    });

    renderTable(filtered);
}


/* =========================================================
   TABLE RENDERER
   ========================================================= */

function renderTable(data) {
    const head = getElement("reportTableHead");
    const body = getElement("reportTableBody");

    if (!head || !body) {
        console.error(
            "[Reports] Table elements are missing from reports.html."
        );
        return;
    }

    const rows = Array.isArray(data) ? data : [];

    /* -----------------------------------------------------
       SALES REPORT
       ----------------------------------------------------- */

    if (currentReportType === "sales") {
        head.innerHTML = `
            <tr>
                <th>S.No</th>
                <th>Product</th>
                <th>Quantity</th>
                <th>Selling Price</th>
                <th>Total</th>
                <th>Date</th>
            </tr>
        `;

        if (rows.length === 0) {
            showEmpty(body, 6);
            return;
        }

        body.innerHTML = rows.map(function (item, index) {
            return `
                <tr>
                    <td>${index + 1}</td>
                    <td>${safe(item.ProductName)}</td>
                    <td>${formatNumber(item.Quantity)}</td>
                    <td>${formatCurrency(item.SellingPrice)}</td>
                    <td>${formatCurrency(item.TotalAmount)}</td>
                    <td>${safe(item.SaleDate)}</td>
                </tr>
            `;
        }).join("");

        return;
    }

    /* -----------------------------------------------------
       INVENTORY REPORT
       ----------------------------------------------------- */

    if (currentReportType === "inventory") {
        head.innerHTML = `
            <tr>
                <th>Product Code</th>
                <th>Product Name</th>
                <th>Category</th>
                <th>Current Stock</th>
                <th>Minimum Stock</th>
                <th>Status</th>
                <th>Purchase Price</th>
                <th>Selling Price</th>
                <th>Stock Value</th>
            </tr>
        `;

        if (rows.length === 0) {
            showEmpty(body, 9);
            return;
        }

        body.innerHTML = rows.map(function (item) {
            return `
                <tr>
                    <td>${safe(item.ProductCode)}</td>
                    <td>${safe(item.ProductName)}</td>
                    <td>${safe(item.CategoryName)}</td>
                    <td>${formatNumber(item.StockQuantity)}</td>
                    <td>${formatNumber(item.MinimumStock)}</td>
                    <td>${getStockStatus(
                item.StockQuantity,
                item.MinimumStock
            )}</td>
                    <td>${formatCurrency(item.PurchasePrice)}</td>
                    <td>${formatCurrency(item.SellingPrice)}</td>
                    <td>${formatCurrency(item.StockValue)}</td>
                </tr>
            `;
        }).join("");

        return;
    }

    /* -----------------------------------------------------
       LOW STOCK REPORT
       ----------------------------------------------------- */

    if (currentReportType === "low-stock") {
        head.innerHTML = `
            <tr>
                <th>Product Code</th>
                <th>Product Name</th>
                <th>Category</th>
                <th>Current Stock</th>
                <th>Minimum Stock</th>
                <th>Shortage</th>
                <th>Status</th>
            </tr>
        `;

        if (rows.length === 0) {
            showEmpty(body, 7);
            return;
        }

        body.innerHTML = rows.map(function (item) {
            const shortage = Math.max(
                Number(item.MinimumStock || 0) -
                Number(item.StockQuantity || 0),
                0
            );

            return `
                <tr>
                    <td>${safe(item.ProductCode)}</td>
                    <td>${safe(item.ProductName)}</td>
                    <td>${safe(item.CategoryName)}</td>
                    <td>${formatNumber(item.StockQuantity)}</td>
                    <td>${formatNumber(item.MinimumStock)}</td>
                    <td>${formatNumber(shortage)}</td>
                    <td>${getStockStatus(
                item.StockQuantity,
                item.MinimumStock
            )}</td>
                </tr>
            `;
        }).join("");

        return;
    }

    /* -----------------------------------------------------
       PRODUCT PERFORMANCE REPORT
       ----------------------------------------------------- */

    if (currentReportType === "product-performance") {
        head.innerHTML = `
            <tr>
                <th>Product Code</th>
                <th>Product Name</th>
                <th>Category</th>
                <th>Current Stock</th>
                <th>Units Sold</th>
                <th>Selling Price</th>
                <th>Total Sales</th>
                <th>Performance</th>
            </tr>
        `;

        if (rows.length === 0) {
            showEmpty(body, 8);
            return;
        }

        body.innerHTML = rows.map(function (item) {
            return `
                <tr>
                    <td>${safe(item.ProductCode)}</td>
                    <td>${safe(item.ProductName)}</td>
                    <td>${safe(item.CategoryName)}</td>
                    <td>${formatNumber(item.StockQuantity)}</td>
                    <td>${formatNumber(item.UnitsSold)}</td>
                    <td>${formatCurrency(item.SellingPrice)}</td>
                    <td>${formatCurrency(item.TotalSales)}</td>
                    <td>${getPerformanceStatus(item.Performance)}</td>
                </tr>
            `;
        }).join("");

        return;
    }

    head.innerHTML = "";
    showEmpty(body, 1, "Unknown report type.");
}


/* =========================================================
   EMPTY TABLE STATE
   ========================================================= */

function showEmpty(body, columnCount, message) {
    body.innerHTML = `
        <tr>
            <td colspan="${columnCount}" class="empty-state">
                ${safe(message || "No report data available.")}
            </td>
        </tr>
    `;
}


/* =========================================================
   STOCK STATUS
   ========================================================= */

function getStockStatus(stock, minimum) {
    stock = Number(stock || 0);
    minimum = Number(minimum || 0);

    if (stock <= 0) {
        return `
            <span class="status-badge status-out-stock">
                Out of Stock
            </span>
        `;
    }

    if (stock <= minimum) {
        return `
            <span class="status-badge status-low-stock">
                Low Stock
            </span>
        `;
    }

    return `
        <span class="status-badge status-in-stock">
            In Stock
        </span>
    `;
}


/* =========================================================
   PRODUCT PERFORMANCE STATUS
   ========================================================= */

function getPerformanceStatus(performance) {
    const value = String(performance || "never").toLowerCase();

    if (value === "top") {
        return `
            <span class="status-badge status-top">Top</span>
        `;
    }

    if (value === "medium") {
        return `
            <span class="status-badge status-medium">Medium</span>
        `;
    }

    if (value === "low") {
        return `
            <span class="status-badge status-low">Low</span>
        `;
    }

    return `
        <span class="status-badge status-never">
            Never Sold
        </span>
    `;
}


/* =========================================================
   NUMBER FORMATTING
   ========================================================= */

function formatNumber(value) {
    const number = Number(value ?? 0);

    return number.toLocaleString("en-IN");
}


/* =========================================================
   CURRENCY FORMATTING
   ========================================================= */

function formatCurrency(value) {
    const number = Number(value ?? 0);

    return "₹" + number.toLocaleString("en-IN", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });
}


/* =========================================================
   SAFE HTML OUTPUT
   ========================================================= */

function safe(value) {
    if (value === null || value === undefined) {
        return "";
    }

    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}
