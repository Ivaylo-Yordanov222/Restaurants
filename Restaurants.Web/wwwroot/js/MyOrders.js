/// <reference path="../lib/signalr/signalr.js" />
$(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/orders")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection
        .start()
        .then(() => {
            $(document).on("submit", "form", function (e) {
                let form = $(this);
                if (!form.hasClass("pays")) {
                    return;
                }
                e.preventDefault();

                let orderId = form.children(0).val();
                
                connection.invoke("RemoveOrder", orderId);
                $("#" + orderId).remove();
            });
            connection.on("ChangeUserOrderStatus", (orderId, status) => {
                
                let table = $("#" + orderId);
                let td = table.find("tr:first").find("th:last").prev();
                let tdSignal = td.prev();
                if (status == 1) {
                    td.html(`<span class="spanBlock">${globalStatus}: ${globalInProgress}</span>`);
                    tdSignal.html(`<a class="orderSignalIconProgress"></a>`);
                }
                else if (status == 2) {
                    td.html(`<span class="spanBlock">${globalStatus}: ${globalDelivered}</span>`);
                    tdSignal.html(`<a class="orderSignalIconDelivered"></a>`);
                    let formPay = `<form class="pays" method="post" asp-area="Cooker" asp-controller="Home" asp-action="Agree">
                                        <input type="hidden" name="OrderId" value="${orderId}" />
                                        <input class="btn btn-sm btn-success" type="submit" value="${globalPay}" />
                                   </form>`
                    table.find("tr:last").find("td:first").html(formPay);
                    clearInterval(timers['i'+orderId]);
                }
            });
            
        }, err => console.log(err));
    connection.onclose(async () => {
        await start();
    });
});
