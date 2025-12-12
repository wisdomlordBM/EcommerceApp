// CREATE PRODUCT
const createProductForm = document.getElementById("createProductForm");
if (createProductForm) {
    createProductForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const model = {
            name: document.getElementById("name").value,
            description: document.getElementById("description").value,
            price: parseFloat(document.getElementById("price").value),
            stock: parseInt(document.getElementById("stock").value),
            category: document.getElementById("category").value,
            imageUrl: document.getElementById("imageUrl").value
        };
        const response = await fetch("/Admin/CreateProduct", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(model)
        });
        const result = await response.json();
        const output = document.getElementById("createProductMsg");
        output.innerHTML = result.success ? `<div class="alert alert-success">${result.message}</div>` : `<div class="alert alert-danger">${result.message}</div>`;
    });
}

// EDIT PRODUCT (similar)
const editProductForm = document.getElementById("editProductForm");
if (editProductForm) {
    editProductForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const model = {
            id: parseInt(document.getElementById("id").value),
            name: document.getElementById("name").value,
            description: document.getElementById("description").value,
            price: parseFloat(document.getElementById("price").value),
            stock: parseInt(document.getElementById("stock").value),
            category: document.getElementById("category").value,
            imageUrl: document.getElementById("imageUrl").value
        };
        const response = await fetch("/Admin/UpdateProduct", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(model)
        });
        const result = await response.json();
        const output = document.getElementById("editProductMsg");
        output.innerHTML = result.success ? `<div class="alert alert-success">${result.message}</div>` : `<div class="alert alert-danger">${result.message}</div>`;
    });
}

// DELETE PRODUCT
async function deleteProduct(id) {
    if (confirm("Delete this product?")) {
        const response = await fetch(`/Admin/DeleteProduct/${id}`, { method: "POST" });
        const result = await response.json();
        alert(result.message);
        window.location.reload();
    }
}