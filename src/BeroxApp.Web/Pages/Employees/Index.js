$(function () {
    var l = abp.localization.getResource('BeroxApp');

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Employees/CreateEditModal',
        modalClass: 'EmployeeCreateEditModal'
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Employees/CreateEditModal',
        modalClass: 'EmployeeCreateEditModal'
    });

    var dataTable = $('#EmployeesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]], // İsme göre sırala
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

                beroxApp.employees.employee.getList({
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
                                    visible: abp.auth.isGranted('BeroxApp.Employees.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('BeroxApp.Employees.Delete'),
                                    confirmMessage: function (data) {
                                        return l('EmployeeDeletionConfirmationMessage', data.record.fullName);
                                    },
                                    action: function (data) {
                                        beroxApp.employees.employee
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
                        return data ? '<a href="tel:' + data + '">' + data + '</a>' : '-';
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
                    title: l('MonthlySalary'),
                    data: "monthlySalary",
                    render: function (data) {
                        return data.toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + ' ₺';
                    }
                },
                {
                    title: l('Status'),
                    data: "isActive",
                    render: function (data) {
                        return data
                            ? '<span class="badge bg-success">' + l('Active') + '</span>'
                            : '<span class="badge bg-danger">' + l('Inactive') + '</span>';
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

    $('#NewEmployeeButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});