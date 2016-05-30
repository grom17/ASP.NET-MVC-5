$(function () {
    setTimeout(ReadRates, 0);
});

function ReadRates()
{
    var div = $("#ratesDiv");
    //LoadingState(true);
    //LoadingStateMessage(div);
    $.ajax({
        url: $("#ratesDiv").data("action-url"),
        success: function (result) {
            //AjaxCommonSuccessHandling(result, function () {
            //    ApplyChart("ratesDiv", $.parseJSON(result));
            //});
            //ApplyChart($.parseJSON(result));
            ApplyChart(result);
        },
        //error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            //LoadingState(false);
        }
    });
}

function GetDate(ticks)
{
    //ticks are in nanotime; convert to microtime
    var ticksToMicrotime = ticks / 10000;

    //ticks are recorded from 1/1/1; get microtime difference from 1/1/1/ to 1/1/1970
    var epochMicrotimeDiff = 2208988800000;

    //new date is ticks, converted to microtime, minus difference from epoch microtime
    var tickDate = new Date(ticksToMicrotime);
    return tickDate;
}

function ApplyChart(data)
{
    var dataPoints1 = [];
    var dataPoints2 = [];
    var dataPoints3 = [];
    data[0].forEach(function(item, i, arr)
    {
        console.log(parseFloat(item[1]));
        dataPoints1.push({
            x: GetDate(item[0]),
            y: parseFloat(item[1]) 
        });
    });
    data[1].forEach(function(item, i, arr)
    {
        dataPoints2.push({
            x: GetDate(item[0]),
            y: parseFloat(item[1])
        });
    });
    data[2].forEach(function (item, i, arr) {
        dataPoints3.push({
            x: GetDate(item[0]),
            y: parseFloat(item[1])
        });
    });
    var options = {
        zoomEnabled: true,
        animationEnabled: true,
        title: {
            text: "Курсы валют Доллар США (USD), Японская йена (JPY), Китайский юань (CNY)"
        },
        axisX: {
            labelAngle: 30
        },
        axisY: {
            valueFormatString: "0.00 Р",
            includeZero: false
        },
        data: [{ 
            // dataSeries1
            type: "spline",
            xValueType: "dateTime",
            valueFormatString: "0.00",
            showInLegend: true,
            name: "USD",
            dataPoints: dataPoints1
        },
        {				
            // dataSeries2
            type: "spline",
            xValueType: "dateTime",
            showInLegend: true,
            name: "JPY" ,
            dataPoints: dataPoints2
        },
        {				
            // dataSeries3
            type: "spline",
            xValueType: "dateTime",
            showInLegend: true,
            name: "CNY" ,
            dataPoints: dataPoints3
        }]
    };

    $("#ratesDiv").CanvasJSChart(options);
    //// dataPoints
    //var dataPoints1 = [];
    //var dataPoints2 = [];
    //var dataPoints3 = [];

    //var chart = new CanvasJS.Chart("ratesDiv",{
    //    zoomEnabled: true,
    //    title: {
    //        text: "Share Value of Three Currencies"		
    //    },
    //    toolTip: {
    //        shared: true
				
    //    },
    //    legend: {
    //        verticalAlign: "top",
    //        horizontalAlign: "center",
    //        fontSize: 14,
    //        fontWeight: "bold",
    //        fontFamily: "calibri",
    //        fontColor: "dimGrey"
    //    },
    //    axisX: {
    //        title: "chart updates every 3 secs"
    //    },
    //    axisY:{
    //        prefix: '$',
    //        includeZero: false
    //    }, 
    //    data: [{ 
    //        // dataSeries1
    //        type: "line",
    //        //xValueType: "dateTime",
    //        showInLegend: true,
    //        name: "USD",
    //        dataPoints: [{ x: 10, y: 14 }, { x: 20, y: 12 }]
    //    },
    //    {				
    //        // dataSeries2
    //        type: "line",
    //        //xValueType: "dateTime",
    //        showInLegend: true,
    //        name: "JPY" ,
    //        dataPoints: [{ x: 11, y: 15 }, { x: 20, y: 12 }]
    //    },
    //    {				
    //        // dataSeries3
    //        type: "line",
    //        //xValueType: "dateTime",
    //        showInLegend: true,
    //        name: "CNY" ,
    //        dataPoints: [{ x: 12, y: 17 }, { x: 20, y: 12 }]
    //    }],
    //    legend:{
    //        cursor:"pointer",
    //        itemclick : function(e) {
    //            if (typeof(e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
    //                e.dataSeries.visible = false;
    //            }
    //            else {
    //                e.dataSeries.visible = true;
    //            }
    //            chart.render();
    //        }
    //    }
    //});

}
    //var seriesOptions = [],
    //    seriesCounter = 0,
    //    names = ['USD', 'JPY', 'CNY'];

    //function createChart() {

    //    $('#ratesDiv').highcharts('StockChart', {

    //        rangeSelector: {
    //            selected: 4
    //        },

    //        //yAxis: {
    //        //    labels: {
    //        //        formatter: function () {
    //        //            return (this.value > 0 ? ' + ' : '') + this.value + '%';
    //        //        }
    //        //    },
    //        //    plotLines: [{
    //        //        value: 0,
    //        //        width: 2,
    //        //        color: 'silver'
    //        //    }]
    //        //},

    //        //plotOptions: {
    //        //    series: {
    //        //        compare: 'percent'
    //        //    }
    //        //},

    //        //tooltip: {
    //        //    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
    //        //    valueDecimals: 2
    //        //},

    //        series: seriesOptions
    //    });
    //}
    //console.log(data);
    //$.each(names, function (i, name) {
    //    seriesOptions[i] = {
    //        name: name,
    //        data: data[i],
    //        dataGrouping: {
    //            enabled: false
    //        }
    //    };

    //    seriesCounter += 1;

    //    if (seriesCounter === names.length) {
    //        createChart();
    //    }
    //});