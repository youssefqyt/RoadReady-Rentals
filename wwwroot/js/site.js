// Site-specific JavaScript
document.addEventListener('DOMContentLoaded', () => {
    // Auto-dismiss alerts after 5 seconds
    const alertEl = document.querySelector('.alert');
    if (alertEl) {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alertEl);
            bsAlert.close();
        }, 5000);
    }
});
