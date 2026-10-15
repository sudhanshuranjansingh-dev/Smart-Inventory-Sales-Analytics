
"use strict";

let allSalesHistory = [];

// ============================================
// LOAD SALES HISTORY
// ============================================

function loadSalesHistory() {
    const tbody = document.getElementById("salesHistoryTableBody");

    if (tbody) {
        tbody.innerHTML = `
            <tr>
                <td colspan="6" class="empty-message">
                    Loading sales history...
                </td>
            </tr>`;
    }

    console.log("Sending loadSales request to parent");

    window.parent.postMessage({
        action: "loadSales"
    }, "*");
}


// ============================================
// RECEIVE SALES DATA
// ============================================
function handleSalesMessage(response) {
    if (!response || !response.type) return;

    console.log("Sales History message received:", response);

    if (response.type === "sales") {
        allSalesHistory = Array.isArray(response.data)
            ? response.data
            : [];

        filterSales();
    } else if (response.type === "saleError") {
        showSalesMessage(
            "Unable to load sales history: " +
            (response.message || "Unknown error")
        );
    } else if (response.type === "sessionInvalid") {
        showSalesMessage("Your session has expired. Please log in again.");
    } else if (response.type === "accessDenied") {
        showSalesMessage("You do not have permission to view sales history.");
    }
}

// Listen for messages from the parent page.
window.addEventListener("message", function (event) {
    handleSalesMessage(event.data);
});

// Listen for direct messages from the WebView2 host.
if (window.chrome && window.chrome.webview) {
    window.chrome.webview.addEventListener("message", function (event) {
        handleSalesMessage(event.data);
    });
}

// ============================================
// FILTER SALES
// ============================================
function filterSales() {
    const searchInput = document.getElementById("searchSales");
    const dateInput = document.getElementById("saleDateFilter");

    if (!searchInput || !dateInput) {
        console.error("Sales filter elements were not found.");
        return;
    }

    const searchText = searchInput.value.trim().toLowerCase();
    const selectedDate = dateInput.value;

    const filteredSales = allSalesHistory.filter(sale => {
        const saleId = String(sale.SaleID ?? "").toLowerCase();
        const productName = String(sale.ProductName ?? "").toLowerCase();

        // SaleDate is formatted as yyyy-MM-dd HH:mm by VB.NET.
        const saleDate = String(sale.SaleDate ?? "").substring(0, 10);

        const matchesSearch =
            saleId.includes(searchText) ||
            productName.includes(searchText);

        const matchesDate =
            !selectedDate || saleDate === selectedDate;

        return matchesSearch && matchesDate;
    });

    renderSalesHistory(filteredSales);
}

// ============================================
// CLEAR FILTERS
// ============================================
function clearSalesFilter() {
    document.getElementById("searchSales").value = "";
    document.getElementById("saleDateFilter").value = "";

    renderSalesHistory(allSalesHistory);
}

// ============================================
// RENDER TABLE
// ============================================
function renderSalesHistory(sales) {
    updateSalesSummary(sales);

    const tbody = document.getElementById("salesHistoryTableBody");

    if (!tbody) return;

    if (!sales || sales.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="6" class="empty-message">
                    No sales records found.
                </td>
            </tr>`;
        return;
    }

    tbody.innerHTML = sales.map(sale => `
        <tr>
            <td>${escapeHtml(sale.SaleID)}</td>
            <td>${escapeHtml(sale.ProductName)}</td>
            <td>${Number(sale.Quantity ?? 0)}</td>
            <td>₹${Number(sale.SellingPrice ?? 0).toFixed(2)}</td>
            <td>₹${Number(sale.TotalAmount ?? 0).toFixed(2)}</td>
            <td>${escapeHtml(sale.SaleDate ?? "")}</td>
        </tr>
    `).join("");
}

// ============================================
// DISPLAY MESSAGE IN TABLE
// ============================================
function showSalesMessage(message) {
    const tbody = document.getElementById("salesHistoryTableBody");

    if (!tbody) return;

    tbody.innerHTML = `
        <tr>
            <td colspan="6" class="empty-message">
                ${escapeHtml(message)}
            </td>
        </tr>`;
}

// ============================================
// ESCAPE HTML
// ============================================
function escapeHtml(value) {
    return String(value ?? "").replace(/[&<>"']/g, character => ({
        "&": "&amp;",
        "<": "&lt;",
        ">": "&gt;",
        '"': "&quot;",
        "'": "&#39;"
    })[character]);
}

// ============================================
// INITIALIZE PAGE
// ============================================
document.addEventListener("DOMContentLoaded", function () {
    console.log("Sales History page initialized.");
    loadSalesHistory();
});

function updateSalesSummary(sales) {
    const totalSalesElement = document.getElementById("totalSales");
    const totalItemsElement = document.getElementById("totalItems");
    const totalRevenueElement = document.getElementById("totalRevenue");

    // Count unique sale transactions, not individual product rows.
    const totalSales = new Set(
        sales.map(sale => sale.SaleID)
    ).size;

    const totalItems = sales.reduce(
        (sum, sale) => sum + Number(sale.Quantity || 0),
        0
    );

    const totalRevenue = sales.reduce(
        (sum, sale) => sum + Number(sale.TotalAmount || 0),
        0
    );

    if (totalSalesElement) {
        totalSalesElement.textContent = totalSales;
    }

    if (totalItemsElement) {
        totalItemsElement.textContent = totalItems;
    }

    if (totalRevenueElement) {
        totalRevenueElement.textContent =
            "₹" + totalRevenue.toFixed(2);
    }
}