"use strict"
var connection = new signalR.HubConnectionBuilder().withUrl("/dashboardHub").build();


$(function () {
    connection.start().then(function () {
        /*    document.getElementById("sendButton").disabled = false;*/
        console.log("Connected signalr !");
        //loadOrder();
        //InvokeWorkOrder();
    }).catch(function (err) {
        return console.error(err.toString());
    });
})


function loadOrder() {
    fetch('https://localhost:7054/api/workorder').then(response =>
    {
            if (!response.ok) {
                throw new Error('Lỗi khi gọi API: ' + response.status);
            }
            return response.json();
    }).then(data => {
            // Xử lý dữ liệu nhận được ở đây
        data.forEach(function (item) {
            // Xử lý từng phần tử trong data ở đây
            const divElement = document.querySelector('span.badge.badge-info.right');
            divElement.textContent = Object.keys(data).length;
            if ((item.systemName == "counter") || (item.systemName == "Counter")) 
            {
                $(".C" + item.location).removeClass("colorC");
                $(".C" + item.location).addClass("btn-danger");
            }
            if (item.systemName == "Gate") {
                $(".Gate" + item.location).removeClass("colorC");
                $(".Gate" + item.location).addClass("btn-danger");
            }
        });
    }).catch(error =>
    {
            console.error('Lỗi:', error);
    });
};

function InvokeWorkOrder() {
    connection.invoke("SendWorkOrders").catch(function (err) {
        return console.error(err.toString());
    });
};

connection.on("ReceiveWorkOrders", function (workOrder) {
    BindWorkOrderToGrid(workOrder);
    //console.log(workOrder);
});
connection.on("ReceivedClientChanged", function (connect,status) {
    console.log( connect+"  "+status);
});
connection.on("ReceiveDoneFromClient", function (id,status,des, timestart) {
    //BindWorkOrderToGrid(workOrder);
    console.log(id + status + des + timestart);
    loadOrder();
});


connection.on("ReceiveWorkOrdersDone", function (name, location) {
    loadOrder();
    if ((name == "counter") || (name == "Counter")) {
        $(".C" + location).removeClass("btn-danger");
        $(".C" + location).addClass("colorC");
    }
    if (name == "Gate") {
        $(".Gate" + location).removeClass("btn-danger");
        $(".Gate" + location).addClass("colorC");
    }
});

connection.on("ReceiveWorkOrdersAdd", function (name, location) {
    loadOrder();
    if ((name == "counter") || (name == "Counter")) {
        $(".C" + location).addClass("btn-danger");
        $(".C" + location).removeClass("colorC");
    }
    if (name == "Gate") {
        $(".Gate" + location).addClass("btn-danger");
        $(".Gate" + location).removeClass("colorC");
    }
});

function BindWorkOrderToGrid(workOrder) {
    $('#tblWorkOrder tbody').empty();
    console.log(workOrder);
    var tr;
    $.each(workOrder, function (index, workOrder) {
        tr = $('<tr/>');
        tr.append(`<td>${(workOrder.groupName)}</tr>`);
        tr.append(`<td>${(workOrder.systemName)}</tr>`);
        tr.append(`<td>${(workOrder.location)}</tr>`);
        tr.append(`<td>${(workOrder.errorDevice)}</tr>`);
        tr.append(`<td>${(workOrder.error)}</tr>`);
        tr.append(`<td>${(workOrder.timeOrder)}</tr>`);
        $('#tblWorkOrder').append(tr);
    });
}
$(document).ready(function () {
    $('.GateChange').on('click', function () {
        // Xử lý sự kiện click tại đây
        var Name = $(this).attr('data-name');
        var Lere = $(this).attr('data-lere');
        $("select." + Name + "." + Lere).removeClass("hiden");
        $("span.GateChange." + Name + "." + Lere).addClass("hiden");
        
    });
    $('.auto').on('click', function () {
        // Xử lý sự kiện click tại đây
        var Name = $(this).attr('data-name');
        var Lere = $(this).attr('data-lere');
        $(".input-group." + Name + "." + Lere).removeClass("hiden");
        $(".auto." + Name + "." + Lere).addClass("hiden");

    });
    $('.auto.btnxmark').on('click', function () {
        // Xử lý sự kiện click tại đây
        var Name = $(this).attr('data-name');
        var Lere = $(this).attr('data-lere');
        $(".auto." + Name + "." + Lere).removeClass("hiden");
        $(".input-group." + Name + "." + Lere).addClass("hiden");

    });

    //$(".selector").on("click", function () {
        
    //    //alert(cellClass);
        
    //});
    $(".selector").on("change", function () {
        var Name = $(this).attr('data-name');
        var Lere = $(this).attr('data-lere');
        var selectedValue = $(this).val();
        //if (selectedValue == 0) {
        //    $("span.GateChange." + Name + "." + Lere).removeClass("hiden");
        //    $("select." + Name + "." + Lere).addClass("hiden");
        //}
        const columnName = 'GateChange';
        const url = `https://localhost:7079/api/DigitalSignage/${Name}/${Lere}?columnName=${columnName}&des=${selectedValue}`;
        fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                // Các tiêu đề khác nếu cần thiết
            },
            body: JSON.stringify({ columnName, selectedValue })
        }).then(response => {
            if (response.ok) {
                location.reload();
                //console.log('Dữ liệu đã được cập nhật thành công');
            } else {
                // Xử lý lỗi
                console.log('Không thể cập nhật dữ liệu');
            }
        }).catch(error => {
            // Xử lý lỗi

        });
    });
    
});



