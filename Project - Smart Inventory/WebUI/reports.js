/* =========================================================
   SMART INVENTORY
   REPORTS MODULE
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
   EVENTS
   ========================================================= */

function setupEvents() {

    document
        .getElementById("reportType")
        .addEventListener("change", function () {

            currentReportType = this.value;

            updateReportUI();

        });


    document
        .getElementById("generateBtn")
        .addEventListener("click", function () {

            loadReports();

        });


    document
        .getElementById("refreshBtn")
        .addEventListener("click", function () {

            loadReports();

        });


    document
        .getElementById("printBtn")
        .addEventListener("click", function () {

            window.print();

        });


    document
        .getElementById("searchInput")
        .addEventListener("input", function () {

            renderFilteredReport();

        });

}


/* =========================================================
   DEFAULT DATES
   ========================================================= */

function setDefaultDates() {

    const today = new Date();

    const year = today.getFullYear();

    const month = String(
        today.getMonth() + 1
    ).padStart(2, "0");

    const day = String(
        today.getDate()
    ).padStart(2, "0");


    const todayString =
        `${year}-${month}-${day}`;


    const firstDay = new Date(
        today.getFullYear(),
        today.getMonth(),
        1
    );


    const firstYear =
        firstDay.getFullYear();

    const firstMonth =
        String(
            firstDay.getMonth() + 1
        ).padStart(2, "0");

    const firstDayNumber =
        String(
            firstDay.getDate()
        ).padStart(2, "0");


    const firstDayString =
        `${firstYear}-${firstMonth}-${firstDayNumber}`;


    document.getElementById(
        "fromDate"
    ).value = firstDayString;


    document.getElementById(
        "toDate"
    ).value = todayString;
}


/* =========================================================
   UPDATE UI
   ========================================================= */

function updateReportUI() {

    const dateControls =
        document.getElementById("dateControls");

    const title =
        document.getElementById("reportTitle");

    const subtitle =
        document.getElementById("reportSubtitle");


    if (currentReportType === "sales") {

        dateControls.style.display = "flex";

        title.textContent = "Sales Report";

        subtitle.textContent =
            "View sales transactions and revenue";

    }

    else if (currentReportType === "inventory") {

        dateControls.style.display = "none";

        title.textContent = "Inventory Report";

        subtitle.textContent =
            "View current inventory and stock valuation";

    }

    else if (currentReportType === "low-stock") {

        dateControls.style.display = "none";

        title.textContent = "Low Stock Report";

        subtitle.textContent =
            "Identify products that require restocking";

    }

    else if (
        currentReportType ===
        "product-performance"
    ) {

        dateControls.style.display = "none";

        title.textContent =
            "Product Performance";

        subtitle.textContent =
            "Analyze product sales performance";

    }

}


/* =========================================================
   LOAD REPORTS
   ========================================================= */

function loadReports() {

    const status =
        document.getElementById("reportStatus");


    status.textContent =
        "Generating report...";


    const fromDate =
        document.getElementById(
            "fromDate"
        ).value;


    const toDate =
        document.getElementById(
            "toDate"
        ).value;


    const request = {

        action: "loadReports",

        reportType:
            currentReportType,

        fromDate:
            fromDate,

        toDate:
            toDate

    };


    console.log(
        "Requesting report:",
        request
    );


    /*
     * IMPORTANT:
     * Reports is inside the iframe.
     * Therefore send the request to main.js.
     */

    if (
        window.parent &&
        window.parent !== window
    ) {

        window.parent.postMessage(
            request,
            "*"
        );

        return;
    }


    /*
     * Fallback for direct WebView2 usage.
     */

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            request
        );

        return;
    }


    status.textContent =
        "WebView2 communication is unavailable.";

}


/* =========================================================
   RECEIVE MESSAGE FROM MAIN.JS
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        const message =
            event.data;


        if (!message) {
            return;
        }


        console.log(
            "Reports received:",
            message
        );


        /* ---------------- REPORT DATA ---------------- */

        if (
            message.type ===
            "reports"
        ) {

            reportData =
                Array.isArray(
                    message.data
                )
                    ? message.data
                    : [];


            if (
                message.reportType
            ) {

                currentReportType =
                    message.reportType;

                document.getElementById(
                    "reportType"
                ).value =
                    currentReportType;

            }


            updateReportUI();


            updateSummary(
                message.summary
            );


            renderFilteredReport();


            document.getElementById(
                "reportStatus"
            ).textContent =
                `${reportData.length} record(s) loaded.`;

        }


        /* ---------------- ERROR ---------------- */

        else if (
            message.type ===
            "reportsError"
        ) {

            reportData = [];

            updateSummary(null);

            renderTable([]);


            document.getElementById(
                "reportStatus"
            ).textContent =
                message.message ||
                "Unable to generate report.";

        }

    }
);


/* =========================================================
   SUMMARY
   ========================================================= */

function updateSummary(summary) {

    const totalRecords =
        document.getElementById(
            "totalRecords"
        );

    const totalUnits =
        document.getElementById(
            "totalUnits"
        );

    const totalAmount =
        document.getElementById(
            "totalAmount"
        );

    const totalAlerts =
        document.getElementById(
            "totalAlerts"
        );


    if (!summary) {

        totalRecords.textContent = "0";

        totalUnits.textContent = "0";

        totalAmount.textContent =
            "₹0.00";

        totalAlerts.textContent = "0";

        return;
    }


    totalRecords.textContent =
        formatNumber(
            summary.totalRecords || 0
        );


    totalUnits.textContent =
        formatNumber(
            summary.totalUnits || 0
        );


    totalAmount.textContent =
        formatCurrency(
            summary.totalAmount || 0
        );


    totalAlerts.textContent =
        formatNumber(
            summary.totalAlerts || 0
        );

}


/* =========================================================
   SEARCH
   ========================================================= */

function renderFilteredReport() {

    const search =
        document.getElementById(
            "searchInput"
        ).value
            .trim()
            .toLowerCase();


    if (!search) {

        renderTable(
            reportData
        );

        return;
    }


    const filtered =
        reportData.filter(
            function (item) {

                return Object
                    .values(item)
                    .some(
                        function (value) {

                            return String(
                                value ?? ""
                            )
                                .toLowerCase()
                                .includes(search);

                        }
                    );

            }
        );


    renderTable(filtered);

}


/* =========================================================
   TABLE RENDERER
   ========================================================= */

function renderTable(data) {

    const head =
        document.getElementById(
            "reportTableHead"
        );

    const body =
        document.getElementById(
            "reportTableBody"
        );


    if (
        currentReportType ===
        "sales"
    ) {

        head.innerHTML = `

            <tr>

                <th>Sale ID</th>

                <th>Product</th>

                <th>Quantity</th>

                <th>Selling Price</th>

                <th>Total</th>

                <th>Date</th>

            </tr>

        `;


        if (!data.length) {

            showEmpty(
                body,
                6
            );

            return;

        }


        body.innerHTML =
            data.map(
                function (item) {

                    return `

                        <tr>

                            <td>
                                ${safe(item.SaleID)}
                            </td>

                            <td>
                                ${safe(item.ProductName)}
                            </td>

                            <td>
                                ${formatNumber(
                        item.Quantity
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.SellingPrice
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.TotalAmount
                    )}
                            </td>

                            <td>
                                ${safe(item.SaleDate)}
                            </td>

                        </tr>

                    `;

                }
            ).join("");

    }


    /* =====================================================
       INVENTORY
       ===================================================== */

    else if (
        currentReportType ===
        "inventory"
    ) {

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


        if (!data.length) {

            showEmpty(
                body,
                9
            );

            return;

        }


        body.innerHTML =
            data.map(
                function (item) {

                    return `

                        <tr>

                            <td>
                                ${safe(
                        item.ProductCode
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.ProductName
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.CategoryName
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.StockQuantity
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.MinimumStock
                    )}
                            </td>

                            <td>
                                ${getStockStatus(
                        item.StockQuantity,
                        item.MinimumStock
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.PurchasePrice
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.SellingPrice
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.StockValue
                    )}
                            </td>

                        </tr>

                    `;

                }
            ).join("");

    }


    /* =====================================================
       LOW STOCK
       ===================================================== */

    else if (
        currentReportType ===
        "low-stock"
    ) {

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


        if (!data.length) {

            showEmpty(
                body,
                7
            );

            return;

        }


        body.innerHTML =
            data.map(
                function (item) {

                    const shortage =
                        Math.max(
                            Number(
                                item.MinimumStock || 0
                            ) -
                            Number(
                                item.StockQuantity || 0
                            ),
                            0
                        );


                    return `

                        <tr>

                            <td>
                                ${safe(
                        item.ProductCode
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.ProductName
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.CategoryName
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.StockQuantity
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.MinimumStock
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        shortage
                    )}
                            </td>

                            <td>
                                ${getStockStatus(
                        item.StockQuantity,
                        item.MinimumStock
                    )}
                            </td>

                        </tr>

                    `;

                }
            ).join("");

    }


    /* =====================================================
       PRODUCT PERFORMANCE
       ===================================================== */

    else if (
        currentReportType ===
        "product-performance"
    ) {

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


        if (!data.length) {

            showEmpty(
                body,
                8
            );

            return;

        }


        body.innerHTML =
            data.map(
                function (item) {

                    return `

                        <tr>

                            <td>
                                ${safe(
                        item.ProductCode
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.ProductName
                    )}
                            </td>

                            <td>
                                ${safe(
                        item.CategoryName
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.StockQuantity
                    )}
                            </td>

                            <td>
                                ${formatNumber(
                        item.UnitsSold
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.SellingPrice
                    )}
                            </td>

                            <td>
                                ${formatCurrency(
                        item.TotalSales
                    )}
                            </td>

                            <td>
                                ${getPerformanceStatus(
                        item.Performance
                    )}
                            </td>

                        </tr>

                    `;

                }
            ).join("");

    }

}


/* =========================================================
   EMPTY TABLE
   ========================================================= */

function showEmpty(
    body,
    columnCount
) {

    body.innerHTML = `

        <tr>

            <td
                colspan="${columnCount}"
                class="empty-state">

                No report data available.

            </td>

        </tr>

    `;

}


/* =========================================================
   STOCK STATUS
   ========================================================= */

function getStockStatus(
    stock,
    minimum
) {

    stock =
        Number(stock || 0);

    minimum =
        Number(minimum || 0);


    if (stock <= 0) {

        return `
            <span
                class="status-badge status-out-stock">
                Out of Stock
            </span>
        `;

    }


    if (stock <= minimum) {

        return `
            <span
                class="status-badge status-low-stock">
                Low Stock
            </span>
        `;

    }


    return `
        <span
            class="status-badge status-in-stock">
            In Stock
        </span>
    `;

}


/* =========================================================
   PERFORMANCE STATUS
   ========================================================= */

function getPerformanceStatus(
    performance
) {

    const value =
        String(
            performance || "never"
        ).toLowerCase();


    if (value === "top") {

        return `
            <span
                class="status-badge status-top">
                Top
            </span>
        `;

    }


    if (value === "medium") {

        return `
            <span
                class="status-badge status-medium">
                Medium
            </span>
        `;

    }


    if (value === "low") {

        return `
            <span
                class="status-badge status-low">
                Low
            </span>
        `;

    }


    return `
        <span
            class="status-badge status-never">
            Never Sold
        </span>
    `;

}


/* =========================================================
   NUMBER FORMAT
   ========================================================= */

function formatNumber(value) {

    const number =
        Number(value || 0);

    return number.toLocaleString(
        "en-IN"
    );

}


/* =========================================================
   CURRENCY FORMAT
   ========================================================= */

function formatCurrency(value) {

    const number =
        Number(value || 0);


    return "₹" +
        number.toLocaleString(
            "en-IN",
            {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }
        );

}


/* =========================================================
   SAFE HTML
   ========================================================= */

function safe(value) {

    if (
        value === null ||
        value === undefined
    ) {

        return "";

    }


    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");

}