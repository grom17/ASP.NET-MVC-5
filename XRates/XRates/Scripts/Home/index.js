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
            ApplyChart($.parseJSON(result));
        },
        //error: AjaxCommonErrorHandling,
        complete: function (req, status) {
            //LoadingState(false);
        }
    });
}

function ApplyChart(data)
{
    var seriesOptions = [],
        seriesCounter = 0,
        names = ['USD', 'JPY', 'CNY'];

    function createChart() {

        $('#ratesDiv').highcharts('StockChart', {

            rangeSelector: {
                selected: 4
            },

            yAxis: {
                labels: {
                    formatter: function () {
                        return (this.value > 0 ? ' + ' : '') + this.value + '%';
                    }
                },
                plotLines: [{
                    value: 0,
                    width: 2,
                    color: 'silver'
                }]
            },

            plotOptions: {
                series: {
                    compare: 'percent'
                }
            },

            tooltip: {
                pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.change}%)<br/>',
                valueDecimals: 2
            },

            series: seriesOptions
        });
    }

    $.each(names, function (i, name) {
        console.log(name, data[i]);
        seriesOptions[i] = {
            name: name,
            data: [[1243814400000, 21.4], [1243900800000, 21.5]]
        };

        seriesCounter += 1;

        if (seriesCounter === names.length) {
            createChart();
        }
    });
}