async function orderProduct(id, name, price, imageUrl) {
    const qty = document.getElementById(`qty-${id}`).value;
    const total = qty * price;
    // Fetch owner phone (assume we get it via API; for now, add an endpoint or hardcode)
    // For real: Make an AJAX to get owner phone by product.OwnerId
    const ownerPhone = "+1234567890"; // Replace with real fetch, e.g., await fetch(`/GetOwnerPhone/${id}`)
    const message = encodeURIComponent(`I want to order ${qty} of ${name} for $${total}. Image: ${imageUrl}`);
    window.location.href = `https://wa.me/${ownerPhone}?text=${message}`;
}