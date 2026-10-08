/* =========================================================
   SMART INVENTORY - DASHBOARD JAVASCRIPT
   ========================================================= */

let monthlySalesChart = null;
let inventoryStatusChart = null;


/* =========================================================
   PAGE LOAD
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        console.log(
            "Smart Inventory Dashboard Loaded"
        );


        /* -------------------------------------------------
           REFRESH BUTTON
           ------------------------------------------------- */

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


        /* -------------------------------------------------
           LOAD DASHBOARD DATA AUTOMATICALLY
           ------------------------------------------------- */

        requestDashboardData();

    }
);


/* =========================================================
   REQUEST DASHBOARD DATA
   ========================================================= */

function requestDashboardData() {

    const message = {

        action: "loadDashboard"

    };


    console.log(
        "Requesting dashboard data:",
        message
    );


    /*
       Dashboard is inside page-frame iframe.

       Send request to main.js.
    */

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


    /*
       Fallback for direct WebView2.
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
           DASHBOARD DATA
           ------------------------------------------------- */

        if (
            data.type === "dashboard" ||
            data.type === "dashboardData"
        ) {

            updateDashboard(data);

        }


        /* -------------------------------------------------
           DASHBOARD ERROR
           ------------------------------------------------- */

        if (
            data.type === "dashboardError"
        ) {

            console.error(
                "Dashboard error:",
                data.message
            );

        }

    }
);


/* =========================================================
   UPDATE DASHBOARD
   ========================================================= */

function updateDashboard(data) {

    if (!data) {

        return;

    }


    /*
       Some backend responses may place values directly
       inside the response, while others may place them
       inside data.data.

       Support both.
    */

    const dashboard =
        data.data &&
        typeof data.data === "object"
            ? data.data
            : data;


    /* =====================================================
       SUMMARY CARDS
       ===================================================== */

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


    /* =====================================================
       INVENTORY STATUS
       ===================================================== */

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


    /* =====================================================
       MONTHLY SALES
       ===================================================== */

    if (
        Array.isArray(dashboard.months) &&
        Array.isArray(dashboard.sales)
    ) {

        createMonthlySalesChart(
            dashboard.months,
            dashboard.sales
        );

    }


    /* =====================================================
       INVENTORY STATUS CHART
       ===================================================== */

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
   FORMAT CURRENCY
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


    if (monthlySalesChart) {

        monthlySalesChart.destroy();

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

                            label:
                                "Monthly Sales",

                            data:
                                sales,

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

                                label:
                                    function (context) {

                                        return " ₹" +
                                            Number(
                                                context.raw || 0
                                            ).toLocaleString(
                                                "en-IN",
                                                {
                                                    minimumFractionDigits: 2
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

                                callback:
                                    function (value) {

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


    if (inventoryStatusChart) {

        inventoryStatusChart.destroy();

    }


    const inStockValue =
        Number(inStock) || 0;


    const lowStockValue =
        Number(lowStock) || 0;


    const outOfStockValue =
        Number(outOfStock) || 0;


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

                                inStockValue,

                                lowStockValue,

                                outOfStockValue

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

                                label:
                                    function (context) {

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