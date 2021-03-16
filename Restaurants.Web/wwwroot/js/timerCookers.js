$(document).ready(function () {
    let cItems = $(".carousel-item");
    $.each(cItems, function (i, carouselElemItem) {

        if (carouselElemItem.id == "") {
            return;
        }
        let carouselItemId = carouselElemItem.id;
        carouselItemsIds.push(carouselItemId);
        let carouselItemJQ = $("#" + carouselElemItem.id);
        let columnElem = carouselItemJQ.find(".timer").eq(0);

        let trimColumnStr = columnElem.text().trim();
        let columnArray = trimColumnStr.match(/\d{4}-\d+-\d+T\d+:\d+:\d{2}/g);

        if (columnArray.length < 2) {
            let spanText = new Date(trimColumnStr);
            setTimer(spanText, columnElem, carouselItemId, timers);
        }
        else {
            let startDate = new Date(columnArray[0]);
            let deliverDate = new Date(columnArray[1]);
            setTimeForDelivery(startDate, deliverDate, columnElem);
        }
    });

    function setTimeForDelivery(startDate, deliverDate, elem) {

        let start = startDate.getTime();
        let end = deliverDate.getTime();
        let distance = end - start;

        let time = setTheDistance(distance);
        elem.prev().css("display", "inline-block");
        elem.next().css("display", "none");
        elem.css("opacity", "1");
        elemText = `${time[0]} : ${time[1]} : ${time[2]} ${globalHoursMsg}`;
        elem.html(elemText);
    }

});