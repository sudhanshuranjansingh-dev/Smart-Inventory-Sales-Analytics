/* =========================================================
   SMART INVENTORY - SALES HISTORY
   ========================================================= */

let salesHistoryData = [];


/* =========================================================
   PAGE LOAD
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {
    loadSalesHistory();
});


/* =========================================================
   LOAD SALES FROM VB.NET
   ========================================================= */

/* =========================================================
   LOAD SALES HISTORY
   ========================================================= */

function loadSalesHistory() {

    console.log("Requesting sales history...");


    if (
        window.parent &&
        window.parent !== window
    ) {

        window.parent.postMessage(
            {
                action: "loadSales"
            },
            "*"
        );


        console.log(
            "Sales history request sent to main.js"
        );

    }

    else if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.parent.postMessage(
            {
                action: "loadSales"
            },
            "*"
        );


        console.log(
            "Sales history request sent directly to VB.NET"
        );

    }

    else {

        console.error(
            "WebView2 communication is unavailable."
        );

    }

}


/* =========================================================
   RECEIVE MESSAGE FROM VB.NET
   ========================================================= */

window.addEventListener("message", function (event) {

    console.log("RAW MESSAGE:", event.data);

    let data = event.data;

    // ---------------------------------------------
    // Convert JSON string to JavaScript object
    // ---------------------------------------------
    if (typeof data === "string") {

        try {
            data = JSON.parse(data);
        }
        catch (error) {

            console.error(
                "JSON parsing failed:",
                error
            );

            return;
        }
    }


    // ---------------------------------------------
    // Check message
    // ---------------------------------------------
    if (!data) {
        console.error("Empty message received.");
        return;
    }


    console.log("MESSAGE OBJECT:", data);
    console.log("MESSAGE TYPE:", data.type);
    console.log("MESSAGE DATA:", data.data);


    // ---------------------------------------------
    // SALES DATA
    // ---------------------------------------------
    if (data.type === "sales") {

        let sales = data.data;


        // -----------------------------------------
        // Sometimes data may arrive as JSON string
        // -----------------------------------------
        if (typeof sales === "string") {

            try {
                sales = JSON.parse(sales);
            }
            catch (error) {

                console.error(
                    "Sales data JSON parsing failed:",
                    error
                );

                return;
            }
        }


        // -----------------------------------------
        // Make sure it is an array
        // -----------------------------------------
        if (!Array.isArray(sales)) {

            console.error(
                "Sales data is not an array:",
                sales
            );

            // If a single object was received,
            // convert it into an array.
            if (
                sales !== null &&
                typeof sales === "object"
            ) {

                sales = [sales];

            }
            else {

                sales = [];

            }
        }


        console.log(
            "FINAL SALES ARRAY:",
            sales
        );


        // -----------------------------------------
        // Store sales
        // -----------------------------------------
        salesHistoryData = sales;


        // -----------------------------------------
        // Render
        // -----------------------------------------
        renderSalesHistory(
            salesHistoryData
        );


        // -----------------------------------------
        // Summary
        // -----------------------------------------
        updateSummary(
            salesHistoryData
        );

    }


    // ---------------------------------------------
    // SALE ADDED
    // ---------------------------------------------
    else if (data.type === "saleAdded") {

        console.log(
            "Sale added. Reloading sales..."
        );

        loadSalesHistory();

    }


    // ---------------------------------------------
    // ERROR
    // ---------------------------------------------
    else if (
        data.type === "saleError" ||
        data.type === "error"
    ) {

        console.error(
            "Sales error:",
            data.message
        );

    }

});


/* =========================================================
   RENDER SALES HISTORY
   ========================================================= */

function renderSalesHistory(sales) {

    const tableBody =
        document.getElementById(
            "salesHistoryTableBody"
        );


    if (!tableBody) {

        console.error(
            "salesHistoryTableBody not found."
        );

        return;

    }


    /* -----------------------------------------------------
       NO DATA
    ----------------------------------------------------- */

    if (
        !Array.isArray(sales) ||
        sales.length === 0
    ) {

        tableBody.innerHTML = `
            <tr>
                <td colspan="6" class="empty-message">
                    No sales records found.
                </td>
            </tr>
        `;

        return;

    }


    /* -----------------------------------------------------
       CLEAR TABLE
    ----------------------------------------------------- */

    tableBody.innerHTML = "";


    /* -----------------------------------------------------
       ADD ROWS
    ----------------------------------------------------- */

    sales.forEach(function (sale) {

        console.log(
            "Rendering sale:",
            sale
        );


        const saleID =
            sale.SaleID ??
            sale.saleID ??
            "-";


        const productName =
            sale.ProductName ??
            sale.productName ??
            "-";


        const quantity =
            sale.Quantity ??
            sale.quantity ??
            0;


        const sellingPrice =
            sale.SellingPrice ??
            sale.sellingPrice ??
            0;


        const totalAmount =
            sale.TotalAmount ??
            sale.totalAmount ??
            0;


        const saleDate =
            sale.SaleDate ??
            sale.saleDate ??
            "";


        const row =
            document.createElement("tr");


        row.innerHTML = `
            <td>
                ${escapeHtml(saleID)}
            </td>

            <td>
                ${escapeHtml(productName)}
            </td>

            <td>
                ${escapeHtml(quantity)}
            </td>

            <td>
                ₹${formatNumber(sellingPrice)}
            </td>

            <td>
                ₹${formatNumber(totalAmount)}
            </td>

            <td>
                ${formatDate(saleDate)}
            </td>
        `;


        tableBody.appendChild(row);

    });

}


/* =========================================================
   UPDATE SUMMARY
   ========================================================= */

function updateSummary(sales) {

    let totalSales = 0;
    let totalItems = 0;
    let totalRevenue = 0;


    if (Array.isArray(sales)) {

        sales.forEach(function (sale) {

            totalSales++;


            totalItems += Number(
                sale.Quantity ??
                sale.quantity ??
                0
            );


            totalRevenue += Number(
                sale.TotalAmount ??
                sale.totalAmount ??
                0
            );

        });

    }


    const totalSalesElement =
        document.getElementById(
            "totalSales"
        );


    const totalItemsElement =
        document.getElementById(
            "totalItems"
        );


    const totalRevenueElement =
        document.getElementById(
            "totalRevenue"
        );


    if (totalSalesElement) {

        totalSalesElement.textContent =
            totalSales;

    }


    if (totalItemsElement) {

        totalItemsElement.textContent =
            totalItems;

    }


    if (totalRevenueElement) {

        totalRevenueElement.textContent =
            "₹" +
            formatNumber(totalRevenue);

    }

}


/* =========================================================
   FILTER SALES
   ========================================================= */

function filterSales() {

    const searchInput =
        document.getElementById(
            "searchSales"
        );


    const dateInput =
        document.getElementById(
            "saleDateFilter"
        );


    const search =
        searchInput
            ? searchInput.value
                .trim()
                .toLowerCase()
            : "";


    const selectedDate =
        dateInput
            ? dateInput.value
            : "";


    const filteredSales =
        salesHistoryData.filter(
            function (sale) {

                const saleID =
                    String(
                        sale.SaleID ??
                        sale.saleID ??
                        ""
                    ).toLowerCase();


                const productName =
                    String(
                        sale.ProductName ??
                        sale.productName ??
                        ""
                    ).toLowerCase();


                const saleDate =
                    sale.SaleDate ??
                    sale.saleDate ??
                    "";


                const matchesSearch =
                    search === "" ||
                    saleID.includes(search) ||
                    productName.includes(search);


                let matchesDate = true;


                if (selectedDate !== "") {

                    matchesDate =
                        getDateOnly(
                            saleDate
                        ) === selectedDate;

                }


                return (
                    matchesSearch &&
                    matchesDate
                );

            }
        );


    renderSalesHistory(
        filteredSales
    );

}


/* =========================================================
   CLEAR FILTER
   ========================================================= */

function clearSalesFilter() {

    const searchInput =
        document.getElementById(
            "searchSales"
        );


    const dateInput =
        document.getElementById(
            "saleDateFilter"
        );


    if (searchInput) {
        searchInput.value = "";
    }


    if (dateInput) {
        dateInput.value = "";
    }


    renderSalesHistory(
        salesHistoryData
    );

}


/* =========================================================
   DATE
   ========================================================= */

function getDateOnly(value) {

    if (!value) {
        return "";
    }


    const text =
        String(value);


    /*
       VB.NET sends:
       yyyy-MM-dd HH:mm

       Therefore take the first 10 characters.
    */

    if (text.length >= 10) {

        return text.substring(
            0,
            10
        );

    }


    const date =
        new Date(value);


    if (isNaN(date.getTime())) {

        return "";

    }


    return date.getFullYear() +
        "-" +
        String(
            date.getMonth() + 1
        ).padStart(2, "0") +
        "-" +
        String(
            date.getDate()
        ).padStart(2, "0");

}


function formatDate(value) {

    if (!value) {
        return "-";
    }


    const text =
        String(value);


    /*
       VB.NET format:
       yyyy-MM-dd HH:mm
    */

    if (
        /^\d{4}-\d{2}-\d{2} \d{2}:\d{2}$/.test(text)
    ) {

        const parts =
            text.split(" ");


        return parts[0] +
            " " +
            parts[1];

    }


    return text;

}


/* =========================================================
   NUMBER FORMAT
   ========================================================= */

function formatNumber(value) {

    const number =
        Number(value);


    if (isNaN(number)) {

        return "0.00";

    }


    return number.toLocaleString(
        "en-IN",
        {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        }
    );

}


/* =========================================================
   HTML ESCAPE
   ========================================================= */

function escapeHtml(value) {

    if (
        value === null ||
        value === undefined
    ) {

        return "";

    }


    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");

}