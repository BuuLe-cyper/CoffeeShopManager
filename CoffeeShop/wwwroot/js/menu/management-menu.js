function submitForm() {
    var form = document.getElementById('productForm');
    if (form) {
        form.submit();
    } else {
        console.error('Form not found');
    }
}

function toggleSizePrice(checkbox) {
    const index = checkbox.getAttribute('data-index');
    const originalPriceInput = document.querySelector(`input[name="ProductSize.SizePrices[${index}].OriginalPrice"]`);
    const priceInput = document.querySelector(`input[name="ProductSize.SizePrices[${index}].Price"]`);

    if (checkbox.checked) {
        originalPriceInput.disabled = false;
        priceInput.disabled = false;
    } else {
        originalPriceInput.disabled = true;
        priceInput.disabled = true;
        originalPriceInput.value = '';
        priceInput.value = '';
    }
}

function checkPrice(priceInput) {
    const index = priceInput.getAttribute('data-index');
    const originalPriceInput = document.querySelector(`input[name="ProductSize.SizePrices[${index}].OriginalPrice"]`);

    if (priceInput.value && originalPriceInput.value) {
        const originalPrice = parseFloat(originalPriceInput.value);
        const price = parseFloat(priceInput.value);

        if (price < originalPrice) {
            const modal = new bootstrap.Modal(document.getElementById('priceModal'));
            modal.show();
        }
    }
}