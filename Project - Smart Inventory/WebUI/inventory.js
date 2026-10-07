/* =========================================================
   SMART INVENTORY - INVENTORY PAGE
   ========================================================= */

let inventoryData = [];


/* =========================================================
   REQUEST INVENTORY DATA
   ========================================================= */

function loadInventory() {

    console.log("Requesting inventory data...");

    if (
        window.parent &&
        window.parent !== window
    ) {

        window.parent.postMessage(
            {
                action: "loadInventory"
            },
            "*"
        );

        console.log(
            "Inventory request sent to main.js"
        );

    }

    else if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            {
                action: "loadInventory"
            }
        );

    }

}


/* =========================================================
   RECEIVE DATA FROM MAIN PAGE
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        if (!event.data) {
            return;
        }

        const data = event.data;

        console.log(
            "Inventory received:",
            data
        );

        /* ---------------------------------------------
           INVENTORY DATA
        --------------------------------------------- */

        if (data.type === "inventory") {

            inventoryData =
                Array.isArray(data.data)
                    ? data.data
                    : [];

            updateSummary();

            renderInventory();

        }


        /* ---------------------------------------------
           ERROR
        --------------------------------------------- */

        if (data.type === "inventoryError") {

            const tableBody =
                document.getElementById(
                    "inventoryTableBody"
                );

            if (tableBody) {

                tableBody.innerHTML = `
                    <tr>
                        <td
                            colspan="8"
                            class="empty-message"
                        >
                            ${escapeHtml(
                    data.message ||
                    "Unable to load inventory."
                )}
                        </td>
                    </tr>
                `;

            }

        }

    }
);


/* =========================================================
   UPDATE SUMMARY CARDS
   ========================================================= */

function updateSummary() {

    let totalProducts = inventoryData.length;

    let totalStock = 0;

    let lowStock = 0;

    let outOfStock = 0;


    inventoryData.forEach(function (product) {

        const stock =
            Number(product.StockQuantity) || 0;

        const minimumStock =
            Number(product.MinimumStock) || 0;


        totalStock += stock;


        if (stock <= 0) {

            outOfStock++;

        }
        else if (stock <= minimumStock) {

            lowStock++;

        }

    });


    document.getElementById(
        "totalProducts"
    ).textContent = totalProducts;


    document.getElementById(
        "totalStock"
    ).textContent = totalStock;


    document.getElementById(
        "lowStock"
    ).textContent = lowStock;


    document.getElementById(
        "outOfStock"
    ).textContent = outOfStock;

}


/* =========================================================
   RENDER INVENTORY TABLE
   ========================================================= */

function renderInventory() {

    const tableBody =
        document.getElementById(
            "inventoryTableBody"
        );

    if (!tableBody) {
        return;
    }


    const searchInput =
        document.getElementById(
            "inventorySearch"
        );


    const filterSelect =
        document.getElementById(
            "stockFilter"
        );


    const searchText =
        searchInput
            ? searchInput.value
                .trim()
                .toLowerCase()
            : "";


    const filter =
        filterSelect
            ? filterSelect.value
            : "all";


    const filteredProducts =
        inventoryData.filter(
            function (product) {

                const productCode =
                    String(
                        product.ProductCode || ""
                    ).toLowerCase();


                const productName =
                    String(
                        product.ProductName || ""
                    ).toLowerCase();


                const category =
                    String(
                        product.CategoryName || ""
                    ).toLowerCase();


                const stock =
                    Number(
                        product.StockQuantity
                    ) || 0;


                const minimumStock =
                    Number(
                        product.MinimumStock
                    ) || 0;


                const matchesSearch =
                    productCode.includes(searchText) ||
                    productName.includes(searchText) ||
                    category.includes(searchText);


                let matchesFilter = true;


                if (filter === "in-stock") {

                    matchesFilter =
                        stock > minimumStock;

                }


                if (filter === "low-stock") {

                    matchesFilter =
                        stock > 0 &&
                        stock <= minimumStock;

                }


                if (filter === "out-of-stock") {

                    matchesFilter =
                        stock <= 0;

                }


                return (
                    matchesSearch &&
                    matchesFilter
                );

            }
        );


    if (filteredProducts.length === 0) {

        tableBody.innerHTML = `
            <tr>
                <td
                    colspan="8"
                    class="empty-message"
                >
                    No inventory records found.
                </td>
            </tr>
        `;

        return;

    }


    tableBody.innerHTML =
        filteredProducts
            .map(function (product) {

                const stock =
                    Number(
                        product.StockQuantity
                    ) || 0;


                const minimumStock =
                    Number(
                        product.MinimumStock
                    ) || 0;


                let statusText = "In Stock";

                let statusClass = "in-stock";


                if (stock <= 0) {

                    statusText = "Out of Stock";

                    statusClass =
                        "out-of-stock";

                }
                else if (
                    stock <= minimumStock
                ) {

                    statusText = "Low Stock";

                    statusClass =
                        "low-stock";

                }


                const purchasePrice =
                    Number(
                        product.PurchasePrice
                    ) || 0;


                const sellingPrice =
                    Number(
                        product.SellingPrice
                    ) || 0;


                return `
                    <tr>

                        <td>
                            ${escapeHtml(
                    product.ProductCode ||
                    "-"
                )}
                        </td>

                        <td>
                            ${escapeHtml(
                    product.ProductName ||
                    "-"
                )}
                        </td>

                        <td>
                            ${escapeHtml(
                    product.CategoryName ||
                    "-"
                )}
                        </td>

                        <td>
                            ${stock}
                        </td>

                        <td>
                            ${minimumStock}
                        </td>

                        <td>

                            <span
                                class="stock-status ${statusClass}"
                            >
                                ${statusText}
                            </span>

                        </td>

                        <td>
                            ₹${purchasePrice.toFixed(2)}
                        </td>

                        <td>
                            ₹${sellingPrice.toFixed(2)}
                        </td>

                    </tr>
                `;

            })
            .join("");

}


/* =========================================================
   SEARCH
   ========================================================= */

const searchInput =
    document.getElementById(
        "inventorySearch"
    );


if (searchInput) {

    searchInput.addEventListener(
        "input",
        function () {

            renderInventory();

        }
    );

}


/* =========================================================
   STOCK FILTER
   ========================================================= */

const stockFilter =
    document.getElementById(
        "stockFilter"
    );


if (stockFilter) {

    stockFilter.addEventListener(
        "change",
        function () {

            renderInventory();

        }
    );

}


/* =========================================================
   REFRESH BUTTON
   ========================================================= */

const refreshButton =
    document.getElementById(
        "refreshInventory"
    );


if (refreshButton) {

    refreshButton.addEventListener(
        "click",
        function () {

            loadInventory();

        }
    );

}


/* =========================================================
   HTML ESCAPE
   ========================================================= */

function escapeHtml(value) {

    return String(value)
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");

}


/* =========================================================
   INITIAL LOAD
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        loadInventory();

    }
);