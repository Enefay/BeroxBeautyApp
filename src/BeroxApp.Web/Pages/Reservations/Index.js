$(function () {
    var l = abp.localization.getResource('BeroxApp');

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Reservations/CreateModal',
        modalClass: 'ReservationCreateModal'
    });

    var dataTable = $('#ReservationsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "desc"]], // Tarih'e göre ters sırala
            searching: false,
            scrollX: true,
            ajax: function (data, callback, settings) {
                var input = $('#ReservationFilterForm').serializeFormToObject();

                beroxApp.reservations.reservation.getList({
                    startDate: input.StartDate || null,
                    endDate: input.EndDate || null,
                    status: input.Status || null,
                    sorting: "reservationDate desc",
                    skipCount: data.start,
                    maxResultCount: data.length
                }).done(function (result) {
                    callback({
                        draw: data.draw,
                        recordsTotal: result.totalCount,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
                }).fail(function (error) {
                    abp.notify.error(error.message || 'Bir hata oluştu');
                });
            },
            columnDefs: [
                {
                    title: l('Actions'),
                    orderable: false,
                    rowAction: {
                        items: [
                            {
                                text: l('Details'),
                                action: function (data) {
                                    // TODO: Detay modal'ı açılabilir
                                }
                            },
                            {
                                text: l('Approve'),
                                visible: function (data) {
                                    return data.status === 0 && abp.auth.isGranted('BeroxApp.Reservations.Approve');
                                },
                                confirmMessage: function (data) {
                                    return l('ReservationApproveConfirmationMessage');
                                },
                                action: function (data) {
                                    beroxApp.reservations.reservation
                                        .approve(data.record.id)
                                        .then(function () {
                                            abp.notify.success(l('ReservationApproved'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('Complete'),
                                visible: function (data) {
                                    return data.status === 1 && abp.auth.isGranted('BeroxApp.Reservations.Complete');
                                },
                                confirmMessage: function (data) {
                                    return l('ReservationCompleteConfirmationMessage');
                                },
                                action: function (data) {
                                    beroxApp.reservations.reservation
                                        .complete(data.record.id)
                                        .then(function () {
                                            abp.notify.success(l('ReservationCompleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('Cancel'),
                                visible: function (data) {
                                    return data.status < 2 && abp.auth.isGranted('BeroxApp.Reservations.Cancel');
                                },
                                confirmMessage: function (data) {
                                    return l('ReservationCancelConfirmationMessage');
                                },
                                action: function (data) {
                                    beroxApp.reservations.reservation
                                        .cancel(data.record.id)
                                        .then(function () {
                                            abp.notify.warning(l('ReservationCancelled'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            },
                            {
                                text: l('Delete'),
                                visible: abp.auth.isGranted('BeroxApp.Reservations.Delete'),
                                confirmMessage: function (data) {
                                    return l('ReservationDeletionConfirmationMessage');
                                },
                                action: function (data) {
                                    beroxApp.reservations.reservation
                                        .delete(data.record.id)
                                        .then(function () {
                                            abp.notify.info(l('SuccessfullyDeleted'));
                                            dataTable.ajax.reload();
                                        });
                                }
                            }
                        ]
                    }
                },
                {
                    title: l('ReservationDate'),
                    data: "reservationDate",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString({
                                ...luxon.DateTime.DATETIME_SHORT,
                                weekday: 'short'
                            });
                    }
                },
                {
                    title: l('Customer'),
                    data: "customerName",
                    render: function (data, type, row) {
                        return data + '<br><small class="text-muted">' + row.customerPhone + '</small>';
                    }
                },
                {
                    title: l('Employee'),
                    data: "employeeName"
                },
                {
                    title: l('Service'),
                    data: "serviceName"
                },
                {
                    title: l('Status'),
                    data: "status",
                    render: function (data) {
                        var statusText = '';
                        var statusClass = '';

                        switch (data) {
                            case 0:
                                statusText = l('Pending');
                                statusClass = 'warning';
                                break;
                            case 1:
                                statusText = l('Approved');
                                statusClass = 'info';
                                break;
                            case 2:
                                statusText = l('Completed');
                                statusClass = 'success';
                                break;
                            case 3:
                                statusText = l('Cancelled');
                                statusClass = 'danger';
                                break;
                        }

                        return '<span class="badge bg-' + statusClass + '">' + statusText + '</span>';
                    }
                },
                {
                    title: l('FinalPrice'),
                    data: "finalPrice",
                    render: function (data) {
                        return data.toLocaleString('tr-TR', {
                            minimumFractionDigits: 2,
                            maximumFractionDigits: 2
                        }) + ' ₺';
                    }
                }
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('SuccessfullyCreated'));
    });

    $('#NewReservationButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $('#FilterButton').click(function (e) {
        e.preventDefault();
        dataTable.ajax.reload();
    });

    $('#ClearFilterButton').click(function (e) {
        e.preventDefault();
        $('#ReservationFilterForm')[0].reset();
        dataTable.ajax.reload();
    });

    // Bugünün tarihini default olarak ayarla
    var today = new Date().toISOString().split('T')[0];
    $('#StartDate').val(today);
    $('#EndDate').val(today);
});