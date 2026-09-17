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


    /* -----------------------------------------------------
       Check page
       ----------------------------------------------------- */

    if (!page) {

        console.error(
            "Page not found:",
            pageName
        );

        return;
    }


    /* -----------------------------------------------------
       Load page into iframe
       ----------------------------------------------------- */

    const pageFrame =
        document.getElementById("page-frame");


    if (pageFrame) {

        pageFrame.src = page.file;

    }


    /* -----------------------------------------------------
       Update page title
       ----------------------------------------------------- */

    const pageTitle =
        document.getElementById("page-title");


    if (pageTitle) {

        pageTitle.textContent =
            page.title;

    }


    /* -----------------------------------------------------
       Update page subtitle
       ----------------------------------------------------- */

    const pageSubtitle =
        document.getElementById("page-subtitle");


    if (pageSubtitle) {

        pageSubtitle.textContent =
            page.subtitle;

    }


    /* -----------------------------------------------------
       Remove active class
       ----------------------------------------------------- */

    const buttons =
        document.querySelectorAll(".nav-item");


    buttons.forEach(button => {

        button.classList.remove("active");

    });


    /* -----------------------------------------------------
       Set active navigation button
       ----------------------------------------------------- */

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

    /*
       Do NOT use JavaScript confirm() here.

       VB.NET will show the confirmation dialog.
       This prevents two confirmation dialogs.
    */


    /* -----------------------------------------------------
       Check WebView2
       ----------------------------------------------------- */

    if (
        window.chrome &&
        window.chrome.webview
    ) {


        /* -------------------------------------------------
           Send logout request to VB.NET
           ------------------------------------------------- */

        window.chrome.webview.postMessage({

            action: "logout"

        });


    }
    else {

        console.error(
            "WebView2 is not available."
        );

    }

}


/* =========================================================
   RECEIVE MESSAGES FROM IFRAME
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        const data =
            event.data;


        console.log(
            "Message received from iframe:",
            data
        );


        /* -------------------------------------------------
           SALES ANALYTICS
           ------------------------------------------------- */

        if (
            data &&
            data.action ===
            "loadSalesAnalytics"
        ) {


            /* ---------------------------------------------
               Forward message to VB.NET
               --------------------------------------------- */

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

    }
);