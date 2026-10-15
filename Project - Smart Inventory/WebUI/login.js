
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
        message.textContent = text;
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
    if (
        window.chrome &&
        window.chrome.webview
    ) {

        window.chrome.webview.addEventListener(
            "message",
            function (event) {

                if (
                    event.data &&
                    typeof event.data.message === "string"
                ) {

                    showMessage(event.data.message);

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