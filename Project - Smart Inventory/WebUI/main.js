
/* =========================================================
   SMART INVENTORY - MAIN NAVIGATION
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
   CURRENT USER
   ========================================================= */

let currentUserRole = "";


/* =========================================================
   STAFF RESTRICTED PAGES
   ========================================================= */

const staffRestrictedPages = [
    "products",
    "categories",
    "suppliers",
    "product-analysis",
    "sales-analytics",
    "reports",
    "admin-approval"
];


function isStaffRestrictedPage(pageName) {

    return (
        currentUserRole === "staff" &&
        staffRestrictedPages.includes(pageName)
    );

}


/* =========================================================
   APPLY ROLE-BASED NAVIGATION
   ========================================================= */

function applyRoleBasedNavigation(role) {

    currentUserRole = String(role || "")
        .trim()
        .toLowerCase();

    document.querySelectorAll(".nav-item").forEach(
        function (button) {

            const command =
                button.getAttribute("onclick") || "";

            const restricted = staffRestrictedPages.some(
                function (pageName) {
                    return command.includes(
                        "'" + pageName + "'"
                    );
                }
            );

            button.style.display =
                currentUserRole === "staff" && restricted
                    ? "none"
                    : "";

        }
    );

    console.log(
        "Navigation applied for role:",
        currentUserRole
    );

}


/* =========================================================
   NAVIGATE TO PAGE
   ========================================================= */

function navigate(pageName) {

    if (isStaffRestrictedPage(pageName)) {

        alert(
            "You do not have permission to access this page."
        );

        return;
    }

    const page = pages[pageName];

    if (!page) {

        console.error("Page not found:", pageName);

        return;
    }

    const pageFrame =
        document.getElementById("page-frame");

    if (pageFrame) {

        pageFrame.src = page.file;

    } else {

        console.error("page-frame was not found.");

    }

    const pageTitle =
        document.getElementById("page-title");

    if (pageTitle) {
        pageTitle.textContent = page.title;
    }

    const pageSubtitle =
        document.getElementById("page-subtitle");

    if (pageSubtitle) {
        pageSubtitle.textContent = page.subtitle;
    }

    document.querySelectorAll(".nav-item").forEach(
        function (button) {

            button.classList.remove("active");

            const command =
                button.getAttribute("onclick") || "";

            if (
                command.includes("'" + pageName + "'")
            ) {
                button.classList.add("active");
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

        window.chrome.webview.postMessage({
            action: "logout"
        });

    } else {

        console.error("WebView2 is not available.");

    }

}


/* =========================================================
   SUPPORTED WEBVIEW2 ACTIONS
   ========================================================= */

const supportedActions = [

    "getCurrentUser",

    "loadDashboard",

    "loadProducts",
    "addProduct",
    "updateProduct",
    "deleteProduct",

    "loadCategories",
    "addCategory",
    "deleteCategory",

    "loadSuppliers",
    "addSupplier",

    "loadPurchases",
    "addPurchase",

    "loadInventory",

    "loadSales",
    "addSale",

    "loadProductAnalysis",
    "loadSalesAnalytics",
    "loadReports",

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

        if (!event.data) {
            return;
        }

        const data = event.data;

        if (
            typeof data !== "object" ||
            !data.action
        ) {
            return;
        }

        if (!supportedActions.includes(data.action)) {

            console.warn(
                "Unsupported WebUI action:",
                data.action
            );

            return;
        }

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage(data);

        } else {

            console.error("WebView2 is not available.");

        }

    }
);


/* =========================================================
   RECEIVE MESSAGES FROM VB.NET
   ========================================================= */

if (
    window.chrome &&
    window.chrome.webview
) {

    window.chrome.webview.addEventListener(
        "message",
        function (event) {

            const data = event.data;

            if (!data) {
                return;
            }

            // Handle current-user response.
            if (
                data.type === "currentUser" ||
                data.action === "currentUser"
            ) {

                const role =
                    data.role ||
                    data.currentUserRole ||
                    (
                        data.currentUser &&
                        data.currentUser.role
                    ) ||
                    "";

                applyRoleBasedNavigation(role);

                if (
                    pageFrame &&
                    pageFrame.contentWindow
                ) {

                    pageFrame.contentWindow.postMessage(
                        data,
                        "*"
                    );

                }

            }

        }
    );

}


/* =========================================================
   PAGE FRAME LOAD EVENT
   ========================================================= */

const pageFrame =
    document.getElementById("page-frame");

if (pageFrame) {

    pageFrame.addEventListener(
        "load",
        function () {

            console.log("Page loaded:", pageFrame.src);

        }
    );

}


/* =========================================================
   INITIAL PAGE
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        navigate("dashboard");

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({
                action: "getCurrentUser"
            });

        }

    }
);
