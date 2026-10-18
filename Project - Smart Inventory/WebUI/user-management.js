
/* =========================================================
   SMART INVENTORY - USER MANAGEMENT
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    const tableBody = document.getElementById("usersTableBody");
    const searchInput = document.getElementById("userSearch");
    const refreshButton = document.getElementById("refreshUsersBtn");
    const message = document.getElementById("userMessage");

    const editDialog = document.getElementById("editUserDialog");
    const editForm = document.getElementById("editUserForm");
    const cancelEditButton = document.getElementById("cancelEditBtn");

    // Verify that the HTML contains the expected elements.
    if (!tableBody || !searchInput || !refreshButton ||
        !message || !editDialog || !editForm || !cancelEditButton) {

        console.error("User Management: Required HTML elements are missing.");
        console.error({
            tableBody: !!tableBody,
            searchInput: !!searchInput,
            refreshButton: !!refreshButton,
            message: !!message,
            editDialog: !!editDialog,
            editForm: !!editForm,
            cancelEditButton: !!cancelEditButton
        });

        return;
    }

    let users = [];

    function showMessage(text) {
        message.textContent = text || "";
    }

    function escapeHtml(value) {
        return String(value ?? "")
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }

    // Send requests through the existing WebView2 bridge.
    function sendRequest(request) {

        console.log("User Management request:", request);

        if (window.parent && window.parent !== window) {
            window.parent.postMessage(request, "*");
            return true;
        }

        if (window.chrome && window.chrome.webview) {
            window.chrome.webview.postMessage(request);
            return true;
        }

        showMessage("Application connection is unavailable.");
        console.error("WebView2 message bridge is unavailable.");

        return false;
    }

    // Request every registered user.
    function loadUsers() {

        tableBody.innerHTML = `
            <tr>
                <td colspan="6" class="empty-message">
                    Loading users...
                </td>
            </tr>`;

        showMessage("");

        if (!sendRequest({ action: "loadUsers" })) {
            tableBody.innerHTML = `
                <tr>
                    <td colspan="6" class="empty-message">
                        Unable to connect to the application.
                    </td>
                </tr>`;
        }
    }

    // Display users matching the search.
    function renderUsers() {

        const search = searchInput.value.trim().toLowerCase();

        const filteredUsers = users.filter(user => {

            return [
                user.UserID,
                user.Username,
                user.FullName,
                user.Role,
                user.AccountStatus
            ].some(value =>
                String(value ?? "").toLowerCase().includes(search)
            );
        });

        tableBody.innerHTML = "";

        if (filteredUsers.length === 0) {

            const row = document.createElement("tr");
            const cell = document.createElement("td");

            cell.colSpan = 6;
            cell.className = "empty-message";
            cell.textContent = users.length
                ? "No matching users found."
                : "No registered users found.";

            row.appendChild(cell);
            tableBody.appendChild(row);

            return;
        }

        filteredUsers.forEach(user => {

            const row = document.createElement("tr");

            row.innerHTML = `
                <td>${escapeHtml(user.UserID)}</td>
                <td>${escapeHtml(user.Username)}</td>
                <td>${escapeHtml(user.FullName || "-")}</td>
                <td>${escapeHtml(user.Role || "-")}</td>
                <td>${escapeHtml(user.AccountStatus || "-")}</td>
                <td>
                    <div class="action-buttons">
                        <button type="button" class="btn-edit">
                            Edit
                        </button>
                        <button type="button" class="btn-delete">
                            Delete
                        </button>
                    </div>
                </td>`;

            // Edit user.
            row.querySelector(".btn-edit").addEventListener("click", () => {

                document.getElementById("editUserId").value =
                    user.UserID;

                document.getElementById("editUsername").value =
                    user.Username || "";

                document.getElementById("editFullName").value =
                    user.FullName || "";

                document.getElementById("editRole").value =
                    user.Role || "";

                document.getElementById("editAccountStatus").value =
                    user.AccountStatus || "Pending";

                editDialog.showModal();
            });

            // Delete user.
            row.querySelector(".btn-delete").addEventListener("click", () => {

                if (!confirm(
                    `Delete user "${user.Username}" (ID ${user.UserID})? This cannot be undone.`
                )) {
                    return;
                }

                sendRequest({
                    action: "deleteUser",
                    userID: Number(user.UserID)
                });
            });

            tableBody.appendChild(row);
        });
    }

    // Refresh and search.
    searchInput.addEventListener("input", renderUsers);
    refreshButton.addEventListener("click", loadUsers);

    // Close edit dialog.
    cancelEditButton.addEventListener("click", () => {
        editDialog.close();
    });

    // Submit user changes.
    editForm.addEventListener("submit", function (event) {

        event.preventDefault();

        const userID = Number(
            document.getElementById("editUserId").value
        );

        const username =
            document.getElementById("editUsername").value.trim();

        const fullName =
            document.getElementById("editFullName").value.trim();

        const role =
            document.getElementById("editRole").value.trim();

        const accountStatus =
            document.getElementById("editAccountStatus").value;

        if (!userID || !username || !role) {
            showMessage("User ID, username, and role are required.");
            return;
        }

        if (!confirm(`Save changes for "${username}"?`)) {
            return;
        }

        sendRequest({
            action: "updateUser",
            userID: userID,
            username: username,
            fullName: fullName,
            role: role,
            accountStatus: accountStatus
        });

        editDialog.close();
    });

    // Receive responses from the host application.
    window.addEventListener("message", function (event) {

        const data = event.data;

        if (!data || typeof data !== "object") {
            return;
        }

        console.log("User Management response:", data);

        switch (data.type) {

            case "usersData":

                users = Array.isArray(data.data) ? data.data : [];

                renderUsers();

                showMessage(`${users.length} user(s) loaded.`);
                break;

            case "usersError":

                tableBody.innerHTML = `
                    <tr>
                        <td colspan="6" class="empty-message">
                            Unable to load users.
                        </td>
                    </tr>`;

                showMessage(data.message || "Unable to load users.");
                break;

            case "userManagementMessage":

                showMessage(data.message || "Operation completed.");
                loadUsers();
                break;

            case "userManagementError":

                showMessage(
                    data.message || "Unable to complete the operation."
                );
                break;
        }
    });

    // Load users when the page opens.
    loadUsers();

});
