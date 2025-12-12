document.addEventListener("DOMContentLoaded", () => {
    // REGISTER HANDLER
    const registerForm = document.getElementById("registerForm");
    if (registerForm) {
        registerForm.addEventListener("submit", async (e) => {
            e.preventDefault();
            const model = {
                fullName: document.getElementById("fullName").value.trim(),
                email: document.getElementById("email").value.trim(),
                password: document.getElementById("password").value.trim(),
                phoneNumber: document.getElementById("phoneNumber").value.trim()
            };
            const response = await fetch("/Account/RegisterUser", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(model)
            });
            const result = await response.json();
            const output = document.getElementById("registerMsg");
            if (result.success) {
                output.innerHTML = `<div class="alert alert-success">${result.message}</div>`;
                setTimeout(() => window.location.href = "/Account/Login", 1500);
            } else {
                output.innerHTML = `<div class="alert alert-danger">${result.message}</div>`;
            }
        });
    }

    // LOGIN HANDLER
    const loginForm = document.getElementById("loginForm");
    if (loginForm) {
        loginForm.addEventListener("submit", async (e) => {
            e.preventDefault();
            const model = {
                email: document.getElementById("loginEmail").value.trim(),
                password: document.getElementById("loginPassword").value.trim()
            };
            const response = await fetch("/Account/LoginUser", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(model)
            });
            const result = await response.json();
            const output = document.getElementById("loginMsg");
            if (result.success) {
                output.innerHTML = `<div class="alert alert-success">Login successful</div>`;
                if (result.role === "Admin") {
                    window.location.href = "/Admin/Dashboard";
                } else {
                    window.location.href = "/User/Home";
                }
            } else {
                output.innerHTML = `<div class="alert alert-danger">${result.message}</div>`;
            }
        });
    }
});