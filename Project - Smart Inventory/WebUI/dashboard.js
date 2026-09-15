/* =========================================================
   SMART INVENTORY - DASHBOARD JAVASCRIPT
   ========================================================= */


/* =========================================================
   CHART VARIABLES
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

                    requestDashboardRefresh();

                }
            );

        }

    }
);


/* =========================================================
   REQUEST REFRESH FROM VB.NET
   ========================================================= */

function requestDashboardRefresh() {

    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage({

            action: "refreshDashboard"

        });

    }
    else {

        console.warn(
            "WebView2 communication is not available."
        );

    }

}


/* =========================================================
   RECEIVE DATA FROM VB.NET
   ========================================================= */

if (
    window.chrome &&
    window.chrome.webview
) {

    window.chrome.webview.addEventListener(
        "message",
        function (event) {

            console.log(
                "Dashboard data received:",
                event.data
            );


            const data =
                event.data;


            if (!data) {

                return;

            }


            updateDashboard(data);

        }
    );

}


/* =========================================================
   UPDATE DASHBOARD
   ========================================================= */

function updateDashboard(data) {

    if (!data) {

        return;

    }


    /* -----------------------------------------------------
       SUMMARY CARDS
       ----------------------------------------------------- */

    setValue(
        "totalProducts",
        data.totalProducts
    );


    setValue(
        "totalSales",
        data.totalSales
    );


    setValue(
        "lowStock",
        data.lowStock
    );


    setValue(
        "totalUnits",
        data.totalUnits
    );


    /* -----------------------------------------------------
       INVENTORY STATUS
       ----------------------------------------------------- */

    setValue(
        "inStock",
        data.inStock
    );


    setValue(
        "lowStockItems",
        data.lowStockItems
    );


    setValue(
        "outOfStock",
        data.outOfStock
    );


    /* -----------------------------------------------------
       MONTHLY SALES CHART
       ----------------------------------------------------- */

    if (
        Array.isArray(data.months) &&
        Array.isArray(data.sales)
    ) {

        createMonthlySalesChart(
            data.months,
            data.sales
        );

    }


    /* -----------------------------------------------------
       INVENTORY STATUS CHART
       ----------------------------------------------------- */

    createInventoryStatusChart(
        data.inStock,
        data.lowStockItems,
        data.outOfStock
    );

}


/* =========================================================
   SET HTML ELEMENT VALUE
   ========================================================= */

function setValue(
    elementId,
    value
) {

    const element =
        document.getElementById(
            elementId
        );


    if (element) {

        element.textContent =
            value ?? "0";

    }

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

        console.error(
            "Sales chart canvas not found."
        );

        return;

    }


    /* -----------------------------------------------------
       Destroy Previous Chart
       ----------------------------------------------------- */

    if (monthlySalesChart) {

        monthlySalesChart.destroy();

    }


    /* -----------------------------------------------------
       Create Chart
       ----------------------------------------------------- */

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
                                                context.raw
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

        console.error(
            "Inventory chart canvas not found."
        );

        return;

    }


    /* -----------------------------------------------------
       Destroy Previous Chart
       ----------------------------------------------------- */

    if (inventoryStatusChart) {

        inventoryStatusChart.destroy();

    }


    /* -----------------------------------------------------
       Convert Values
       ----------------------------------------------------- */

    const inStockValue =
        Number(inStock) || 0;


    const lowStockValue =
        Number(lowStock) || 0;


    const outOfStockValue =
        Number(outOfStock) || 0;


    /* -----------------------------------------------------
       Create Doughnut Chart
       ----------------------------------------------------- */

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