/* =========================================================
   SALES NOVA - SALES
   ========================================================= */

let products = [];
let sales = [];


/* =========================================================
   WEBVIEW2 BRIDGE
   ========================================================= */

function sendToVB(data) {

    if (window.parent && window.parent !== window) {

        window.parent.postMessage(data, "*");

    } else if (
        window.chrome &&
        window.chrome.webview
    ) {

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
        Number(
            document.getElementById("product").value || 0
        );

    const product =
        products.find(
            p =>
                Number(p.ProductID) === productID
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
        Number(
            product.SellingPrice || 0
        ).toFixed(2);


    stockInput.value =
        Number(
            product.StockQuantity || 0
        );


    calculateTotal();

}


/* =========================================================
   CALCULATE TOTAL
   ========================================================= */

function calculateTotal() {

    const price =
        Number(
            document.getElementById("sellingPrice").value || 0
        );

    const quantity =
        Number(
            document.getElementById("quantity").value || 0
        );


    const total =
        price * quantity;


    document.getElementById("totalAmount").textContent =
        "₹" +
        total.toLocaleString(
            "en-IN",
            {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }
        );

}


/* =========================================================
   RENDER PRODUCTS
   ========================================================= */

function renderProducts(data) {

    products = Array.isArray(data)
        ? data
        : [];


    const select =
        document.getElementById("product");


    select.innerHTML = `
        <option value="">
            Select Product
        </option>
    `;


    products.forEach(
        function (product) {

            const option =
                document.createElement("option");

            option.value =
                product.ProductID;

            option.textContent =
                product.ProductName +
                " (" +
                product.ProductCode +
                ")";


            select.appendChild(option);

        }
    );

}


/* =========================================================
   RENDER SALES
   ========================================================= */

function renderSales(data) {

    sales = Array.isArray(data)
        ? data
        : [];


    const tableBody =
        document.getElementById("salesTableBody");


    const count =
        document.getElementById("salesCount");


    count.textContent =
        sales.length +
        (sales.length === 1
            ? " Sale"
            : " Sales");


    if (sales.length === 0) {

        tableBody.innerHTML = `
            <tr>
                <td
                    colspan="6"
                    class="empty-state">

                    No sales found.

                </td>
            </tr>
        `;

        return;

    }


    tableBody.innerHTML =
        sales.map(
            function (sale) {

                return `
                    <tr>

                        <td>
                            ${sale.SaleID ?? "-"}
                        </td>

                        <td>
                            ${sale.ProductName ?? "-"}
                        </td>

                        <td>
                            ${sale.Quantity ?? "-"}
                        </td>

                        <td>
                            ₹${Number(
                    sale.SellingPrice || 0
                ).toFixed(2)}
                        </td>

                        <td>
                            ₹${Number(
                    sale.TotalAmount || 0
                ).toFixed(2)}
                        </td>

                        <td>
                            ${sale.SaleDate ?? "-"}
                        </td>

                    </tr>
                `;

            }
        ).join("");

}


/* =========================================================
   SHOW MESSAGE
   ========================================================= */

function showMessage(message, type) {

    const messageBox =
        document.getElementById("message");


    messageBox.textContent =
        message;


    messageBox.className =
        "message " + type;


    setTimeout(
        function () {

            messageBox.className =
                "message";

        },
        4000
    );

}


/* =========================================================
   COMPLETE SALE
   ========================================================= */

function completeSale(event) {

    event.preventDefault();


    const productID =
        Number(
            document.getElementById("product").value || 0
        );


    const quantity =
        Number(
            document.getElementById("quantity").value || 0
        );


    const sellingPrice =
        Number(
            document.getElementById("sellingPrice").value || 0
        );


    const availableStock =
        Number(
            document.getElementById("availableStock").value || 0
        );


    if (productID <= 0) {

        showMessage(
            "Please select a product.",
            "error"
        );

        return;

    }


    if (
        !Number.isInteger(quantity) ||
        quantity <= 0
    ) {

        showMessage(
            "Quantity must be a positive whole number.",
            "error"
        );

        return;

    }


    if (quantity > availableStock) {

        showMessage(
            "Insufficient stock.",
            "error"
        );

        return;

    }


    if (sellingPrice <= 0) {

        showMessage(
            "Selling price is invalid.",
            "error"
        );

        return;

    }


    const confirmed =
        confirm(
            "Are you sure you want to complete this sale?"
        );


    if (!confirmed) {
        return;
    }


    sendToVB({

        action: "addSale",

        ProductID:
            productID,

        Quantity:
            quantity,

        SellingPrice:
            sellingPrice

    });

}


/* =========================================================
   RESET FORM
   ========================================================= */

function resetForm() {

    document
        .getElementById("saleForm")
        .reset();


    document
        .getElementById("sellingPrice")
        .value = "";


    document
        .getElementById("availableStock")
        .value = "";


    calculateTotal();

}


/* =========================================================
   RECEIVE DATA FROM VB.NET
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        if (!event.data) {
            return;
        }


        const data =
            event.data;


        console.log(
            "Sales received:",
            data
        );


        if (data.type === "products") {

            renderProducts(
                data.data
            );

            return;

        }


        if (data.type === "sales") {

            renderSales(
                data.data
            );

            return;

        }


        if (data.type === "saleAdded") {

            showMessage(
                data.message ||
                "Sale completed successfully.",
                "success"
            );


            resetForm();


            loadProducts();
            loadSales();

            return;

        }


        if (data.type === "saleError") {

            showMessage(
                data.message ||
                "Unable to complete sale.",
                "error"
            );

            return;

        }

    }
);


/* =========================================================
   INITIALIZE
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        document
            .getElementById("product")
            .addEventListener(
                "change",
                updateProductInformation
            );


        document
            .getElementById("quantity")
            .addEventListener(
                "input",
                calculateTotal
            );


        document
            .getElementById("saleForm")
            .addEventListener(
                "submit",
                completeSale
            );


        document
            .getElementById("btnClear")
            .addEventListener(
                "click",
                function () {

                    setTimeout(
                        calculateTotal,
                        0
                    );

                }
            );


        loadProducts();

        loadSales();

    }
);