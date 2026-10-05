/* =========================================================
   SMART INVENTORY
   PURCHASES MODULE
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    /* =====================================================
       DOM ELEMENTS
       ===================================================== */

    const supplierSelect =
        document.getElementById("supplierSelect");

    const productSelect =
        document.getElementById("productSelect");

    const quantityInput =
        document.getElementById("quantity");

    const purchasePriceInput =
        document.getElementById("purchasePrice");

    /*
     * These are SUMMARY DISPLAY elements.
     * They may be div/span elements, not inputs.
     */
    const currentStockElement =
        document.getElementById("currentStock");

    const summaryQuantityElement =
        document.getElementById("summaryQuantity");

    const newStockElement =
        document.getElementById("newStock");

    const totalAmountElement =
        document.getElementById("totalAmount");

    const purchaseForm =
        document.getElementById("purchaseForm");

    const clearButton =
        document.getElementById("clearPurchaseBtn");

    const tableBody =
        document.getElementById("purchasesTableBody");


    /* =====================================================
       HELPER - SET DISPLAY VALUE
       ===================================================== */

    function setDisplayValue(element, value) {

        if (!element) {
            return;
        }

        /*
         * If element is an input,
         * use value.
         */
        if (
            element.tagName === "INPUT" ||
            element.tagName === "SELECT" ||
            element.tagName === "TEXTAREA"
        ) {

            element.value = value;

        }
        else {

            /*
             * For div, span, p, etc.
             */
            element.textContent = value;

        }

    }


    /* =====================================================
       HELPER - GET NUMBER FROM INPUT
       ===================================================== */

    function getNumber(element) {

        if (!element) {
            return 0;
        }

        const value =
            Number(element.value);

        if (Number.isNaN(value)) {
            return 0;
        }

        return value;

    }


    /* =====================================================
       SEND MESSAGE TO VB.NET
       ===================================================== */

    function sendToVB(data) {

        /*
         * Purchases is loaded inside an iframe.
         * Send the message to the parent WebUI first.
         */
        if (
            window.parent &&
            window.parent !== window
        ) {

            window.parent.postMessage(
                data,
                "*"
            );

            return;

        }


        /*
         * Fallback for direct WebView2 page.
         */
        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage(
                data
            );

            return;

        }


        console.error(
            "WebView2 communication is unavailable."
        );

    }


    /* =====================================================
       REQUEST SUPPLIERS
       ===================================================== */

    function requestSuppliers() {

        console.log(
            "Purchases: requesting suppliers..."
        );

        sendToVB({
            action: "loadSuppliers"
        });

    }


    /* =====================================================
       REQUEST PRODUCTS
       ===================================================== */

    function requestProducts() {

        console.log(
            "Purchases: requesting products..."
        );

        sendToVB({
            action: "loadProducts"
        });

    }


    /* =====================================================
       REQUEST PURCHASES
       ===================================================== */

    function requestPurchases() {

        console.log(
            "Purchases: requesting purchases..."
        );

        sendToVB({
            action: "loadPurchases"
        });

    }


    /* =====================================================
       POPULATE SUPPLIERS
       ===================================================== */

    function populateSuppliers(suppliers) {

        if (!supplierSelect) {

            console.error(
                "supplierSelect not found."
            );

            return;

        }


        supplierSelect.innerHTML =
            '<option value="">Select Supplier</option>';


        if (
            !Array.isArray(suppliers) ||
            suppliers.length === 0
        ) {

            console.warn(
                "No suppliers received."
            );

            return;

        }


        suppliers.forEach(
            function (supplier) {

                const option =
                    document.createElement("option");


                option.value =
                    supplier.SupplierID;


                option.textContent =
                    supplier.SupplierName;


                supplierSelect.appendChild(
                    option
                );

            }
        );


        console.log(
            "Suppliers loaded:",
            suppliers.length
        );

    }


    /* =====================================================
       POPULATE PRODUCTS
       ===================================================== */

    function populateProducts(products) {

        if (!productSelect) {

            console.error(
                "productSelect not found."
            );

            return;

        }


        productSelect.innerHTML =
            '<option value="">Select Product</option>';


        if (
            !Array.isArray(products) ||
            products.length === 0
        ) {

            console.warn(
                "No products received."
            );

            return;

        }


        products.forEach(
            function (product) {

                const option =
                    document.createElement("option");


                option.value =
                    product.ProductID;


                option.textContent =
                    product.ProductName;


                /*
                 * Store stock and purchase price
                 * inside the option.
                 */
                option.dataset.stock =
                    product.StockQuantity ?? 0;


                option.dataset.purchasePrice =
                    product.PurchasePrice ?? 0;


                productSelect.appendChild(
                    option
                );

            }
        );


        console.log(
            "Products loaded:",
            products.length
        );

    }


    /* =====================================================
       CALCULATE PURCHASE SUMMARY
       ===================================================== */

    function calculatePurchaseSummary() {

        const quantity =
            getNumber(quantityInput);


        const purchasePrice =
            getNumber(purchasePriceInput);


        /*
         * Current stock is stored in the
         * selected product option.
         */
        let currentStock = 0;


        if (
            productSelect &&
            productSelect.selectedIndex >= 0
        ) {

            const selectedOption =
                productSelect.options[
                productSelect.selectedIndex
                ];


            if (
                selectedOption &&
                selectedOption.value
            ) {

                currentStock =
                    Number(
                        selectedOption.dataset.stock || 0
                    );

            }

        }


        const newStock =
            currentStock + quantity;


        const totalAmount =
            quantity * purchasePrice;


        /* -------------------------------------------------
           UPDATE SUMMARY
           ------------------------------------------------- */

        setDisplayValue(
            currentStockElement,
            currentStock
        );


        setDisplayValue(
            summaryQuantityElement,
            quantity
        );


        setDisplayValue(
            newStockElement,
            newStock
        );


        setDisplayValue(
            totalAmountElement,
            "₹" +
            totalAmount.toLocaleString(
                "en-IN",
                {
                    minimumFractionDigits: 2,
                    maximumFractionDigits: 2
                }
            )
        );


        console.log(
            "Purchase Summary:",
            {
                currentStock:
                    currentStock,

                quantity:
                    quantity,

                newStock:
                    newStock,

                totalAmount:
                    totalAmount
            }
        );

    }


    /* =====================================================
       PRODUCT SELECTION
       ===================================================== */

    if (productSelect) {

        productSelect.addEventListener(
            "change",
            function () {

                const selectedOption =
                    productSelect.options[
                    productSelect.selectedIndex
                    ];


                /*
                 * Nothing selected
                 */
                if (
                    !selectedOption ||
                    !selectedOption.value
                ) {

                    if (purchasePriceInput) {

                        purchasePriceInput.value =
                            "";

                    }


                    setDisplayValue(
                        currentStockElement,
                        0
                    );


                    setDisplayValue(
                        summaryQuantityElement,
                        0
                    );


                    setDisplayValue(
                        newStockElement,
                        0
                    );


                    setDisplayValue(
                        totalAmountElement,
                        "₹0.00"
                    );


                    return;

                }


                /*
                 * Get product data
                 */
                const stock =
                    Number(
                        selectedOption.dataset.stock || 0
                    );


                const purchasePrice =
                    Number(
                        selectedOption.dataset.purchasePrice || 0
                    );


                /*
                 * Fill purchase price
                 */
                if (purchasePriceInput) {

                    purchasePriceInput.value =
                        purchasePrice.toFixed(2);

                }


                /*
                 * Update summary
                 */
                calculatePurchaseSummary();

            }
        );

    }


    /* =====================================================
       QUANTITY CHANGE
       ===================================================== */

    if (quantityInput) {

        quantityInput.addEventListener(
            "input",
            function () {

                calculatePurchaseSummary();

            }
        );

    }


    /* =====================================================
       PURCHASE PRICE CHANGE
       ===================================================== */

    if (purchasePriceInput) {

        purchasePriceInput.addEventListener(
            "input",
            function () {

                calculatePurchaseSummary();

            }
        );

    }


    /* =====================================================
       CLEAR FORM
       ===================================================== */

    function clearForm() {

        if (purchaseForm) {

            purchaseForm.reset();

        }


        setDisplayValue(
            currentStockElement,
            0
        );


        setDisplayValue(
            summaryQuantityElement,
            0
        );


        setDisplayValue(
            newStockElement,
            0
        );


        setDisplayValue(
            totalAmountElement,
            "₹0.00"
        );

    }


    /* =====================================================
       CLEAR BUTTON
       ===================================================== */

    if (clearButton) {

        clearButton.addEventListener(
            "click",
            function () {

                clearForm();

            }
        );

    }


    /* =====================================================
       SUBMIT PURCHASE
       ===================================================== */

    if (purchaseForm) {

        purchaseForm.addEventListener(
            "submit",
            function (event) {

                event.preventDefault();


                const supplierID =
                    Number(
                        supplierSelect?.value || 0
                    );


                const productID =
                    Number(
                        productSelect?.value || 0
                    );


                const quantity =
                    Number(
                        quantityInput?.value || 0
                    );


                const purchasePrice =
                    Number(
                        purchasePriceInput?.value || 0
                    );


                /* -----------------------------------------
                   VALIDATE SUPPLIER
                   ----------------------------------------- */

                if (supplierID <= 0) {

                    alert(
                        "Please select a supplier."
                    );

                    return;

                }


                /* -----------------------------------------
                   VALIDATE PRODUCT
                   ----------------------------------------- */

                if (productID <= 0) {

                    alert(
                        "Please select a product."
                    );

                    return;

                }


                /* -----------------------------------------
                   VALIDATE QUANTITY
                   ----------------------------------------- */

                if (
                    !Number.isInteger(quantity) ||
                    quantity <= 0
                ) {

                    alert(
                        "Quantity must be a positive whole number."
                    );

                    return;

                }


                /* -----------------------------------------
                   VALIDATE PRICE
                   ----------------------------------------- */

                if (purchasePrice <= 0) {

                    alert(
                        "Purchase price must be greater than zero."
                    );

                    return;

                }


                /* -----------------------------------------
                   CONFIRM
                   ----------------------------------------- */

                const confirmed =
                    confirm(
                        "Are you sure you want to add this purchase?"
                    );


                if (!confirmed) {

                    return;

                }


                /* -----------------------------------------
                   SEND PURCHASE TO VB.NET
                   ----------------------------------------- */

                sendToVB({

                    action:
                        "addPurchase",

                    SupplierID:
                        supplierID,

                    ProductID:
                        productID,

                    Quantity:
                        quantity,

                    PurchasePrice:
                        purchasePrice

                });

            }
        );

    }


    /* =====================================================
       RENDER PURCHASE HISTORY
       ===================================================== */

    function renderPurchases(purchases) {

        if (!tableBody) {

            console.error(
                "purchasesTableBody not found."
            );

            return;

        }


        tableBody.innerHTML = "";


        if (
            !Array.isArray(purchases) ||
            purchases.length === 0
        ) {

            tableBody.innerHTML = `
                <tr>
                    <td colspan="4">
                        No purchase records found.
                    </td>
                </tr>
            `;

            return;

        }


        purchases.forEach(
            function (purchase) {

                const row =
                    document.createElement("tr");


                const purchaseID =
                    purchase.PurchaseID ?? "-";


                const supplierName =
                    purchase.SupplierName ?? "-";


                const purchaseDate =
                    purchase.PurchaseDate ?? "-";


                const totalAmount =
                    Number(
                        purchase.TotalAmount || 0
                    );


                row.innerHTML = `
                    <td>${purchaseID}</td>

                    <td>${supplierName}</td>

                    <td>${purchaseDate}</td>

                    <td>
                        ₹${totalAmount.toLocaleString(
                    "en-IN",
                    {
                        minimumFractionDigits: 2,
                        maximumFractionDigits: 2
                    }
                )}
                    </td>
                `;


                tableBody.appendChild(
                    row
                );

            }
        );

    }


    /* =====================================================
       RECEIVE DATA FROM MAIN WEBUI / VB.NET
       ===================================================== */

    window.addEventListener(
        "message",
        function (event) {

            if (!event.data) {

                return;

            }


            const data =
                event.data;


            console.log(
                "Purchases received:",
                data
            );


            /* ---------------------------------------------
               SUPPLIERS
               --------------------------------------------- */

            if (
                data.type === "suppliers"
            ) {

                populateSuppliers(
                    data.data
                );

                return;

            }


            /* ---------------------------------------------
               PRODUCTS
               --------------------------------------------- */

            if (
                data.type === "products"
            ) {

                populateProducts(
                    data.data
                );

                return;

            }


            /* ---------------------------------------------
               PURCHASES
               --------------------------------------------- */

            if (
                data.type === "purchases"
            ) {

                renderPurchases(
                    data.data
                );

                return;

            }


            /* ---------------------------------------------
               PURCHASE SUCCESS
               --------------------------------------------- */

            if (
                data.type === "purchaseAdded"
            ) {

                alert(
                    data.message ||
                    "Purchase added successfully."
                );


                clearForm();


                requestPurchases();

                requestProducts();

                return;

            }


            /* ---------------------------------------------
               PURCHASE ERROR
               --------------------------------------------- */

            if (
                data.type === "purchaseError"
            ) {

                alert(
                    data.message ||
                    "Unable to process purchase."
                );

                return;

            }

        }
    );


    /* =====================================================
       INITIAL LOAD
       ===================================================== */

    console.log(
        "Purchases page initialized."
    );


    requestSuppliers();

    requestProducts();

    requestPurchases();

});