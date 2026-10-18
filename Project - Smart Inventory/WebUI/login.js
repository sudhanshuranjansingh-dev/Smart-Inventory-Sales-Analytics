
document.addEventListener("DOMContentLoaded", function () {

    const usernameInput =
        document.getElementById("username");

    const passwordInput =
        document.getElementById("password");

    const btnLogin =
        document.getElementById("btnLogin");

    const btnRegister =
        document.getElementById("btnRegister");

    const btnExit =
        document.getElementById("btnExit");

    const message =
        document.getElementById("message");


    //=========================================================
    // CHECK REQUIRED ELEMENTS
    //=========================================================
    if (
        !usernameInput ||
        !passwordInput ||
        !btnLogin ||
        !btnRegister ||
        !btnExit ||
        !message
    ) {
        console.error(
            "Login page error: Required HTML elements are missing."
        );

        return;
    }


    //=========================================================
    // SHOW MESSAGE
    //=========================================================
    function showMessage(text) {

        const cleanText =
            typeof text === "string" ? text.trim() : "";

        message.textContent = cleanText;

        if (cleanText.length === 0) {
            message.style.setProperty(
                "display",
                "none",
                "important"
            );
            return;
        }

        message.style.removeProperty("display");
        message.style.display = "flex";
    }

    //=========================================================
    // LOGIN
    //=========================================================
    function login() {

        const username =
            usernameInput.value.trim();

        const password =
            passwordInput.value;


        if (username === "") {
            showMessage("Please enter your username.");
            usernameInput.focus();
            return;
        }


        if (password === "") {
            showMessage("Please enter your password.");
            passwordInput.focus();
            return;
        }


        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({

                action: "login",
                username: username,
                password: password

            });

        } else {

            showMessage(
                "WebView2 connection is not available."
            );

        }
    }


    btnLogin.addEventListener("click", login);


    //=========================================================
    // REGISTER
    //=========================================================
    btnRegister.addEventListener("click", function (event) {

        event.preventDefault();

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({
                action: "registerPage"
            });

        } else {

            showMessage(
                "WebView2 connection is not available."
            );

        }

    });


    //=========================================================
    // EXIT
    //=========================================================
    btnExit.addEventListener("click", function () {

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({
                action: "exit"
            });

        } else {

            showMessage(
                "WebView2 connection is not available."
            );

        }

    });


    //=========================================================
    // RECEIVE MESSAGE FROM VB.NET
    //=========================================================

    if (window.chrome && window.chrome.webview) {

        window.chrome.webview.addEventListener(
            "message",
            function (event) {

                console.log("VB.NET response received:", event.data);

                let data = event.data;

                // Handle JSON strings if VB.NET sends one
                if (typeof data === "string") {
                    try {
                        data = JSON.parse(data);
                    } catch {
                        showMessage(data);
                        return;
                    }
                }

                if (typeof data === "string") {
                    showMessage(data);
                    return;
                }

                if (!data || typeof data !== "object") {
                    showMessage("Unable to process the login response.");
                    return;
                }

                if (typeof data.message === "string") {
                    showMessage(data.message);
                } else if (typeof data.error === "string") {
                    showMessage(data.error);
                } else if (data.success === false) {
                    showMessage(
                        "Login failed. Please check your credentials."
                    );
                }

            }
        );

    }

    //=========================================================
    // ENTER KEY LOGIN
    //=========================================================
    usernameInput.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Enter") {
                event.preventDefault();
                login();
            }

        }
    );


    passwordInput.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Enter") {
                event.preventDefault();
                login();
            }

        }
    );

});