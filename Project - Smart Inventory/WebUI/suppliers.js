/* =========================================================
   SALES NOVA - SUPPLIERS
   ========================================================= */

"use strict";

let suppliers = [];

/* =========================================================
   WEBVIEW2 / IFRAME BRIDGE
   ========================================================= */

function sendToVB(data) {

    if (window.parent && window.parent !== window) {
        window.parent.postMessage(data, "*");
    }
    else if (window.chrome?.webview) {
        window.chrome.webview.postMessage(data);
    }

}

/* =========================================================
   LOAD SUPPLIERS
   ========================================================= */

function loadSuppliers() {

    sendToVB({
        action: "loadSuppliers"
    });

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

    // Clear any previous reset timer.
    if (messageBox._resetTimer) {
        clearTimeout(messageBox._resetTimer);
    }

    messageBox._resetTimer = setTimeout(() => {
        messageBox.className = "message";
        messageBox.textContent = "";
    }, 4000);

}

/* =========================================================
   RENDER SUPPLIERS
   ========================================================= */

function renderSuppliers(data) {

    suppliers = Array.isArray(data) ? data : [];

    const tableBody =
        document.getElementById("supplierTableBody");

    const count =
        document.getElementById("supplierCount");

    if (!tableBody) {
        console.error("Supplier table body not found.");
        return;
    }

    if (count) {
        count.textContent =
            suppliers.length +
            (suppliers.length === 1
                ? " Supplier"
                : " Suppliers");
    }

    if (suppliers.length === 0) {

        tableBody.innerHTML = `
        <tr>
            <td colspan="5" class="empty-state">
                No suppliers found.
            </td>
        </tr>
    `;

        return;
    }

    tableBody.innerHTML = suppliers.map((supplier, index) => `
    <tr data-supplier-id="${escapeHtml(supplier.SupplierID)}">
        <td>${index + 1}</td>
        <td><strong>${escapeHtml(supplier.SupplierName)}</strong></td>
        <td>${escapeHtml(supplier.Phone || "-")}</td>
        <td>${escapeHtml(supplier.Email || "-")}</td>
        <td>${escapeHtml(supplier.Address || "-")}</td>
    </tr>
`).join("");

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
   OPEN SUPPLIER FORM
   ========================================================= */

function openSupplierForm() {

    const formCard =
        document.getElementById("supplierFormCard");

    const supplierName =
        document.getElementById("supplierName");

    if (formCard) {
        formCard.classList.remove("hidden");
    }

    if (supplierName) {
        supplierName.focus();
    }

}

/* =========================================================
   CLOSE SUPPLIER FORM
   ========================================================= */

function closeSupplierForm() {

    const formCard =
        document.getElementById("supplierFormCard");

    const form =
        document.getElementById("supplierForm");

    if (formCard) {
        formCard.classList.add("hidden");
    }

    if (form) {
        form.reset();
    }

}

/* =========================================================
   ADD SUPPLIER - VALIDATE ALL REQUIRED FIELDS
   ========================================================= */

function addSupplier(event) {

    event.preventDefault();

    const supplierName =
        document.getElementById("supplierName")?.value.trim() || "";

    const phone =
        document.getElementById("phone")?.value.trim() || "";

    const email =
        document.getElementById("email")?.value.trim() || "";

    const address =
        document.getElementById("address")?.value.trim() || "";

    // Validate required fields
    if (!supplierName || !phone || !email || !address) {

        showMessage(
            "Please fill in all supplier fields.",
            "error"
        );

        return;
    }

    // Validate phone number: 10 digits
    if (!/^[0-9]{10}$/.test(phone)) {

        showMessage(
            "Please enter a valid 10-digit phone number.",
            "error"
        );

        document.getElementById("phone")?.focus();

        return;
    }

    // Validate email format
    const emailPattern =
        /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(email)) {

        showMessage(
            "Please enter a valid email address.",
            "error"
        );

        document.getElementById("email")?.focus();

        return;
    }

    // Send data only after successful validation
    sendToVB({

        action: "addSupplier",

        SupplierName: supplierName,

        Phone: phone,

        Email: email,

        Address: address

    });

}

/* =========================================================
   RECEIVE MESSAGES FROM VB.NET
   ========================================================= */

window.addEventListener("message", function (event) {

    const data = event.data;

    if (!data || typeof data !== "object") {
        return;
    }

    switch (data.type) {

        case "suppliers":

            renderSuppliers(data.data);
            break;

        case "supplierAdded":

            showMessage(
                data.message || "Supplier added successfully.",
                "success"
            );

            closeSupplierForm();
            loadSuppliers();
            break;

        case "supplierError":

            showMessage(
                data.message || "Unable to add supplier.",
                "error"
            );

            break;
    }

});

/* =========================================================
   INITIALIZE EVENTS
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    document
        .getElementById("btnAddSupplier")
        ?.addEventListener("click", openSupplierForm);

    document
        .getElementById("btnCloseForm")
        ?.addEventListener("click", closeSupplierForm);

    document
        .getElementById("btnCancel")
        ?.addEventListener("click", closeSupplierForm);

    document
        .getElementById("supplierForm")
        ?.addEventListener("submit", addSupplier);

    loadSuppliers();

});