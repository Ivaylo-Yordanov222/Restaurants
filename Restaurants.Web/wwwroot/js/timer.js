$(function () {
    let tablesIds = [];
    let tables = $("table");
    $.each(tables, function (i, tableElem) {
        let tableId = tableElem.id;
        tablesIds.push(tableId);
    });

    let columns = $(".timer");
    $.each(columns, function (i, elem) {
        
        let columnArray = elem.innerText.split(" ");
        if (columnArray.length < 2) {
            let spanText = new Date(elem.innerText);
            setTimer(spanText, elem, tablesIds[i], timers);
        }
        else {
            let startDate = new Date(columnArray[0]);
            let deliverDate = new Date(columnArray[1]);
            setTimeForDelivery(startDate, deliverDate, elem);
        }
    });

    function setTimer(countDownDate, elem, tableId, timers) {

        let countDownDateTime = countDownDate.getTime();
        let limitedDate = countDownDateTime + timeLimit;
        let isSetDiscount = false;
        
        timers['i' + tableId] = setInterval(function () {

            let now = new Date().getTime();

            if (limitedDate < now && isSetDiscount == false) {
                let table = elem.parentElement.parentElement.parentElement.parentElement;
                let jqueryTable = $("#" + table.id);
                let td = jqueryTable.find("tr:last").find("td:last");
                let oldPrice = td.text().match(/[\d]*[\d]+/g);
                oldPrice = oldPrice.join(".");
                let newPrice = oldPrice * discountMultiplier;
                
                td.text(`${globalPriceWith}${discount.toFixed(2)}${globalDiscountMes}${newPrice.toFixed(2)}`);
                isSetDiscount = true;
            }
            let distance = now - countDownDateTime;

            let time = setTheDistance(distance);
            
            elem.previousElementSibling.style.display = "inline-block";
            elem.nextElementSibling.style.display = "none";
            elem.style.opacity = 1;
            elem.innerHTML = time[0] + ":" + time[1] + ":" + time[2] + globalHours;
        }, 1000);
    }

    function setTimeForDelivery(startDate, deliverDate, elem) {

        let start = startDate.getTime();
        let end = deliverDate.getTime();
        let distance = end - start;

        let time = setTheDistance(distance);
        elem.previousElementSibling.style.display = "inline-block";
        elem.nextElementSibling.style.display = "none";
        elem.style.opacity = 1;
        elem.innerHTML = time[0] + ":" + time[1] + ":" + time[2] + globalHours;
    }

    function setTheDistance(distance) {
        let timeArray = [];
        let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((distance % (1000 * 60)) / 1000);

        timeArray[0] = (String(hours).length >= 2) ? hours : '0' + hours;
        timeArray[1] = (String(minutes).length >= 2) ? minutes : '0' + minutes;
        timeArray[2] = (String(seconds).length >= 2) ? seconds : '0' + seconds;

        return timeArray;
    }
});