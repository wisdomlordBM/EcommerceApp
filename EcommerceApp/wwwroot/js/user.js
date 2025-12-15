document.querySelectorAll(".order-btn").forEach(btn => {
    btn.addEventListener("click", () => {

        const id = btn.dataset.id;
        const name = btn.dataset.name;
        const price = Number(btn.dataset.price);
        const imageUrl = btn.dataset.image;

        const qtyInput = document.getElementById(`qty-${id}`);
        const qty = qtyInput ? Number(qtyInput.value) : 1;

        if (qty < 1) {
            alert("Quantity must be at least 1");
            return;
        }

        const total = qty * price;

        // WhatsApp number WITHOUT "+"
        const ownerPhone = "2348032110372";

        const message = encodeURIComponent(
            `Hello 👋
I want to order:

🛒 Product: ${name}
📦 Quantity: ${qty}B
💰 Total: ₦${total.toLocaleString()}

🖼 Image: ${window.location.origin}${imageUrl}`
        );

        const whatsappUrl = `https://wa.me/${ownerPhone}?text=${message}`;

        window.open(whatsappUrl, "_blank");
    });
});
