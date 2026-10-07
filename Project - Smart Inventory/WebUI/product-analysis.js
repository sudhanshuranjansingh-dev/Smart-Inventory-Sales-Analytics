/* =========================================================
   SMART INVENTORY - PRODUCT ANALYSIS
   ========================================================= */

let analysisData = [];


/* =========================================================
   REQUEST PRODUCT ANALYSIS
   ========================================================= */

function loadProductAnalysis() {

    console.log(
        "Requesting product analysis..."
    );


    if (
        window.parent &&
        window.parent !== window
    ) {

        window.parent.postMessage(
            {
                action: "loadProductAnalysis"
            },
            "*"
        );


        console.log(
            "Product analysis request sent to main.js"
        );

    }

    else if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.postMessage(
            {
                action: "loadProductAnalysis"
            }
        );

    }

}


/* =========================================================
   RECEIVE DATA
   ========================================================= */

window.addEventListener(
    "message",
    function (event) {

        if (!event.data) {

            return;

        }


        const data = event.data;


        console.log(
            "Product analysis received:",
            data
        );


        /* ---------------------------------------------
           PRODUCT ANALYSIS DATA
        --------------------------------------------- */

        if (
            data.type ===
            "productAnalysis"
        ) {

            analysisData =
                Array.isArray(data.data)
                    ? data.data
                    : [];


            updateSummary();

            renderAnalysis();

        }


        /* ---------------------------------------------
           ERROR
        --------------------------------------------- */

        if (
            data.type ===
            "productAnalysisError"
        ) {

            const tableBody =
                document.getElementById(
                    "analysisTableBody"
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
                    "Unable to load product analysis."
                )}
                        </td>

                    </tr>
                `;

            }

        }

    }
);


/* =========================================================
   UPDATE SUMMARY
   ========================================================= */

function updateSummary() {

    const totalProducts =
        analysisData.length;


    let productsSold = 0;

    let totalUnitsSold = 0;

    let totalSales = 0;


    analysisData.forEach(
        function (product) {

            const unitsSold =
                Number(
                    product.UnitsSold
                ) || 0;


            const sales =
                Number(
                    product.TotalSales
                ) || 0;


            if (unitsSold > 0) {

                productsSold++;

            }


            totalUnitsSold +=
                unitsSold;


            totalSales +=
                sales;

        }
    );


    document.getElementById(
        "totalProducts"
    ).textContent =
        totalProducts;


    document.getElementById(
        "productsSold"
    ).textContent =
        productsSold;


    document.getElementById(
        "totalUnitsSold"
    ).textContent =
        totalUnitsSold;


    document.getElementById(
        "totalSales"
    ).textContent =
        "₹" +
        totalSales.toFixed(2);

}


/* =========================================================
   RENDER ANALYSIS TABLE
   ========================================================= */

function renderAnalysis() {

    const tableBody =
        document.getElementById(
            "analysisTableBody"
        );


    if (!tableBody) {

        return;

    }


    const searchInput =
        document.getElementById(
            "analysisSearch"
        );


    const filterSelect =
        document.getElementById(
            "performanceFilter"
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
        analysisData.filter(
            function (product) {

                const productCode =
                    String(
                        product.ProductCode ||
                        ""
                    ).toLowerCase();


                const productName =
                    String(
                        product.ProductName ||
                        ""
                    ).toLowerCase();


                const category =
                    String(
                        product.CategoryName ||
                        ""
                    ).toLowerCase();


                const performance =
                    String(
                        product.Performance ||
                        ""
                    ).toLowerCase();


                const matchesSearch =
                    productCode.includes(
                        searchText
                    ) ||

                    productName.includes(
                        searchText
                    ) ||

                    category.includes(
                        searchText
                    );


                let matchesFilter = true;


                if (
                    filter === "top"
                ) {

                    matchesFilter =
                        performance ===
                        "top";

                }


                if (
                    filter === "medium"
                ) {

                    matchesFilter =
                        performance ===
                        "medium";

                }


                if (
                    filter === "low"
                ) {

                    matchesFilter =
                        performance ===
                        "low";

                }


                if (
                    filter === "never"
                ) {

                    matchesFilter =
                        performance ===
                        "never";

                }


                return (
                    matchesSearch &&
                    matchesFilter
                );

            }
        );


    if (
        filteredProducts.length === 0
    ) {

        tableBody.innerHTML = `
            <tr>

                <td
                    colspan="8"
                    class="empty-message"
                >
                    No product analysis records found.
                </td>

            </tr>
        `;

        return;

    }


    tableBody.innerHTML =
        filteredProducts
            .map(
                function (product) {

                    const currentStock =
                        Number(
                            product.StockQuantity
                        ) || 0;


                    const unitsSold =
                        Number(
                            product.UnitsSold
                        ) || 0;


                    const sellingPrice =
                        Number(
                            product.SellingPrice
                        ) || 0;


                    const totalSales =
                        Number(
                            product.TotalSales
                        ) || 0;


                    const performance =
                        String(
                            product.Performance ||
                            "never"
                        ).toLowerCase();


                    let performanceText =
                        "No Sales";


                    let performanceClass =
                        "never";


                    if (
                        performance ===
                        "top"
                    ) {

                        performanceText =
                            "Top Performing";

                        performanceClass =
                            "top";

                    }

                    else if (
                        performance ===
                        "medium"
                    ) {

                        performanceText =
                            "Medium";

                        performanceClass =
                            "medium";

                    }

                    else if (
                        performance ===
                        "low"
                    ) {

                        performanceText =
                            "Low";

                        performanceClass =
                            "low";

                    }


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
                                ${currentStock}
                            </td>


                            <td>
                                ${unitsSold}
                            </td>


                            <td>
                                ₹${sellingPrice.toFixed(2)}
                            </td>


                            <td>
                                ₹${totalSales.toFixed(2)}
                            </td>


                            <td>

                                <span
                                    class="performance-status ${performanceClass}"
                                >
                                    ${performanceText}
                                </span>

                            </td>

                        </tr>
                    `;

                }
            )
            .join("");

}


/* =========================================================
   SEARCH
   ========================================================= */

const searchInput =
    document.getElementById(
        "analysisSearch"
    );


if (searchInput) {

    searchInput.addEventListener(
        "input",
        function () {

            renderAnalysis();

        }
    );

}


/* =========================================================
   PERFORMANCE FILTER
   ========================================================= */

const performanceFilter =
    document.getElementById(
        "performanceFilter"
    );


if (performanceFilter) {

    performanceFilter.addEventListener(
        "change",
        function () {

            renderAnalysis();

        }
    );

}


/* =========================================================
   REFRESH
   ========================================================= */

const refreshButton =
    document.getElementById(
        "refreshAnalysis"
    );


if (refreshButton) {

    refreshButton.addEventListener(
        "click",
        function () {

            loadProductAnalysis();

        }
    );

}


/* =========================================================
   HTML ESCAPE
   ========================================================= */

function escapeHtml(value) {

    return String(value)

        .replaceAll(
            "&",
            "&amp;"
        )

        .replaceAll(
            "<",
            "&lt;"
        )

        .replaceAll(
            ">",
            "&gt;"
        )

        .replaceAll(
            '"',
            "&quot;"
        )

        .replaceAll(
            "'",
            "&#039;"
        );

}


/* =========================================================
   INITIAL LOAD
   ========================================================= */

document.addEventListener(
    "DOMContentLoaded",
    function () {

        loadProductAnalysis();

    }
);