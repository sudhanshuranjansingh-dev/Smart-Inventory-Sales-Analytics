
/* =========================================================
   SMART INVENTORY - MAIN NAVIGATION
   ========================================================= */

/* PAGE CONFIGURATION */

const pages = {
    dashboard: {
        file: "dashboard.html",
        title: "Dashboard",
        subtitle: "Overview of your inventory"
    },
    products: {
        file: "products.html",
        title: "Products",
        subtitle: "Manage products"
    },
    categories: {
        file: "categories.html",
        title: "Categories",
        subtitle: "Manage product categories"
    },
    suppliers: {
        file: "suppliers.html",
        title: "Suppliers",
        subtitle: "Manage suppliers"
    },
    purchases: {
        file: "purchases.html",
        title: "Purchases",
        subtitle: "Manage purchases"
    },
    sales: {
        file: "sales.html",
        title: "Sales",
        subtitle: "Manage sales"
    },
    "sales-history": {
        file: "sales-history.html",
        title: "Sales History",
        subtitle: "View previous sales"
    },
    inventory: {
        file: "inventory.html",
        title: "Inventory",
        subtitle: "Monitor stock levels"
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
        subtitle: "View business reports"
    },
    "admin-approval": {
        file: "admin-approval.html",
        title: "Admin Approval",
        subtitle: "Review administrator requests"
    },
    "user-management": {
        file: "user-management.html",
        title: "User Management",
        subtitle: "Manage registered accounts"
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
   PAGE PERMISSIONS
   ========================================================= */

// Pages hidden from Staff users.
const staffRestrictedPages = [
    "products",
    "categories",
    "suppliers",
    "product-analysis",
    "sales-analytics",
    "reports",
    "admin-approval",
    "user-management"
];

// Pages available only to administrators.
const adminOnlyPages = [
    "admin-approval",
    "user-management"
];

function isStaffRestrictedPage(pageName) {
    return (
        currentUserRole === "staff" &&
        staffRestrictedPages.includes(pageName)
    );
}

function isAdminOnlyPage(pageName) {
    return adminOnlyPages.includes(pageName);
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
   ========================================================= */

function setLoggedOutState(loggedOut) {
    const app = document.querySelector(".app");

    if (app) {
        app.classList.toggle("logged-out", loggedOut);
    }

    isLoggedIn = !loggedOut;
}


/* =========================================================
   NAVIGATION BUTTON HELPERS
   ========================================================= */

function getNavigationPageName(button) {
    const onclick = button.getAttribute("onclick") || "";

    const match = onclick.match(
        /navigate\(\s*['"]([^'"]+)['"]\s*\)/
    );

    return match ? match[1] : null;
}


/* =========================================================
   ROLE-BASED NAVIGATION
   ========================================================= */

function applyRoleBasedNavigation(role) {
    const normalizedRole = String(role || "")
        .trim()
        .toLowerCase();

    document.querySelectorAll(".nav-item").forEach(button => {
        const pageName = getNavigationPageName(button);

        if (!pageName) {
            return;
        }

        // Only Admin can see Admin-only navigation items.
        if (isAdminOnlyPage(pageName)) {
            button.style.display =
                normalizedRole === "admin" ? "" : "none";

            return;
        }

        // Hide restricted pages from Staff.
        if (
            normalizedRole === "staff" &&
            staffRestrictedPages.includes(pageName)
        ) {
            button.style.display = "none";
            return;
        }

        button.style.display = "";
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

    if (nameElement) {
        nameElement.textContent = "User";
    }

    if (roleElement) {
        roleElement.textContent = "User";
    }

    if (avatarElement) {
        avatarElement.textContent = "U";
    }

    document.querySelectorAll(".nav-item").forEach(button => {
        button.classList.remove("active");
        button.style.display = "";
    });
}


/* =========================================================
   APPLY AUTHENTICATED USER
   ========================================================= */

function updateUserHeader(response) {
    console.log("Current-user response received:", response);

    if (!response || typeof response !== "object") {
        console.error("Invalid current-user response.");
        return;
    }

    if (logoutPending) {
        return;
    }

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
            response
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

    if (nameElement) {
        nameElement.textContent = name;
    }

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
        console.warn("Navigation blocked: session is not initialized.");
        return;
    }

    const page = pages[pageName];

    if (!page) {
        console.error("Unknown page:", pageName);
        return;
    }

    // Enforce page permissions before loading.
    if (
        isAdminOnlyPage(pageName) &&
        currentUserRole !== "admin"
    ) {
        alert("Administrator access is required for this page.");
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

    if (title) {
        title.textContent = page.title;
    }

    if (subtitle) {
        subtitle.textContent = page.subtitle;
    }

    document.querySelectorAll(".nav-item").forEach(button => {
        button.classList.remove("active");

        if (getNavigationPageName(button) === pageName) {
            button.classList.add("active");
        }
    });

    if (pageFrame.getAttribute("src") !== page.file) {
        pageFrame.setAttribute("src", page.file);
    }

    console.log("Navigation successful:", page.file);
}


/* =========================================================
   LOGOUT REQUEST
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

// Called when the user cancels logout.
function logoutCancelled() {
    logoutPending = false;

    if (currentUserRole) {
        setLoggedOutState(false);
        applyRoleBasedNavigation(currentUserRole);
    }

    console.log("Logout cancelled. Session preserved.");
}


// Called when logout is confirmed.
function completeLogout() {
    logoutPending = false;

    isLoggedIn = false;
    currentUserRole = "";
    currentUserName = "";
    currentUsername = "";
    hasInitializedUser = false;

    resetUserHeader();
    setLoggedOutState(true);

    currentPage = "dashboard";

    const titleElement = document.getElementById("page-title");
    const subtitleElement = document.getElementById("page-subtitle");

    if (titleElement) {
        titleElement.textContent = "Login";
    }

    if (subtitleElement) {
        subtitleElement.textContent = "Please sign in to continue.";
    }

    const pageFrame = document.getElementById("page-frame");

    if (pageFrame) {
        pageFrame.setAttribute("src", "about:blank");
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
    window.chrome.webview.addEventListener("message", function (event) {
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

        // Do not forward messages from a previous session.
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
    });
}


/* =========================================================
   PAGE FRAME LOAD
   ========================================================= */

if (pageFrame) {
    pageFrame.addEventListener("load", function () {
        console.log(
            "Page loaded:",
            pageFrame.getAttribute("src")
        );
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
    ).forEach(element => element.remove());
}


/* =========================================================
   REQUEST CURRENT USER
   ========================================================= */

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
