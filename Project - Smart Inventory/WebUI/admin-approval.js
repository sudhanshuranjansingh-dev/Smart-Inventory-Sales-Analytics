
/* =========================================================
   SMART INVENTORY
   ADMIN APPROVAL
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {

    /* =====================================================
       GET ELEMENTS
       ===================================================== */

    const tableBody =
        document.getElementById("approvalTableBody");

    const refreshButton =
        document.getElementById("btnRefresh");

    const message =
        document.getElementById("message");


    /* =====================================================
       SHOW MESSAGE
       ===================================================== */

    function showMessage(text) {
        if (message) {
            message.textContent = text;
        }
    }


    /* =====================================================
       SHOW LOADING
       ===================================================== */

    function showLoading() {
        if (!tableBody) {
            return;
        }

        tableBody.innerHTML = `
            <tr>
                <td colspan="7" class="loading">
                    Loading requests...
                </td>
            </tr>
        `;
    }


    /* =====================================================
       SHOW EMPTY
       ===================================================== */

    function showEmpty() {
        if (!tableBody) {
            return;
        }

        tableBody.innerHTML = `
            <tr>
                <td colspan="7" class="empty-message">
                    No pending requests.
                </td>
            </tr>
        `;
    }


    /* =====================================================
       LOAD PENDING REQUESTS
       ===================================================== */

    function loadPendingAdmins() {
        showLoading();
        showMessage("");

        if (
            window.parent &&
            window.parent !== window
        ) {
            window.parent.postMessage(
                {
                    action: "loadPendingAdmins"
                },
                "*"
            );
        }
    }


    /* =====================================================
       APPROVE REQUEST
       ===================================================== */

    function approveAdmin(userID, userRole) {

        if (!userID) {
            return;
        }

        const role = String(userRole || "User").trim();

        const confirmed = confirm(
            `Are you sure you want to approve this ${role} request?`
        );

        if (!confirmed) {
            return;
        }

        window.parent.postMessage(
            {
                action: "approveAdmin",
                userID: userID
            },
            "*"
        );
    }


    /* =====================================================
       REJECT REQUEST
       ===================================================== */

    function rejectAdmin(userID, userRole) {

        if (!userID) {
            return;
        }

        const role = String(userRole || "User").trim();

        const confirmed = confirm(
            `Are you sure you want to reject this ${role} request?`
        );

        if (!confirmed) {
            return;
        }

        window.parent.postMessage(
            {
                action: "rejectAdmin",
                userID: userID
            },
            "*"
        );
    }


    /* =====================================================
       RENDER REQUESTS
       ===================================================== */

    function renderRequests(requests) {

        if (!tableBody) {
            return;
        }

        tableBody.innerHTML = "";

        if (!requests || requests.length === 0) {
            showEmpty();
            return;
        }

        requests.forEach(function (user) {

            const row = document.createElement("tr");

            const userID = user.UserID ?? "-";
            const username = user.Username ?? "-";
            const fullName = user.FullName ?? "-";
            const role = user.Role ?? "User";
            const accountStatus = user.AccountStatus ?? "-";
            const requested = user.CreatedAt ?? "-";

            row.innerHTML = `
                <td>${escapeHtml(userID)}</td>

                <td>${escapeHtml(username)}</td>

                <td>${escapeHtml(fullName)}</td>

                <td>${escapeHtml(role)}</td>

                <td>
                    <span class="status pending">
                        ${escapeHtml(accountStatus)}
                    </span>
                </td>

                <td>${escapeHtml(requested)}</td>

                <td>
                    <div class="action-buttons">

                        <button
                            type="button"
                            class="btn-approve"
                            data-user-id="${escapeHtml(userID)}">
                            Approve
                        </button>

                        <button
                            type="button"
                            class="btn-reject"
                            data-user-id="${escapeHtml(userID)}">
                            Reject
                        </button>

                    </div>
                </td>
            `;

            tableBody.appendChild(row);


            /* =============================================
               APPROVE BUTTON
               ============================================= */

            const approveButton =
                row.querySelector(".btn-approve");

            approveButton.addEventListener(
                "click",
                function () {

                    const id = parseInt(
                        this.dataset.userId,
                        10
                    );

                    approveAdmin(id, role);
                }
            );


            /* =============================================
               REJECT BUTTON
               ============================================= */

            const rejectButton =
                row.querySelector(".btn-reject");

            rejectButton.addEventListener(
                "click",
                function () {

                    const id = parseInt(
                        this.dataset.userId,
                        10
                    );

                    rejectAdmin(id, role);
                }
            );

        });
    }


    /* =====================================================
       ESCAPE HTML
       ===================================================== */

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


    /* =====================================================
       RECEIVE MESSAGES FROM FRMMAIN
       ===================================================== */

    window.addEventListener(
        "message",
        function (event) {

            if (!event.data) {
                return;
            }

            const data = event.data;


            /* =============================================
               PENDING REQUEST DATA
               ============================================= */

            if (data.type === "pendingAdmins") {

                renderRequests(data.data || []);
                return;
            }


            /* =============================================
               SUCCESS MESSAGE
               ============================================= */

            if (data.type === "adminApprovalMessage") {

                showMessage(data.message || "");

                if (
                    data.message &&
                    (
                        data.message.toLowerCase().includes("approved") ||
                        data.message.toLowerCase().includes("rejected")
                    )
                ) {
                    setTimeout(function () {
                        loadPendingAdmins();
                    }, 300);
                }

                return;
            }


            /* =============================================
               ERROR MESSAGE
               ============================================= */

            if (data.type === "adminApprovalError") {

                showMessage(
                    data.message ||
                    "Unable to process request."
                );

                return;
            }

        }
    );


    /* =====================================================
       REFRESH BUTTON
       ===================================================== */

    if (refreshButton) {

        refreshButton.addEventListener(
            "click",
            function () {
                loadPendingAdmins();
            }
        );
    }


    /* =====================================================
       INITIAL LOAD
       ===================================================== */

    loadPendingAdmins();

});
