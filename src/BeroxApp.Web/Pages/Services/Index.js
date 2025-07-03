$(function () {
    var l = abp.localization.getResource('BeroxApp');
    //var createModal = new abp.ModalManager(abp.appPath + 'Services/CreateEditModal');
    //var editModal = new abp.ModalManager(abp.appPath + 'Services/CreateEditModal');


    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Services/CreateEditModal',
        modalClass: 'ServiceCreateEditModal'
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Services/CreateEditModal',
        modalClass: 'ServiceCreateEditModal'
    });

    var dataTable = $('#ServicesTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: true,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(beroxApp.services.service.getList),
            columnDefs: [
                {
                    title: l('ServiceName'),
                    data: "name"
                },
                {
                    title: l('Description'),
                    data: "description"
                },
                {
                    title: l('Duration'),
                    data: "duration",
                    render: function (data) {
                        return data + ' ' + l('Minutes');
                    }
                },
                {
                    title: l('Price'),
                    data: "price",
                    render: function (data) {
                        return data + ' ₺';
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
                },
                {
                    title: l('Actions'),
                    rowAction: {
                        items:
                            [
                                {
                                    text: l('Edit'),
                                    visible: abp.auth.isGranted('BeroxApp.Services.Edit'),
                                    action: function (data) {
                                        editModal.open({ id: data.record.id });
                                    }
                                },
                                {
                                    text: l('Delete'),
                                    visible: abp.auth.isGranted('BeroxApp.Services.Delete'),
                                    confirmMessage: function (data) {
                                        return l('ServiceDeletionConfirmationMessage', data.record.name);
                                    },
                                    action: function (data) {
                                        beroxApp.services.service
                                            .delete(data.record.id)
                                            .then(function () {
                                                abp.notify.info(l('SuccessfullyDeleted'));
                                                dataTable.ajax.reload();
                                            });
                                    }
                                }
                            ]
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

    $('#NewServiceButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});