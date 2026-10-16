
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
                <td colspan="3" class="empty-message">
                    Loading sales history...
                </td>
            </tr>`;
    }

    window.parent.postMessage({
        action: "loadSalesHistory"
    }, "*");
}

// ============================================
// RECEIVE SALES DATA
// ============================================


function handleSalesMessage(response) {
    if (!response || !response.type) return;

    console.log("Sales History message received:", response);

    if (
        response.type === "salesHistory" ||
        response.type === "sales"
    ) {
        allSalesHistory = Array.isArray(response.data)
            ? response.data
            : [];

        updateSalesSummary(allSalesHistory);
        filterSales();

    } else if (
        response.type === "salesHistoryError" ||
        response.type === "salesError" ||
        response.type === "saleError"
    ) {
        showSalesMessage(
            "Unable to load sales history: " +
            (response.message || "Unknown error")
        );

    } else if (response.type === "sessionInvalid") {
        showSalesMessage(
            "Your session has expired. Please log in again."
        );

    } else if (response.type === "accessDenied") {
        showSalesMessage(
            "You do not have permission to view sales history."
        );
    }
}


// Receive messages from the parent page
window.addEventListener("message", function (event) {
    handleSalesMessage(event.data);
});

// Receive direct messages from WebView2
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
        const saleDate = String(sale.SaleDate ?? "").substring(0, 10);

        const matchesSearch = saleId.includes(searchText);
        const matchesDate = !selectedDate || saleDate === selectedDate;

        return matchesSearch && matchesDate;
    });

    renderSalesHistory(filteredSales);
}

// ============================================
// CLEAR FILTERS
// ============================================

function clearSalesFilter() {
    const searchInput = document.getElementById("searchSales");
    const dateInput = document.getElementById("saleDateFilter");

    if (searchInput) searchInput.value = "";
    if (dateInput) dateInput.value = "";

    renderSalesHistory(allSalesHistory);
}

// ============================================
// RENDER TRANSACTION TABLE
// ============================================


function renderSalesHistory(sales) {
    const tableBody = document.getElementById("salesHistoryTableBody");

    console.log("Sales data received:", sales);
    console.log("Is array:", Array.isArray(sales));
    console.log("Number of records:", sales?.length);

    if (!tableBody) {
        console.error("Sales history table body not found.");
        return;
    }

    if (!Array.isArray(sales) || sales.length === 0) {
        tableBody.innerHTML = `
            <tr>
                <td colspan="3" class="empty-message">
                    No sales records found.
                </td>
            </tr>`;
        return;
    }

    tableBody.innerHTML = sales.map(sale => `
        <tr>
            <td>${escapeHtml(sale.SaleID ?? "")}</td>
            <td>₹${Number(sale.TotalAmount ?? 0).toFixed(2)}</td>
            <td>${escapeHtml(sale.SaleDate ?? "")}</td>
        </tr>
    `).join("");
}


// ============================================
// SUMMARY CARDS
// ============================================

function updateSalesSummary(sales) {
    const totalSalesElement = document.getElementById("totalSales");
    const totalItemsElement = document.getElementById("totalItems");
    const totalRevenueElement = document.getElementById("totalRevenue");

    // One row now represents one sale transaction.
    const totalSales = sales.length;

    // Quantity is not included in the transaction-level query.
    // Leave this at zero until the backend supplies TotalItems.
    const totalItems = sales.reduce(
        (sum, sale) => sum + Number(sale.TotalItems ?? 0),
        0
    );

    const totalRevenue = sales.reduce(
        (sum, sale) => sum + Number(sale.TotalAmount ?? 0),
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

// ============================================
// DISPLAY MESSAGE IN TABLE
// ============================================

function showSalesMessage(message) {
    const tbody = document.getElementById("salesHistoryTableBody");

    if (!tbody) return;

    tbody.innerHTML = `
        <tr>
            <td colspan="3" class="empty-message">
                ${escapeHtml(message)}
            </td>
        </tr>`;
}

// ============================================
// ESCAPE HTML
// ============================================

function escapeHtml(value) {
    return String(value).replace(/[&<>"']/g, character => ({
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
