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

document.addEventListener("DOMContentLoaded", function () {

    console.log("Products page loaded.");

    requestProducts();
    requestCategories();
    requestSuppliers();

});


/* =========================================================
   SEND MESSAGE TO MAIN.JS
   ========================================================= */

function sendToVB(message) {

    console.log("Sending message:", message);

    /*
       Products page is inside page-frame iframe.

       Therefore send the message to the parent page
       (main.js), which will forward it to VB.NET.
    */

    if (window.parent && window.parent !== window) {

        window.parent.postMessage(
            message,
            "*"
        );

        return;
    }


    /*
       Fallback for direct WebView2 use.
    */

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            JSON.stringify(message)
        );

        return;
    }


    console.warn(
        "WebView2 communication is not available."
    );

}


/* =========================================================
   REQUEST PRODUCTS
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
   RECEIVE DATA FROM MAIN.JS / VB.NET
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
                "Invalid message received:",
                error
            );

            return;

        }


        console.log(
            "Products page received:",
            data
        );


        /* =================================================
           PRODUCTS
           ================================================= */

        if (data.type === "products") {

            products =
                Array.isArray(data.data)
                    ? data.data
                    : [];

            renderProducts(products);

            updateSummary();

        }


        /* =================================================
           CATEGORIES
           ================================================= */

        if (data.type === "categories") {

            categories =
                Array.isArray(data.data)
                    ? data.data
                    : [];

            console.log(
                "Categories received:",
                categories
            );

            loadCategoryDropdown();

        }


        /* =================================================
           SUPPLIERS
           ================================================= */

        if (data.type === "suppliers") {

            suppliers =
                Array.isArray(data.data)
                    ? data.data
                    : [];

            console.log(
                "Suppliers received:",
                suppliers
            );

            loadSupplierDropdown();

        }


        /* =================================================
           PRODUCT ERROR
           ================================================= */

        if (data.type === "productError") {

            alert(
                data.message ||
                "Product operation failed."
            );

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


    if (!select) {

        console.error(
            "Category dropdown #product-category not found."
        );

        return;

    }


    /*
       Remember currently selected category.
       This is useful while editing.
    */

    const previousValue =
        select.value;


    select.innerHTML = "";


    const defaultOption =
        document.createElement("option");


    defaultOption.value = "";

    defaultOption.textContent =
        "Select Category";


    select.appendChild(
        defaultOption
    );


    categories.forEach(function (category) {

        const categoryId =
            category.CategoryID;


        const categoryName =
            category.CategoryName;


        if (
            categoryId === undefined ||
            categoryId === null
        ) {

            return;

        }


        const option =
            document.createElement("option");


        option.value =
            String(categoryId);


        option.textContent =
            categoryName || "Unnamed Category";


        select.appendChild(
            option
        );

    });


    /*
       Restore previous value if possible.
    */

    if (previousValue !== "") {

        select.value =
            previousValue;

    }


    /*
       If editing a product,
       restore its category.
    */

    if (
        editingProductId !== null
    ) {

        const product =
            products.find(
                p =>
                    Number(p.ProductID) ===
                    Number(editingProductId)
            );


        if (product) {

            if (
                product.CategoryID !== undefined &&
                product.CategoryID !== null
            ) {

                select.value =
                    String(product.CategoryID);

            }

        }

    }


    console.log(
        "Category dropdown populated."
    );

}


/* =========================================================
   LOAD SUPPLIER DROPDOWN
   ========================================================= */

function loadSupplierDropdown() {

    const select =
        document.getElementById(
            "product-supplier"
        );


    if (!select) {

        console.error(
            "Supplier dropdown #product-supplier not found."
        );

        return;

    }


    const previousValue =
        select.value;


    select.innerHTML = "";


    const defaultOption =
        document.createElement("option");


    defaultOption.value = "";

    defaultOption.textContent =
        "Select Supplier";


    select.appendChild(
        defaultOption
    );


    suppliers.forEach(function (supplier) {

        const supplierId =
            supplier.SupplierID;


        const supplierName =
            supplier.SupplierName;


        if (
            supplierId === undefined ||
            supplierId === null
        ) {

            return;

        }


        const option =
            document.createElement("option");


        option.value =
            String(supplierId);


        option.textContent =
            supplierName || "Unnamed Supplier";


        select.appendChild(
            option
        );

    });


    /*
       Restore previous value.
    */

    if (previousValue !== "") {

        select.value =
            previousValue;

    }


    /*
       Restore supplier while editing.
    */

    if (
        editingProductId !== null
    ) {

        const product =
            products.find(
                p =>
                    Number(p.ProductID) ===
                    Number(editingProductId)
            );


        if (product) {

            if (
                product.SupplierID !== undefined &&
                product.SupplierID !== null
            ) {

                select.value =
                    String(product.SupplierID);

            }

        }

    }


    console.log(
        "Supplier dropdown populated."
    );

}


/* =========================================================
   OPEN ADD PRODUCT
   ========================================================= */

function openAddProduct() {

    editingProductId = null;


    document.getElementById(
        "modal-title"
    ).textContent =
        "Add Product";


    document.getElementById(
        "product-form"
    ).reset();


    /*
       Make sure dropdowns are populated.
    */

    loadCategoryDropdown();

    loadSupplierDropdown();


    document.getElementById(
        "minimum-stock"
    ).value = 10;


    document.getElementById(
        "product-modal"
    ).classList.add("show");

}


/* =========================================================
   CLOSE PRODUCT MODAL
   ========================================================= */

function closeProductModal() {

    document.getElementById(
        "product-modal"
    ).classList.remove("show");


    editingProductId = null;

}


/* =========================================================
   SAVE PRODUCT
   ========================================================= */

function saveProduct(event) {

    event.preventDefault();


    const categoryID =
        document.getElementById(
            "product-category"
        ).value;


    const supplierID =
        document.getElementById(
            "product-supplier"
        ).value;


    /*
       Validate Category
    */

    if (!categoryID) {

        alert(
            "Please select a Category."
        );

        return;

    }


    /*
       Validate Supplier
    */

    if (!supplierID) {

        alert(
            "Please select a Supplier."
        );

        return;

    }


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
            Number(categoryID),


        SupplierID:
            Number(supplierID),


        PurchasePrice:
            Number(
                document.getElementById(
                    "purchase-price"
                ).value || 0
            ),


        SellingPrice:
            Number(
                document.getElementById(
                    "selling-price"
                ).value || 0
            ),


        StockQuantity:
            Number(
                document.getElementById(
                    "stock-quantity"
                ).value || 0
            ),


        MinimumStock:
            Number(
                document.getElementById(
                    "minimum-stock"
                ).value || 10
            )

    };


    console.log(
        "Saving product:",
        product
    );


    sendToVB(product);

}


/* =========================================================
   EDIT PRODUCT
   ========================================================= */

function editProduct(id) {

    const product =
        products.find(
            p =>
                Number(p.ProductID) ===
                Number(id)
        );


    if (!product) {

        alert(
            "Product not found."
        );

        return;

    }


    editingProductId =
        Number(product.ProductID);


    document.getElementById(
        "modal-title"
    ).textContent =
        "Edit Product";


    document.getElementById(
        "product-code"
    ).value =
        product.ProductCode || "";


    document.getElementById(
        "product-name"
    ).value =
        product.ProductName || "";


    /*
       Category
    */

    document.getElementById(
        "product-category"
    ).value =
        product.CategoryID || "";


    /*
       Supplier
    */

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
            p =>
                Number(p.ProductID) ===
                Number(id)
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

        action:
            "deleteProduct",

        ProductID:
            Number(id)

    });

}


/* =========================================================
   RENDER PRODUCTS
   ========================================================= */

function renderProducts(
    list = products
) {

    const tbody =
        document.getElementById(
            "product-table-body"
        );


    if (!tbody) {

        console.error(
            "Product table body not found."
        );

        return;

    }


    if (
        !list ||
        list.length === 0
    ) {

        tbody.innerHTML = `

            <tr>

                <td
                    colspan="10"
                    class="empty-state">

                    No products available

                </td>

            </tr>

        `;

        return;

    }


    tbody.innerHTML = "";


    list.forEach(function (product) {

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

            status =
                "Out of Stock";

            statusClass =
                "status-out";

        }
        else if (stock <= minimum) {

            status =
                "Low Stock";

            statusClass =
                "status-low";

        }
        else {

            status =
                "In Stock";

            statusClass =
                "status-good";

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

                <span
                    class="status ${statusClass}">

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
        products.filter(function (product) {

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

    const totalProducts =
        document.getElementById(
            "total-products"
        );


    if (totalProducts) {

        totalProducts.textContent =
            products.length;

    }


    const lowStock =
        products.filter(function (product) {

            const stock =
                Number(
                    product.StockQuantity || 0
                );


            const minimum =
                Number(
                    product.MinimumStock || 10
                );


            return (
                stock > 0 &&
                stock <= minimum
            );

        }).length;


    const lowStockElement =
        document.getElementById(
            "low-stock"
        );


    if (lowStockElement) {

        lowStockElement.textContent =
            lowStock;

    }


    const inventoryValue =
        products.reduce(
            function (total, product) {

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


    const inventoryValueElement =
        document.getElementById(
            "inventory-value"
        );


    if (inventoryValueElement) {

        inventoryValueElement.textContent =
            "₹" +
            inventoryValue.toFixed(2);

    }


    const totalCategories =
        document.getElementById(
            "total-categories"
        );


    if (totalCategories) {

        totalCategories.textContent =
            categories.length;

    }

}