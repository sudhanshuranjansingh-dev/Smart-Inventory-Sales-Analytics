/* =========================================================
   SALES NOVA - SUPPLIERS
   ========================================================= */

let suppliers = [];


/* =========================================================
   WEBVIEW2 BRIDGE
   ========================================================= */

function sendToVB(data) {

    if (window.parent && window.parent !== window) {

        window.parent.postMessage(data, "*");

    } else if (window.chrome && window.chrome.webview) {

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

function showMessage(message, type) {

    const messageBox = document.getElementById("message");

    messageBox.textContent = message;
    messageBox.className = "message " + type;

    setTimeout(() => {

        messageBox.className = "message";

    }, 4000);

}


/* =========================================================
   RENDER SUPPLIERS
   ========================================================= */

function renderSuppliers(data) {

    suppliers = data || [];

    const tableBody =
        document.getElementById("supplierTableBody");

    const count =
        document.getElementById("supplierCount");

    count.textContent =
        suppliers.length +
        (suppliers.length === 1 ? " Supplier" : " Suppliers");


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


    tableBody.innerHTML = suppliers.map(supplier => {

        return `
            <tr>

                <td>${escapeHtml(supplier.SupplierID)}</td>

                <td>
                    <strong>
                        ${escapeHtml(supplier.SupplierName)}
                    </strong>
                </td>

                <td>
                    ${escapeHtml(supplier.Phone || "-")}
                </td>

                <td>
                    ${escapeHtml(supplier.Email || "-")}
                </td>

                <td>
                    ${escapeHtml(supplier.Address || "-")}
                </td>

            </tr>
        `;

    }).join("");

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
   OPEN FORM
   ========================================================= */

function openSupplierForm() {

    document
        .getElementById("supplierFormCard")
        .classList.remove("hidden");

    document
        .getElementById("supplierName")
        .focus();

}


/* =========================================================
   CLOSE FORM
   ========================================================= */

function closeSupplierForm() {

    document
        .getElementById("supplierFormCard")
        .classList.add("hidden");

    document
        .getElementById("supplierForm")
        .reset();

}


/* =========================================================
   ADD SUPPLIER
   ========================================================= */

function addSupplier(event) {

    event.preventDefault();


    const supplierName =
        document.getElementById("supplierName").value.trim();

    const phone =
        document.getElementById("phone").value.trim();

    const email =
        document.getElementById("email").value.trim();

    const address =
        document.getElementById("address").value.trim();


    if (!supplierName) {

        showMessage(
            "Supplier name is required.",
            "error"
        );

        return;
    }


    sendToVB({

        action: "addSupplier",

        SupplierName: supplierName,

        Phone: phone,

        Email: email,

        Address: address

    });

}


/* =========================================================
   RECEIVE MESSAGE FROM VB.NET
   ========================================================= */

window.addEventListener("message", function (event) {

    const data = event.data;

    if (!data) {
        return;
    }


    /* SUPPLIER LIST */

    if (data.type === "suppliers") {

        renderSuppliers(data.data);

    }


    /* SUPPLIER ADDED */

    else if (data.type === "supplierAdded") {

        showMessage(
            data.message || "Supplier added successfully.",
            "success"
        );

        closeSupplierForm();

        loadSuppliers();

    }


    /* ERROR */

    else if (data.type === "supplierError") {

        showMessage(
            data.message || "Unable to add supplier.",
            "error"
        );

    }

});


/* =========================================================
   EVENTS
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {


    document
        .getElementById("btnAddSupplier")
        .addEventListener("click", openSupplierForm);


    document
        .getElementById("btnCloseForm")
        .addEventListener("click", closeSupplierForm);


    document
        .getElementById("btnCancel")
        .addEventListener("click", closeSupplierForm);


    document
        .getElementById("supplierForm")
        .addEventListener("submit", addSupplier);


    /* Load existing suppliers */

    loadSuppliers();

});