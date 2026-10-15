
/* =========================================================
   SMART INVENTORY
   PURCHASES MODULE
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    /* =====================================================
       DOM ELEMENTS
       ===================================================== */

    const supplierSelect = document.getElementById("supplierSelect");
    const productSelect = document.getElementById("productSelect");
    const quantityInput = document.getElementById("quantity");
    const purchasePriceInput = document.getElementById("purchasePrice");

    if (purchasePriceInput) {
        purchasePriceInput.readOnly = true;
    }

    const currentStockElement = document.getElementById("currentStock");
    const summaryQuantityElement = document.getElementById("summaryQuantity");
    const newStockElement = document.getElementById("newStock");
    const totalAmountElement = document.getElementById("totalAmount");

    const purchaseForm = document.getElementById("purchaseForm");
    const clearButton = document.getElementById("clearPurchaseBtn");
    const tableBody = document.getElementById("purchasesTableBody");

    let selectedSupplierID = "";
    let productRequestNumber = 0;
    let purchaseSubmitting = false;


    /* =====================================================
       DISPLAY HELPERS
       ===================================================== */

    function setDisplayValue(element, value) {
        if (!element) return;

        if (["INPUT", "SELECT", "TEXTAREA"].includes(element.tagName)) {
            element.value = value;
        } else {
            element.textContent = value;
        }
    }

    function getNumber(element) {
        if (!element) return 0;

        const value = Number(element.value);
        return Number.isFinite(value) ? value : 0;
    }

    function formatCurrency(value) {
        return "₹" + Number(value || 0).toLocaleString("en-IN", {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function escapeHtml(value) {
        if (value === null || value === undefined) return "";

        return String(value)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }


    /* =====================================================
       WEBVIEW2 / IFRAME BRIDGE
       ===================================================== */

    function sendToVB(data) {
        if (window.parent && window.parent !== window) {
            window.parent.postMessage(data, "*");
            return;
        }

        if (window.chrome?.webview) {
            window.chrome.webview.postMessage(data);
            return;
        }

        console.error("WebView2 communication is unavailable.");
    }


    /* =====================================================
       REQUEST DATA
       ===================================================== */

    function requestSuppliers() {
        sendToVB({ action: "loadSuppliers" });
    }

    function requestPurchases() {
        sendToVB({ action: "loadPurchases" });
    }

    function requestProductsBySupplier(supplierID) {
        // Invalidate any outstanding product response.
        const requestNumber = ++productRequestNumber;

        if (!productSelect) return;

        if (!supplierID) {
            productSelect.innerHTML =
                '<option value="">Select supplier first</option>';
            return;
        }

        productSelect.innerHTML =
            '<option value="">Loading products...</option>';

        productSelect.disabled = true;

        sendToVB({
            action: "loadProductsBySupplier",
            SupplierID: Number(supplierID),
            RequestNumber: requestNumber
        });
    }


    /* =====================================================
       RESET PURCHASE SUMMARY
       ===================================================== */

    function resetSummary() {
        if (purchasePriceInput) purchasePriceInput.value = "";

        setDisplayValue(currentStockElement, 0);
        setDisplayValue(summaryQuantityElement, 0);
        setDisplayValue(newStockElement, 0);
        setDisplayValue(totalAmountElement, formatCurrency(0));
    }


    /* =====================================================
       POPULATE SUPPLIERS
       ===================================================== */

    function populateSuppliers(data) {
        if (!supplierSelect) {
            console.error("supplierSelect not found.");
            return;
        }

        const previousSupplierID = selectedSupplierID;

        supplierSelect.innerHTML =
            '<option value="">Select Supplier</option>';

        if (!Array.isArray(data) || data.length === 0) {
            selectedSupplierID = "";
            requestProductsBySupplier("");
            console.warn("No suppliers received.");
            return;
        }

        data.forEach(function (supplier) {
            const option = document.createElement("option");

            option.value = supplier.SupplierID;
            option.textContent = supplier.SupplierName;

            supplierSelect.appendChild(option);
        });

        // Restore the selection only if it still exists.
        if (
            previousSupplierID &&
            data.some(s => String(s.SupplierID) === String(previousSupplierID))
        ) {
            supplierSelect.value = previousSupplierID;
        }

        selectedSupplierID = supplierSelect.value || "";

        if (selectedSupplierID) {
            requestProductsBySupplier(selectedSupplierID);
        } else {
            requestProductsBySupplier("");
        }

        console.log("Suppliers loaded:", data.length);
    }


    /* =====================================================
       POPULATE PRODUCTS
       ===================================================== */

    function populateProducts(data) {
        if (!productSelect) return;

        productSelect.innerHTML =
            '<option value="">Select Product</option>';

        if (!Array.isArray(data) || data.length === 0) {
            productSelect.innerHTML =
                '<option value="">No products for this supplier</option>';

            productSelect.disabled = true;
            resetSummary();
            return;
        }

        data.forEach(function (product) {
            const option = document.createElement("option");

            option.value = product.ProductID;
            option.textContent = product.ProductName;

            option.dataset.stock = product.StockQuantity ?? 0;
            option.dataset.purchasePrice = product.PurchasePrice ?? 0;

            productSelect.appendChild(option);
        });

        productSelect.disabled = false;

        console.log("Filtered products loaded:", data.length);
    }


    /* =====================================================
       SUPPLIER CHANGE
       ===================================================== */

    if (supplierSelect) {
        supplierSelect.addEventListener("change", function () {
            selectedSupplierID = this.value || "";

            // Clear the old product and summary immediately.
            if (productSelect) {
                productSelect.innerHTML =
                    '<option value="">Select supplier first</option>';
                productSelect.disabled = !selectedSupplierID;
            }

            resetSummary();

            requestProductsBySupplier(selectedSupplierID);
        });
    }


    /* =====================================================
       CALCULATE PURCHASE SUMMARY
       ===================================================== */

    function calculatePurchaseSummary() {
        const quantity = getNumber(quantityInput);
        const purchasePrice = getNumber(purchasePriceInput);

        let currentStock = 0;

        if (productSelect && productSelect.selectedIndex >= 0) {
            const selectedOption =
                productSelect.options[productSelect.selectedIndex];

            if (selectedOption?.value) {
                currentStock = Number(selectedOption.dataset.stock || 0);
            }
        }

        const newStock = currentStock + quantity;
        const totalAmount = quantity * purchasePrice;

        setDisplayValue(currentStockElement, currentStock);
        setDisplayValue(summaryQuantityElement, quantity);
        setDisplayValue(newStockElement, newStock);
        setDisplayValue(totalAmountElement, formatCurrency(totalAmount));
    }


    /* =====================================================
       PRODUCT CHANGE
       ===================================================== */

    if (productSelect) {
        productSelect.addEventListener("change", function () {
            const option = this.options[this.selectedIndex];

            if (!option || !option.value) {
                resetSummary();
                return;
            }

            const price = Number(option.dataset.purchasePrice || 0);

            if (purchasePriceInput) {
                purchasePriceInput.value = price.toFixed(2);
            }

            calculatePurchaseSummary();
        });
    }

    if (quantityInput) {
        quantityInput.addEventListener("input", calculatePurchaseSummary);
    }


    /* =====================================================
       CLEAR FORM
       ===================================================== */

    function clearForm() {
        if (purchaseForm) purchaseForm.reset();

        selectedSupplierID = supplierSelect?.value || "";

        resetSummary();

        if (selectedSupplierID) {
            requestProductsBySupplier(selectedSupplierID);
        } else if (productSelect) {
            productSelect.innerHTML =
                '<option value="">Select supplier first</option>';
            productSelect.disabled = true;
        }
    }

    if (clearButton) {
        clearButton.addEventListener("click", clearForm);
    }


    /* =====================================================
       SUBMIT PURCHASE
       ===================================================== */

    if (purchaseForm) {
        purchaseForm.addEventListener("submit", function (event) {
            event.preventDefault();

            if (purchaseSubmitting) return;

            const supplierID = Number(supplierSelect?.value || 0);
            const productID = Number(productSelect?.value || 0);
            const quantity = getNumber(quantityInput);
            const purchasePrice = getNumber(purchasePriceInput);

            if (supplierID <= 0) {
                alert("Please select a supplier.");
                return;
            }

            if (productID <= 0) {
                alert("Please select a product.");
                return;
            }

            if (!Number.isInteger(quantity) || quantity <= 0) {
                alert("Quantity must be a positive whole number.");
                return;
            }

            if (!Number.isFinite(purchasePrice) || purchasePrice <= 0) {
                alert("Purchase price must be greater than zero.");
                return;
            }

            // Check that the chosen product belongs to the selected supplier.
            const selectedOption =
                productSelect.options[productSelect.selectedIndex];

            if (
                !selectedOption ||
                !selectedOption.value ||
                String(supplierID) !== String(selectedSupplierID)
            ) {
                alert("Please select a valid product for this supplier.");
                return;
            }

            if (!confirm("Are you sure you want to add this purchase?")) {
                return;
            }

            purchaseSubmitting = true;

            sendToVB({
                action: "addPurchase",
                SupplierID: supplierID,
                ProductID: productID,
                Quantity: quantity,
                PurchasePrice: purchasePrice
            });
        });
    }


    /* =====================================================
       RENDER PURCHASE HISTORY
       ===================================================== */

    function renderPurchases(data) {
        if (!tableBody) {
            console.error("purchasesTableBody not found.");
            return;
        }

        tableBody.innerHTML = "";

        if (!Array.isArray(data) || data.length === 0) {
            tableBody.innerHTML = `
    < tr >
    <td colspan="4">No purchase records found.</td>
                </tr >`;
            return;
        }

        data.forEach(function (purchase) {
            const row = document.createElement("tr");

            const values = [
                purchase.PurchaseID ?? "-",
                purchase.SupplierName ?? "-",
                purchase.PurchaseDate ?? "-",
                formatCurrency(purchase.TotalAmount)
            ];

            values.forEach(function (value) {
                const cell = document.createElement("td");
                cell.textContent = value;
                row.appendChild(cell);
            });

            tableBody.appendChild(row);
        });
    }


    /* =====================================================
       RECEIVE MESSAGES FROM VB.NET
       ===================================================== */

    window.addEventListener("message", function (event) {
        const data = event.data;

        if (!data || typeof data !== "object") return;

        console.log("Purchases received:", data);

        switch (data.type) {

            case "suppliers":
                populateSuppliers(data.data);
                break;

            case "products":
                // Do not repopulate the dropdown with the full product list.
                // This response is intentionally ignored.
                console.log("Ignoring unfiltered products response.");
                break;

            case "supplierProducts":
                // Ignore responses for an older request or different supplier.
                if (
                    String(data.SupplierID) !== String(selectedSupplierID) ||
                    String(data.SupplierID) !== String(supplierSelect?.value || "") ||
                    (data.RequestNumber != null &&
                     Number(data.RequestNumber) !== productRequestNumber)
                ) {
                    break;
                }

                populateProducts(data.data);
                break;

            case "supplierProductsError":
                if (
                    String(data.SupplierID) !== String(selectedSupplierID)
                ) {
                    break;
                }

                if (productSelect) {
                    productSelect.innerHTML =
                        '<option value="">Unable to load products</option>';
                    productSelect.disabled = true;
                }

                alert(data.message || "Unable to load supplier products.");
                break;

            case "purchases":
                renderPurchases(data.data);
                break;

            case "purchaseAdded":
                purchaseSubmitting = false;

                alert(data.message || "Purchase added successfully.");

                clearForm();
                requestPurchases();
                break;

            case "purchaseError":
                purchaseSubmitting = false;

                alert(data.message || "Unable to process purchase.");
                break;
        }
    });


    /* =====================================================
       INITIAL LOAD
       ===================================================== */

    console.log("Purchases page initialized.");

    if (productSelect) {
        productSelect.innerHTML =
            '<option value="">Select supplier first</option>';
        productSelect.disabled = true;
    }

    requestSuppliers();
    requestPurchases();

});
