document.addEventListener(
    "DOMContentLoaded",
    function () {

        const fullNameInput =
            document.getElementById("fullName");

        const usernameInput =
            document.getElementById("username");

        const passwordInput =
            document.getElementById("password");

        const confirmPasswordInput =
            document.getElementById("confirmPassword");

        const roleInput =
            document.getElementById("role");

        const btnRegister =
            document.getElementById("btnRegister");

        const btnBackLogin =
            document.getElementById("btnBackLogin");

        const message =
            document.getElementById("message");


        /* =====================================================
           SHOW MESSAGE
           ===================================================== */

        function showMessage(text) {

            message.textContent = text;

        }


        /* =====================================================
           REGISTER
           ===================================================== */

        btnRegister.addEventListener(
            "click",
            function () {

                const fullName =
                    fullNameInput.value.trim();

                const username =
                    usernameInput.value.trim();

                const password =
                    passwordInput.value;

                const confirmPassword =
                    confirmPasswordInput.value;

                const role =
                    roleInput.value;


                /* -------------------------------------------------
                   VALIDATION
                   ------------------------------------------------- */

                if (fullName === "") {

                    showMessage(
                        "Please enter your full name."
                    );

                    fullNameInput.focus();

                    return;
                }


                if (username === "") {

                    showMessage(
                        "Please enter a username."
                    );

                    usernameInput.focus();

                    return;
                }


                if (password === "") {

                    showMessage(
                        "Please enter a password."
                    );

                    passwordInput.focus();

                    return;
                }


                if (confirmPassword === "") {

                    showMessage(
                        "Please confirm your password."
                    );

                    confirmPasswordInput.focus();

                    return;
                }


                if (password !== confirmPassword) {

                    showMessage(
                        "Passwords do not match."
                    );

                    confirmPasswordInput.focus();

                    return;
                }


                if (role === "") {

                    showMessage(
                        "Please select a role."
                    );

                    roleInput.focus();

                    return;
                }


                /* -------------------------------------------------
                   WEBVIEW2
                   ------------------------------------------------- */

                if (
                    window.chrome &&
                    window.chrome.webview
                ) {

                    window.chrome.webview.postMessage({

                        action: "register",

                        fullName: fullName,

                        username: username,

                        password: password,

                        role: role

                    });

                }
                else {

                    showMessage(
                        "WebView2 connection is not available."
                    );

                }

            }
        );


        /* =====================================================
           BACK TO LOGIN
           ===================================================== */

        btnBackLogin.addEventListener(
            "click",
            function () {

                if (
                    window.chrome &&
                    window.chrome.webview
                ) {

                    window.chrome.webview.postMessage({

                        action: "backToLogin"

                    });

                }
                else {

                    showMessage(
                        "WebView2 connection is not available."
                    );

                }

            }
        );


        /* =====================================================
           RECEIVE MESSAGE FROM VB.NET
           ===================================================== */

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.addEventListener(
                "message",
                function (event) {

                    if (
                        event.data &&
                        event.data.message
                    ) {

                        showMessage(
                            event.data.message
                        );

                    }

                }
            );

        }

    }
);