document.addEventListener("DOMContentLoaded", function () {

    const categoryForm = document.getElementById("categoryForm");
    const categoryName = document.getElementById("categoryName");
    const btnClear = document.getElementById("btnClear");
    const message = document.getElementById("categoryMessage");
    const tableBody = document.getElementById("categoriesTableBody");
    const categoryCount = document.getElementById("categoryCount");


    // =========================================================
    // SEND MESSAGE TO VB.NET THROUGH MAIN IFRAME
    // =========================================================

    function sendToVB(data) {

        try {

            // Categories page is inside page-frame iframe.
            window.parent.postMessage(data, "*");

        } catch (error) {

            console.error("Unable to communicate with VB.NET:", error);

        }

    }


    // =========================================================
    // SHOW MESSAGE
    // =========================================================

    function showMessage(text, success = false) {

        message.textContent = text;

        if (success) {

            message.style.color = "#198754";

        } else {

            message.style.color = "#d32f2f";

        }

    }


    // =========================================================
    // LOAD CATEGORIES
    // =========================================================

    function loadCategories() {

        tableBody.innerHTML =
            `<tr>
                <td colspan="3" class="empty-row">
                    Loading categories...
                </td>
            </tr>`;

        sendToVB({
            action: "loadCategories"
        });

    }


    // =========================================================
    // DISPLAY CATEGORIES
    // =========================================================

    function renderCategories(categories) {

        tableBody.innerHTML = "";

        if (!Array.isArray(categories) || categories.length === 0) {

            tableBody.innerHTML =
                `<tr>
                    <td colspan="3" class="empty-row">
                        No categories found.
                    </td>
                </tr>`;

            categoryCount.textContent = "0";

            return;
        }


        categoryCount.textContent = categories.length;


        categories.forEach(function (category) {

            const row = document.createElement("tr");

            row.innerHTML = `
                <td>${category.CategoryID}</td>

                <td>
                    ${escapeHtml(category.CategoryName)}
                </td>

                <td>
                    <button
                        type="button"
                        class="delete-button"
                        data-id="${category.CategoryID}"
                        data-name="${escapeHtml(category.CategoryName)}">
                        Delete
                    </button>
                </td>
            `;

            tableBody.appendChild(row);

        });


        // Attach delete events

        const deleteButtons =
            document.querySelectorAll(".delete-button");

        deleteButtons.forEach(function (button) {

            button.addEventListener("click", function () {

                const categoryID =
                    parseInt(button.dataset.id);

                const categoryNameValue =
                    button.dataset.name;

                deleteCategory(
                    categoryID,
                    categoryNameValue
                );

            });

        });

    }


    // =========================================================
    // HTML ESCAPE
    // =========================================================

    function escapeHtml(value) {

        if (value === null || value === undefined) {

            return "";

        }

        return String(value)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");

    }


    // =========================================================
    // ADD CATEGORY
    // =========================================================

    categoryForm.addEventListener("submit", function (event) {

        event.preventDefault();


        const name =
            categoryName.value.trim();


        if (name === "") {

            showMessage(
                "Please enter a category name."
            );

            categoryName.focus();

            return;
        }


        sendToVB({

            action: "addCategory",

            CategoryName: name

        });

    });


    // =========================================================
    // DELETE CATEGORY
    // =========================================================

    function deleteCategory(categoryID, name) {

        const confirmed =
            confirm(
                `Are you sure you want to delete "${name}"?`
            );


        if (!confirmed) {

            return;

        }


        sendToVB({

            action: "deleteCategory",

            CategoryID: categoryID

        });

    }


    // =========================================================
    // CLEAR BUTTON
    // =========================================================

    btnClear.addEventListener("click", function () {

        categoryName.value = "";

        message.textContent = "";

        categoryName.focus();

    });


    // =========================================================
    // RECEIVE DATA FROM VB.NET
    // =========================================================

    window.addEventListener("message", function (event) {

        if (!event.data) {

            return;

        }


        const data = event.data;


        // -----------------------------------------------------
        // CATEGORIES LOADED
        // -----------------------------------------------------

        if (data.type === "categories") {

            renderCategories(data.data);

        }


        // -----------------------------------------------------
        // CATEGORY ADDED
        // -----------------------------------------------------

        else if (data.type === "categoryAdded") {

            showMessage(
                data.message || "Category added successfully.",
                true
            );


            categoryName.value = "";

            loadCategories();

        }


        // -----------------------------------------------------
        // CATEGORY DELETED
        // -----------------------------------------------------

        else if (data.type === "categoryDeleted") {

            showMessage(
                data.message || "Category deleted successfully.",
                true
            );


            loadCategories();

        }


        // -----------------------------------------------------
        // CATEGORY ERROR
        // -----------------------------------------------------

        else if (data.type === "categoryError") {

            showMessage(
                data.message || "Unable to process category."
            );

        }

    });


    // =========================================================
    // INITIAL LOAD
    // =========================================================

    loadCategories();

});