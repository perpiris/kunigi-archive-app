$(document).ready(function () {
    function updateSubmitButton() {
        let hasErrors = $('.is-invalid').length > 0 ||
            $('span[data-valmsg-for]').filter(function() {
                return $(this).text().trim().length > 0;
            }).length > 0;

        $('button[type="submit"]').prop('disabled', hasErrors);
    }

    $('span[data-valmsg-for]').each(function () {
        if ($(this).text().trim().length > 0) {
            const fieldName = $(this).attr('data-valmsg-for');
            if (fieldName) {
                $(`[name="${fieldName}"]`).addClass('is-invalid');
            }
        }
    });

    updateSubmitButton();

    $('input, select').on('input change', function() {
        const field = $(this);
        const fieldName = field.attr('name');

        field.removeClass('is-invalid');

        if (fieldName) {
            $(`span[data-valmsg-for="${fieldName}"]`).html('');
        }

        setTimeout(updateSubmitButton, 100);
    });

    $('form').on('submit', function() {
        if (!$(this).valid()) {
            $('.input-validation-error').addClass('is-invalid');
            updateSubmitButton();
            return false;
        }
    });
});