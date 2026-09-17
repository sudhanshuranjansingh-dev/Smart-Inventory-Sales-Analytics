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
    // SHOW MESSAGE
    //=========================================================
    function showMessage(text) {

        message.textContent = text;

    }


    //=========================================================
    // LOGIN
    //=========================================================
    btnLogin.addEventListener("click", function () {

        const username =
            usernameInput.value.trim();

        const password =
            passwordInput.value;


        // Validate username
        if (username === "") {

            showMessage(
                "Please enter your username."
            );

            usernameInput.focus();

            return;
        }


        // Validate password
        if (password === "") {

            showMessage(
                "Please enter your password."
            );

            passwordInput.focus();

            return;
        }


        // Check WebView2
        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({

                action: "login",

                username: username,

                password: password

            });

        }
        else {

            showMessage(
                "WebView2 connection is not available."
            );

        }

    });


    //=========================================================
    // REGISTER
    //=========================================================
   btnRegister.addEventListener(
    "click",
    function () {

        if (
            window.chrome &&
            window.chrome.webview
        ) {

            window.chrome.webview.postMessage({

                action: "registerPage"

            });

        }
        else {

            showMessage(
                "WebView2 connection is not available."
            );

        }

    }
);


    //=========================================================
    // EXIT
    //=========================================================
    btnExit.addEventListener(
        "click",
        function () {

            if (
                window.chrome &&
                window.chrome.webview
            ) {

                window.chrome.webview.postMessage({

                    action: "exit"

                });

            }
            else {

                showMessage(
                    "WebView2 connection is not available."
                );

            }

        }
    );


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
                    event.data.message
                ) {

                    showMessage(
                        event.data.message
                    );

                }

            }
        );

    }


    //=========================================================
    // ENTER KEY LOGIN
    //=========================================================
    passwordInput.addEventListener(
        "keydown",
        function (event) {

            if (event.key === "Enter") {

                btnLogin.click();

            }

        }
    );

});