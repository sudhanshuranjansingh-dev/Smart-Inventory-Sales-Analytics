
/* =========================================================
   SMART INVENTORY - DASHBOARD JAVASCRIPT
   ========================================================= */

let monthlySalesChart = null;
let inventoryStatusChart = null;
let currentUserRole = "";
let currentUserName = "";


/* =========================================================
   PAGE LOAD
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        console.log(
            "Smart Inventory Dashboard Loaded"
        );

        // Refresh Dashboard Button
        const refreshButton =
            document.getElementById("refreshBtn");

        if (refreshButton) {

            refreshButton.addEventListener(
                "click",
                function () {

                    requestDashboardData();

                }
            );

        }

        // Request logged-in user details
        requestCurrentUser();

        // Request dashboard statistics
        requestDashboardData();

    }
);


/* =========================================================
   REQUEST CURRENT USER
   ========================================================= */

function requestCurrentUser() {

    sendMessageToHost({
        action: "getCurrentUser"
    });

}


/* =========================================================
   REQUEST DASHBOARD DATA
   ========================================================= */

function requestDashboardData() {

    sendMessageToHost({
        action: "loadDashboard"
    });

}


/* =========================================================
   SEND MESSAGE TO HOST
   ========================================================= */

function sendMessageToHost(message) {

    // Communication through parent main.js
    if (
        window.parent &&
        window.parent !== window
    ) {

        window.parent.postMessage(
            message,
            "*"
        );

        return;

    }

    // Direct WebView2 communication
    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            message
        );

        return;

    }

    console.warn(
        "WebView2 communication is not available."
    );

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
                "Invalid dashboard message:",
                error
            );

            return;

        }

        console.log(
            "Dashboard data received:",
            data
        );


        /* -------------------------------------------------
           CURRENT USER RESPONSE
           ------------------------------------------------- */

        if (data.type === "currentUser") {

            currentUserRole =
                String(data.role || "").trim();

            currentUserName =
                String(
                    data.fullName || data.name || ""
                ).trim();

            if (currentUserRole.toLowerCase() === "staff") {

                applyStaffRestrictions();

            }

        }


        /* -------------------------------------------------
           DASHBOARD DATA RESPONSE
           ------------------------------------------------- */

        if (
            data.type === "dashboard" ||
            data.type === "dashboardData"
        ) {

            updateDashboard(data);

        }


        /* -------------------------------------------------
           DASHBOARD ERROR RESPONSE
           ------------------------------------------------- */

        if (data.type === "dashboardError") {

            console.error(
                "Dashboard error:",
                data.message
            );

        }

    }
);



/* =========================================================
   STAFF RESTRICTIONS
   ========================================================= */

function applyStaffRestrictions() {

    const restrictedMenuTexts = [
        "Categories",
        "Suppliers",
        "Product Analysis",
        "Sales Analytics",
        "Reports",
        "Admin Approval"
    ];

    function applyRestrictions() {

        // Hide restricted sidebar items in dashboard document
        hideRestrictedItems(
            document,
            restrictedMenuTexts
        );

        // Hide the complete dashboard data panels for Staff
        hideDashboardDataForStaff();

        // Hide restricted sidebar items in parent layout
        try {

            if (
                window.parent &&
                window.parent !== window
            ) {

                hideRestrictedItems(
                    window.parent.document,
                    restrictedMenuTexts
                );

            }

        }
        catch (error) {

            console.warn(
                "Unable to access parent layout:",
                error
            );

        }

    }

    // Apply immediately
    applyRestrictions();

    // Apply again after parent layout renders
    setTimeout(
        applyRestrictions,
        500
    );

}


/* =========================================================
   HIDE DASHBOARD DATA PANELS FOR STAFF
   ========================================================= */

function hideDashboardDataForStaff() {

    const dashboardPanels = [
        ".cards",
        ".sales-section",
        ".inventory-section",
        ".dashboard-footer"
    ];

    dashboardPanels.forEach(function (selector) {

        const panel =
            document.querySelector(selector);

        if (panel) {

            panel.style.setProperty(
                "display",
                "none",
                "important"
            );

        }

    });

    console.log(
        "Staff dashboard data panels hidden."
    );

}


/* =========================================================
   HIDE RESTRICTED ITEMS AND PROFILE ELEMENTS
   ========================================================= */

function hideRestrictedItems(
    targetDocument,
    restrictedMenuTexts
) {

    if (!targetDocument) {
        return;
    }

    const restrictedLower =
        restrictedMenuTexts.map(function (text) {

            return text.toLowerCase();

        });

    // Hide restricted navigation items
    targetDocument
        .querySelectorAll(
            "a, button, li, " +
            ".nav-item, " +
            ".sidebar-item, " +
            ".menu-item"
        )
        .forEach(function (item) {

            const text =
                (item.textContent || "")
                    .replace(/\s+/g, " ")
                    .trim()
                    .toLowerCase();

            const shouldHide =
                restrictedLower.some(function (restrictedText) {

                    return text.includes(restrictedText);

                });

            if (shouldHide) {

                const parentItem =
                    item.closest(
                        "li, " +
                        ".nav-item, " +
                        ".sidebar-item, " +
                        ".menu-item"
                    );

                if (parentItem) {

                    parentItem.style.display = "none";

                }
                else {

                    item.style.display = "none";

                }

            }

        });

    // Hide notification and profile elements for Staff
    targetDocument
        .querySelectorAll(
            ".notification, " +
            ".notification-icon, " +
            ".bell-icon, " +
            ".user-profile, " +
            ".user-area, " +
            ".profile-area, " +
            ".avatar, " +
            ".top-user, " +
            ".username, " +
            ".header-user, " +
            ".user-info, " +
            ".user-menu"
        )
        .forEach(function (item) {

            item.style.display = "none";

        });

}



/* =========================================================
   UPDATE DASHBOARD
   ========================================================= */

function updateDashboard(data) {

    if (!data) {
        return;
    }

    // Support both wrapped and unwrapped responses
    const dashboard =
        data.data &&
        typeof data.data === "object"
            ? data.data
            : data;


    /* -----------------------------------------------------
       DASHBOARD CARDS
       ----------------------------------------------------- */

    setValue(
        "totalProducts",
        dashboard.totalProducts
    );

    setValue(
        "totalSales",
        formatCurrency(
            dashboard.totalSales
        )
    );

    setValue(
        "lowStock",
        dashboard.lowStock
    );

    setValue(
        "totalUnits",
        dashboard.totalUnits
    );


    /* -----------------------------------------------------
       INVENTORY STATUS
       ----------------------------------------------------- */

    setValue(
        "inStock",
        dashboard.inStock
    );

    setValue(
        "lowStockItems",
        dashboard.lowStockItems
    );

    setValue(
        "outOfStock",
        dashboard.outOfStock
    );


    /* -----------------------------------------------------
       MONTHLY SALES CHART
       ----------------------------------------------------- */

    if (
        Array.isArray(dashboard.months) &&
        Array.isArray(dashboard.sales)
    ) {

        createMonthlySalesChart(
            dashboard.months,
            dashboard.sales
        );

    }


    /* -----------------------------------------------------
       INVENTORY STATUS CHART
       ----------------------------------------------------- */

    createInventoryStatusChart(
        dashboard.inStock,
        dashboard.lowStockItems,
        dashboard.outOfStock
    );

}


/* =========================================================
   SET HTML VALUE
   ========================================================= */

function setValue(
    elementId,
    value
) {

    const element =
        document.getElementById(
            elementId
        );

    if (!element) {

        console.warn(
            "Dashboard element not found:",
            elementId
        );

        return;

    }

    if (
        value === null ||
        value === undefined ||
        value === ""
    ) {

        element.textContent = "0";

    }
    else {

        element.textContent = value;

    }

}


/* =========================================================
   FORMAT CURRENCY - INDIAN RUPEES
   ========================================================= */

function formatCurrency(value) {

    const number =
        Number(value) || 0;

    return "₹" +
        number.toLocaleString(
            "en-IN",
            {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            }
        );

}


/* =========================================================
   MONTHLY SALES CHART
   ========================================================= */

function createMonthlySalesChart(
    months,
    sales
) {

    const canvas =
        document.getElementById(
            "salesChart"
        );

    if (!canvas) {

        console.warn(
            "Sales chart canvas not found."
        );

        return;

    }

    // Prevent duplicate chart instances
    if (monthlySalesChart) {

        monthlySalesChart.destroy();

        monthlySalesChart = null;

    }


    monthlySalesChart =
        new Chart(
            canvas,
            {
                type: "bar",

                data: {

                    labels: months,

                    datasets: [
                        {
                            label: "Monthly Sales",

                            data: sales,

                            borderWidth: 1,

                            borderRadius: 6
                        }
                    ]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false,

                    plugins: {

                        legend: {
                            display: true
                        },

                        tooltip: {

                            callbacks: {

                                label: function (context) {

                                    return " ₹" +
                                        Number(
                                            context.raw || 0
                                        ).toLocaleString(
                                            "en-IN",
                                            {
                                                minimumFractionDigits: 2,
                                                maximumFractionDigits: 2
                                            }
                                        );

                                }

                            }

                        }

                    },

                    scales: {

                        y: {

                            beginAtZero: true,

                            ticks: {

                                callback: function (value) {

                                    return "₹" +
                                        Number(
                                            value
                                        ).toLocaleString(
                                            "en-IN"
                                        );

                                }

                            }

                        },

                        x: {

                            grid: {

                                display: false

                            }

                        }

                    }

                }

            }
        );

}


/* =========================================================
   INVENTORY STATUS CHART
   ========================================================= */

function createInventoryStatusChart(
    inStock,
    lowStock,
    outOfStock
) {

    const canvas =
        document.getElementById(
            "inventoryChart"
        );

    if (!canvas) {

        console.warn(
            "Inventory chart canvas not found."
        );

        return;

    }

    // Prevent duplicate chart instances
    if (inventoryStatusChart) {

        inventoryStatusChart.destroy();

        inventoryStatusChart = null;

    }


    inventoryStatusChart =
        new Chart(
            canvas,
            {
                type: "doughnut",

                data: {

                    labels: [
                        "In Stock",
                        "Low Stock",
                        "Out of Stock"
                    ],

                    datasets: [
                        {

                            data: [
                                Number(inStock) || 0,
                                Number(lowStock) || 0,
                                Number(outOfStock) || 0
                            ],

                            borderWidth: 2

                        }
                    ]

                },

                options: {

                    responsive: true,

                    maintainAspectRatio: false,

                    plugins: {

                        legend: {

                            position: "bottom"

                        },

                        tooltip: {

                            callbacks: {

                                label: function (context) {

                                    return " " +
                                        context.label +
                                        ": " +
                                        context.raw +
                                        " items";

                                }

                            }

                        }

                    }

                }

            }
        );

}