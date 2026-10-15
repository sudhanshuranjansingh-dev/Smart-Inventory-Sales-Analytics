
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
    if (!response || typeof response !== "object") {
        console.error("Invalid current-user response:", response);
        return;
    }

    // Ignore any user response received while logout is pending.
    if (logoutPending) {
        return;
    }

    const user =
        response.currentUser ||
        response.user ||
        response.data ||
        response;

    const name = String(
        user.FullName ||
        user.fullName ||
        user.Name ||
        user.name ||
        user.Username ||
        user.username ||
        user.UserName ||
        user.userName ||
        ""
    ).trim();

    const role = String(
        user.Role ||
        user.role ||
        response.currentUserRole ||
        response.Role ||
        response.role ||
        ""
    ).trim().toLowerCase();

    if (!name || !role) {
        console.error(
            "Current-user response is missing name or role:",
            response
        );
        return;
    }

    const normalizedUsername = String(
        user.Username ||
        user.username ||
        user.UserName ||
        user.userName ||
        name
    ).trim();

    const sameUser =
        isLoggedIn &&
        currentUsername.toLowerCase() ===
            normalizedUsername.toLowerCase() &&
        currentUserRole === role;

    currentUserName = name;
    currentUsername = normalizedUsername;
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
        avatarElement.textContent =
            name.charAt(0).toUpperCase();
    }

    setLoggedOutState(false);
    applyRoleBasedNavigation(role);

    // Navigate only on a new login/session.
    // Duplicate current-user responses must not reload the iframe.
    if (!sameUser || !hasInitializedUser) {
        hasInitializedUser = true;
        navigate("dashboard");
    }

    console.log("Authenticated user:", {
        username: currentUsername,
        name: currentUserName,
        role: currentUserRole
    });
}

/* =========================================================
   NAVIGATION
   ========================================================= */

function navigate(pageName) {
    console.log("Sidebar clicked:", pageName);
    console.log("Session:", isLoggedIn, "Role:", currentUserRole);

    if (logoutPending) {
        console.warn("Navigation blocked: logout confirmation pending.");
        return;
    }

    if (!isLoggedIn || !currentUserRole) {
        console.warn("Navigation blocked: no active session.");
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

    currentPage = pageName;

    const pageTitle = document.getElementById("page-title");
    const pageSubtitle = document.getElementById("page-subtitle");
    const pageFrame = document.getElementById("page-frame");

    if (pageTitle) pageTitle.textContent = page.title;
    if (pageSubtitle) pageSubtitle.textContent = page.subtitle;

    document.querySelectorAll(".nav-item").forEach(function (button) {
        button.classList.remove("active");

        const command = button.getAttribute("onclick") || "";

        if (command.includes("'" + pageName + "'")) {
            button.classList.add("active");
        }
    });

    if (pageFrame && pageFrame.getAttribute("src") !== page.file) {
        pageFrame.setAttribute("src", page.file);
    }
}
/* =========================================================
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
    isLoggedIn = false;
    hasInitializedUser = false;

    resetUserHeader();
    setLoggedOutState(true);

    currentPage = "dashboard";

    const pageTitle = document.getElementById("page-title");
    const pageSubtitle = document.getElementById("page-subtitle");

    if (pageTitle) {
        pageTitle.textContent = "Login";
    }

    if (pageSubtitle) {
        pageSubtitle.textContent = "Please sign in to continue";
    }

    // Do not clear or reload the iframe here.
    // Let VB.NET handle the confirmed logout.
    sendToHost({
        action: "logoutConfirmed"
    });

    console.log("Logout completed.");
}


/* =========================================================
   SUPPORTED IFRAME ACTIONS
   ========================================================= */

const supportedActions = [
    "getCurrentUser",
    "loadDashboard",
    "loadProducts",
    "addProduct",
    "updateProduct",
    "deleteProduct",
    "loadCategories",
    "addCategory",
    "deleteCategory",
    "loadSuppliers",
    "addSupplier",
    "loadPurchases",
    "addPurchase",
    "loadInventory",
    "loadSales",
    "addSale",
    "loadProductAnalysis",
    "loadSalesAnalytics",
    "loadReports",
    "loadPendingAdmins",
    "approveAdmin",
    "rejectAdmin"
];

/* =========================================================
   RECEIVE IFRAME MESSAGES
   ========================================================= */

window.addEventListener("message", function (event) {
    const data = event.data;

    if (!data || typeof data !== "object" || !data.action) {
        return;
    }

    if (!supportedActions.includes(data.action)) {
        console.warn("Unsupported iframe action:", data.action);
        return;
    }

    if (!isLoggedIn || logoutPending) {
        console.warn("Action blocked: session inactive.", data.action);
        return;
    }

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

                if (
                    isLoggedIn &&
                    pageFrame &&
                    pageFrame.contentWindow
                ) {
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
