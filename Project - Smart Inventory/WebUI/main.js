
/* =========================================================
   SMART INVENTORY - MAIN NAVIGATION
   ========================================================= */

/* PAGE CONFIGURATION */
const pages = {
    dashboard: {
        file: "dashboard.html",
        title: "Dashboard",
        subtitle: "Overview of your inventory and sales"
    },
    products: {
        file: "products.html",
        title: "Products",
        subtitle: "Manage your inventory products"
    },
    categories: {
        file: "categories.html",
        title: "Categories",
        subtitle: "Manage product categories"
    },
    suppliers: {
        file: "suppliers.html",
        title: "Suppliers",
        subtitle: "Manage your suppliers"
    },
    purchases: {
        file: "purchases.html",
        title: "Purchases",
        subtitle: "Manage purchase transactions"
    },
    sales: {
        file: "sales.html",
        title: "Sales",
        subtitle: "Manage sales transactions"
    },
    "sales-history": {
        file: "sales-history.html",
        title: "Sales History",
        subtitle: "View previous sales transactions"
    },
    inventory: {
        file: "inventory.html",
        title: "Inventory",
        subtitle: "Monitor your stock levels"
    },
    "product-analysis": {
        file: "product-analysis.html",
        title: "Product Analysis",
        subtitle: "Analyze product performance"
    },
    "sales-analytics": {
        file: "sales-analytics.html",
        title: "Sales Analytics",
        subtitle: "Analyze sales performance"
    },
    reports: {
        file: "reports.html",
        title: "Reports",
        subtitle: "Generate inventory and sales reports"
    },
    "admin-approval": {
        file: "admin-approval.html",
        title: "Admin Approval",
        subtitle: "Review and manage access requests"
    }
};

/* =========================================================
   SESSION STATE
   ========================================================= */

let currentUserRole = "";
let currentUserName = "";
let currentUsername = "";

let isLoggedIn = false;
let logoutPending = false;
let currentPage = "dashboard";
let hasInitializedUser = false;

/* =========================================================
   STAFF PERMISSIONS
   ========================================================= */

const staffRestrictedPages = [
    "products",
    "categories",
    "suppliers",
    "product-analysis",
    "sales-analytics",
    "reports",
    "admin-approval"
];

function isStaffRestrictedPage(pageName) {
    return (
        currentUserRole === "staff" &&
        staffRestrictedPages.includes(pageName)
    );
}

/* =========================================================
   WEBVIEW2 HELPER
   ========================================================= */

function sendToHost(message) {
    if (
        window.chrome &&
        window.chrome.webview
    ) {
        window.chrome.webview.postMessage(message);
        return true;
    }

    console.error("WebView2 is not available.");
    return false;
}

/* =========================================================
   APPLICATION VISIBILITY
   Requires .app.logged-out CSS below.
   ========================================================= */

function setLoggedOutState(loggedOut) {
    const app = document.querySelector(".app");

    if (app) {
        app.classList.toggle("logged-out", loggedOut);
    }

    isLoggedIn = !loggedOut;
}

/* =========================================================
   ROLE-BASED NAVIGATION
   ========================================================= */


function applyRoleBasedNavigation(role) {
    const normalizedRole = String(role || "").trim().toLowerCase();

    // Features restricted to Staff
    const staffRestrictedPages = [
        "products",
        "categories",
        "suppliers",
        "product-analysis",
        "sales-analytics",
        "reports",
        "admin-approval"
    ];

    // Admin Approval is only available to Admin users
    const adminOnlyPages = [
        "admin-approval"
    ];

    document.querySelectorAll(".nav-item").forEach(button => {
        const onclick = button.getAttribute("onclick") || "";
        const match = onclick.match(/navigate\(['"]([^'"]+)['"]\)/);

        if (!match) return;

        const pageName = match[1];

        if (adminOnlyPages.includes(pageName)) {
            button.style.display =
                normalizedRole === "admin" ? "" : "none";
        } else if (
            normalizedRole === "staff" &&
            staffRestrictedPages.includes(pageName)
        ) {
            button.style.display = "none";
        } else {
            button.style.display = "";
        }
    });
}

/* =========================================================
   RESET USER HEADER
   ========================================================= */

function resetUserHeader() {
    currentUserRole = "";
    currentUserName = "";
    currentUsername = "";

    const nameElement = document.getElementById("username");
    const roleElement = document.getElementById("user-role");
    const avatarElement = document.getElementById("user-avatar");

    if (nameElement) nameElement.textContent = "User";
    if (roleElement) roleElement.textContent = "User";
    if (avatarElement) avatarElement.textContent = "U";

    // Remove active selection without hiding sidebar buttons.
    document.querySelectorAll(".nav-item").forEach(function (button) {
        button.classList.remove("active");
        button.style.display = "";
    });
}
/* =========================================================
   APPLY AUTHENTICATED USER
   Do not reload the dashboard for duplicate responses.
   ========================================================= */
function updateUserHeader(response) {
    console.log("Current-user response received:", response);

    if (!response || typeof response !== "object") {
        console.error("Invalid current-user response.");
        return;
    }

    if (logoutPending) return;

    const user =
        response.currentUser ||
        response.user ||
        response.data ||
        response;

    const name = String(
        user.fullName ||
        user.FullName ||
        user.name ||
        user.Name ||
        user.username ||
        user.Username ||
        user.userName ||
        user.UserName ||
        ""
    ).trim();

    const username = String(
        user.username ||
        user.Username ||
        user.userName ||
        user.UserName ||
        name
    ).trim();

    const role = String(
        user.role ||
        user.Role ||
        response.currentUserRole ||
        ""
    ).trim().toLowerCase();

    if (!name || !role) {
        console.error(
            "Cannot initialize session: name or role is missing.",
            JSON.stringify(response, null, 2)
        );

        setLoggedOutState(true);
        return;
    }

    const sameUser =
        isLoggedIn &&
        currentUsername.toLowerCase() === username.toLowerCase() &&
        currentUserRole === role;

    currentUserName = name;
    currentUsername = username;
    currentUserRole = role;

    const nameElement = document.getElementById("username");
    const roleElement = document.getElementById("user-role");
    const avatarElement = document.getElementById("user-avatar");

    if (nameElement) nameElement.textContent = name;

    if (roleElement) {
        roleElement.textContent =
            role.charAt(0).toUpperCase() + role.slice(1);
    }

    if (avatarElement) {
        avatarElement.textContent = name.charAt(0).toUpperCase();
    }

    setLoggedOutState(false);
    applyRoleBasedNavigation(role);

    if (!sameUser || !hasInitializedUser) {
        hasInitializedUser = true;
        navigate("dashboard");
    }

    console.log("Session initialized:", {
        username: currentUsername,
        role: currentUserRole,
        isLoggedIn: isLoggedIn
    });
}
/* =========================================================
   NAVIGATION
   ========================================================= */

function navigate(pageName) {
    console.log("Sidebar clicked:", pageName);

    if (logoutPending) {
        console.warn("Navigation blocked: logout pending.");
        return;
    }

    if (!isLoggedIn || !currentUserRole) {
        console.warn(
            "Navigation blocked: session is not initialized.",
            { isLoggedIn, currentUserRole }
        );
        return;
    }

    const page = pages[pageName];

    if (!page) {
        console.error("Unknown page:", pageName);
        return;
    }

    if (isStaffRestrictedPage(pageName)) {
        alert("You do not have permission to access this page.");
        return;
    }

    const pageFrame = document.getElementById("page-frame");

    if (!pageFrame) {
        console.error("Cannot navigate: #page-frame was not found.");
        return;
    }

    currentPage = pageName;

    const title = document.getElementById("page-title");
    const subtitle = document.getElementById("page-subtitle");

    if (title) title.textContent = page.title;
    if (subtitle) subtitle.textContent = page.subtitle;

    document.querySelectorAll(".nav-item").forEach(function (button) {
        button.classList.remove("active");

        const onclick = button.getAttribute("onclick") || "";
        const match = onclick.match(/navigate\(['"]([^'"]+)['"]\)/);

        if (match && match[1] === pageName) {
            button.classList.add("active");
        }
    });

    if (pageFrame.getAttribute("src") !== page.file) {
        pageFrame.setAttribute("src", page.file);
    }

    console.log("Navigation successful:", page.file);
}/* =========================================================
   LOGOUT REQUEST
   Do not clear or modify the current screen before
   the user confirms logout in the VB.NET dialog.
   ========================================================= */



function logout() {
    if (!isLoggedIn || logoutPending) {
        return;
    }

    logoutPending = true;

    const sent = sendToHost({
        action: "logout"
    });

    if (!sent) {
        logoutPending = false;
    }
}


/* =========================================================
   LOGOUT RESPONSE HANDLERS
   ========================================================= */

// Called when the user selects NO in the VB.NET dialog.
function logoutCancelled() {
    logoutPending = false;

    // Preserve the existing authenticated session.
    if (currentUserRole) {
        setLoggedOutState(false);
        applyRoleBasedNavigation(currentUserRole);
    }

    console.log("Logout cancelled. Session preserved.");
    console.log("isLoggedIn:", isLoggedIn);
    console.log("currentUserRole:", currentUserRole);
}

// Called when the user selects YES in the VB.NET dialog.

function completeLogout() {
    logoutPending = false;

    // Clear the previous user's session in JavaScript.
    isLoggedIn = false;
    currentUserRole = "";
    currentUserName = "";
    currentUsername = "";
    hasInitializedUser = false;

    // Reset the sidebar and user header.
    resetUserHeader();
    setLoggedOutState(true);

    // Reset the visible page.
    currentPage = "dashboard";

    const titleElement = document.getElementById("page-title");
    const subtitleElement = document.getElementById("page-subtitle");

    if (titleElement) titleElement.textContent = "Login";
    if (subtitleElement) {
        subtitleElement.textContent = "Please sign in to continue.";
    }

    console.log("Logout complete. Previous user state cleared.");
}
/* =========================================================
   RECEIVE IFRAME MESSAGES
   ========================================================= */

window.addEventListener("message", function (event) {
    const frame = document.getElementById("page-frame");

    if (!frame || event.source !== frame.contentWindow) {
        return;
    }

    const data = event.data;

    if (!data || typeof data !== "object" || !data.action) {
        return;
    }

    if (logoutPending || !isLoggedIn) {
        console.warn("Iframe action blocked:", data.action);
        return;
    }

    console.log("Forwarding action to VB.NET:", data.action);
    sendToHost(data);
});
/* =========================================================
   RECEIVE VB.NET / WEBVIEW2 MESSAGES
   ========================================================= */

const pageFrame = document.getElementById("page-frame");

if (window.chrome && window.chrome.webview) {
    window.chrome.webview.addEventListener(
        "message",
        function (event) {
            const data = event.data;

            if (!data || typeof data !== "object") {
                return;
            }

            const messageType = String(
                data.type || data.action || ""
            ).toLowerCase();

            if (messageType === "currentuser") {
                updateUserHeader(data);

                if (isLoggedIn && pageFrame && pageFrame.contentWindow) {
                    pageFrame.contentWindow.postMessage(data, "*");
                }

                return;
            }

            if (
                messageType === "logoutcancelled" ||
                messageType === "logoutcanceled"
            ) {
                logoutCancelled();
                return;
            }

            if (
                messageType === "logoutcomplete" ||
                messageType === "logoutconfirmed"
            ) {
                completeLogout();
                return;
            }

            // Never forward old session data after logout.
            if (!isLoggedIn || logoutPending) {
                return;
            }

            if (
                data.type &&
                pageFrame &&
                pageFrame.contentWindow
            ) {
                pageFrame.contentWindow.postMessage(data, "*");
            }
        }
    );
}

/* =========================================================
   PAGE FRAME LOAD
   ========================================================= */

if (pageFrame) {
    pageFrame.addEventListener("load", function () {
        console.log("Page loaded:", pageFrame.getAttribute("src"));
    });
}

/* =========================================================
   HIDE NOTIFICATION ELEMENTS
   ========================================================= */

function hideNotificationBell() {
    document.querySelectorAll(
        ".notification, .notification-btn, " +
        ".notification-button, .notification-icon, " +
        ".notification-bell, #notification, " +
        "#notification-btn, #notification-button, " +
        "#notification-bell"
    ).forEach(function (element) {
        element.remove();
    });
}



function requestCurrentUser() {
    if (!window.chrome?.webview) {
        console.error("WebView2 is not available.");
        return;
    }

    console.log("Requesting current user from VB.NET...");

    window.chrome.webview.postMessage({
        action: "getCurrentUser"
    });
}



/* =========================================================
   INITIALIZATION
   ========================================================= */

document.addEventListener("DOMContentLoaded", function () {
    hideNotificationBell();

    resetUserHeader();
    setLoggedOutState(true);

    requestCurrentUser();
});
