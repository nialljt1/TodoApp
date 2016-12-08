export function configure(aurelia) {
    aurelia.use.standardConfiguration()
        .developmentLogging()
        .plugin('aurelia-table');

    aurelia.start().then(a => a.setRoot("src/app"));
}