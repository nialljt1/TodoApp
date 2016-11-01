﻿import {inject} from "aurelia-framework";
import {Router} from "aurelia-router";

@inject(Router)
export class App {
    router: Router;

    constructor() { }

    configureRouter(config, router: Router) {
        this.router = router;

        config.title = "Todo App";
        config.map([
            { route: ["", "todos"], moduleId: "./views/todos", nav: true, title: "Todo List" },

        ]);
    }
}