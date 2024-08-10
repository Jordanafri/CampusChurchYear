$(document).ready(function () {
    $('#tblData').DataTable({
        "serverSide": true,
        "processing": true,
        "ajax": {
            "url": "/Customer/Sermon/GetData",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "imagePath",
                "render": function (data) {
                    return `<img src="/icons/${data}" alt="Sermon Image" class="sermon-image" />`;
                },
                "width": "10%"
            },
            {
                "data": null,
                "render": function (data) {
                    return `
                        <div class="sermon-details">
                            <div class="sermon-date">${new Date(data.date).toLocaleDateString()}</div>
                            <div class="sermon-title">${data.title}</div>
                            ${data.seriesName ? `<div class="sermon-series"><a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></div>` : ''}
                        </div>
                        <div class="details-box" id="details-${data.id}">
                            <p><strong>Description:</strong> ${data.description}</p>
                            ${data.seriesName ? `<p><strong>Series:</strong> <a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></p>` : ''}
                        </div>`;
                },
                "width": "60%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="sermon-actions">
                            <a href="/Customer/Sermon/Download/${data}" class="btn btn-primary text-white"><i class="bi bi-download"></i> Download</a>
                            <button class="more-info"><i class="bi bi-chevron-down toggle-arrow" data-id="${data}"></i></button>
                        </div>`;
                },
                "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "No records found"
        },
        "width": "100%"
    });

    $('#tblData tbody').on('click', 'button.more-info', function () {
        var sermonId = $(this).find('.toggle-arrow').data('id');
        $(`#details-${sermonId}`).slideToggle();
        $(this).find('.toggle-arrow').toggleClass('bi-chevron-down bi-chevron-up');
    });
});




//$(document).ready(function () {
//    $('#tblData').DataTable({
//        "ajax": {
//            "url": "/Customer/Sermon/GetAll",
//            "type": "GET",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                if (!json.data) {
//                    console.error("Error: Data not in expected format.");
//                    return [];
//                }
//                return json.data;
//            },
//            "error": function (jqXHR, textStatus, errorThrown) {
//                console.error("Error loading data: ", textStatus, errorThrown);
//                console.log("Response: ", jqXHR.responseText);
//            }
//        },
//        "columns": [
//            {
//                "data": "imagePath",
//                "render": function (data) {
//                    return `<img src="/icons/${data}" alt="Sermon Image" class="sermon-image" />`;
//                },
//                "width": "10%"
//            },
//            {
//                "data": null,
//                "render": function (data) {
//                    return `
//                        <div class="sermon-details">
//                            <div class="sermon-date">${new Date(data.date).toLocaleDateString()}</div>
//                            <div class="sermon-title">${data.title}</div>
//                            ${data.seriesName ? `<div class="sermon-series"><a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></div>` : ''}
//                        </div>
//                        <div class="details-box" id="details-${data.id}">
//                            <p><strong>Description:</strong> ${data.description}</p>
//                            ${data.seriesName ? `<p><strong>Series:</strong> <a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></p>` : ''}
//                        </div>`;
//                },
//                "width": "60%"
//            },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `
//                        <div class="sermon-actions">
//                            <a href="/Customer/Sermon/Download/${data}" class="btn btn-primary text-white"><i class="bi bi-download"></i> Download</a>
//                            <button class="more-info"><i class="bi bi-chevron-down toggle-arrow" data-id="${data}"></i></button>
//                        </div>`;
//                },
//                "width": "30%"
//            }
//        ],
//        "language": {
//            "emptyTable": "No records found"
//        },
//        "width": "100%"
//    });

//    $('#tblData tbody').on('click', 'button.more-info', function () {
//        var sermonId = $(this).find('.toggle-arrow').data('id');
//        $(`#details-${sermonId}`).slideToggle();
//        $(this).find('.toggle-arrow').toggleClass('bi-chevron-down bi-chevron-up');
//    });
//});




//$(document).ready(function () {
//    $('#tblData').DataTable({
//        "ajax": {
//            "url": "/Customer/Sermon/GetAll",
//            "type": "GET",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                if (!json.data) {
//                    console.error("Error: Data not in expected format.");
//                    return [];
//                }
//                return json.data;
//            },
//            "error": function (jqXHR, textStatus, errorThrown) {
//                console.error("Error loading data: ", textStatus, errorThrown);
//                console.log("Response: ", jqXHR.responseText);
//            }
//        },
//        "columns": [
//            {
//                "data": "imagePath",
//                "render": function (data) {
//                    return `<img src="/icons/${data}" alt="Sermon Image" class="sermon-image" />`;
//                },
//                "width": "10%"
//            },
//            {
//                "data": null,
//                "render": function (data) {
//                    return `
//                        <div class="sermon-details">
//                            <div class="sermon-date">${new Date(data.date).toLocaleDateString()}</div>
//                            <div class="sermon-title">${data.title}</div>
//                            ${data.seriesName ? `<div class="sermon-series"><a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></div>` : ''}
//                        </div>
//                        <div class="details-box" id="details-${data.id}">
//                            <p><strong>Description:</strong> ${data.description}</p>
//                            ${data.seriesName ? `<p><strong>Series:</strong> <a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></p>` : ''}
//                        </div>`;
//                },
//                "width": "60%"
//            },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `
//                        <div class="sermon-actions">
//                            <a href="/Customer/Sermon/Download/${data}" class="btn btn-primary text-white">Download</a>
//                            <button class="more-info" data-id="${data}">More</button>
//                        </div>`;
//                },
//                "width": "30%"
//            }
//        ],
//        "language": {
//            "emptyTable": "No records found"
//        },
//        "width": "100%"
//    });

//    $('#tblData tbody').on('click', 'button.more-info', function () {
//        var sermonId = $(this).data('id');
//        $(`#details-${sermonId}`).slideToggle();
//    });
//});




//$(document).ready(function () {
//    $('#tblData').DataTable({
//        "ajax": {
//            "url": "/Customer/Sermon/GetAll",
//            "type": "GET",
//            "datatype": "json",
//            "dataSrc": function (json) {
//                if (!json.data) {
//                    console.error("Error: Data not in expected format.");
//                    return [];
//                }
//                return json.data;
//            },
//            "error": function (jqXHR, textStatus, errorThrown) {
//                console.error("Error loading data: ", textStatus, errorThrown);
//                console.log("Response: ", jqXHR.responseText);
//            }
//        },
//        "columns": [
//            {
//                "data": "imagePath",
//                "render": function (data) {
//                    return `<img src="/icons/${data}" alt="Sermon Image" class="sermon-image" />`;
//                },
//                "width": "10%"
//            },
//            {
//                "data": null,
//                "render": function (data) {
//                    return `
//                        <div class="sermon-details">
//                            <div class="sermon-date">${new Date(data.date).toLocaleDateString()}</div>
//                            <div class="sermon-title">${data.title}</div>
//                            ${data.seriesName ? `<div class="sermon-series"><a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></div>` : ''}
//                        </div>
//                        <div class="details-box" id="details-${data.id}">
//                            <p><strong>Description:</strong> ${data.description}</p>
//                            ${data.seriesName ? `<p><strong>Series:</strong> <a href="/Customer/Sermon/Series/${data.seriesId}">${data.seriesName}</a></p>` : ''}
//                        </div>`;
//                },
//                "width": "60%"
//            },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `
//                        <div class="sermon-actions">
//                            <a href="/Customer/Sermon/Download/${data}" class="btn btn-primary text-white">Download</a>
//                            <button class="more-info" data-id="${data}">More</button>
//                        </div>`;
//                },
//                "width": "30%"
//            }
//        ],
//        "language": {
//            "emptyTable": "No records found"
//        },
//        "width": "100%"
//    });

//    $('#tblData tbody').on('click', 'button.more-info', function () {
//        var sermonId = $(this).data('id');
//        $(`#details-${sermonId}`).slideToggle();
//    });
//});
