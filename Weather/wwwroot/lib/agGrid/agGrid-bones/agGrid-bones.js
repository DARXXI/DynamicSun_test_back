var localeText = AG_GRID_LOCALE_RU;

function GetSortValue(params) {
    console.log(params);
    let sortModel = params.request.sortModel;

    let sortValue = { sortColumn: 'date', sortOrder: 'Desc' };

    if (sortModel.length !== 0) {
        sortValue.sortColumn = sortModel[0].colId;
        sortValue.sortOrder = sortModel[0].sort;
    };
    return sortValue;
}

function getServerSideDatasource(url) {
    return {
        getRows: (params) => {
            var sort = GetSortValue(params);
            console.log('[Datasource] - rows requested by grid: ', params.request);
            $.ajax({
                type: "GET",
                url: url,
                dataType: 'json',
                data: {
                    StartRow: params.request.startRow,
                    EndRow: params.request.endRow,
                    SortColumn: sort.sortColumn,
                    SortOrder: sort.sortOrder,
                    Year: document.getElementById("myMonth").value.slice(0, 4),
                    Month: document.getElementById("myMonth").value.slice(5),
                },
                success: function (retrievedData) {
                    console.log("Retrived data:" + retrievedData.data + retrievedData.lastRowIndex);
                    params.success({
                        rowData: retrievedData.data,
                        rowCount: retrievedData.lastRowIndex,
                    });
                },
                error: function() {
                    Swal.fire({
                        icon: 'error',
                        title: 'Упс, произошла ошибка!',
                        showConfirmButton: false,
                        timer: 1500
                    });
                }
            });
        }
    };
}

function gridInit(url,/*columns,*/gridId) {
    // setup the grid after the page has finished loading
    /*gridOptions.columnDefs = columns;*/

    document.addEventListener('DOMContentLoaded', function () {
        var gridDiv = document.querySelector('#' + gridId);
        new agGrid.Grid(gridDiv, gridOptions);
        // create datasource with a reference to the fake server
        var datasource = getServerSideDatasource(url);
        // register the datasource with the grid
        gridOptions.api.setServerSideDatasource(datasource);
    });
}


