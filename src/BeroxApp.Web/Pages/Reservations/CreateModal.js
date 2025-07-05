abp.modals.ReservationCreateModal = function () {

    function initModal(modalManager, args) {
        var l = abp.localization.getResource('BeroxApp');

        var $form = modalManager.getForm();
        var $serviceSelect = $form.find('#Reservation_ServiceId');
        var $dateInput = $form.find('#Reservation_ReservationDate');
        var $timeSelect = $form.find('#ReservationTime');
        var $serviceInfo = $form.find('#ServiceInfo');
        var $serviceInfoText = $form.find('#ServiceInfoText');

        // Servis seçildiğinde bilgi göster
        $serviceSelect.on('change', function () {
            var selectedOption = $(this).find('option:selected');
            if (selectedOption.val()) {
                var price = selectedOption.data('price');
                var duration = selectedOption.data('duration');

                $serviceInfoText.html(
                    l('Price') + ': <strong>' + price + ' ₺</strong><br>' +
                    l('Duration') + ': <strong>' + duration + ' ' + l('Minutes') + '</strong>'
                );
                $serviceInfo.show();
            } else {
                $serviceInfo.hide();
            }
        });

        // Form submit edildiğinde tarih ve saati birleştir
        $form.on('submit', function (e) {
            var date = $dateInput.val();
            var time = $timeSelect.val();

            if (date && time) {
                var dateTime = date + 'T' + time + ':00';
                $dateInput.val(dateTime);
            }
        });

        // Form validation
        $form.validate({
            rules: {
                'Reservation.CustomerId': {
                    required: true
                },
                'Reservation.EmployeeId': {
                    required: true
                },
                'Reservation.ServiceId': {
                    required: true
                },
                'Reservation.ReservationDate': {
                    required: true
                },
                'ReservationTime': {
                    required: true
                }
            },
            messages: {
                'Reservation.CustomerId': l('ThisFieldIsRequired'),
                'Reservation.EmployeeId': l('ThisFieldIsRequired'),
                'Reservation.ServiceId': l('ThisFieldIsRequired'),
                'Reservation.ReservationDate': l('ThisFieldIsRequired'),
                'ReservationTime': l('ThisFieldIsRequired')
            }
        });

        // Bugünün tarihini default yap
        if (!$dateInput.val()) {
            var today = new Date().toISOString().split('T')[0];
            $dateInput.val(today);
        }
    };

    return {
        initModal: initModal
    };
};