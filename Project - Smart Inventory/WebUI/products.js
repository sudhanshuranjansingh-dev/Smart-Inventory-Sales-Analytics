/* =========================================================
   SMART INVENTORY
   PRODUCTS JAVASCRIPT
   ========================================================= */

let products = [];

let categories = [];

let suppliers = [];

let editingProductId = null;


/* =========================================================
   PAGE LOAD
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        requestProducts();

        requestCategories();

        requestSuppliers();

    }
);


/* =========================================================
   REQUEST PRODUCTS FROM VB.NET
   ========================================================= */

function requestProducts() {

    sendToVB({
        action: "loadProducts"
    });

}


/* =========================================================
   REQUEST CATEGORIES
   ========================================================= */

function requestCategories() {

    sendToVB({
        action: "loadCategories"
    });

}


/* =========================================================
   REQUEST SUPPLIERS
   ========================================================= */

function requestSuppliers() {

    sendToVB({
        action: "loadSuppliers"
    });

}


/* =========================================================
   SEND MESSAGE TO VB.NET
   ========================================================= */

function sendToVB(message) {

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            JSON.stringify(message)
        );

    }
    else {

        console.warn(
            "WebView2 is not available."
        );

    }

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


        let data;

        try {

            data =
                typeof event.data === "string"
                    ? JSON.parse(event.data)
                    : event.data;

        }
        catch (error) {

            console.error(
                "Invalid message:",
                error
            );

            return;

        }


        /* Products */

        if (data.type === "products") {

            products =
                data.data || [];

            renderProducts(products);

            updateSummary();

        }


        /* Categories */

        if (data.type === "categories") {

            categories =
                data.data || [];

            loadCategoryDropdown();

        }


        /* Suppliers */

        if (data.type === "suppliers") {

            suppliers =
                data.data || [];

            loadSupplierDropdown();

        }

    }
);


/* =========================================================
   LOAD CATEGORY DROPDOWN
   ========================================================= */

function loadCategoryDropdown() {

    const select =
        document.getElementById(
            "product-category"
        );


    select.innerHTML = `
        <option value="">
            Select Category
        </option>
    `;


    categories.forEach(category => {

        const option =
            document.createElement("option");

        option.value =
            category.CategoryID;

        option.textContent =
            category.CategoryName;

        select.appendChild(option);

    });

}


/* =========================================================
   LOAD SUPPLIER DROPDOWN
   ========================================================= */

function loadSupplierDropdown() {

    const select =
        document.getElementById(
            "product-supplier"
        );


    select.innerHTML = `
        <option value="">
            Select Supplier
        </option>
    `;


    suppliers.forEach(supplier => {

        const option =
            document.createElement("option");

        option.value =
            supplier.SupplierID;

        option.textContent =
            supplier.SupplierName;

        select.appendChild(option);

    });

}


/* =========================================================
   OPEN ADD PRODUCT
   ========================================================= */

function openAddProduct() {

    editingProductId = null;


    document.getElementById(
        "modal-title"
    ).textContent = "Add Product";


    document.getElementById(
        "product-form"
    ).reset();


    document.getElementById(
        "minimum-stock"
    ).value = 10;


    document.getElementById(
        "product-modal"
    ).classList.add("show");

}


/* =========================================================
   CLOSE MODAL
   ========================================================= */

function closeProductModal() {

    document.getElementById(
        "product-modal"
    ).classList.remove("show");

}


/* =========================================================
   SAVE PRODUCT
   ========================================================= */

function saveProduct(event) {

    event.preventDefault();


    const product = {

        action:
            editingProductId === null
                ? "addProduct"
                : "updateProduct",

        ProductID:
            editingProductId,

        ProductCode:
            document.getElementById(
                "product-code"
            ).value.trim(),

        ProductName:
            document.getElementById(
                "product-name"
            ).value.trim(),

        CategoryID:
            document.getElementById(
                "product-category"
            ).value,

        SupplierID:
            document.getElementById(
                "product-supplier"
            ).value,

        PurchasePrice:
            document.getElementById(
                "purchase-price"
            ).value,

        SellingPrice:
            document.getElementById(
                "selling-price"
            ).value,

        StockQuantity:
            document.getElementById(
                "stock-quantity"
            ).value || 0,

        MinimumStock:
            document.getElementById(
                "minimum-stock"
            ).value || 10

    };


    sendToVB(product);

}


/* =========================================================
   EDIT PRODUCT
   ========================================================= */

function editProduct(id) {

    const product =
        products.find(
            p => Number(p.ProductID) === Number(id)
        );


    if (!product) {

        return;

    }


    editingProductId =
        Number(product.ProductID);


    document.getElementById(
        "modal-title"
    ).textContent = "Edit Product";


    document.getElementById(
        "product-code"
    ).value =
        product.ProductCode || "";


    document.getElementById(
        "product-name"
    ).value =
        product.ProductName || "";


    document.getElementById(
        "product-category"
    ).value =
        product.CategoryID || "";


    document.getElementById(
        "product-supplier"
    ).value =
        product.SupplierID || "";


    document.getElementById(
        "purchase-price"
    ).value =
        product.PurchasePrice || 0;


    document.getElementById(
        "selling-price"
    ).value =
        product.SellingPrice || 0;


    document.getElementById(
        "stock-quantity"
    ).value =
        product.StockQuantity || 0;


    document.getElementById(
        "minimum-stock"
    ).value =
        product.MinimumStock || 10;


    document.getElementById(
        "product-modal"
    ).classList.add("show");

}


/* =========================================================
   DELETE PRODUCT
   ========================================================= */

function deleteProduct(id) {

    const product =
        products.find(
            p => Number(p.ProductID) === Number(id)
        );


    if (!product) {

        return;

    }


    const result =
        confirm(
            `Delete "${product.ProductName}"?`
        );


    if (!result) {

        return;

    }


    sendToVB({

        action: "deleteProduct",

        ProductID: Number(id)

    });

}


/* =========================================================
   RENDER PRODUCTS
   ========================================================= */

function renderProducts(list = products) {

    const tbody =
        document.getElementById(
            "product-table-body"
        );


    if (!list || list.length === 0) {

        tbody.innerHTML = `
            <tr>
                <td colspan="10"
                    class="empty-state">
                    No products available
                </td>
            </tr>
        `;

        return;

    }


    tbody.innerHTML = "";


    list.forEach(product => {

        const stock =
            Number(
                product.StockQuantity || 0
            );


        const minimum =
            Number(
                product.MinimumStock || 10
            );


        let status = "";

        let statusClass = "";


        if (stock === 0) {

            status = "Out of Stock";

            statusClass = "status-out";

        }
        else if (stock <= minimum) {

            status = "Low Stock";

            statusClass = "status-low";

        }
        else {

            status = "In Stock";

            statusClass = "status-good";

        }


        const row =
            document.createElement("tr");


        row.innerHTML = `

            <td>
                ${product.ProductID}
            </td>

            <td>
                ${product.ProductCode || "-"}
            </td>

            <td>
                <strong>
                    ${product.ProductName || "-"}
                </strong>
            </td>

            <td>
                ${product.CategoryName || "-"}
            </td>

            <td>
                ${product.SupplierName || "-"}
            </td>

            <td>
                ₹${Number(
                    product.PurchasePrice || 0
                ).toFixed(2)}
            </td>

            <td>
                ₹${Number(
                    product.SellingPrice || 0
                ).toFixed(2)}
            </td>

            <td>
                ${stock}
            </td>

            <td>

                <span class="status ${statusClass}">
                    ${status}
                </span>

            </td>

            <td>

                <div class="action-buttons">

                    <button
                        class="action-button"
                        onclick="editProduct(${product.ProductID})"
                        title="Edit">

                        ✏️

                    </button>


                    <button
                        class="action-button"
                        onclick="deleteProduct(${product.ProductID})"
                        title="Delete">

                        🗑️

                    </button>

                </div>

            </td>

        `;


        tbody.appendChild(row);

    });

}


/* =========================================================
   SEARCH
   ========================================================= */

function searchProducts() {

    const search =
        document.getElementById(
            "search-product"
        ).value
        .toLowerCase()
        .trim();


    const filtered =
        products.filter(product => {

            return (

                String(
                    product.ProductName || ""
                )
                .toLowerCase()
                .includes(search)

                ||

                String(
                    product.ProductCode || ""
                )
                .toLowerCase()
                .includes(search)

                ||

                String(
                    product.CategoryName || ""
                )
                .toLowerCase()
                .includes(search)

                ||

                String(
                    product.SupplierName || ""
                )
                .toLowerCase()
                .includes(search)

            );

        });


    renderProducts(filtered);

}


/* =========================================================
   UPDATE SUMMARY
   ========================================================= */

function updateSummary() {

    document.getElementById(
        "total-products"
    ).textContent =
        products.length;


    const lowStock =
        products.filter(product => {

            const stock =
                Number(
                    product.StockQuantity || 0
                );

            const minimum =
                Number(
                    product.MinimumStock || 10
                );

            return stock > 0 && stock <= minimum;

        }).length;


    document.getElementById(
        "low-stock"
    ).textContent =
        lowStock;


    const inventoryValue =
        products.reduce(
            (total, product) => {

                return total +
                    (
                        Number(
                            product.PurchasePrice || 0
                        )
                        *
                        Number(
                            product.StockQuantity || 0
                        )
                    );

            },
            0
        );


    document.getElementById(
        "inventory-value"
    ).textContent =
        "₹" +
        inventoryValue.toFixed(2);


    document.getElementById(
        "total-categories"
    ).textContent =
        categories.length;

}