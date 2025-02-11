document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById("login-form");
    const emailInput = document.getElementById("email");
    const passwordInput = document.getElementById("password");
    const errorMessage = document.getElementById("error-message");

    loginForm.addEventListener("submit", async function (event) {
        event.preventDefault();
        errorMessage.textContent = "";

        const email = emailInput.value.trim();
        const password = passwordInput.value.trim();

        if (!email || !password) {
            errorMessage.textContent = "Email and password are required!";
            return;
        }

        try {
            const response = await axios.post("http://localhost:5076/api/auth/login", {
                username: email,
                password: password
            });

            localStorage.setItem("user", JSON.stringify(response.data));
            alert("Login successful!");
            window.location.href = "/dashboard.html"; // 跳转到用户主页
        } catch (error) {
            errorMessage.textContent = "Invalid username or password";
        }
    });
});
