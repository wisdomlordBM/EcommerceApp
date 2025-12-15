/* =========================
   IMAGE PREVIEW (REUSABLE)
========================= */
function setupImagePreview(inputId, previewId) {
    const input = document.getElementById(inputId);
    const preview = document.getElementById(previewId);

    if (!input || !preview) return;

    input.addEventListener("change", () => {
        const file = input.files[0];
        if (!file) return;

        preview.src = URL.createObjectURL(file);
        preview.style.display = "block";
    });
}

// Init image preview
setupImagePreview("imageFile", "imagePreview");

/* =========================
   CREATE PRODUCT
========================= */
const createProductForm = document.getElementById("createProductForm");

if (createProductForm) {
    createProductForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const output = document.getElementById("createProductMsg");
        const formData = new FormData(createProductForm);

        try {
            const response = await fetch("/Admin/CreateProduct", {
                method: "POST",
                body: formData
            });

            const result = await response.json();

            output.innerHTML = `
                <div class="alert alert-${result.success ? "success" : "danger"}">
                    ${result.message}
                </div>`;

            if (result.success) {
                setTimeout(() => {
                    window.location.href = "/Admin/Products";
                }, 1200);
            }
        } catch {
            output.innerHTML = `<div class="alert alert-danger">Something went wrong.</div>`;
        }
    });
}

/* =========================
   EDIT PRODUCT
========================= */
const editProductForm = document.getElementById("editProductForm");

if (editProductForm) {
    editProductForm.addEventListener("submit", async (e) => {
        e.preventDefault();

        const output = document.getElementById("editProductMsg");
        const formData = new FormData(editProductForm);

        try {
            const response = await fetch("/Admin/UpdateProduct", {
                method: "POST",
                body: formData
            });

            const result = await response.json();

            output.innerHTML = `
                <div class="alert alert-${result.success ? "success" : "danger"}">
                    ${result.message}
                </div>`;

            if (result.success) {
                setTimeout(() => {
                    window.location.href = "/Admin/Products";
                }, 1200);
            }
        } catch {
            output.innerHTML = `<div class="alert alert-danger">Something went wrong.</div>`;
        }
    });
}

/* =========================
   DELETE PRODUCT
========================= */
async function deleteProduct(id) {
    if (!confirm("Delete this product?")) return;

    try {
        const response = await fetch(`/Admin/DeleteProduct/${id}`, {
            method: "POST"
        });

        const result = await response.json();
        alert(result.message);

        if (result.success) {
            window.location.reload();
        }
    } catch {
        alert("Failed to delete product.");
    }
}
