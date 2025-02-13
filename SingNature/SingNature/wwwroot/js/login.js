document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById("login-form");
    const usernameInput = document.getElementById("username");
    const passwordInput = document.getElementById("password");

    loginForm.addEventListener("submit", async function (event) {
        event.preventDefault();

        const username = usernameInput.value.trim();
        const password = passwordInput.value.trim();

        if (!username || !password) {
            alert("Username and password are required!");
            return;
        }

        try {
           
            const response = await axios.post("https://localhost:5076/api/auth/login", {
                userName: username,
                password: password
            }, { withCredentials: true });

            if (response.status === 200) {
                localStorage.setItem("user", JSON.stringify(response.data));
                alert("Login successful!");

                window.location.href = "/User/Profile";
            }
        } catch (error) {
            console.error("Login error:", error);
            alert("Invalid username or password");
        }
    });
});

function redirectToRegister() {
    window.location.href = "/User/Register"; // 跳转到注册页面
}
