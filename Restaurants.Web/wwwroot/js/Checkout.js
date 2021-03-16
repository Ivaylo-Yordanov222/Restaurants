/// <reference path="../lib/signalr/signalr.js" />
$(document).ready(function () {

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
                    if (form.hasClass("logout")) {
                        return;
                    }
                    e.preventDefault();

                    let orderId = form.children(0).val();
                    connection.invoke("UpdateOrder", orderId);
                });

                connection.on("showOrder", (jsonOrder) => {
                    let data = JSON.parse(jsonOrder);
                    let carouselId = data.Username;
                    let elemExits = document.getElementById(carouselId);

                    if (elemExits == null) {
                        displayNewCarousel(data);
                        displayOrder(data);
                    }
                    else {
                        displayOrder(data);
                    }

                    let carouselItemId = document.getElementById(data.OrderId).id;
                    carouselItemsIds.push(carouselItemId);
                    let carouselItemJQ = $("#" + carouselItemId);
                    let columnElem = carouselItemJQ.find(".timer").eq(0);

                    let trimColumnStr = columnElem.text().trim();
                    let columnArray = trimColumnStr.match(/\d{4}-\d+-\d+T\d+:\d+:\d{2}/g);
                    let spanText = new Date(trimColumnStr);
                    setTimer(spanText, columnElem, carouselItemId, timers);

                    startTheBulb(data.Username);
                });

                connection.on("updateOrderStatus", (id, status) => {
                    
                    let orderToUpdate = $("#" + id);
                    let submitButton = orderToUpdate.find(':input[type="submit"]');
                    let hiddenField = orderToUpdate.find(':input[type="hidden"]');
                    let columnStatus = orderToUpdate.find("table tr:last").find("td:last").prev();
                    let columnBulb = columnStatus.prev();
                    let btnValue = "";

                    if (status == 0) {
                        btnValue = $('<textarea />').html(globalAccept).text();
                        columnStatus.html(`${globalStatus} ${globalSent}`);
                        columnBulb.html(`<a class="orderSignalIconAccept"></a>`);
                        submitButton.val(btnValue);
                    }
                    else if (status == 1) {
                        btnValue = $('<textarea />').html(globalDelivered).text();
                        columnStatus.html(`${globalStatus} ${globalInProgress}`);
                        columnBulb.html(`<a class="orderSignalIconProgress"></a>`);
                        submitButton.val(btnValue);
                    }
                    else if (status == 2) {
                        submitButton.remove();
                        columnStatus.html(`${globalStatus} ${globalDelivered}`);
                        columnBulb.html(`<a class="orderSignalIconDelivered"></a>`);
                        clearInterval(timers["i" + id]);
                    }
                });

                connection.on("RemoveOrderFromList", (id) => {

                    let orderToRemove = $("#" + id);
                    let currentCarousel = orderToRemove.closest(".carousel.slide");
                    let carouselInner = currentCarousel.find(".carousel-inner");
                    let carouselIndicators = currentCarousel.find(".carousel-indicators");
                    let lastItemIndex = carouselInner.find(".carousel-item:last-child").index();
                    let currentIndex = orderToRemove.index();

                    carouselIndicators.find("li").eq(currentIndex).remove();
                    let i = 0;
                    carouselIndicators.find("li").each(function () {
                        $(this).attr("data-slide-to", i);
                        i++;
                    });
                    orderToRemove.remove();
                    carouselInner.find(".carousel-item.active").removeClass("active");
                    carouselInner.find(".carousel-item:first-child").addClass("active");
                    currentCarousel.carousel(0);
                    delete timers["i" + id];

                });
            }, err => console.log(err));
        connection.onclose(async () => {
            await start();
        });
    });

    function displayOrder(data) {

        let newOrderItem = data.OrderHtml;
        let carouselInner = $("#" + data.Username).find(".carousel-inner").eq(0);
        let carouselIndicators = $("#" + data.Username).find(".carousel-indicators").eq(0);
        let numberOfElementsInCurrentCarousel = carouselInner.find(".carousel-item").length;
        let li = `<li data-target="#${data.Username}" data-slide-to="${numberOfElementsInCurrentCarousel}" class="active" style="background-color:aqua"></li>`;
        carouselInner.append(newOrderItem);
        carouselIndicators.append(li);

        $("#" + data.Username).carousel(numberOfElementsInCurrentCarousel);
    }

    function displayNewCarousel(data) {

        let carousel = `<div class="col-lg-6" style="margin-bottom:10px">
                        <div id="${data.Username}" class="carousel slide" data-interval="false">
                            <ol class="carousel-indicators">
                                 <li data-target="#${data.Username}" data - slide - to="0" class="active" ></li>
                            </ol>
                            <div class="carousel-inner" >
                                <div class="carousel-item active">
                                    <img src="Images/transperant.png" class="d-block w-100" style="height:500px" alt="...">
                                    <div class="carousel-caption d-md-block" style="transform:translateY(-50%); top:50%; bottom:0;">
                                        <h5 class="h5custom">${data.Username}</h5>
                                    </div>
                                </div>
                            </div>
                            <a class="carousel-control-prev" href="#${data.Username}" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                            </a>
                            <a class="carousel-control-next" href="#${data.Username}" role="button" data-slide="next">
                                <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                            </a>
                    </div>`;
        $("#carousel-holder").append(carousel);
    }

    function startTheBulb(tableName) {
        var aBulb = document.getElementById("orderSignalIcon");
        aBulb.style.animation = "bulb-pulse-1 0.8s linear 0s infinite alternate";

        aBulb.setAttribute("href", "#" + tableName);
    }
});

