/* =========================================================
   SMART INVENTORY
   MAIN NAVIGATION
   ========================================================= */


/* =========================================================
   PAGE CONFIGURATION
   ========================================================= */

const pages = {

    dashboard: {
        file: "dashboard.html",
        title: "Dashboard",
        subtitle: "Overview of your inventory and sales"
    },

    products: {
        file: "products.html",
        title: "Products",
        subtitle: "Manage your inventory products"
    },

    categories: {
        file: "categories.html",
        title: "Categories",
        subtitle: "Manage product categories"
    },

    suppliers: {
        file: "suppliers.html",
        title: "Suppliers",
        subtitle: "Manage your suppliers"
    },

    purchases: {
        file: "purchases.html",
        title: "Purchases",
        subtitle: "Manage purchase transactions"
    },

    sales: {
        file: "sales.html",
        title: "Sales",
        subtitle: "Manage sales transactions"
    },

    "sales-history": {
        file: "sales-history.html",
        title: "Sales History",
        subtitle: "View previous sales transactions"
    },

    inventory: {
        file: "inventory.html",
        title: "Inventory",
        subtitle: "Monitor your stock levels"
    },

    "product-analysis": {
        file: "product-analysis.html",
        title: "Product Analysis",
        subtitle: "Analyze product performance"
    },

    "sales-analytics": {
        file: "sales-analytics.html",
        title: "Sales Analytics",
        subtitle: "Analyze sales performance"
    },

    "admin-approval": {
        file: "admin-approval.html",
        title: "Admin Approval",
        subtitle: "Review and manage Admin access requests"
    },

    reports: {
        file: "reports.html",
        title: "Reports",
        subtitle: "Generate inventory and sales reports"
    }

};


/* =========================================================
   NAVIGATE TO PAGE
   ========================================================= */

function navigate(pageName) {

    const page = pages[pageName];


    /* -----------------------------------------------------
       Check page configuration
    ----------------------------------------------------- */

    if (!page) {

        console.error(
            "Page not found:",
            pageName
        );

        return;

    }


    /* -----------------------------------------------------
       Get iframe
    ----------------------------------------------------- */

    const pageFrame =
        document.getElementById(
            "page-frame"
        );


    /* -----------------------------------------------------
       Load page
    ----------------------------------------------------- */

    if (pageFrame) {

        pageFrame.src =
            page.file;

    }
    else {

        console.error(
            "page-frame was not found."
        );

    }


    /* -----------------------------------------------------
       Update page title
    ----------------------------------------------------- */

    const pageTitle =
        document.getElementById(
            "page-title"
        );


    if (pageTitle) {

        pageTitle.textContent =
            page.title;

    }


    /* -----------------------------------------------------
       Update page subtitle
    ----------------------------------------------------- */

    const pageSubtitle =
        document.getElementById(
            "page-subtitle"
        );


    if (pageSubtitle) {

        pageSubtitle.textContent =
            page.subtitle;

    }


    /* -----------------------------------------------------
       Update active navigation item
    ----------------------------------------------------- */

    const buttons =
        document.querySelectorAll(
            ".nav-item"
        );


    buttons.forEach(
        function (button) {

            button.classList.remove(
                "active"
            );

        }
    );


    buttons.forEach(
        function (button) {

            const command =
                button.getAttribute(
                    "onclick"
                );


            if (
                command &&
                command.includes(
                    "'" + pageName + "'"
                )
            ) {

                button.classList.add(
                    "active"
                );

            }

        }
    );

}


/* =========================================================
   LOGOUT
   ========================================================= */

function logout() {

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            {
                action: "logout"
            }
        );

        return;

    }


    console.error(
        "WebView2 is not available."
    );

}


/* =========================================================
   SUPPORTED WEBVIEW2 ACTIONS
   ========================================================= */

const supportedActions = [

    /* -----------------------------------------------------
       Products
    ----------------------------------------------------- */

    "loadProducts",
    "addProduct",
    "updateProduct",
    "deleteProduct",


    /* -----------------------------------------------------
       Categories
    ----------------------------------------------------- */

    "loadCategories",
    "addCategory",
    "deleteCategory",


    /* -----------------------------------------------------
       Suppliers
    ----------------------------------------------------- */

    "loadSuppliers",
    "addSupplier",


    /* -----------------------------------------------------
       Purchases
    ----------------------------------------------------- */

    "loadPurchases",
    "addPurchase",
   

    /* -----------------------------------------------------
       Sales Analytics
    ----------------------------------------------------- */
    "loadSales",
    "addSale",
    "loadSalesAnalytics",


    /* -----------------------------------------------------
       Admin Approval
    ----------------------------------------------------- */

    "loadPendingAdmins",
    "approveAdmin",
    "rejectAdmin"

];


/* =========================================================
   RECEIVE MESSAGES FROM IFRAME
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        /* -------------------------------------------------
           Ignore empty messages
        ------------------------------------------------- */

        if (!event.data) {

            return;

        }


        const data =
            event.data;


        /* -------------------------------------------------
           Ignore messages without action
        ------------------------------------------------- */

        if (
            typeof data !== "object" ||
            !data.action
        ) {

            return;

        }


        /* -------------------------------------------------
           Check supported action
        ------------------------------------------------- */

        if (
            !supportedActions.includes(
                data.action
            )
        ) {

            console.warn(
                "Unsupported WebUI action:",
                data.action
            );

            return;

        }


        /* -------------------------------------------------
           Forward message to VB.NET
        ------------------------------------------------- */

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage(
                data
            );

        }
        else {

            console.error(
                "WebView2 is not available."
            );

        }

    }
);


/* =========================================================
   PAGE FRAME LOAD EVENT
   ========================================================= */

const pageFrame =
    document.getElementById(
        "page-frame"
    );


if (pageFrame) {

    pageFrame.addEventListener(
        "load",
        function () {

            console.log(
                "Page loaded:",
                pageFrame.src
            );

        }
    );

}


/* =========================================================
   INITIAL PAGE
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        /*
         * Load Dashboard when the main WebUI starts.
         */

        navigate("dashboard");

    }
);