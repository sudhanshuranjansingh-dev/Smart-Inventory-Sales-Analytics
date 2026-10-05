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
                    No pending Admin requests.
                </td>
            </tr>
        `;

    }


    /* =====================================================
       LOAD PENDING ADMIN REQUESTS
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
       APPROVE ADMIN
       ===================================================== */

    function approveAdmin(userID) {

        if (!userID) {
            return;
        }

        const confirmed =
            confirm(
                "Are you sure you want to approve this Admin request?"
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
       REJECT ADMIN
       ===================================================== */

    function rejectAdmin(userID) {

        if (!userID) {
            return;
        }

        const confirmed =
            confirm(
                "Are you sure you want to reject this Admin request?"
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

        if (
            !requests ||
            requests.length === 0
        ) {

            showEmpty();

            return;
        }


        requests.forEach(function (user) {

            const row =
                document.createElement("tr");


            const userID =
                user.UserID ?? "-";

            const username =
                user.Username ?? "-";

            const fullName =
                user.FullName ?? "-";

            const role =
                user.Role ?? "-";

            const accountStatus =
                user.AccountStatus ?? "-";

            const requested =
                user.CreatedAt ?? "-";


            row.innerHTML = `
                <td>
                    ${escapeHtml(userID)}
                </td>

                <td>
                    ${escapeHtml(username)}
                </td>

                <td>
                    ${escapeHtml(fullName)}
                </td>

                <td>
                    ${escapeHtml(role)}
                </td>

                <td>
                    <span class="status pending">
                        ${escapeHtml(accountStatus)}
                    </span>
                </td>

                <td>
                    ${escapeHtml(requested)}
                </td>

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

        });


        /* =================================================
           APPROVE BUTTONS
           ================================================= */

        const approveButtons =
            tableBody.querySelectorAll(
                ".btn-approve"
            );


        approveButtons.forEach(function (button) {

            button.addEventListener(
                "click",
                function () {

                    const userID =
                        parseInt(
                            button.dataset.userId,
                            10
                        );

                    approveAdmin(userID);

                }
            );

        });


        /* =================================================
           REJECT BUTTONS
           ================================================= */

        const rejectButtons =
            tableBody.querySelectorAll(
                ".btn-reject"
            );


        rejectButtons.forEach(function (button) {

            button.addEventListener(
                "click",
                function () {

                    const userID =
                        parseInt(
                            button.dataset.userId,
                            10
                        );

                    rejectAdmin(userID);

                }
            );

        });

    }


    /* =====================================================
       ESCAPE HTML
       ===================================================== */

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


    /* =====================================================
       RECEIVE MESSAGES FROM FRMMAIN
       ===================================================== */

    window.addEventListener(
        "message",
        function (event) {

            if (!event.data) {
                return;
            }


            const data =
                event.data;


            /* ---------------------------------------------
               PENDING ADMIN DATA
               --------------------------------------------- */

            if (
                data.type === "pendingAdmins"
            ) {

                renderRequests(
                    data.data || []
                );

                return;
            }


            /* ---------------------------------------------
               SUCCESS MESSAGE
               --------------------------------------------- */

            if (
                data.type === "adminApprovalMessage"
            ) {

                showMessage(
                    data.message || ""
                );


                if (
                    data.message &&
                    (
                        data.message
                            .toLowerCase()
                            .includes("approved") ||

                        data.message
                            .toLowerCase()
                            .includes("rejected")
                    )
                ) {

                    setTimeout(
                        function () {

                            loadPendingAdmins();

                        },
                        300
                    );

                }

                return;
            }


            /* ---------------------------------------------
               ERROR MESSAGE
               --------------------------------------------- */

            if (
                data.type === "adminApprovalError"
            ) {

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