function toggleSizePrice(checkbox) {
    const index = checkbox.getAttribute('data-index');
    const priceInput = document.getElementById(`inputSize_${index}`);

    if (checkbox.checked) {
        priceInput.disabled = true;
        priceInput.value = '';
    } else {
        priceInput.disabled = false;
    }
}
