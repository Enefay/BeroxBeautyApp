$(function () {
    var l = abp.localization.getResource('BeroxApp');

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Customers/CreateEditModal',
        modalClass: 'CustomerCreateEditModal'
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Customers/CreateEditModal',
        modalClass: 'CustomerCreateEditModal'
    });

    var dataTable = $('#CustomersTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[5, "desc"]], // CreationTime'a göre sırala
            searching: true,
            scrollX: true,
            ajax: function (data, callback, settings) {
                var filter = data.search.value || '';

                // Sıralama bilgisini düzgün hazırlayalım
                var sorting = '';
                if (data.order && data.order.length > 0) {
                    var sortColumn = data.columns[data.order[0].column];
                    if (sortColumn && sortColumn.data && sortColumn.orderable) {
                        sorting = sortColumn.data + ' ' + data.order[0].dir;
                    }
                }

                beroxApp.customers.customer.getList({
                    filter: filter,
                    sorting: sorting,
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
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('BeroxApp.Customers.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('BeroxApp.Customers.Delete'),
                                    confirmMessage: function (data) {
                                        return l('CustomerDeletionConfirmationMessage', data.record.fullName);
                                    },
                                    action: function (data) {
                                        beroxApp.customers.customer
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
                    title: l('FullName'),
                    data: "fullName",
                    render: function (data, type, row) {
                        return '<strong>' + data + '</strong>';
                    }
                },
                {
                    title: l('PhoneNumber'),
                    data: "phoneNumber",
                    render: function (data) {
                        return '<a href="tel:' + data + '">' + data + '</a>';
                    }
                },
                {
                    title: l('Email'),
                    data: "email",
                    render: function (data) {
                        return data ? '<a href="mailto:' + data + '">' + data + '</a>' : '-';
                    }
                },
                {
                    title: l('Notes'),
                    data: "notes",
                    render: function (data) {
                        return data || '-';
                    }
                },
                {
                    title: l('CreationTime'),
                    data: "creationTime",
                    render: function (data) {
                        return luxon
                            .DateTime
                            .fromISO(data, {
                                locale: abp.localization.currentCulture.name
                            }).toLocaleString(luxon.DateTime.DATETIME_SHORT);
                    }
                }
            ]
        })
    );

    createModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('SuccessfullyCreated'));
    });

    editModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('SuccessfullyUpdated'));
    });

    $('#NewCustomerButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});