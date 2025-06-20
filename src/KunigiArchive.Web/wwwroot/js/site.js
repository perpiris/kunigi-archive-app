document.addEventListener('DOMContentLoaded', function() {
    function updateValidationClasses() {
        const allFields = document.querySelectorAll('input, select, textarea');
        allFields.forEach(function(field) {
            const fieldName = field.name || field.id;
            if (!fieldName) {
                return;
            }

            const validationSpan = document.querySelector(`span[data-valmsg-for="${fieldName}"]`);
            const hasError = validationSpan && validationSpan.textContent.trim() !== '';

            if (hasError) {
                field.classList.add('is-invalid');
            } else {
                field.classList.remove('is-invalid');
            }
        });
    }

    const observer = new MutationObserver(function(mutations) {
        updateValidationClasses();
    });

    observer.observe(document.body, {
        childList: true,
        subtree: true,
        characterData: true
    });

    updateValidationClasses();
});