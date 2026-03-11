// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Hàm AJAX dùng chung để gom các luồng gọi API về một nơi xử lý.
window.appApi = {
    async request(url, options = {}) {
        const response = await fetch(url, {
            method: options.method || "GET",
            headers: {
                "Content-Type": "application/json",
                ...(options.headers || {})
            },
            body: options.body ? JSON.stringify(options.body) : undefined
        });

        let payload = null;
        const contentType = response.headers.get("content-type") || "";
        if (contentType.includes("application/json")) {
            payload = await response.json();
        } else {
            payload = await response.text();
        }

        if (!response.ok) {
            throw new Error(typeof payload === "string" ? payload : (payload?.message || "API request failed"));
        }

        return payload;
    },

    get(url, options = {}) {
        return this.request(url, { ...options, method: "GET" });
    },

    post(url, body, options = {}) {
        return this.request(url, { ...options, method: "POST", body });
    }
};
