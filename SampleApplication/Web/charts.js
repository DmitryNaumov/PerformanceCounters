$(function () {
    window.Charts = function (counterIndex, title) {

        var width = 300;
        var height = 100;
        var maxLength = 100;
        
        var seriesData = [];
        var container = null;
        
        init();

        function init() {
            var element = $('<div class="chart-container" />');
            var chartElement = ["<div class='chart-title' style='max-width: ", width, "px'>", title, "</div>"].join("");
            var appendToElement = $("#placeholder");
            container = element
                            .append(chartElement)
                            .appendTo(appendToElement);

            initData(new Date());

            var graph = createGraphInternal(container, seriesData, width, height);
            $(document).on("sample.updateEvent", function (event, data, timeStamp) {

                timeStamp = timeStamp || new Date();
                var point = { x: timeStamp.getTime() / 1000, y: data[counterIndex] };
                seriesData.push(point);

                graph.update();
            });
            
        }

        function initData(timeStamp) {
            // fills data with n initial values so that it looks like chart is floating to the left, not transforming
            for (var i = maxLength - 1; i > 0; i--) {
                seriesData.push({ x: (timeStamp).getTime() / 1000 - i, y: 0 });
            };
        };

        function createGraphInternal(appendTo, data, width, height) {

            var graph = new Rickshaw.Graph({
                element: $('<div class="chart" />').appendTo(appendTo)[0],
                width: width,
                height: height,
                renderer: 'area',
                stroke: true,
                interpolation: "cardinal",
                padding: {
                    top: 0.2
                },
                series: [{
                    color: '#C9E63C',
                    data: data,
                    name: title
                }
                ]
            });

            graph.render();

            //var slider = new Rickshaw.Graph.RangeSlider({
            //    graph: graph,
            //    element: $('<div class="slider" />').appendTo(appendTo)
            //});

            // slider.element.slider("option", "values", [maxLength - 20, maxLength]);

            var hoverDetail = new Rickshaw.Graph.HoverDetail({
                graph: graph
            });

            var ticksTreatment = 'glow';

            var xAxis = new Rickshaw.Graph.Axis.Time({
                graph: graph,
                ticksTreatment: ticksTreatment
            });

            xAxis.render();

            var yAxis = new Rickshaw.Graph.Axis.Y({
                graph: graph,
                tickFormat: Rickshaw.Fixtures.Number.formatKMBT,
                ticksTreatment: ticksTreatment,
                pixelsPerTick: 35
            });

            yAxis.render();

            return graph;
        };
    };
});