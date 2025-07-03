abp.modals.CustomerCreateEditModal = function () {

    function initModal(modalManager, args) {
        var l = abp.localization.getResource('BeroxApp');

        var $form = modalManager.getForm();

        // Telefon numarası formatlaması (opsiyonel)
        var $phoneInput = $form.find('input[name="Customer.PhoneNumber"]');
        $phoneInput.on('input', function () {
            var value = $(this).val().replace(/\D/g, '');
            if (value.length > 10) {
                value = value.substring(0, 10);
            }
            $(this).val(value);
        });

        // Form validation
        $form.validate({
            rules: {
                'Customer.FirstName': {
                    required: true,
                    maxlength: 64
                },
                'Customer.LastName': {
                    required: true,
                    maxlength: 64
                },
                'Customer.PhoneNumber': {
                    required: true,
                    minlength: 10,
                    maxlength: 10,
                    digits: true
                },
                'Customer.Email': {
                    email: true,
                    maxlength: 256
                },
                'Customer.Notes': {
                    maxlength: 1024
                }
            },
            messages: {
                'Customer.PhoneNumber': {
                    minlength: l('PhoneNumberMustBe10Digits'),
                    maxlength: l('PhoneNumberMustBe10Digits'),
                    digits: l('PhoneNumberMustBeDigits')
                }
            }
        });
    };

    return {
        initModal: initModal
    };
};