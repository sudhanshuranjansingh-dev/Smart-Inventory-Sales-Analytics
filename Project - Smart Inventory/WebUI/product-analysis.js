
"use strict";

/* =========================================================
   SMART INVENTORY - PRODUCT ANALYSIS
   ========================================================= */

let analysisData = [];
let requestInProgress = false;

const ANALYSIS_RESPONSE_TYPE = "productAnalysis";
const ANALYSIS_ERROR_TYPE = "productAnalysisError";


/* =========================================================
   HELPERS
   ========================================================= */

function getElement(id) {
    return document.getElementById(id);
}

function formatMoney(value) {
    const amount = Number(value);

    return "₹" + (Number.isFinite(amount) ? amount : 0).toFixed(2);
}

function formatNumber(value) {
    const number = Number(value);

    return new Intl.NumberFormat("en-IN").format(
        Number.isFinite(number) ? number : 0
    );
}

function escapeHtml(value) {
    return String(value ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

function showMessage(message, isError = false) {
    console.log(
        isError ? "Product analysis error:" : "Product analysis:",
        message
    );

    // Use a page message element if the HTML contains one.
    const messageElement = getElement("analysisMessage");

    if (messageElement) {
        messageElement.textContent = message;
        messageElement.style.color = isError ? "#b42318" : "";
    }
}

function setLoading(isLoading) {
    requestInProgress = isLoading;

    const refreshButton = getElement("refreshAnalysis");

    if (refreshButton) {
        refreshButton.disabled = isLoading;
    }

    const tableBody = getElement("analysisTableBody");

    if (isLoading && tableBody) {
        tableBody.innerHTML = `
            <tr>
                <td colspan="8" class="empty-message">
                    Loading product analysis...
                </td>
            </tr>
        `;
    }
}


/* =========================================================
   REQUEST PRODUCT ANALYSIS
   ========================================================= */


/* =========================================================
   REQUEST PRODUCT ANALYSIS
   ========================================================= */

function loadProductAnalysis() {
    if (requestInProgress) {
        console.log("Product analysis request is already in progress.");
        return;
    }

    console.log("Requesting product analysis...");

    setLoading(true);
    showMessage("Loading product analysis...");

    const message = {
        action: "loadProductAnalysis"
    };

    try {
        // When running inside an iframe, route through main.js.
        if (window.parent && window.parent !== window) {
            window.parent.postMessage(message, "*");
            console.log(
                "Product analysis request sent through parent page."
            );
            return;
        }

        // Direct WebView2 route for top-level documents.
        if (window.chrome && window.chrome.webview) {
            window.chrome.webview.postMessage(message);
            console.log(
                "Product analysis request sent directly to WebView2."
            );
            return;
        }

        setLoading(false);

        showMessage(
            "The application communication bridge is unavailable.",
            true
        );

        renderEmptyState(
            "Unable to connect to the application."
        );

    } catch (error) {
        console.error(
            "Could not request product analysis:",
            error
        );

        setLoading(false);

        showMessage(
            "Could not request product analysis. Check the application logs.",
            true
        );

        renderEmptyState(
            "Unable to load product analysis."
        );
    }
}


/* =========================================================
   RECEIVE DATA FROM VB.NET / MAIN.JS
   ========================================================= */

function handleProductAnalysisMessage(event) {
    const data = event.data;

    if (!data || typeof data !== "object") {
        return;
    }

    console.log("Product analysis received:", data);

    /*
     * Ignore unrelated application messages.
     */
    if (
        data.type !== ANALYSIS_RESPONSE_TYPE &&
        data.type !== ANALYSIS_ERROR_TYPE &&
        data.type !== "webUIError" &&
        data.type !== "accessDenied" &&
        data.type !== "sessionInvalid"
    ) {
        return;
    }

    if (data.type === ANALYSIS_RESPONSE_TYPE) {
        setLoading(false);

        analysisData = Array.isArray(data.data)
            ? data.data
            : [];

        updateSummary();
        renderAnalysis();

        showMessage(
            analysisData.length > 0
                ? "Product analysis loaded successfully."
                : "No product analysis records were returned."
        );

        return;
    }

    if (
        data.type === ANALYSIS_ERROR_TYPE ||
        data.type === "webUIError" ||
        data.type === "accessDenied" ||
        data.type === "sessionInvalid"
    ) {
        setLoading(false);

        const message =
            data.message || "Unable to load product analysis.";

        showMessage(message, true);
        renderEmptyState(message);
    }
}


/* =========================================================
   UPDATE SUMMARY CARDS
   ========================================================= */

function updateSummary() {
    let productsSold = 0;
    let totalUnitsSold = 0;
    let totalSales = 0;

    analysisData.forEach(product => {
        const unitsSold = Number(product.UnitsSold) || 0;
        const sales = Number(product.TotalSales) || 0;

        if (unitsSold > 0) {
            productsSold++;
        }

        totalUnitsSold += unitsSold;
        totalSales += sales;
    });

    const totalProductsElement = getElement("totalProducts");
    const productsSoldElement = getElement("productsSold");
    const totalUnitsSoldElement = getElement("totalUnitsSold");
    const totalSalesElement = getElement("totalSales");

    if (totalProductsElement) {
        totalProductsElement.textContent =
            formatNumber(analysisData.length);
    }

    if (productsSoldElement) {
        productsSoldElement.textContent =
            formatNumber(productsSold);
    }

    if (totalUnitsSoldElement) {
        totalUnitsSoldElement.textContent =
            formatNumber(totalUnitsSold);
    }

    if (totalSalesElement) {
        totalSalesElement.textContent =
            formatMoney(totalSales);
    }
}


/* =========================================================
   FILTER AND RENDER PRODUCT ANALYSIS
   ========================================================= */

function renderAnalysis() {
    const tableBody = getElement("analysisTableBody");

    if (!tableBody) {
        console.error("Missing HTML element: analysisTableBody");
        return;
    }

    const searchInput = getElement("analysisSearch");
    const filterSelect = getElement("performanceFilter");

    const searchText = searchInput
        ? searchInput.value.trim().toLowerCase()
        : "";

    const filter = filterSelect
        ? filterSelect.value.toLowerCase()
        : "all";

    const filteredProducts = analysisData.filter(product => {
        const productCode = String(
            product.ProductCode ?? ""
        ).toLowerCase();

        const productName = String(
            product.ProductName ?? ""
        ).toLowerCase();

        const categoryName = String(
            product.CategoryName ?? ""
        ).toLowerCase();

        const performance = String(
            product.Performance ?? "never"
        ).toLowerCase();

        const matchesSearch =
            productCode.includes(searchText) ||
            productName.includes(searchText) ||
            categoryName.includes(searchText);

        const matchesFilter =
            filter === "all" || performance === filter;

        return matchesSearch && matchesFilter;
    });

    if (filteredProducts.length === 0) {
        renderEmptyState(
            analysisData.length === 0
                ? "No product analysis records available."
                : "No products match your search or performance filter."
        );

        return;
    }

    tableBody.innerHTML = filteredProducts.map(product => {
        const stock = Number(product.StockQuantity) || 0;
        const unitsSold = Number(product.UnitsSold) || 0;
        const sellingPrice = Number(product.SellingPrice) || 0;
        const totalSales = Number(product.TotalSales) || 0;

        const performance = String(
            product.Performance ?? "never"
        ).toLowerCase();

        let performanceText = "No Sales";
        let performanceClass = "never";

        switch (performance) {
            case "top":
                performanceText = "Top Performing";
                performanceClass = "top";
                break;

            case "medium":
                performanceText = "Medium";
                performanceClass = "medium";
                break;

            case "low":
                performanceText = "Low";
                performanceClass = "low";
                break;

            case "never":
                performanceText = "No Sales";
                performanceClass = "never";
                break;
        }

        return `
            <tr>
                <td>${escapeHtml(product.ProductCode || "-")}</td>
                <td>${escapeHtml(product.ProductName || "-")}</td>
                <td>${escapeHtml(product.CategoryName || "-")}</td>
                <td>${formatNumber(stock)}</td>
                <td>${formatNumber(unitsSold)}</td>
                <td>${formatMoney(sellingPrice)}</td>
                <td>${formatMoney(totalSales)}</td>
                <td>
                    <span class="performance-status ${performanceClass}">
                        ${performanceText}
                    </span>
                </td>
            </tr>
        `;
    }).join("");
}


/* =========================================================
   EMPTY / ERROR TABLE STATE
   ========================================================= */

function renderEmptyState(message) {
    const tableBody = getElement("analysisTableBody");

    if (!tableBody) {
        return;
    }

    const row = document.createElement("tr");
    const cell = document.createElement("td");

    cell.colSpan = 8;
    cell.className = "empty-message";
    cell.textContent = message;

    row.appendChild(cell);
    tableBody.replaceChildren(row);
}


/* =========================================================
   SEARCH AND PERFORMANCE FILTER
   ========================================================= */

function initializeFilters() {
    const searchInput = getElement("analysisSearch");
    const performanceFilter = getElement("performanceFilter");

    if (searchInput) {
        searchInput.addEventListener("input", renderAnalysis);
    }

    if (performanceFilter) {
        performanceFilter.addEventListener("change", renderAnalysis);
    }

    const refreshButton = getElement("refreshAnalysis");

    if (refreshButton) {
        refreshButton.addEventListener(
            "click",
            loadProductAnalysis
        );
    }
}


/* =========================================================
   INITIALIZATION
   ========================================================= */

function initializeProductAnalysis() {
    initializeFilters();

    /*
     * Register both listeners because the application may deliver
     * responses through WebView2 or the parent iframe.
     */
    if (window.chrome && window.chrome.webview) {
        window.chrome.webview.addEventListener(
            "message",
            handleProductAnalysisMessage
        );
    }

    window.addEventListener(
        "message",
        handleProductAnalysisMessage
    );

    loadProductAnalysis();
}

/*
 * Works whether this script executes before or after DOMContentLoaded.
 */
if (document.readyState === "loading") {
    document.addEventListener(
        "DOMContentLoaded",
        initializeProductAnalysis,
        { once: true }
    );
} else {
    initializeProductAnalysis();
}
