$(document).ready(function () {
    $('span[data-valmsg-for]').each(function () {
        if ($(this).text().trim().length > 0) {
            const fieldName = $(this).attr('data-valmsg-for');
            if (fieldName) {
                $(`[name="${fieldName}"]`).addClass('is-invalid');
            }
        }
    });

    let $form = $('form');

    $form.on('input change', 'input, select', function() {
        const form = $(this).closest('form');
        form.find('.is-invalid').removeClass('is-invalid');
        form.find('span[data-valmsg-for]').html('');
    });

    $form.on('submit', function() {
        if (!$(this).valid()) {
            $('.input-validation-error').addClass('is-invalid');
            return false;
        }
    });
});