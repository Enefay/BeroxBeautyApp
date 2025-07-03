abp.modals.EmployeeCreateEditModal = function () {

    function initModal(modalManager, args) {
        var l = abp.localization.getResource('BeroxApp');

        var $form = modalManager.getForm();

        // Telefon numarası formatlaması
        var $phoneInput = $form.find('input[name="Employee.PhoneNumber"]');
        $phoneInput.on('input', function () {
            var value = $(this).val().replace(/\D/g, '');
            if (value.length > 10) {
                value = value.substring(0, 10);
            }
            $(this).val(value);
        });

        // Maaş formatlaması
        var $salaryInput = $form.find('input[name="Employee.MonthlySalary"]');
        $salaryInput.on('blur', function () {
            var value = parseFloat($(this).val());
            if (!isNaN(value)) {
                $(this).val(value.toFixed(2));
            }
        });

        // Form validation
        $form.validate({
            rules: {
                'Employee.FirstName': {
                    required: true,
                    maxlength: 64
                },
                'Employee.LastName': {
                    required: true,
                    maxlength: 64
                },
                'Employee.PhoneNumber': {
                    minlength: 10,
                    maxlength: 10,
                    digits: true
                },
                'Employee.Email': {
                    email: true,
                    maxlength: 256
                },
                'Employee.MonthlySalary': {
                    required: true,
                    min: 0,
                    number: true
                }
            },
            messages: {
                'Employee.PhoneNumber': {
                    minlength: l('PhoneNumberMustBe10Digits'),
                    maxlength: l('PhoneNumberMustBe10Digits'),
                    digits: l('PhoneNumberMustBeDigits')
                },
                'Employee.MonthlySalary': {
                    min: l('SalaryMustBePositive')
                }
            }
        });
    };

    return {
        initModal: initModal
    };
};