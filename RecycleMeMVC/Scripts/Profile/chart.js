$(document).ready(function () {

    $('.main .settings').click(function () {
        $('.set').css('top', '0px');
    });
    $('.set .settings').click(function () {
        $('.set').css('top', '-500px');
    });
    $('.initialset .settings').click(function () {
        $('.initialset').css('top', '-500px');
    });
    //onward march
    var width = 350;
    $('.slide').width(width);

    $('.next').click(function () {
        var $first = $(".slide").first();
        $first.animate({
            marginLeft: -width
        }, 500, 'easeOutExpo', function () {
            $first.appendTo($first.parent()).css('margin-left', '0');
        });
    });
    //reteat
    $('.prev').click(function () {
        var $first = $(".slide").first();
        var $last = $(".slide").last();
        $last.prependTo($last.parent()).css('margin-left', '0');
        $last.css('margin-left', -width)
        $last.animate({
            marginLeft: '0px'
        }, 500, 'easeOutExpo', function () { });
    });
});



var options = {
    animation: true,
    scaleShowLabels: false,
    scaleOverride: true,
    scaleShowGridLines: false,
    scaleSteps: 10,
    scaleStepWidth: 1,
    scaleStartValue: 0,
    scaleOverlay: false
}

var data = {
    labels: ["", "", "", "", ""],
    datasets: [
        {
            fillColor: "rgba(220,220,220,0.5)",
            strokeColor: "rgba(220,220,220,1)",
            pointColor: "rgba(220,220,220,1)",
            pointStrokeColor: "rgba(255,255,255,1)",
            data: [7.25, 2.56, 0, 0, 0]
        }
    ]
}
var ctx = $("#weekView").get(0).getContext("2d");
var weekViewChart = new Chart(ctx);
weekViewChart.Line(data, options);


var options = {
    animation: true,
    animationEasing: "easeOutSine",
    percentageInnerCutout: 50,
    segmentShowStroke: false
}

var day = [
    {
        value: 30,
        color: "rgb(126,179,72)"
    },
    {
        value: 70,
        color: "rgba(126,179,72,0.6)"
    }

]
var ctx = $("#day").get(0).getContext("2d");
var dayObject = new Chart(ctx);
dayObject.Doughnut(day, options);

var week = [
    {
        value: 20.7,
        color: "rgb(207,54,54)"
    },
    {
        value: 79.3,
        color: "rgba(207,54,54,0.6)"
    }

]

var ctx = $("#week").get(0).getContext("2d");
var weekObject = new Chart(ctx);
weekObject.Doughnut(week, options);

var month = [
    {
        value: 35.5,
        color: "rgb(207,54,54)"
    },
    {
        value: 64.5,
        color: "rgba(207,54,54,0.6)"
    }

]

var ctx = $("#month").get(0).getContext("2d");
var monthObject = new Chart(ctx);
monthObject.Doughnut(month, options);