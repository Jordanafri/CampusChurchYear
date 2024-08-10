var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        responsive: true,
        "ajax": { url: '/admin/sermon/getall' },
        "columns": [
            {
                data: 'imagePath',
                "width": "10%",
                "render": function (data) {
                    return `<img src="/icons/${data}" alt="Sermon Image" class="sermon-image" />`;
                }
            },
            { data: 'title', "width": "15%" },
            { data: 'date', "width": "15%" },
            { data: 'seriesName', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/sermon/upsert?id=${data}" class="btn btn-primary mx-2">
                        <i class="bi bi-pencil-square"></i> Edit
                    </a>
                    <a onClick=Delete('/admin/sermon/delete/${data}') class="btn btn-danger mx-2">
                        <i class="bi bi-trash-fill"></i> Delete
                    </a>
                    </div>`;
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            });
        }
    });
}
