
document.addEventListener("DOMContentLoaded", function () {

    const fullNameInput = document.getElementById("fullName");
    const usernameInput = document.getElementById("username");
    const passwordInput = document.getElementById("password");
    const confirmPasswordInput = document.getElementById("confirmPassword");
    const roleInput = document.getElementById("role");

    const btnRegister = document.getElementById("btnRegister");
    const btnBackLogin = document.getElementById("btnBackLogin");
    const message = document.getElementById("message");


    /* =====================================================
       SHOW MESSAGE
       ===================================================== */

    function showMessage(text, type = "error") {

        message.textContent = text;

        message.className = "";

        if (type === "error") {
            message.classList.add("error-message");
        } else if (type === "success") {
            message.classList.add("success-message");
        } else {
            message.classList.add("info-message");
        }

        message.setAttribute("role", type === "error" ? "alert" : "status");
        message.setAttribute("aria-live", "polite");

    }


    /* =====================================================
       CLEAR MESSAGE
       ===================================================== */

    function clearMessage() {

        message.textContent = "";
        message.className = "";
        message.removeAttribute("role");

    }


    /* =====================================================
       CLEAR ERROR WHEN USER STARTS CORRECTING INPUT
       ===================================================== */

    [
        fullNameInput,
        usernameInput,
        passwordInput,
        confirmPasswordInput,
        roleInput
    ].forEach(function (input) {

        input.addEventListener("input", clearMessage);
        input.addEventListener("change", clearMessage);

    });


    /* =====================================================
       SEND MESSAGE TO VB.NET WEBVIEW2
       ===================================================== */

    function sendToApplication(request) {

        if (window.chrome && window.chrome.webview) {

            window.chrome.webview.postMessage(request);
            return true;

        }

        showMessage(
            "Connection to the application is unavailable. Please reopen the application.",
            "error"
        );

        return false;

    }


    /* =====================================================
       REGISTER
       ===================================================== */

    btnRegister.addEventListener("click", function () {

        const fullName = fullNameInput.value.trim();
        const username = usernameInput.value.trim();
        const password = passwordInput.value;
        const confirmPassword = confirmPasswordInput.value;
        const role = roleInput.value;


        /* -------------------------------------------------
           VALIDATE FULL NAME
           ------------------------------------------------- */

        if (fullName === "") {

            showMessage("Please enter your full name.");
            fullNameInput.focus();
            return;

        }


        /* -------------------------------------------------
           VALIDATE USERNAME
           ------------------------------------------------- */

        if (username === "") {

            showMessage("Please enter a username.");
            usernameInput.focus();
            return;

        }


        /* -------------------------------------------------
           VALIDATE PASSWORD
           ------------------------------------------------- */

        if (password === "") {

            showMessage("Please enter a password.");
            passwordInput.focus();
            return;

        }


        /* -------------------------------------------------
           VALIDATE CONFIRM PASSWORD
           ------------------------------------------------- */

        if (confirmPassword === "") {

            showMessage("Please confirm your password.");
            confirmPasswordInput.focus();
            return;

        }


        /* -------------------------------------------------
           CHECK PASSWORD MATCH
           ------------------------------------------------- */

        if (password !== confirmPassword) {

            showMessage("Passwords do not match.");
            confirmPasswordInput.focus();
            confirmPasswordInput.select();
            return;

        }


        /* -------------------------------------------------
           VALIDATE ROLE
           ------------------------------------------------- */

        if (role === "") {

            showMessage("Please select a role.");
            roleInput.focus();
            return;

        }


        /* -------------------------------------------------
           CLEAR VALIDATION MESSAGE
           ------------------------------------------------- */

        clearMessage();


        /* -------------------------------------------------
           SEND REGISTRATION REQUEST TO VB.NET
           ------------------------------------------------- */

        sendToApplication({

            action: "register",
            fullName: fullName,
            username: username,
            password: password,
            role: role

        });

    });


    /* =====================================================
       BACK TO LOGIN
       ===================================================== */

    btnBackLogin.addEventListener("click", function () {

        sendToApplication({
            action: "backToLogin"
        });

    });


    /* =====================================================
       RECEIVE MESSAGE FROM VB.NET
       ===================================================== */

    if (window.chrome && window.chrome.webview) {

        window.chrome.webview.addEventListener("message", function (event) {

            const data = event.data;

            if (!data) {
                return;
            }

            if (typeof data.message === "string" && data.message.trim() !== "") {

                const text = data.message;
                const lowerText = text.toLowerCase();

                if (
                    lowerText.includes("success") ||
                    lowerText.includes("successful") ||
                    lowerText.includes("created")
                ) {
                    showMessage(text, "success");
                } else {
                    showMessage(text, "error");
                }

            }

        });

    }

});
