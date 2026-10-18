
document.addEventListener("DOMContentLoaded", function () {

    const categoryForm = document.getElementById("categoryForm");
    const categoryName = document.getElementById("categoryName");
    const message = document.getElementById("categoryMessage");
    const tableBody = document.getElementById("categoriesTableBody");
    const categoryCount = document.getElementById("categoryCount");
    const addButton = document.getElementById("btnAddCategory");

    let categoriesLoaded = false;

    // =========================================================
    // SEND MESSAGE TO VB.NET THROUGH THE PARENT IFRAME
    // =========================================================

    function sendToVB(data) {
        try {
            if (!window.parent || window.parent === window) {
                throw new Error("Parent page / iframe bridge is unavailable.");
            }

            window.parent.postMessage(data, "*");

        } catch (error) {
            console.error("Unable to communicate with VB.NET:", error);
            showMessage("Unable to communicate with the application.");
        }
    }

    // =========================================================
    // SHOW MESSAGE
    // =========================================================

    function showMessage(text, success = false) {
        message.textContent = text;
        message.style.color = success ? "#198754" : "#d32f2f";
    }

    // =========================================================
    // ESCAPE HTML
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
    // LOAD CATEGORIES
    // =========================================================

    function loadCategories() {

        categoriesLoaded = false;

        tableBody.innerHTML = `
            <tr>
                <td colspan="3" class="empty-row">
                    Loading categories...
                </td>
            </tr>
        `;

        categoryCount.textContent = "0";

        sendToVB({
            action: "loadCategories"
        });
    }

    // =========================================================
    // RENDER CATEGORIES IN ALPHABETICAL ORDER
    // =========================================================

    function renderCategories(categories) {

        tableBody.innerHTML = "";

        if (!Array.isArray(categories) || categories.length === 0) {

            tableBody.innerHTML = `
                <tr>
                    <td colspan="3" class="empty-row">
                        No categories found.
                    </td>
                </tr>
            `;

            categoryCount.textContent = "0";
            categoriesLoaded = true;
            return;
        }

        const sortedCategories = [...categories].sort(function (a, b) {

            return String(a.CategoryName || "").localeCompare(
                String(b.CategoryName || ""),
                undefined,
                {
                    sensitivity: "base",
                    numeric: true
                }
            );
        });

        categoryCount.textContent = String(sortedCategories.length);

        sortedCategories.forEach(function (category, index) {

            const row = document.createElement("tr");

            const categoryID = Number(category.CategoryID);
            const name = String(category.CategoryName || "");

            const serialCell = document.createElement("td");
            serialCell.textContent = String(index + 1);

            const nameCell = document.createElement("td");
            nameCell.textContent = name;

            const actionCell = document.createElement("td");

            const deleteButton = document.createElement("button");
            deleteButton.type = "button";
            deleteButton.className = "delete-button";
            deleteButton.textContent = "Delete";
            deleteButton.dataset.id = String(categoryID);
            deleteButton.dataset.name = name;
            deleteButton.setAttribute(
                "aria-label",
                "Delete category " + name
            );

            deleteButton.addEventListener("click", function () {
                deleteCategory(categoryID, name);
            });

            actionCell.appendChild(deleteButton);

            row.appendChild(serialCell);
            row.appendChild(nameCell);
            row.appendChild(actionCell);

            tableBody.appendChild(row);
        });

        categoriesLoaded = true;
    }

    // =========================================================
    // ADD CATEGORY
    // =========================================================

    categoryForm.addEventListener("submit", function (event) {

        event.preventDefault();

        const name = categoryName.value.trim();

        if (!name) {
            showMessage("Please enter a category name.");
            categoryName.focus();
            return;
        }

        if (addButton) {
            addButton.disabled = true;
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

        if (!Number.isInteger(categoryID) || categoryID <= 0) {
            showMessage("Invalid category ID.");
            return;
        }

        const confirmed = confirm(
            'Are you sure you want to delete "' + name + '"?'
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
    // RECEIVE RESPONSES FROM VB.NET
    // =========================================================

    window.addEventListener("message", function (event) {

        const data = event.data;

        if (!data || typeof data !== "object") {
            return;
        }

        // Categories loaded
        if (data.type === "categories") {

            renderCategories(data.data);
        }

        // Category added
        else if (data.type === "categoryAdded") {

            if (addButton) {
                addButton.disabled = false;
            }

            showMessage(
                data.message || "Category added successfully.",
                true
            );

            categoryName.value = "";

            loadCategories();
        }

        // Category deleted
        else if (data.type === "categoryDeleted") {

            showMessage(
                data.message || "Category deleted successfully.",
                true
            );

            loadCategories();
        }

        // Category error
        else if (data.type === "categoryError") {

            if (addButton) {
                addButton.disabled = false;
            }

            showMessage(
                data.message || "Unable to process category."
            );

            // Avoid leaving the table stuck on the loading placeholder.
            if (!categoriesLoaded) {
                tableBody.innerHTML = `
                    <tr>
                        <td colspan="3" class="empty-row">
                            Unable to load categories.
                        </td>
                    </tr>
                `;
            }
        }
    });

    // =========================================================
    // INITIAL LOAD
    // =========================================================

    loadCategories();

});
