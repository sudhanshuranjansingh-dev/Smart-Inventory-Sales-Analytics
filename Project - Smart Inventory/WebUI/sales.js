
/* =========================================================
   SALES NOVA - SALES
   ========================================================= */

"use strict";

let products = [];
let sales = [];

/* =========================================================
   WEBVIEW2 BRIDGE
   ========================================================= */

function sendToVB(data) {

    if (window.parent && window.parent !== window) {
        window.parent.postMessage(data, "*");
    } else if (window.chrome?.webview) {
        window.chrome.webview.postMessage(data);
    }

}

/* =========================================================
   LOAD PRODUCTS
   ========================================================= */

function loadProducts() {

    sendToVB({
        action: "loadProducts"
    });

}

/* =========================================================
   LOAD SALES
   ========================================================= */

function loadSales() {

    sendToVB({
        action: "loadSales"
    });

}

/* =========================================================
   PRODUCT SELECT CHANGE
   ========================================================= */

function updateProductInformation() {

    const productID =
        Number(document.getElementById("product").value || 0);

    const product = products.find(
        p => Number(p.ProductID) === productID
    );

    const priceInput =
        document.getElementById("sellingPrice");

    const stockInput =
        document.getElementById("availableStock");

    if (!product) {

        priceInput.value = "";
        stockInput.value = "";

        calculateTotal();
        return;

    }

    priceInput.value =
        Number(product.SellingPrice || 0).toFixed(2);

    stockInput.value =
        Number(product.StockQuantity || 0);

    calculateTotal();

}

/* =========================================================
   CALCULATE TOTAL
   ========================================================= */

function calculateTotal() {

    const price =
        Number(document.getElementById("sellingPrice").value || 0);

    const quantity =
        Number(document.getElementById("quantity").value || 0);

    const total = price * quantity;

    document.getElementById("totalAmount").textContent =
        "₹" + total.toLocaleString("en-IN", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });

}

/* =========================================================
   RENDER PRODUCTS
   ========================================================= */

function renderProducts(data) {

    products = Array.isArray(data) ? data : [];

    const select = document.getElementById("product");

    select.innerHTML = `
        <option value="">Select Product</option>
    `;

    products.forEach(function (product) {

        const option = document.createElement("option");

        option.value = product.ProductID;

        option.textContent =
            `${product.ProductName || ""} (${product.ProductCode || ""})`;

        select.appendChild(option);

    });

    updateProductInformation();

}

/* =========================================================
   ESCAPE HTML
   ========================================================= */

function escapeHtml(value) {

    if (value === null || value === undefined) {
        return "";
    }

    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");

}

/* =========================================================
   RENDER SALES
   Requires VB.NET to return:
   SaleID, ProductID, ProductName, CustomerName,
   CustomerPhone, Quantity, SellingPrice, TotalAmount,
   SaleDate
   ========================================================= */

function renderSales(data) {

    sales = Array.isArray(data) ? data : [];

    const tableBody =
        document.getElementById("salesTableBody");

    const count =
        document.getElementById("salesCount");

    count.textContent =
        `${sales.length} ${sales.length === 1 ? "Sale" : "Sales"}`;

    if (sales.length === 0) {

        tableBody.innerHTML = `
            <tr>
                <td colspan="9" class="empty-state">
                    No sales found.
                </td>
            </tr>
        `;

        return;

    }

    tableBody.innerHTML = sales.map(function (sale, index) {

        return `
            <tr>
                <td>${index + 1}</td>
                <td>${escapeHtml(sale.ProductCode ?? "-")}</td>
                <td>${escapeHtml(sale.ProductName ?? "-")}</td>
                <td>${escapeHtml(sale.CustomerName ?? "-")}</td>
                <td>${escapeHtml(sale.CustomerPhone ?? "-")}</td>
                <td>${escapeHtml(sale.Quantity ?? "-")}</td>
                <td>₹${Number(sale.SellingPrice || 0).toFixed(2)}</td>
                <td>₹${Number(sale.TotalAmount || 0).toFixed(2)}</td>
                <td>${escapeHtml(sale.SaleDate ?? "-")}</td>
            </tr>
        `;

    }).join("");

}

/* =========================================================
   SHOW MESSAGE
   ========================================================= */

function showMessage(message, type = "success") {

    const messageBox = document.getElementById("message");

    if (!messageBox) {
        console.log(message);
        return;
    }

    messageBox.textContent = message;
    messageBox.className = "message " + type;

    if (messageBox._resetTimer) {
        clearTimeout(messageBox._resetTimer);
    }

    messageBox._resetTimer = setTimeout(function () {
        messageBox.className = "message";
        messageBox.textContent = "";
    }, 4000);

}

/* =========================================================
   COMPLETE SALE
   ========================================================= */

function completeSale(event) {

    event.preventDefault();

    const customerName =
        document.getElementById("customerName").value.trim();

    const customerPhone =
        document.getElementById("customerPhone").value.trim();

    const productID =
        Number(document.getElementById("product").value || 0);

    const quantity =
        Number(document.getElementById("quantity").value || 0);

    const sellingPrice =
        Number(document.getElementById("sellingPrice").value || 0);

    const availableStock =
        Number(document.getElementById("availableStock").value || 0);

    // Validate customer information
    if (!customerName || !customerPhone) {

        showMessage(
            "Customer name and phone number are required.",
            "error"
        );

        return;

    }

    if (!/^[0-9]{10}$/.test(customerPhone)) {

        showMessage(
            "Enter a valid 10-digit customer phone number.",
            "error"
        );

        document.getElementById("customerPhone").focus();
        return;

    }

    if (productID <= 0) {

        showMessage("Please select a product.", "error");
        return;

    }

    if (!Number.isInteger(quantity) || quantity <= 0) {

        showMessage(
            "Quantity must be a positive whole number.",
            "error"
        );

        return;

    }

    if (quantity > availableStock) {

        showMessage("Insufficient stock.", "error");
        return;

    }

    if (!Number.isFinite(sellingPrice) || sellingPrice <= 0) {

        showMessage("Selling price is invalid.", "error");
        return;

    }

    if (!products.some(p => Number(p.ProductID) === productID)) {

        showMessage("Please select a valid product.", "error");
        return;

    }

    const confirmed = confirm(
        `Complete sale for ${customerName}?\n` +
        `Phone: ${customerPhone}\n` +
        `Total: ₹${(sellingPrice * quantity).toFixed(2)}`
    );

    if (!confirmed) {
        return;
    }

    sendToVB({

        action: "addSale",

        CustomerName: customerName,
        CustomerPhone: customerPhone,

        ProductID: productID,
        Quantity: quantity,
        SellingPrice: sellingPrice

    });

}

/* =========================================================
   RESET FORM
   ========================================================= */

function resetForm() {

    document.getElementById("saleForm").reset();

    document.getElementById("sellingPrice").value = "";
    document.getElementById("availableStock").value = "";

    calculateTotal();

}

/* =========================================================
   RECEIVE DATA FROM VB.NET
   ========================================================= */

window.addEventListener("message", function (event) {

    const data = event.data;

    if (!data || typeof data !== "object") {
        return;
    }

    console.log("Sales received:", data);

    switch (data.type) {

        case "products":
            renderProducts(data.data);
            break;

        case "sales":
            renderSales(data.data);
            break;

        case "saleAdded":

            showMessage(
                data.message || "Sale completed successfully.",
                "success"
            );

            resetForm();
            loadProducts();
            loadSales();
            break;

        case "saleError":

            showMessage(
                data.message || "Unable to complete sale.",
                "error"
            );

            break;

    }

});

/* =========================================================
   INITIALIZE
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    document.getElementById("product")
        .addEventListener("change", updateProductInformation);

    document.getElementById("quantity")
        .addEventListener("input", calculateTotal);

    document.getElementById("saleForm")
        .addEventListener("submit", completeSale);

    document.getElementById("btnClear")
        .addEventListener("click", function () {
            setTimeout(function () {
                calculateTotal();
                showMessage("", "");
            }, 0);
        });

    loadProducts();
    loadSales();

});
