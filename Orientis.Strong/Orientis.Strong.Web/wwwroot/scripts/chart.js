window.chartInstances = {}; // Dictionary to hold charts by ID

window.setup = (id, config, exerciseType) => {
    var ctx = document.getElementById(id).getContext('2d');

    // set the callbacks here, otherwise it is interpreted as string instead of a JavaScript function

    if (config.options.scales.yl.ticks.callback) {
        //config.options.scales.yl.ticks.callback = window.chartHelper.formatInteger;
        switch (exerciseType) {
            case 0:
                config.options.scales.yl.ticks.callback = window.chartHelper.formatInteger;
                break;
            case 1:
                config.options.scales.yl.ticks.callback = window.chartHelper.formatInteger;
                break;
            case 2:
                config.options.scales.yl.ticks.callback = window.chartHelper.formatSeconds;
                break;
        }
    }

    if (config.options.plugins.tooltip.callbacks.title) {
        config.options.plugins.tooltip.callbacks.title = window.chartHelper.formatTooltipTitle;
    }
    if (config.options.plugins.tooltip.callbacks.label) {
        config.options.plugins.tooltip.callbacks.label = window.chartHelper.formatTooltipLabel;
    }
    if (config.options.plugins.tooltip.callbacks.footer) {
        config.options.plugins.tooltip.callbacks.footer = window.chartHelper.formatTooltipFooter;
    }

    var chart = new Chart(ctx, config);

    window.chartInstances[id] = chart; // Store the instance for later
};

window.update = (id, data) => {
    var chart = window.chartInstances[id];

    if (chart) {
        chart.data = data;
        chart.update();
    }
};

window.dispose = (id) => {
    var chart = window.chartInstances[id];

    if (chart) {
        chart.destroy();
        delete window.chartInstances[id];
    }
};

window.chartHelper = {
    formatInteger: function (value) {
        return value;
    },
    formatSeconds: function (value) {
        return formatSeconds(value);
    },
    // context: TooltipItem[]
    formatTooltipTitle: function (context) {
        let date = new Date(context[0].raw.x);

        if (isNaN(date)) {
            return value;
        }

        let day = date.getDate().toString().padStart(2, '0');
        let month = date.toLocaleDateString('default', { month: 'short' });
        let year = date.getUTCFullYear();

        return `${day} ${month} ${year}`;
    },
    // context: TooltipItem
    formatTooltipLabel: function (context) {
        let label = context.dataset.label || '';

        if (label) {
            label += ': ';
        }
        if (context.parsed.y !== null) {
            const firstSet = context.raw.sets[0];
            switch (firstSet.exerciseType) {
                case 0:
                    label += context.parsed.y;
                    break;
                case 1:
                    label += context.parsed.y;
                    break;
                case 2:
                    label += formatSeconds(context.parsed.y);
                    break;
            }
        }
        return label;
    },
    // context: TooltipItem[]
    formatTooltipFooter: function (context) {
        let firstContext = context[0];
        let sets = firstContext.dataset.data[firstContext.dataIndex].sets;
        if (sets === undefined) {
            return '';
        }
        let setTextArray = [];
        for (const set of sets) {
            switch (set.exerciseType) {
                case 0:
                    setTextArray.push(`Set ${set.order}: ${set.weight} ${set.weightUnit} x ${set.reps} reps`);
                    break;
                case 1:
                    setTextArray.push(`Set ${set.order}: ${set.reps} reps`);
                    break;
                case 2:
                    setTextArray.push(`Set ${set.order}: ${formatSeconds(set.seconds)} seconds`);
                    break;
            }
            
        }
        return setTextArray;
    }
};

function formatSeconds(seconds) {
    let minutes = Math.floor(seconds / 60);
    let remainingSeconds = (seconds % 60).toString().padStart(2, '0');

    return `${minutes}:${remainingSeconds}`;
}
