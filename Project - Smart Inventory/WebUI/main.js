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

    reports: {
        file: "reports.html",
        title: "Reports",
        subtitle: "Generate inventory and sales reports"
    }

};


/* =========================================================
   NAVIGATE
   ========================================================= */

function navigate(pageName) {

    const page = pages[pageName];

    if (!page) {

        console.error(
            "Page not found:",
            pageName
        );

        return;
    }


    /* Load page */

    document
        .getElementById("page-frame")
        .src = page.file;


    /* Update heading */

    document
        .getElementById("page-title")
        .textContent = page.title;


    document
        .getElementById("page-subtitle")
        .textContent = page.subtitle;


    /* Update active button */

    const buttons =
        document.querySelectorAll(".nav-item");

    buttons.forEach(button => {

        button.classList.remove("active");

    });


    buttons.forEach(button => {

        const command =
            button.getAttribute("onclick");

        if (
            command &&
            command.includes(
                "'" + pageName + "'"
            )
        ) {

            button.classList.add("active");

        }

    });

}


/* =========================================================
   LOGOUT
   ========================================================= */

function logout() {

    const confirmLogout =
        confirm(
            "Are you sure you want to logout?"
        );


    if (!confirmLogout) {
        return;
    }


    /*
       Later this will communicate
       with VB.NET.
    */

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            JSON.stringify({
                action: "logout"
            })
        );

    }

}